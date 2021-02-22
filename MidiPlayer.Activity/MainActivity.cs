
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

namespace MidiPlayer.Activity {

    [Activity(Label = "@string/app_name", Theme = "@style/Base.Theme.MaterialComponents.Light.DarkActionBar.Bridge", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : AppCompatActivity {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        string soundFontPath = "undefined";

        string midiFilePath = "undefined";

        PlayList playList;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainActivity() {
            playList = new PlayList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // protected Methods [verb]

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            requestPermissions();
            Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            initializeComponent();
            Conf.Load();

            Synth.OnStart += () => {
                Log.Info("OnStart called.");
                MainThread.BeginInvokeOnMainThread(() => {
                    Title = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                });
            };

            Synth.OnEnd += () => {
                Log.Info("OnEnd called.");
                if (!playList.Ready) {
                    stopSong();
                    playSong();
                } else {
                    stopSong();
                    Synth.MidiFilePath = playList.Next;
                    playSong();
                }
            };
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnResume() {
            base.OnResume();
        }

        protected override void OnPause() {
            base.OnPause();
        }

        protected override void OnStop() {
            base.OnStop();
            Conf.Value.PlayList = playList.List;
            Conf.Save();
        }

        protected override void OnDestroy() {
            try {
                stopSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            } finally {
                base.OnDestroy();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
            switch (requestCode) {
                case (int) Request.SoundFont:
                    soundFontPath = getActualPathBy(data);
                    if (!(soundFontPath.Contains(".SF2") || soundFontPath.Contains(".sf2"))) {
                        Log.Warn("not a sound font.");
                        break;
                    }
                    Log.Info($"selected: {soundFontPath}");
                    Synth.SoundFontPath = soundFontPath;
                    Env.SoundFontDir = soundFontPath.ToDirectoryName();
                    Title = $"MidiPlayer: {midiFilePath.ToFileName()} {soundFontPath.ToFileName()}";
                    break;
                case (int) Request.MidiFile:
                    midiFilePath = getActualPathBy(data);
                    if (!(midiFilePath.Contains(".MID") || midiFilePath.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {midiFilePath}");
                    Synth.MidiFilePath = midiFilePath;
                    Env.MidiFileDir = midiFilePath.ToDirectoryName();
                    Title = $"MidiPlayer: {midiFilePath.ToFileName()} {soundFontPath.ToFileName()}";
                    break;
                case (int) Request.AddPlayList:
                    var _midiFilePath = getActualPathBy(data);
                    if (!(_midiFilePath.Contains(".MID") || _midiFilePath.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {_midiFilePath}");
                    playList.Add(_midiFilePath); // add to playlist
                    Env.MidiFileDir = _midiFilePath.ToDirectoryName();
                    break;
                default:
                    break;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        void requestPermissions() {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int) Permission.Granted) {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 0);
            }
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int) Permission.Granted) {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 0);
            }
        }

        /// <summary>
        /// initialize the component.
        /// </summary>
        void initializeComponent() {
            Button _loadSoundFontButton = FindViewById<Button>(Resource.Id.loadSoundFontButton);
            _loadSoundFontButton.Click += onLoadSoundFontButton_Click;

            Button _loadMidiButton = FindViewById<Button>(Resource.Id.loadMidiButton);
            _loadMidiButton.Click += onLoadMidiButton_Click;

            Button _startButton = FindViewById<Button>(Resource.Id.startButton);
            _startButton.Click += onStartButton_Click;

            Button _stopButton = FindViewById<Button>(Resource.Id.stopButton);
            _stopButton.Click += onStopButton_Click;

            Button _addPlaylistButton = FindViewById<Button>(Resource.Id.addPlaylistButton);
            _addPlaylistButton.Click += onAddPlaylistButton_Click;

            Button _deletePlaylistButton = FindViewById<Button>(Resource.Id.deletePlaylistButton);
            _deletePlaylistButton.Click += onDeletePlaylistButton_Click;
        }

        void onLoadSoundFontButton_Click(object sender, EventArgs e) {
            Log.Info("loadSoundFontButton clicked.");
            try {
                if (Synth.Playing) {
                    stopSong();
                }
                callIntent(Env.SoundFontDir, (int) Request.SoundFont);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onLoadMidiButton_Click(object sender, EventArgs e) {
            Log.Info("loadMidiButton clicked.");
            try {
                if (Synth.Playing) {
                    stopSong();
                }
                callIntent(Env.MidiFileDir, (int) Request.MidiFile);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onStartButton_Click(object sender, EventArgs e) {
            Log.Info("startButton clicked.");
            try {
                if (!midiFilePath.HasValue()) {
                    return;
                }
                playSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onStopButton_Click(object sender, EventArgs e) {
            Log.Info("stopButton clicked.");
            try {
                stopSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onAddPlaylistButton_Click(object sender, EventArgs e) {
            Log.Info("addPlaylistButton clicked.");
            try {
                callIntent(Env.MidiFileDir, (int) Request.AddPlayList);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onDeletePlaylistButton_Click(object sender, EventArgs e) {
            Log.Info("deletePlaylistButton clicked.");
            try {
                playList.Clear();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void callIntent(string targetDir, int requestCode) {
            var _intent = new Intent(Intent.ActionOpenDocument);
            var _uri = Android.Net.Uri.Parse($"content://com.android.externalstorage.documents/document/primary%3A{targetDir}");
            _intent.SetData(_uri);
            _intent.SetType("*/*");
            _intent.PutExtra("android.provider.extra.INITIAL_URI", _uri);
            _intent.PutExtra("android.content.extra.SHOW_ADVANCED", true);
            _intent.AddCategory(Intent.CategoryOpenable);
            StartActivityForResult(_intent, requestCode);
        }

        static string getActualPathBy(Intent data) {
            var _uri = data.Data;
            string _docId = DocumentsContract.GetDocumentId(_uri);
            char[] _charArray = { ':' };
            string[] _stringArray = _docId.Split(_charArray);
            string _type = _stringArray[0]; // primary
            string _path = "";
            if ("primary".Equals(_type, StringComparison.OrdinalIgnoreCase)) {
                _path = Android.OS.Environment.ExternalStorageDirectory + "/" + _stringArray[1];
            }
            return _path;
        }

        async void playSong() {
            try {
                await Task.Run(() => {
                    if (!playList.Ready) {
                        Synth.MidiFilePath = midiFilePath;
                        Synth.Start();
                    } else {
                        Synth.MidiFilePath = playList.Next;
                        Synth.Start();
                    }
                });
                logMemoryInto();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        async void stopSong() {
            try {
                await Task.Run(() => Synth.Stop());
                logMemoryInto();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        static void logMemoryInto() {
            var _maxMemory = Java.Lang.Runtime.GetRuntime().MaxMemory();
            var _freeMemory = Java.Lang.Runtime.GetRuntime().FreeMemory();
            var _totalMemory = Java.Lang.Runtime.GetRuntime().TotalMemory();
            Log.Info($"maxMemory: {_maxMemory.ToMegabytes()}MB");
            Log.Info($"freeMemory: {_freeMemory.ToMegabytes()}MB");
            Log.Info($"totalMemory: {_totalMemory.ToMegabytes()}MB");
            // TODO: Mono runtime.
        }
    }
}
