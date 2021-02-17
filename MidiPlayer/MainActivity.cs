
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

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

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        async Task<bool> loadSoundFont() {
            try {
                FileData _fileData = await CrossFilePicker.Current.PickFile();
                if (_fileData is null) {
                    return false; // user canceled file picking
                }
                soundFontPath = _fileData.FilePath;
                Env.SoundFontDir = soundFontPath.ToDirectoryName();
                var _fileName = _fileData.FileName;
                if (!(_fileName.Contains(".SF2") || _fileName.Contains(".sf2"))) {
                    Log.Warn("not a sound font.");
                    return false;
                }
                Log.Info($"select the sound font: {_fileName}");
                return true;
            } catch (Exception ex) {
                Log.Error(ex.Message);
                return false;
            }
        }

        async Task<bool> loadMidiFile() {
            try {
                FileData _fileData = await CrossFilePicker.Current.PickFile(new string[] { "audio/midi" });
                if (_fileData is null) {
                    return false; // user canceled file picking
                }
                midiFilePath = _fileData.FilePath;
                Env.MidiFileDir = midiFilePath.ToDirectoryName();
                var _fileName = _fileData.FileName;
                if (!(_fileName.Contains(".MID") || _fileName.Contains(".mid"))) {
                    Log.Warn("not a midi file.");
                    return false;
                }
                Log.Info($"select the midi file: {_fileName}");
                return true;
            } catch (Exception ex) {
                Log.Error(ex.Message);
                return false;
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
                var _result = await loadSoundFont();
                Title = $"MidiPlayer: {midiFilePath.ToFileName()} {soundFontPath.ToFileName()}";
                Synth.SoundFontPath = soundFontPath;
                Synth.Init();
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
                var _result = await loadMidiFile();
                Title = $"MidiPlayer: {midiFilePath.ToFileName()} {soundFontPath.ToFileName()}";
                Synth.MidiFilePath = midiFilePath;
                Synth.Init();
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

        void logMemoryInto() {
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
            static Fluidsynth.handle_midi_event_func_t event_callback = (void_ptr data, fluid_midi_event_t midi_event) => {
                Log.Info(cont.ToString());
                cont++;
                return Fluidsynth.fluid_synth_handle_midi_event(synth, midi_event);
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
                        Log.Info("loaded the sound font.");
                    }
                    if (Fluidsynth.fluid_is_midifile(MidiFilePath) != 1) {
                        Log.Error("not a midi file.");
                        return;
                    }
                    int _result = Fluidsynth.fluid_player_add(player, MidiFilePath);
                    if (_result == Fluidsynth.FLUID_FAILED) {
                        Log.Error("failed to load the midi file.");
                        return;
                    } else {
                        Log.Info("loaded the midi file.");
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

            static string soundFontDir = "";

            static string midiFileDir = "";

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjective] 

            public static string SoundFontDir {
                get => soundFontDir;
                set => soundFontDir = value;
            }

            public static string MidiFileDir {
                get => midiFileDir;
                set => midiFileDir = value;
            }
        }
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