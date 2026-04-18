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
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

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
            _refresh_timer = createRefreshTask(_refresh_timer_cts.Token);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // EventHandler

        /// <summary>
        /// Activity OnRequestPermissionsResult.
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Activity OnCreate.
        /// </summary>
        protected override void OnCreate(Bundle? savedInstanceState) {
            base.OnCreate(savedInstanceState);
            requestPermissions();
            Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            initializeComponent();
            // inject the OS-resolved external files directory before loading conf to
            // satisfy Android 10+ Scoped Storage policy (no hardcoded /storage/emulated/0/ paths).
            Conf.SetAndroidFilesDir(GetExternalFilesDir(null).AbsolutePath);
            Conf.Load();
            loadPreviousSetting();
            _refresh_timer.Start();

            /// <summary>
            /// add a callback function to be called when the synth is playback.
            /// </summary>
            Synth.Playbacking += (IntPtr data, IntPtr evt) => {
                return Synth.HandleEvent(data, evt);
            };

            /// <summary>
            /// add a callback function to be called when the synth started.
            /// </summary>
            Synth.Started += () => {
                Log.Info("Started called.");
                MainThread.BeginInvokeOnMainThread(action: () => {
                    Title = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                });
                initializeListItem();
            };

            /// <summary>
            /// add a callback function to be called when the synth ended.
            /// </summary>
            Synth.Ended += () => {
                Log.Info("Ended called.");
                if (!_playlist.Ready) {
                    Synth.Stop();
                    Synth.Start();
                } else {
                    Synth.Stop();
                    Synth.MidiFilePath = _playlist.Next;
                    Synth.Start();
                }
            };

            /// <summary>
            /// add a callback function to be called when the synth updated.
            /// </summary>
            /// <remarks>
            /// update listitem values by track values.
            /// </remarks>
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
                var track = (Synth.Track) sender;
                ListItem list_item = _listitem_list[track.IndexWithExcludingConductor];
                list_item.Name = track.Name;
                list_item.Instrument = Synth.GetVoice(track.Index);
                list_item.Channel = track.ChannelAsOneBased.ToString();
            };

            /// <summary>
            /// add a callback function to be called when the synth updated.
            /// </summary>
            /// <remarks>
            /// update fader values by track values.
            /// </remarks>
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
                var track = (Synth.Track) sender;
                Mixer.Fader fader = Mixer.GetBy(track.IndexWithExcludingConductor);
                if (e.PropertyName is nameof(Synth.Track.Channel)) {
                    Log.Debug($"Synth.Updated: track {track.Index} Channel is {track.Channel}");
                    fader.Channel = track.Channel;
                }
                if (e.PropertyName is nameof(Synth.Track.Program)) {
                    Log.Debug($"Synth.Updated: track {track.Index} Program is {track.Program}");
                    fader.Program = track.Program;
                }
                if (e.PropertyName is nameof(Synth.Track.Pan)) {
                    Log.Debug($"Synth.Updated: track {track.Index} Pan is {track.Pan}");
                    fader.Pan = track.Pan;
                }
                if (e.PropertyName is nameof(Synth.Track.Volume)) {
                    Log.Debug($"Synth.Updated: track {track.Index} Volume is {track.Volume}");
                    fader.Volume = track.Volume;
                }
            };

            /// <summary>
            /// add a callback function to be called when the mixer selected.
            /// </summary>
            Mixer.Selected += (object sender, PropertyChangedEventArgs e) => {
                if (e.PropertyName is nameof(Mixer.Current)) {
                    Mixer.Fader fader = Mixer.GetCurrent();
                    _textview_no.Text = fader.IndexAsOneBased.ToString();
                    _textview_channel.Text = fader.ChannelAsOneBased.ToString();
                    _numberpicker_prog.Value = fader.ProgramAsOneBased;
                    _numberpicker_pan.Value = fader.Pan;
                    _numberpicker_vol.Value = fader.Volume;
                    _checkbox_mute.Checked = !fader.Sounds;
                }
            };

            /// <summary>
            /// add a callback function to be called when the mixer updated.
            /// </summary>
            Mixer.Updated += (object sender, PropertyChangedEventArgs e) => {
                var fader = (Mixer.Fader) sender;
                if (fader.Index == Mixer.Current) {
                    Log.Debug($"Mixer.Updated: mixer.current {Mixer.Current}: fader.Index {fader.Index}");
                    if (e.PropertyName is nameof(Mixer.Fader.Name)) {
                        Log.Debug($"fadar {fader.Index} Name is {fader.Name}");
                    }
                    if (e.PropertyName is nameof(Mixer.Fader.Bank)) {
                        Log.Debug($"fadar {fader.Index} Bank is {fader.Bank}");
                    }
                    if (e.PropertyName is nameof(Mixer.Fader.Program)) {
                        Log.Debug($"fadar {fader.Index} Program is {fader.Program}");
                    }
                    if (e.PropertyName is nameof(Mixer.Fader.Volume)) {
                        Log.Debug($"fadar {fader.Index} Volume is {fader.Volume}");
                    }
                    if (e.PropertyName is nameof(Mixer.Fader.Pan)) {
                        Log.Debug($"fadar {fader.Index} Pan is {fader.Pan}");
                    }
                }
            };
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
                Log.Error(ex.Message);
            } finally {
                base.OnDestroy();
            }
        }

        /// <summary>
        /// Activity OnActivityResult.
        /// </summary>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data) {
            switch (requestCode) {
                case (int) Request.SoundFont:
                    _sound_font_path = getActualPathBy(data);
                    if (!(_sound_font_path.Contains(".SF2") || _sound_font_path.Contains(".sf2"))) {
                        Log.Warn("not a sound font.");
                        break;
                    }
                    Log.Info($"selected: {_sound_font_path}");
                    Synth.SoundFontPath = _sound_font_path;
                    Env.SoundFontPath = _sound_font_path;
                    Title = $"MidiPlayer: {_midi_file_path.ToFileName()} {_sound_font_path.ToFileName()}";
                    break;
                case (int) Request.MidiFile:
                    _midi_file_path = getActualPathBy(data);
                    if (!(_midi_file_path.Contains(".MID") || _midi_file_path.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {_midi_file_path}");
                    Synth.MidiFilePath = _midi_file_path;
                    Env.MidiFilePath = _midi_file_path;
                    Title = $"MidiPlayer: {_midi_file_path.ToFileName()} {_sound_font_path.ToFileName()}";
                    break;
                case (int) Request.AddPlayList:
                    var midi_file_path = getActualPathBy(data);
                    if (!(midi_file_path.Contains(".MID") || midi_file_path.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {midi_file_path}");
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
                        Log.Error(ex.Message);
                    }
                }, TaskCreationOptions.LongRunning);
            } catch (Exception ex) {
                Log.Error(ex.Message);
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
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// refresh the view in a few seconds.
        /// </summary>
        /// <remarks>
        /// the loop exits cleanly when the token is cancelled (Activity destroyed).
        /// OperationCanceledException from Task.Delay is swallowed intentionally.
        /// </remarks>
        Task createRefreshTask(CancellationToken token) {
            return new(action: async () => {
                try {
                    var listitem_adapter = (ListItemAdapter) _listview_item.Adapter;
                    while (!token.IsCancellationRequested) {
                        RunOnUiThread(action: () => {
                            listitem_adapter.NotifyDataSetChanged();
                        });
                        await Task.Delay(VIEW_REFRESH_TIME, token);
                    }
                } catch (OperationCanceledException) {
                    // intentional: loop ends cleanly when the Activity is destroyed.
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
            Log.Debug($"JVM max memory: {jvm_max_memory.ToMegabytes()}MB");
            Log.Debug($"JVM free memory: {jvm_free_memory.ToMegabytes()}MB");
            Log.Debug($"JVM total memory: {jvm_total_memory.ToMegabytes()}MB");
            // Mono runtime.
            var mono_total_memory = GC.GetTotalMemory(false);
            Log.Debug($"Mono total memory: {mono_total_memory.ToMegabytes()}MB");
        }
    }
}
