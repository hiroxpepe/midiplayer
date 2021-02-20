
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
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

using NativeFuncs;
using void_ptr = System.IntPtr;
using fluid_settings_t = System.IntPtr;
using fluid_synth_t = System.IntPtr;
using fluid_audio_driver_t = System.IntPtr;
using fluid_player_t = System.IntPtr;
using fluid_midi_event_t = System.IntPtr;

namespace MidiPlayer {

    [Activity(Label = "@string/app_name", Theme = "@style/Base.Theme.MaterialComponents.Light.DarkActionBar.Bridge", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : AppCompatActivity {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        string soundFontPath = "undefined";

        string midiFilePath = "undefined";

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainActivity() {
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
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            try {
                Synth.Stop();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
            switch (requestCode) {
                case (int) BaseDir.SoundFont:
                    soundFontPath = getActualPathBy(data);
                    if (!(soundFontPath.Contains(".SF2") || soundFontPath.Contains(".sf2"))) {
                        Log.Warn("not a sound font.");
                        break;
                    }
                    Log.Info($"selected: {soundFontPath}");
                    Synth.SoundFontPath = soundFontPath;
                    Env.SoundFontDir = soundFontPath.ToDirectoryName();
                    break;
                case (int) BaseDir.MidiFile:
                    midiFilePath = getActualPathBy(data);
                    if (!(midiFilePath.Contains(".MID") || midiFilePath.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {midiFilePath}");
                    Synth.MidiFilePath = midiFilePath;
                    Env.MidiFileDir = midiFilePath.ToDirectoryName();
                    break;
                default:
                    break;
            }
            Title = $"MidiPlayer: {midiFilePath.ToFileName()} {soundFontPath.ToFileName()}";
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

        async Task<int> playSong() {
            try {
                await Task.Run(() => Synth.Start());
                logMemoryInto();
                return 1;
            } catch (Exception ex) {
                Log.Error(ex.Message);
                return 0;
            }
        }

        async Task<int> stopSong() {
            try {
                await Task.Run(() => Synth.Stop());
                logMemoryInto();
                return 1;
            } catch (Exception ex) {
                Log.Error(ex.Message);
                return 0;
            }
        }

        async void onLoadSoundFontButton_Click(object sender, EventArgs e) {
            Log.Info("loadSoundFontButton clicked.");
            try {
                if (Synth.Playing) {
                    await stopSong();
                }
                Intent _intent = new Intent(Intent.ActionOpenDocument);
                Android.Net.Uri _uri = Android.Net.Uri.Parse($"content://com.android.externalstorage.documents/document/primary%3A{Env.SoundFontDir}");
                _intent.SetData(_uri);
                _intent.SetType("*/*");
                _intent.PutExtra("android.provider.extra.INITIAL_URI", _uri);
                _intent.PutExtra("android.content.extra.SHOW_ADVANCED", true);
                _intent.AddCategory(Intent.CategoryOpenable);
                StartActivityForResult(_intent, (int) BaseDir.SoundFont);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        async void onLoadMidiButton_Click(object sender, EventArgs e) {
            Log.Info("loadMidiButton clicked.");
            try {
                if (Synth.Playing) {
                    await stopSong();
                }
                Intent _intent = new Intent(Intent.ActionOpenDocument);
                Android.Net.Uri _uri = Android.Net.Uri.Parse($"content://com.android.externalstorage.documents/document/primary%3A{Env.MidiFileDir}");
                _intent.SetData(_uri);
                _intent.SetType("*/*");
                _intent.PutExtra("android.provider.extra.INITIAL_URI", _uri);
                _intent.PutExtra("android.content.extra.SHOW_ADVANCED", true);
                _intent.AddCategory(Intent.CategoryOpenable);
                StartActivityForResult(_intent, (int) BaseDir.MidiFile);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        async void onStartButton_Click(object sender, EventArgs e) {
            Log.Info("startButton clicked.");
            try {
                if (!midiFilePath.HasValue()) {
                    return;
                }
                await playSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        async void onStopButton_Click(object sender, EventArgs e) {
            Log.Info("stopButton clicked.");
            try {
                await stopSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
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

        static void logMemoryInto() {
            var _maxMemory = Java.Lang.Runtime.GetRuntime().MaxMemory();
            var _freeMemory = Java.Lang.Runtime.GetRuntime().FreeMemory();
            var _totalMemory = Java.Lang.Runtime.GetRuntime().TotalMemory();
            Log.Info($"maxMemory: {_maxMemory.ToMegabytes()}MB");
            Log.Info($"freeMemory: {_freeMemory.ToMegabytes()}MB");
            Log.Info($"totalMemory: {_totalMemory.ToMegabytes()}MB");
            // TODO: Mono runtime.
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class Synth {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields

            static fluid_settings_t setting = IntPtr.Zero;

            static fluid_synth_t synth = IntPtr.Zero;

            static fluid_player_t player = IntPtr.Zero;

            static fluid_audio_driver_t adriver = IntPtr.Zero;

            static bool ready = false;

            static int cont = 0;
            static Fluidsynth.handle_midi_event_func_t event_callback = (void_ptr data, fluid_midi_event_t evt) => {
                Log.Info(cont.ToString());
                cont++;
                return Fluidsynth.fluid_synth_handle_midi_event(synth, evt);
            };

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjective] 

            public static string SoundFontPath {
                get; set;
            }

            public static string MidiFilePath {
                get; set;
            }

            public static bool Playing {
                get => ready;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // public Methods [verb]

            public static void Init() {
                try {
                    if (!SoundFontPath.HasValue() || !MidiFilePath.HasValue()) {
                        Log.Warn("no sound font or no midi file.");
                        return;
                    }
                    setting = Fluidsynth.new_fluid_settings();
                    synth = Fluidsynth.new_fluid_synth(setting);
                    player = Fluidsynth.new_fluid_player(synth);
                    Log.Info($"try to load the sound font: {SoundFontPath}");
                    if (Fluidsynth.fluid_is_soundfont(SoundFontPath) != 1) {
                        Log.Error("not a sound font.");
                        return;
                    }
                    Fluidsynth.fluid_player_set_playback_callback(player, event_callback, synth);
                    int _sfont_id = Fluidsynth.fluid_synth_sfload(synth, SoundFontPath, true);
                    if (_sfont_id == Fluidsynth.FLUID_FAILED) {
                        Log.Error("failed to load the sound font.");
                        return;
                    } else {
                        Log.Info($"loaded the sound font: {SoundFontPath}");
                    }
                    Log.Info($"try to load the midi file: {MidiFilePath}");
                    if (Fluidsynth.fluid_is_midifile(MidiFilePath) != 1) {
                        Log.Error("not a midi file.");
                        return;
                    }
                    int _result = Fluidsynth.fluid_player_add(player, MidiFilePath);
                    if (_result == Fluidsynth.FLUID_FAILED) {
                        Log.Error("failed to load the midi file.");
                        return;
                    } else {
                        Log.Info($"loaded the midi file: {MidiFilePath}");
                    }
                    ready = true;
                    Log.Info("init :)");
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            }

            public static void Start() {
                try {
                    if (!ready) {
                        Init();
                    }
                    adriver = Fluidsynth.new_fluid_audio_driver(setting, synth); // start the synthesizer thread
                    Fluidsynth.fluid_player_play(player); // play the midi files, if any
                    Log.Info("start :)");
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            }

            public static void Stop() {
                try {
                    if (!player.IsZero()) {
                        Fluidsynth.fluid_player_stop(player);
                    }
                    final();
                    Log.Info("stop :|");
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // private Methods [verb]

            static void final() {
                try {
                    Fluidsynth.delete_fluid_audio_driver(adriver);
                    Fluidsynth.delete_fluid_player(player);
                    Fluidsynth.delete_fluid_synth(synth);
                    Fluidsynth.delete_fluid_settings(setting);
                    adriver = IntPtr.Zero;
                    player = IntPtr.Zero;
                    synth = IntPtr.Zero;
                    setting = IntPtr.Zero;
                    Log.Info("final :|");
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                } finally {
                    ready = false;
                }
            }
        }

        class Env {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields

            static string soundFontDir = "Music";

            static string midiFileDir = "Music";

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjective] 

            public static string SoundFontDir {
                get => soundFontDir.Replace("/", "%2F");
                set => soundFontDir = value.Replace("/storage/emulated/0/", "");
            }

            public static string MidiFileDir {
                get => midiFileDir.Replace("/", "%2F");
                set => midiFileDir = value.Replace("/storage/emulated/0/", "");
            }
        }
    }

    public enum BaseDir {
        SoundFont = 128,
        MidiFile = 256,
    }

    /// <summary>
    /// common extension method
    /// </summary>
    public static class Extensions {

        /// <summary>
        /// to directory name
        /// </summary>
        public static string ToDirectoryName(this string source) {
            return Path.GetDirectoryName(source);
        }

        /// <summary>
        /// to file name
        /// </summary>
        public static string ToFileName(this string source) {
            return Path.GetFileName(source);
        }

        /// <summary>
        /// bytes to megabytes.
        /// </summary>
        public static long ToMegabytes(this long source) {
            return source / (1024 * 1024);
        }

        /// <summary>
        /// returns TRUE if the string is not null or an empty string "".
        /// </summary>
        public static bool HasValue(this string source) {
            return !(source is null || source.Equals(""));
        }

        /// <summary>
        /// returns TRUE if IntPtr is IntPtr.Zero.
        /// </summary>
        public static bool IsZero(this IntPtr source) {
            return source == IntPtr.Zero;
        }
    }
}
