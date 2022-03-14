
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
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MidiPlayer.Droid {

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

        string _soundFontPath = "undefined";

        string _midiFilePath = "undefined";

        PlayList _playList;

        List<ListItem> _itemList;

        Task _refreshTimer;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainActivity() {
            _playList = new();
            _itemList = new();
            _refreshTimer = createRefreshTask();
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
            Conf.Load();
            loadPreviousSetting();
            _refreshTimer.Start();

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
                MainThread.BeginInvokeOnMainThread(() => {
                    Title = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                });
            };

            /// <summary>
            /// add a callback function to be called when the synth ended.
            /// </summary>
            Synth.Ended += () => {
                Log.Info("Ended called.");
                if (!_playList.Ready) {
                    Synth.Stop();
                    Synth.Start();
                } else {
                    Synth.Stop();
                    Synth.MidiFilePath = _playList.Next;
                    Synth.Start();
                }
            };

            /// <summary>
            /// add a callback function to be called when the synth updated.
            /// </summary>
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
                var track = (Synth.Track) sender;
                updateList(track);
            };

            /// <summary>
            /// add a callback function to be called when the mixer updated.
            /// </summary>
            Mixer.Selected += (object sender, PropertyChangedEventArgs e) => {
                if (e.PropertyName is nameof(Mixer.Current)) {
                    loadFader();
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
                    _soundFontPath = getActualPathBy(data);
                    if (!(_soundFontPath.Contains(".SF2") || _soundFontPath.Contains(".sf2"))) {
                        Log.Warn("not a sound font.");
                        break;
                    }
                    Log.Info($"selected: {_soundFontPath}");
                    Synth.SoundFontPath = _soundFontPath;
                    Env.SoundFontPath = _soundFontPath;
                    Title = $"MidiPlayer: {_midiFilePath.ToFileName()} {_soundFontPath.ToFileName()}";
                    break;
                case (int) Request.MidiFile:
                    _midiFilePath = getActualPathBy(data);
                    if (!(_midiFilePath.Contains(".MID") || _midiFilePath.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {_midiFilePath}");
                    Synth.MidiFilePath = _midiFilePath;
                    Env.MidiFilePath = _midiFilePath;
                    Title = $"MidiPlayer: {_midiFilePath.ToFileName()} {_soundFontPath.ToFileName()}";
                    break;
                case (int) Request.AddPlayList:
                    var midiFilePath = getActualPathBy(data);
                    if (!(midiFilePath.Contains(".MID") || midiFilePath.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {midiFilePath}");
                    _playList.Add(midiFilePath); // add to playlist
                    Env.MidiFilePath = midiFilePath;
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

        void callIntent(string targetDir, int requestCode) {
            var intent = new Intent(Intent.ActionOpenDocument);
            var uri = Android.Net.Uri.Parse($"content://com.android.externalstorage.documents/document/primary%3A{targetDir}");
            intent.SetData(uri);
            intent.SetType("*/*");
            intent.PutExtra("android.provider.extra.INITIAL_URI", uri);
            intent.PutExtra("android.content.extra.SHOW_ADVANCED", true);
            intent.AddCategory(Intent.CategoryOpenable);
            StartActivityForResult(intent, requestCode);
        }

        static string getActualPathBy(Intent data) {
            var uri = data.Data;
            string docId = DocumentsContract.GetDocumentId(uri);
            char[] charArray = { ':' };
            string[] stringArray = docId.Split(charArray);
            string type = stringArray[0]; // primary
            string path = "";
            if ("primary".Equals(type, StringComparison.OrdinalIgnoreCase)) {
                path = Android.OS.Environment.ExternalStorageDirectory + "/" + stringArray[1];
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
                _soundFontPath = Env.SoundFontPath;
                _midiFilePath = Env.MidiFilePath;
            }
        }

        /// <summary>
        /// play a song.
        /// </summary>
        async void playSong() {
            try {
                await Task.Run(() => {
                    if (!_playList.Ready) {
                        Synth.MidiFilePath = _midiFilePath;
                        Synth.Start();
                    } else {
                        Synth.MidiFilePath = _playList.Next;
                        Synth.Start();
                    }
                });
                logMemoryInro();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// stop the song.
        /// </summary>
        async void stopSong() {
            try {
                await Task.Run(() => Synth.Stop());
                Conf.Value.PlayList = _playList.List; // TODO: save
                Conf.Save(); // TODO: save
                logMemoryInro();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// a callback function to be called when the synth updated.
        /// </summary>
        void updateList(Synth.Track track) {
            var trackIdx = track.Index - 1; // exclude conductor track;
            //Log.Info($"index: {trackIdx} name:  {track.Name} Voice: {Synth.GetVoice(track.Index)} Chan: {track.Channel}");
            var listItem = _itemList[trackIdx];
            listItem.Name = track.Name;
            listItem.Instrument = Synth.GetVoice(track.Index);
            listItem.Channel = track.Channel.ToString();
        }

        /// <summary>
        /// refresh the view in a few seconds.
        /// </summary>
        Task createRefreshTask() {
            return new(async () => {
                var listItemAdapter = (ListItemAdapter) _itemListView.Adapter;
                while (true) {
                    RunOnUiThread(() => {
                        listItemAdapter.NotifyDataSetChanged();
                    });
                    await Task.Delay(VIEW_REFRESH_TIME);
                }
            });
        }

        /// <summary>
        /// load the current fader value to control value.
        /// </summary>
        void loadFader() {
            saveFader();
            var fader = Mixer.GetCurrent();
            _textViewNo.Text = (Mixer.Current + 1).ToString(); // mixer is 0 base.
            ListItem listItem = _itemListView.GetItemAtPosition(Mixer.Current).Cast<ListItem>();
            _textViewChannel.Text = listItem.Channel;
            _numberPickerProg.Value = fader.Program;
            _numberPickerPan.Value = fader.Pan;
            _numberPickerVol.Value = fader.Volume;
            _checkBoxMute.Checked = !fader.Sounds;
        }

        /// <summary>
        /// save control value as the previous fader value.
        /// </summary>
        void saveFader() {
            var fader = Mixer.GetPrevious();
            if (int.TryParse(_textViewChannel.Text, out int channel)) {
                fader.Channel = channel;
                fader.Program = _numberPickerProg.Value;
                fader.Pan = _numberPickerPan.Value;
                fader.Volume = _numberPickerVol.Value;
                fader.Sounds = !_checkBoxMute.Checked;
            }
        }

        /// <summary>
        /// show memory information to log.
        /// FIXME: delete
        /// </summary>
        static void logMemoryInro() {
            // JVM runtime.
            var jvmMaxMemory = Java.Lang.Runtime.GetRuntime().MaxMemory();
            var jvmFreeMemory = Java.Lang.Runtime.GetRuntime().FreeMemory();
            var jvmTotalMemory = Java.Lang.Runtime.GetRuntime().TotalMemory();
            Log.Debug($"JVM maxMemory: {jvmMaxMemory.ToMegabytes()}MB");
            Log.Debug($"JVM freeMemory: {jvmFreeMemory.ToMegabytes()}MB");
            Log.Debug($"JVM totalMemory: {jvmTotalMemory.ToMegabytes()}MB");
            // Mono runtime.
            var monoTotalMemory = GC.GetTotalMemory(false);
            Log.Debug($"Mono totalMemory: {monoTotalMemory.ToMegabytes()}MB");
        }
    }
}
