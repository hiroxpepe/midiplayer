/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MidiPlayer.Droid {
    /// <summary>
    ///  partial class for MainActivity.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/Base.Theme.MaterialComponents.Light.DarkActionBar.Bridge",
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, 
        ScreenOrientation = ScreenOrientation.Portrait
    )]
    public partial class MainActivity : AppCompatActivity {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int VIEW_REFRESH_TIME = 2000; // msec.

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        /// <summary>
        /// current sound font file path, loaded from the file picker.
        /// </summary>
        string _sound_font_path = "undefined";

        /// <summary>
        /// current MIDI file path, loaded from the file picker.
        /// </summary>
        string _midi_file_path = "undefined";

        /// <summary>
        /// playlist holding MIDI file paths to play sequentially.
        /// </summary>
        PlayList _playlist;

        /// <summary>
        /// list data backing the ListView adapter.
        /// </summary>
        List<ListItem> _listitem_list;

        /// <summary>
        /// source used to cancel the view refresh loop when the Activity is destroyed.
        /// </summary>
        CancellationTokenSource _refresh_timer_cts;

        /// <summary>
        /// task running the view-refresh loop; started in OnCreate, cancelled via _refresh_timer_cts.
        /// </summary>
        Task _refresh_timer;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainActivity() {
            _playlist = new();
            _listitem_list = new();
            _refresh_timer_cts = new();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // EventHandler

        /// <summary>
        /// called by Android when the user responds to a runtime permission dialog.
        /// forwards the result to the base class so the framework can process it.
        /// </summary>
        /// <param name="requestCode">the integer request code passed to <c>RequestPermissions</c>.</param>
        /// <param name="permissions">the array of requested permissions.</param>
        /// <param name="grantResults">the array of grant results for each permission.</param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// called by the OS when the Activity is first created.
        /// initializes UI, configuration, and all synth event callbacks.
        /// </summary>
        /// <param name="savedInstanceState">the previously saved instance state, or <c>null</c> on first launch.</param>
        protected override void OnCreate(Bundle? savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Global unhandled-exception hooks: log the FULL inner exception so the real
            // cause of JavaProxyThrowable is visible in logcat before the process dies.
            AppDomain.CurrentDomain.UnhandledException += (s, ev) => {
                var ex = ev.ExceptionObject as Exception;
                //Log.Error($"[UNHANDLED AppDomain] {ex}");
            };
            TaskScheduler.UnobservedTaskException += (s, ev) => {
                //Log.Error($"[UNHANDLED Task] {ev.Exception}");
                ev.SetObserved();
            };
            AndroidEnvironment.UnhandledExceptionRaiser += (s, ev) => {
                //Log.Error($"[UNHANDLED Android] {ev.Exception}");
                ev.Handled = true;
            };

            requestPermissions();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            initializeComponent();
            // set the app-specific external files directory so that Env resolves all paths
            // under the Meowziq hierarchy (AppRootPath/Music/SoundFont, AppRootPath/Music/MIDI),
            // satisfying Android 10+ Scoped Storage policy (no hardcoded /storage/emulated/0/ paths).
            Env.AppRootPath = GetExternalFilesDir(null)?.AbsolutePath ?? string.Empty;
            Conf.Load();
            loadPreviousSetting();
            // Create and start the refresh timer only after initializeComponent() has set
            // _listview_item.Adapter; creating it earlier (e.g. in the constructor) would
            // start the loop before the adapter exists, risking NullReferenceException.
            _refresh_timer = createRefreshTask(_refresh_timer_cts.Token);
            _refresh_timer.Start();

            /// <summary>
            /// add a callback function to be called when the synth started.
            /// </summary>
            Synth.Started += () => {
                //Log.Info("Started called.");
                RunOnUiThread(() => {
                    try {
                        Title = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                    } catch (Exception ex) {
                        //Log.Error($"[Started/UI] {ex}");
                    }
                });
            };

            /// <summary>
            /// add a callback function to be called when the synth ended.
            /// posted to the UI thread via RunOnUiThread so it is serialised with the STOP button handler,
            /// preventing the Init/Stop race condition that caused a hang on the LongRunning thread.
            /// </summary>
            Synth.Ended += () => {
                //Log.Info("Ended called.");
                RunOnUiThread(() => {
                    try {
                        if (!Synth.Playing) {
                            //Log.Info("Ended: Synth not playing, skip restart.");
                            return;
                        }
                        stopSong();
                        if (!_playlist.Ready) {
                            playSong();
                        } else {
                            Synth.MidiFilePath = _playlist.Next;
                            playSong();
                        }
                    } catch (Exception ex) {
                        //Log.Error($"[Ended/UI] {ex}");
                    }
                });
            };

            /// <summary>
            /// add a callback function to be called when the synth updated.
            /// </summary>
            /// <remarks>
            /// update listitem values by track values.
            /// Writes directly to in-memory _listitem_list without calling RunOnUiThread —
            /// the Updated callback fires from the native FluidSynth audio thread which is not
            /// JNI-attached, so any JNI call (including RunOnUiThread) causes an unrecoverable crash.
            /// The periodic refresh timer calls NotifyDataSetChanged() on the UI thread to display
            /// the updated values safely.
            /// </remarks>
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
                try {
                    var track = (Synth.Track) sender;
                    if (track.Index == 0) return; // conductor track has no list slot
                    ListItem list_item = _listitem_list[track.IndexWithExcludingConductor];
                    list_item.Name = track.Name;
                    list_item.Instrument = Synth.GetVoice(track.Index);
                    list_item.Channel = track.ChannelAsOneBased.ToString();
                } catch (Exception) {
                    // Swallow: this handler runs on the native audio thread.
                    // Any Log/JNI call here would crash the process on Android.
                }
            };

            /// <summary>
            /// add a callback function to be called when the synth updated.
            /// </summary>
            /// <remarks>
            /// update fader values by track values.
            /// Same no-JNI constraint as the listitem handler above.
            /// </remarks>
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
                try {
                    var track = (Synth.Track) sender;
                    if (track.Index == 0) return; // conductor track has no fader slot
                    Mixer.Fader fader = Mixer.GetBy(track.IndexWithExcludingConductor);
                    if (e.PropertyName is nameof(Synth.Track.Channel)) {
                        fader.Channel = track.Channel;
                    }
                    if (e.PropertyName is nameof(Synth.Track.Program)) {
                        fader.Program = track.Program;
                    }
                    if (e.PropertyName is nameof(Synth.Track.Pan)) {
                        fader.Pan = track.Pan;
                    }
                    if (e.PropertyName is nameof(Synth.Track.Volume)) {
                        fader.Volume = track.Volume;
                    }
                } catch (Exception) {
                    // Swallow: this handler runs on the native audio thread.
                    // Any Log/JNI call here would crash the process on Android.
                }
            };

            /// <summary>
            /// add a callback function to be called when the mixer selected.
            /// </summary>
            Mixer.Selected += (object sender, PropertyChangedEventArgs e) => {
                if (e.PropertyName is nameof(Mixer.Current)) {
                    Mixer.Fader fader = Mixer.GetCurrent();
                    RunOnUiThread(() => {
                        try {
                            _textview_no.Text = fader.IndexAsOneBased.ToString();
                            _textview_channel.Text = fader.ChannelAsOneBased.ToString();
                            _numberpicker_prog.Value = fader.ProgramAsOneBased;
                            _numberpicker_pan.Value = fader.Pan;
                            _numberpicker_vol.Value = fader.Volume;
                            _checkbox_mute.Checked = !fader.Sounds;
                        } catch (Exception ex) {
                            //Log.Error($"[Mixer.Selected/UI] {ex}");
                        }
                    });
                }
            };

            /// <summary>
            /// add a callback function to be called when the mixer updated.
            /// </summary>
            /// <remarks>
            /// This handler is invoked from the native FluidSynth audio thread (a bare POSIX thread
            /// not registered with JNI). Any call that crosses the JNI boundary — including Log.Debug,
            /// Log.Error, or any Android API — will crash the process immediately on Android.
            /// Keep this handler empty; fader state is already written by the Synth.Updated handler
            /// and displayed by the periodic NotifyDataSetChanged() timer.
            /// </remarks>
            Mixer.Updated += (object sender, PropertyChangedEventArgs e) => { };
        }

        /// <summary>
        /// Activity OnStart.
        /// </summary>
        protected override void OnStart() {
            base.OnStart();
        }

        /// <summary>
        /// Activity OnResume.
        /// </summary>
        protected override void OnResume() {
            base.OnResume();
        }

        /// <summary>
        /// Activity OnPause.
        /// </summary>
        protected override void OnPause() {
            base.OnPause();
        }

        /// <summary>
        /// Activity OnStop.
        /// </summary>
        protected override void OnStop() {
            base.OnStop();
        }

        /// <summary>
        /// Activity OnDestroy.
        /// </summary>
        protected override void OnDestroy() {
            try {
                // cancel the view refresh loop before stopping playback to prevent the
                // task from touching the destroyed Activity's UI after OnDestroy returns.
                _refresh_timer_cts.Cancel();
                stopSong();
            } catch (Exception ex) {
                //Log.Error($"[OnDestroy] {ex}");
            } finally {
                base.OnDestroy();
            }
        }

        /// <summary>
        /// Activity OnActivityResult.
        /// </summary>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data) {
            if (resultCode != Result.Ok || data is null) {
                return;
            }
            switch (requestCode) {
                case (int) Request.SoundFont:
                    _sound_font_path = getActualPathBy(data);
                    if (!(_sound_font_path.Contains(".SF2") || _sound_font_path.Contains(".sf2"))) {
                        //Log.Warn("not a sound font.");
                        break;
                    }
                    //Log.Info($"selected: {_sound_font_path}");
                    Synth.SoundFontPath = _sound_font_path;
                    Env.SoundFontPath = _sound_font_path;
                    Title = $"MidiPlayer: {_midi_file_path.ToFileName()} {_sound_font_path.ToFileName()}";
                    break;
                case (int) Request.MidiFile:
                    _midi_file_path = getActualPathBy(data);
                    if (!(_midi_file_path.Contains(".MID") || _midi_file_path.Contains(".mid"))) {
                        //Log.Warn("not a midi file.");
                        break;
                    }
                    //Log.Info($"selected: {_midi_file_path}");
                    Synth.MidiFilePath = _midi_file_path;
                    Env.MidiFilePath = _midi_file_path;
                    Title = $"MidiPlayer: {_midi_file_path.ToFileName()} {_sound_font_path.ToFileName()}";
                    break;
                case (int) Request.AddPlayList:
                    var midi_file_path = getActualPathBy(data);
                    if (!(midi_file_path.Contains(".MID") || midi_file_path.Contains(".mid"))) {
                        //Log.Warn("not a midi file.");
                        break;
                    }
                    //Log.Info($"selected: {midi_file_path}");
                    _playlist.Add(midi_file_path); // add to playlist
                    Env.MidiFilePath = midi_file_path;
                    break;
                default:
                    break;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        /// <summary>
        /// request permissions.
        /// </summary>
        void requestPermissions() {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int) Permission.Granted) {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 0);
            }
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int) Permission.Granted) {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 0);
            }
        }

        /// <summary>
        /// call Intent.
        /// </summary>
        void callIntent(string target_dir, int request_code) {
            var intent = new Intent(Intent.ActionOpenDocument);
            var uri = Android.Net.Uri.Parse($"content://com.android.externalstorage.documents/document/primary%3A{target_dir}");
            intent.SetData(uri);
            intent.SetType("*/*");
            intent.PutExtra("android.provider.extra.INITIAL_URI", uri);
            intent.PutExtra("android.content.extra.SHOW_ADVANCED", true);
            intent.AddCategory(Intent.CategoryOpenable);
            StartActivityForResult(intent, request_code);
        }

        /// <summary>
        /// get an actual path.
        /// </summary>
        static string getActualPathBy(Intent data) {
            var uri = data.Data;
            string doc_id = DocumentsContract.GetDocumentId(uri);
            char[] char_array = { ':' };
            string[] string_array = doc_id.Split(char_array);
            string type = string_array[0]; // primary
            string path = string.Empty;
            if ("primary".Equals(type, StringComparison.OrdinalIgnoreCase)) {
                path = Android.OS.Environment.ExternalStorageDirectory + "/" + string_array[1];
            }
            return path;
        }

        /// <summary>
        /// load previous setting.
        /// </summary>
        void loadPreviousSetting() {
            if (Env.ExistsSoundFont && Env.ExistsMidiFile) {
                Synth.SoundFontPath = Env.SoundFontPath;
                Synth.MidiFilePath = Env.MidiFilePath;
                Title = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                _sound_font_path = Env.SoundFontPath;
                _midi_file_path = Env.MidiFilePath;
            }
        }

        /// <summary>
        /// play the song.
        /// </summary>
        /// <remarks>
        /// TaskCreationOptions.LongRunning is used instead of Task.Run because Synth.Start()
        /// calls fluid_player_join() internally, which blocks until the song finishes.
        /// LongRunning signals the scheduler to allocate a dedicated thread outside the ThreadPool,
        /// preventing Thread Pool Starvation when multiple songs play consecutively.
        /// </remarks>
        void playSong() {
            try {
                Task.Factory.StartNew(() => {
                    try {
                        if (!_playlist.Ready) {
                            Synth.MidiFilePath = _midi_file_path;
                            Synth.Start();
                        } else {
                            Synth.MidiFilePath = _playlist.Next;
                            Synth.Start();
                        }
                        logMemoryInfo();
                    } catch (Exception ex) {
                        //Log.Error($"[playSong task] {ex}");
                    }
                }, TaskCreationOptions.LongRunning);
            } catch (Exception ex) {
                //Log.Error($"[playSong] {ex}");
            }
        }

        /// <summary>
        /// stop the song.
        /// </summary>
        /// <remarks>
        /// kept synchronous intentionally: Synth.Stop() calls fluid_player_stop + final(),
        /// both of which return quickly. making this async void would silently swallow any
        /// exception that escapes the try-catch, crashing the process without a call-site trace.
        /// </remarks>
        void stopSong() {
            try {
                Synth.Stop();
                Conf.Value.PlayList = _playlist.List; // TODO: save
                Conf.Save(); // TODO: save
                logMemoryInfo();
            } catch (Exception ex) {
                //Log.Error($"[stopSong] {ex}");
            }
        }

        /// <summary>
        /// refresh the view in a few seconds.
        /// </summary>
        /// <remarks>
        /// Uses a synchronous blocking loop (CancellationToken.WaitHandle.WaitOne) instead of
        /// async/await to avoid the "new Task(async lambda)" pitfall where the async continuation
        /// runs as an async-void delegate. Exceptions thrown after the first await point in that
        /// pattern are unobserved and cross the Java boundary as JavaProxyThrowable.
        /// WaitHandle.WaitOne returns false when the token is cancelled, ending the loop cleanly.
        /// </remarks>
        Task createRefreshTask(CancellationToken token) {
            return new Task(() => {
                try {
                    while (!token.IsCancellationRequested) {
                        RunOnUiThread(() => {
                            try {
                                if (_listview_item?.Adapter != null) {
                                    ((ListItemAdapter) _listview_item.Adapter).NotifyDataSetChanged();
                                }
                            } catch (Exception ex) {
                                //Log.Error($"[refreshTask UI] {ex}");
                            }
                        });
                        token.WaitHandle.WaitOne(VIEW_REFRESH_TIME);
                    }
                } catch (Exception ex) {
                    //Log.Error($"[refreshTask] {ex}");
                }
            }, token);
        }

        /// <summary>
        /// initialize listItem.
        /// </summary>
        void initializeListItem() {
            Enumerable.Range(start: MIDI_TRACK_BASE, count: MIDI_TRACK_COUNT).ToList().ForEach(x => {
                var listitem = _listitem_list[x];
                listitem.Name = "------"; listitem.Instrument = "------"; listitem.Channel = "---";
            });
        }

        /// <summary>
        /// show memory information to log.
        /// </summary>
        /// <note>
        /// development
        /// </note>
        static void logMemoryInfo() {
            // JVM runtime.
            var jvm_max_memory = Java.Lang.Runtime.GetRuntime().MaxMemory();
            var jvm_free_memory = Java.Lang.Runtime.GetRuntime().FreeMemory();
            var jvm_total_memory = Java.Lang.Runtime.GetRuntime().TotalMemory();
            //Log.Debug($"JVM max memory: {jvm_max_memory.ToMegabytes()}MB");
            //Log.Debug($"JVM free memory: {jvm_free_memory.ToMegabytes()}MB");
            //Log.Debug($"JVM total memory: {jvm_total_memory.ToMegabytes()}MB");
            // Mono runtime.
            var mono_total_memory = GC.GetTotalMemory(false);
            //Log.Debug($"Mono total memory: {mono_total_memory.ToMegabytes()}MB");
        }
    }
}
