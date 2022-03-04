
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
using Android.Widget;

using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MidiPlayer.Droid {

    [Activity(Label = "@string/app_name", Theme = "@style/Base.Theme.MaterialComponents.Light.DarkActionBar.Bridge", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public partial class MainActivity : AppCompatActivity {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        string _soundFontPath = "undefined";

        string _midiFilePath = "undefined";

        PlayList _playList;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainActivity() {
            _playList = new PlayList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // EventHandler

        /// <summary>
        /// Activity OnRequestPermissionsResult().
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Activity OnCreate()
        /// </summary>
        protected override void OnCreate(Bundle? savedInstanceState) {
            base.OnCreate(savedInstanceState);
            requestPermissions();
            Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            initializeComponent();
            Conf.Load();

            // load previous setting.
            if (Env.ExistsSoundFont && Env.ExistsMidiFile) {
                Synth.SoundFontPath = Env.SoundFontPath;
                Synth.MidiFilePath = Env.MidiFilePath;
                Title = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                _soundFontPath = Env.SoundFontPath;
                _midiFilePath = Env.MidiFilePath;
            }

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

        void buttonLoadSoundFont_Click(object sender, EventArgs e) {
            Log.Info("buttonLoadSoundFont clicked.");
            try {
                if (Synth.Playing) {
                    stopSong();
                }
                callIntent(Env.SoundFontDirForIntent, (int) Request.SoundFont);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonLoadMidiFile_Click(object sender, EventArgs e) {
            Log.Info("buttonLoadMidiFile clicked.");
            try {
                if (Synth.Playing) {
                    stopSong();
                }
                callIntent(Env.MidiFileDirForIntent, (int) Request.MidiFile);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonStart_Click(object sender, EventArgs e) {
            Log.Info("buttonStart clicked.");
            try {
                if (!_midiFilePath.HasValue()) { // FIXME: case sounFdont
                    Log.Warn("midiFilePath has no value.");
                    return;
                }
                playSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonStop_Click(object sender, EventArgs e) {
            Log.Info("buttonStop clicked.");
            try {
                stopSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonAddPlaylist_Click(object sender, EventArgs e) {
            Log.Info("buttonAddPlaylist clicked.");
            try {
                callIntent(Env.MidiFileDir, (int) Request.AddPlayList);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonDeletePlaylist_Click(object sender, EventArgs e) {
            Log.Info("buttonDeletePlaylist clicked.");
            try {
                _playList.Clear();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_1_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_1 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_1).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_1).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_1).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch1, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

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
        /// play the song.
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
        /// show memory information to log.
        /// FIXME: delete
        /// </summary>
        static void logMemoryInro() {
            var maxMemory = Java.Lang.Runtime.GetRuntime().MaxMemory();
            var freeMemory = Java.Lang.Runtime.GetRuntime().FreeMemory();
            var totalMemory = Java.Lang.Runtime.GetRuntime().TotalMemory();
            Log.Info($"maxMemory: {maxMemory.ToMegabytes()}MB");
            Log.Info($"freeMemory: {freeMemory.ToMegabytes()}MB");
            Log.Info($"totalMemory: {totalMemory.ToMegabytes()}MB");
            // TODO: Mono runtime.
        }
    }
}
