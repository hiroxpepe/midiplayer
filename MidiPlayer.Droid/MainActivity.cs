﻿
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

    [Activity(Label = "@string/app_name", Theme = "@style/Base.Theme.MaterialComponents.Light.DarkActionBar.Bridge", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            requestPermissions();
            Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            initializeComponent();
            Conf.Load();

            Synth.Playbacking += (IntPtr data, IntPtr evt) => {
                return Synth.HandleEvent(data, evt);
            };

            Synth.Started += () => {
                Log.Info("Started called.");
                MainThread.BeginInvokeOnMainThread(() => {
                    Title = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                });
            };

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
            Conf.Value.PlayList = _playList.List;
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
                    _soundFontPath = getActualPathBy(data);
                    if (!(_soundFontPath.Contains(".SF2") || _soundFontPath.Contains(".sf2"))) {
                        Log.Warn("not a sound font.");
                        break;
                    }
                    Log.Info($"selected: {_soundFontPath}");
                    Synth.SoundFontPath = _soundFontPath;
                    Env.SoundFontDir = _soundFontPath.ToDirectoryName();
                    Title = $"MidiPlayer: {this._midiFilePath.ToFileName()} {_soundFontPath.ToFileName()}";
                    break;
                case (int) Request.MidiFile:
                    this._midiFilePath = getActualPathBy(data);
                    if (!(this._midiFilePath.Contains(".MID") || this._midiFilePath.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {this._midiFilePath}");
                    Synth.MidiFilePath = this._midiFilePath;
                    Env.MidiFileDir = this._midiFilePath.ToDirectoryName();
                    Title = $"MidiPlayer: {this._midiFilePath.ToFileName()} {_soundFontPath.ToFileName()}";
                    break;
                case (int) Request.AddPlayList:
                    var midiFilePath = getActualPathBy(data);
                    if (!(midiFilePath.Contains(".MID") || midiFilePath.Contains(".mid"))) {
                        Log.Warn("not a midi file.");
                        break;
                    }
                    Log.Info($"selected: {midiFilePath}");
                    _playList.Add(midiFilePath); // add to playlist
                    Env.MidiFileDir = midiFilePath.ToDirectoryName();
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
                callIntent(Env.SoundFontDir, (int) Request.SoundFont);
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
                callIntent(Env.MidiFileDir, (int) Request.MidiFile);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonStart_Click(object sender, EventArgs e) {
            Log.Info("buttonStart clicked.");
            try {
                if (!_midiFilePath.HasValue()) {
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

        void buttonSendSynth_2_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_2 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_2).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_2).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_2).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch2, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_3_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_3 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_3).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_3).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_3).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch3, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_4_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_4 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_4).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_4).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_4).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch4, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_5_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_5 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_5).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_5).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_5).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch5, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_6_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_6 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_6).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_6).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_6).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch6, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_7_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_7 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_7).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_7).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_7).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch7, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_8_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_8 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_8).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_8).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_8).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch8, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_9_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_9 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_9).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_9).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_9).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch9, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_10_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_10 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_10).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_10).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_10).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch10, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_11_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_11 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_11).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_11).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_11).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch11, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_12_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_12 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_12).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_12).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_12).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch12, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_13_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_13 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_13).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_13).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_13).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch13, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_14_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_14 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_14).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_14).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_14).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch14, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_15_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_15 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_15).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_15).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_15).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch15, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonSendSynth_16_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth_16 clicked.");
            try {
                var data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_16).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_16).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_16).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch16, data);
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
