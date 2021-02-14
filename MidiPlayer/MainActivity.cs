﻿
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using System;
using System.Linq;
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

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        const string soundFontPath = "/storage/emulated/0/Music/SoundFont/GeneralUser GS v1.47.sf2";

        string filePath;

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

        async Task<bool> loadTarget() {
            try {
                FileData _fileData = await CrossFilePicker.Current.PickFile();
                if (_fileData is null) {
                    return false; // user canceled file picking
                }
                filePath = _fileData.FilePath;
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
                return 1;
            } catch (Exception ex) {
                Log.Error(ex.Message);
                return 0;
            }
        }

        async Task<int> stopSong() {
            try {
                await Task.Run(() => Synth.Stop());
                return 1;
            } catch (Exception ex) {
                Log.Error(ex.Message);
                return 0;
            }
        }

        async void onOpenButton_Click(object sender, EventArgs e) {
            Log.Info("openButton clicked.");
            try {
                if (Synth.Playing) {
                    await stopSong();
                }
                var _result = await loadTarget();
                Title = $"MidiPlayer: {filePath.Split("/").ToList().Last()}";
                Synth.FilePath = filePath;
                Synth.Init();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        async void onStartButton_Click(object sender, EventArgs e) {
            Log.Info("startButton clicked.");
            try {
                if (!filePath.HasValue()) {
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
        /// コンポーネントを初期化します
        /// </summary>
        void initializeComponent() {
            Button _openButton = FindViewById<Button>(Resource.Id.openButton);
            _openButton.Click += onOpenButton_Click;

            Button _startButton = FindViewById<Button>(Resource.Id.startButton);
            _startButton.Click += onStartButton_Click;

            Button _stopButton = FindViewById<Button>(Resource.Id.stopButton);
            _stopButton.Click += onStopButton_Click;
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

            public static string FilePath {
                get; set;
            }

            public static bool Playing {
                get => ready;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // public Methods [verb]

            public static void Init() {
                try {
                    setting = Fluidsynth.new_fluid_settings();
                    synth = Fluidsynth.new_fluid_synth(setting);
                    player = Fluidsynth.new_fluid_player(synth);
                    if (Fluidsynth.fluid_is_soundfont(soundFontPath) != 1) {
                        Log.Error("not a sound font.");
                        return;
                    }
                    Fluidsynth.fluid_player_set_playback_callback(player, event_callback, synth);
                    int _sfont_id = Fluidsynth.fluid_synth_sfload(synth, soundFontPath, true);
                    if (_sfont_id == Fluidsynth.FLUID_FAILED) {
                        Log.Error("failed to load the sound font.");
                        return;
                    } else {
                        Log.Info("loaded the sound font.");
                    }
                    if (Fluidsynth.fluid_is_midifile(FilePath) != 1) {
                        Log.Error("not a midi file.");
                        return;
                    }
                    int _result = Fluidsynth.fluid_player_add(player, FilePath);
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
    }

    /// <summary>
    /// 共通拡張メソッド
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// 文字列が null または 空文字("")ではない場合 TRUE を返します
        /// </summary>
        public static bool HasValue(this string source) {
            return !(source is null || source.Equals(""));
        }

        /// <summary>
        /// IntPtr が IntPtr.Zero の場合 TRUE を返します
        /// </summary>
        public static bool IsZero(this IntPtr source) {
            return source == IntPtr.Zero;
        }
    }
}