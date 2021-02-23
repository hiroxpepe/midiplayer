
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

            int _count = 0;
            Synth.OnMessage += (IntPtr data, IntPtr evt) => {
                //Log.Info($"OnMessage count: {_count}");
                _count++;
                return Synth.HandleEvent(data, evt);
            };

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
            var _loadSoundFontButton = FindViewById<Button>(Resource.Id.load_soundfont_button);
            _loadSoundFontButton.Click += onLoadSoundFontButton_Click;

            var _loadMidiFileButton = FindViewById<Button>(Resource.Id.load_midifile_button);
            _loadMidiFileButton.Click += onLoadMidiFileButton_Click;

            var _startButton = FindViewById<Button>(Resource.Id.start_button);
            _startButton.Click += onStartButton_Click;

            var _stopButton = FindViewById<Button>(Resource.Id.stop_button);
            _stopButton.Click += onStopButton_Click;

            var _addPlaylistButton = FindViewById<Button>(Resource.Id.add_playlist_button);
            _addPlaylistButton.Click += onAddPlaylistButton_Click;

            var _deletePlaylistButton = FindViewById<Button>(Resource.Id.delete_playlist_button);
            _deletePlaylistButton.Click += onDeletePlaylistButton_Click;

            // fader1
            var _progNumberPicker_1 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_1);
            _progNumberPicker_1.MinValue = 1;
            _progNumberPicker_1.MaxValue = 128;
            var _panNumberPicker_1 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_1);
            _panNumberPicker_1.MinValue = 1;
            _panNumberPicker_1.MaxValue = 128;
            _panNumberPicker_1.Value = 65;
            var _volNumberPicker_1 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_1);
            _volNumberPicker_1.MinValue = 1;
            _volNumberPicker_1.MaxValue = 128;
            _volNumberPicker_1.Value = 104;
            var _sendSynthButton_1 = FindViewById<Button>(Resource.Id.send_synth_button_1);
            _sendSynthButton_1.Click += onSendSynthButton_1_Click;

            // fader2
            var _progNumberPicker_2 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_2);
            _progNumberPicker_2.MinValue = 1;
            _progNumberPicker_2.MaxValue = 128;
            var _panNumberPicker_2 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_2);
            _panNumberPicker_2.MinValue = 1;
            _panNumberPicker_2.MaxValue = 128;
            _panNumberPicker_2.Value = 65;
            var _volNumberPicker_2 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_2);
            _volNumberPicker_2.MinValue = 1;
            _volNumberPicker_2.MaxValue = 128;
            _volNumberPicker_2.Value = 104;
            var _sendSynthButton_2 = FindViewById<Button>(Resource.Id.send_synth_button_2);
            _sendSynthButton_2.Click += onSendSynthButton_2_Click;

            // fader3
            var _progNumberPicker_3 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_3);
            _progNumberPicker_3.MinValue = 1;
            _progNumberPicker_3.MaxValue = 128;
            var _panNumberPicker_3 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_3);
            _panNumberPicker_3.MinValue = 1;
            _panNumberPicker_3.MaxValue = 128;
            _panNumberPicker_3.Value = 65;
            var _volNumberPicker_3 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_3);
            _volNumberPicker_3.MinValue = 1;
            _volNumberPicker_3.MaxValue = 128;
            _volNumberPicker_3.Value = 104;
            var _sendSynthButton_3 = FindViewById<Button>(Resource.Id.send_synth_button_3);
            _sendSynthButton_3.Click += onSendSynthButton_3_Click;

            // fader4
            var _progNumberPicker_4 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_4);
            _progNumberPicker_4.MinValue = 1;
            _progNumberPicker_4.MaxValue = 128;
            var _panNumberPicker_4 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_4);
            _panNumberPicker_4.MinValue = 1;
            _panNumberPicker_4.MaxValue = 128;
            _panNumberPicker_4.Value = 65;
            var _volNumberPicker_4 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_4);
            _volNumberPicker_4.MinValue = 1;
            _volNumberPicker_4.MaxValue = 128;
            _volNumberPicker_4.Value = 104;
            var _sendSynthButton_4 = FindViewById<Button>(Resource.Id.send_synth_button_4);
            _sendSynthButton_4.Click += onSendSynthButton_4_Click;

            // fader5
            var _progNumberPicker_5 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_5);
            _progNumberPicker_5.MinValue = 1;
            _progNumberPicker_5.MaxValue = 128;
            var _panNumberPicker_5 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_5);
            _panNumberPicker_5.MinValue = 1;
            _panNumberPicker_5.MaxValue = 128;
            _panNumberPicker_5.Value = 65;
            var _volNumberPicker_5 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_5);
            _volNumberPicker_5.MinValue = 1;
            _volNumberPicker_5.MaxValue = 128;
            _volNumberPicker_5.Value = 104;
            var _sendSynthButton_5 = FindViewById<Button>(Resource.Id.send_synth_button_5);
            _sendSynthButton_5.Click += onSendSynthButton_5_Click;

            // fader6
            var _progNumberPicker_6 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_6);
            _progNumberPicker_6.MinValue = 1;
            _progNumberPicker_6.MaxValue = 128;
            var _panNumberPicker_6 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_6);
            _panNumberPicker_6.MinValue = 1;
            _panNumberPicker_6.MaxValue = 128;
            _panNumberPicker_6.Value = 65;
            var _volNumberPicker_6 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_6);
            _volNumberPicker_6.MinValue = 1;
            _volNumberPicker_6.MaxValue = 128;
            _volNumberPicker_6.Value = 104;
            var _sendSynthButton_6 = FindViewById<Button>(Resource.Id.send_synth_button_6);
            _sendSynthButton_6.Click += onSendSynthButton_6_Click;

            // fader7
            var _progNumberPicker_7 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_7);
            _progNumberPicker_7.MinValue = 1;
            _progNumberPicker_7.MaxValue = 128;
            var _panNumberPicker_7 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_7);
            _panNumberPicker_7.MinValue = 1;
            _panNumberPicker_7.MaxValue = 128;
            _panNumberPicker_7.Value = 65;
            var _volNumberPicker_7 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_7);
            _volNumberPicker_7.MinValue = 1;
            _volNumberPicker_7.MaxValue = 128;
            _volNumberPicker_7.Value = 104;
            var _sendSynthButton_7 = FindViewById<Button>(Resource.Id.send_synth_button_7);
            _sendSynthButton_7.Click += onSendSynthButton_7_Click;

            // fader8
            var _progNumberPicker_8 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_8);
            _progNumberPicker_8.MinValue = 1;
            _progNumberPicker_8.MaxValue = 128;
            var _panNumberPicker_8 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_8);
            _panNumberPicker_8.MinValue = 1;
            _panNumberPicker_8.MaxValue = 128;
            _panNumberPicker_8.Value = 65;
            var _volNumberPicker_8 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_8);
            _volNumberPicker_8.MinValue = 1;
            _volNumberPicker_8.MaxValue = 128;
            _volNumberPicker_8.Value = 104;
            var _sendSynthButton_8 = FindViewById<Button>(Resource.Id.send_synth_button_8);
            _sendSynthButton_8.Click += onSendSynthButton_8_Click;

            // fader9
            var _progNumberPicker_9 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_9);
            _progNumberPicker_9.MinValue = 1;
            _progNumberPicker_9.MaxValue = 128;
            var _panNumberPicker_9 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_9);
            _panNumberPicker_9.MinValue = 1;
            _panNumberPicker_9.MaxValue = 128;
            _panNumberPicker_9.Value = 65;
            var _volNumberPicker_9 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_9);
            _volNumberPicker_9.MinValue = 1;
            _volNumberPicker_9.MaxValue = 128;
            _volNumberPicker_9.Value = 104;
            var _sendSynthButton_9 = FindViewById<Button>(Resource.Id.send_synth_button_9);
            _sendSynthButton_9.Click += onSendSynthButton_9_Click;

            // fader10
            var _progNumberPicker_10 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_10);
            _progNumberPicker_10.MinValue = 1;
            _progNumberPicker_10.MaxValue = 128;
            var _panNumberPicker_10 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_10);
            _panNumberPicker_10.MinValue = 1;
            _panNumberPicker_10.MaxValue = 128;
            _panNumberPicker_10.Value = 65;
            var _volNumberPicker_10 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_10);
            _volNumberPicker_10.MinValue = 1;
            _volNumberPicker_10.MaxValue = 128;
            _volNumberPicker_10.Value = 104;
            var _sendSynthButton_10 = FindViewById<Button>(Resource.Id.send_synth_button_10);
            _sendSynthButton_10.Click += onSendSynthButton_10_Click;

            // fader11
            var _progNumberPicker_11 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_11);
            _progNumberPicker_11.MinValue = 1;
            _progNumberPicker_11.MaxValue = 128;
            var _panNumberPicker_11 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_11);
            _panNumberPicker_11.MinValue = 1;
            _panNumberPicker_11.MaxValue = 128;
            _panNumberPicker_11.Value = 65;
            var _volNumberPicker_11 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_11);
            _volNumberPicker_11.MinValue = 1;
            _volNumberPicker_11.MaxValue = 128;
            _volNumberPicker_11.Value = 104;
            var _sendSynthButton_11 = FindViewById<Button>(Resource.Id.send_synth_button_11);
            _sendSynthButton_11.Click += onSendSynthButton_11_Click;

            // fader12
            var _progNumberPicker_12 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_12);
            _progNumberPicker_12.MinValue = 1;
            _progNumberPicker_12.MaxValue = 128;
            var _panNumberPicker_12 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_12);
            _panNumberPicker_12.MinValue = 1;
            _panNumberPicker_12.MaxValue = 128;
            _panNumberPicker_12.Value = 65;
            var _volNumberPicker_12 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_12);
            _volNumberPicker_12.MinValue = 1;
            _volNumberPicker_12.MaxValue = 128;
            _volNumberPicker_12.Value = 104;
            var _sendSynthButton_12 = FindViewById<Button>(Resource.Id.send_synth_button_12);
            _sendSynthButton_12.Click += onSendSynthButton_12_Click;

            // fader13
            var _progNumberPicker_13 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_13);
            _progNumberPicker_13.MinValue = 1;
            _progNumberPicker_13.MaxValue = 128;
            var _panNumberPicker_13 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_13);
            _panNumberPicker_13.MinValue = 1;
            _panNumberPicker_13.MaxValue = 128;
            _panNumberPicker_13.Value = 65;
            var _volNumberPicker_13 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_13);
            _volNumberPicker_13.MinValue = 1;
            _volNumberPicker_13.MaxValue = 128;
            _volNumberPicker_13.Value = 104;
            var _sendSynthButton_13 = FindViewById<Button>(Resource.Id.send_synth_button_13);
            _sendSynthButton_13.Click += onSendSynthButton_13_Click;

            // fader14
            var _progNumberPicker_14 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_14);
            _progNumberPicker_14.MinValue = 1;
            _progNumberPicker_14.MaxValue = 128;
            var _panNumberPicker_14 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_14);
            _panNumberPicker_14.MinValue = 1;
            _panNumberPicker_14.MaxValue = 128;
            _panNumberPicker_14.Value = 65;
            var _volNumberPicker_14 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_14);
            _volNumberPicker_14.MinValue = 1;
            _volNumberPicker_14.MaxValue = 128;
            _volNumberPicker_14.Value = 104;
            var _sendSynthButton_14 = FindViewById<Button>(Resource.Id.send_synth_button_14);
            _sendSynthButton_14.Click += onSendSynthButton_14_Click;

            // fader15
            var _progNumberPicker_15 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_15);
            _progNumberPicker_15.MinValue = 1;
            _progNumberPicker_15.MaxValue = 128;
            var _panNumberPicker_15 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_15);
            _panNumberPicker_15.MinValue = 1;
            _panNumberPicker_15.MaxValue = 128;
            _panNumberPicker_15.Value = 65;
            var _volNumberPicker_15 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_15);
            _volNumberPicker_15.MinValue = 1;
            _volNumberPicker_15.MaxValue = 128;
            _volNumberPicker_15.Value = 104;
            var _sendSynthButton_15 = FindViewById<Button>(Resource.Id.send_synth_button_15);
            _sendSynthButton_15.Click += onSendSynthButton_15_Click;

            // fader16
            var _progNumberPicker_16 = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_16);
            _progNumberPicker_16.MinValue = 1;
            _progNumberPicker_16.MaxValue = 128;
            var _panNumberPicker_16 = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_16);
            _panNumberPicker_16.MinValue = 1;
            _panNumberPicker_16.MaxValue = 128;
            _panNumberPicker_16.Value = 65;
            var _volNumberPicker_16 = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_16);
            _volNumberPicker_16.MinValue = 1;
            _volNumberPicker_16.MaxValue = 128;
            _volNumberPicker_16.Value = 104;
            var _sendSynthButton_16 = FindViewById<Button>(Resource.Id.send_synth_button_16);
            _sendSynthButton_16.Click += onSendSynthButton_16_Click;
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

        void onLoadMidiFileButton_Click(object sender, EventArgs e) {
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

        void onSendSynthButton_1_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_1 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_1).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_1).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_1).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch1, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_2_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_2 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_2).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_2).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_2).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch2, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_3_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_3 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_3).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_3).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_3).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch3, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_4_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_4 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_4).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_4).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_4).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch4, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_5_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_5 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_5).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_5).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_5).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch5, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_6_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_6 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_6).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_6).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_6).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch6, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_7_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_7 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_7).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_7).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_7).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch7, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_8_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_8 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_8).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_8).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_8).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch8, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_9_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_9 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_9).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_9).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_9).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch9, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_10_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_10 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_10).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_10).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_10).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch10, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_11_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_11 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_11).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_11).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_11).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch11, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_12_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_12 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_12).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_12).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_12).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch12, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_13_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_13 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_13).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_13).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_13).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch13, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_14_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_14 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_14).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_14).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_14).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch14, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_15_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_15 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_15).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_15).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_15).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch15, _data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void onSendSynthButton_16_Click(object sender, EventArgs e) {
            Log.Info("onSendSynthButton_16 clicked.");
            try {
                var _data = new Data() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.prog_number_picker_16).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.pan_number_picker_16).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.vol_number_picker_16).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch16, _data);
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
