
using Android.Support.V7.App;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiPlayer.Droid {
    /// <summary>
    ///  partial class for initialize the component
    /// </summary>
    public partial class MainActivity : AppCompatActivity {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        Button _buttonloadSoundFont;
        Button _buttonloadMidiFile;
        Button _buttonStart;
        Button _buttonStop;
        Button _buttonAddPlaylist;
        Button _buttonDeletePlaylist;
        Button _buttonSendSynth;
        TextView _textViewNo;
        TextView _textViewChannel;
        NumberPicker _numberPickerProg;
        NumberPicker _numberPickerPan;
        NumberPicker _numberPickerVol;
        CheckBox _checkBoxMute;
        ListView _titleListView;
        ListView _itemListView;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        /// <summary>
        /// initialize the component.
        /// </summary>
        void initializeComponent() {

            /// <summary>
            /// buttonLoadSoundFont
            /// </summary>
            _buttonloadSoundFont = FindViewById<Button>(Resource.Id.button_load_soundfont);
            _buttonloadSoundFont.Click += (object sender, EventArgs e) => {
                Log.Info("buttonLoadSoundFont clicked.");
                try {
                    if (Synth.Playing) {
                        stopSong();
                    }
                    callIntent(Env.SoundFontDirForIntent, (int) Request.SoundFont);
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// buttonLoadMidiFile
            /// </summary>
            _buttonloadMidiFile = FindViewById<Button>(Resource.Id.button_load_midifile);
            _buttonloadMidiFile.Click += (object sender, EventArgs e) => {
                Log.Info("buttonLoadMidiFile clicked.");
                try {
                    if (Synth.Playing) {
                        stopSong();
                    }
                    callIntent(Env.MidiFileDirForIntent, (int) Request.MidiFile);
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// buttonStart
            /// </summary>
            _buttonStart = FindViewById<Button>(Resource.Id.button_start);
            _buttonStart.Click += (object sender, EventArgs e) => {
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
            };

            /// <summary>
            /// buttonStop
            /// </summary>
            _buttonStop = FindViewById<Button>(Resource.Id.button_stop);
            _buttonStop.Click += (object sender, EventArgs e) => {
                Log.Info("buttonStop clicked.");
                try {
                    stopSong();
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// buttonAddPlaylist
            /// </summary>
            _buttonAddPlaylist = FindViewById<Button>(Resource.Id.button_add_playlist);
            _buttonAddPlaylist.Click += (object sender, EventArgs e) => {
                Log.Info("buttonAddPlaylist clicked.");
                try {
                    callIntent(Env.MidiFileDir, (int) Request.AddPlayList);
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// buttonDeletePlaylist
            /// </summary>
            _buttonDeletePlaylist = FindViewById<Button>(Resource.Id.button_delete_playlist);
            _buttonDeletePlaylist.Click += (object sender, EventArgs e) => {
                Log.Info("buttonDeletePlaylist clicked.");
                try {
                    _playList.Clear();
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// buttonSendSynth
            /// </summary>
            _buttonSendSynth = FindViewById<Button>(Resource.Id.button_send_synth);
            _buttonSendSynth.Click += (object sender, EventArgs e) => {
                Log.Info("buttonSendSynth clicked.");
                try {
                    Mixer.Fader fader = Mixer.GetCurrent();
                    Log.Debug($"track index {fader.Index}: send a data to MIDI {fader.Channel} channel.");
                    Log.Debug($"prog: {fader.Program} pan: {fader.Pan} vol: {fader.Volume}.");
                    Data data = new() {
                        Channel = fader.Channel,
                        Program = fader.Program,
                        Pan = fader.Pan,
                        Volume = fader.Volume,
                        Mute = !fader.Sounds
                    };
                    EventQueue.Enqueue(fader.Index, data);
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// textViewNo, textViewChannel
            /// </summary>
            _textViewNo = FindViewById<TextView>(Resource.Id.text_view_no);
            _textViewChannel = FindViewById<TextView>(Resource.Id.text_view_channel);

            /// <summary>
            /// numberPickerProg
            /// </summary>
            _numberPickerProg = FindViewById<NumberPicker>(Resource.Id.number_picker_prog);
            _numberPickerProg.MinValue = 1;
            _numberPickerProg.MaxValue = 128;
            _numberPickerProg.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                Log.Debug($"_numberPickerProg.Value: {_numberPickerProg.Value}");
                fader.ProgramAsOneBased = _numberPickerProg.Value;
            };

            /// <summary>
            /// numberPickerPan
            /// </summary>
            _numberPickerPan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan);
            _numberPickerPan.MinValue = 1;
            _numberPickerPan.MaxValue = 128;
            _numberPickerPan.Value = 65;
            _numberPickerPan.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                Log.Debug($"_numberPickerPan.Value: {_numberPickerPan.Value}");
                fader.Pan = _numberPickerPan.Value;
            };

            /// <summary>
            /// numberPickerVol
            /// </summary>
            _numberPickerVol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol);
            _numberPickerVol.MinValue = 1;
            _numberPickerVol.MaxValue = 128;
            _numberPickerVol.Value = 104;
            _numberPickerVol.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                Log.Debug($"_numberPickerVol.Value: {_numberPickerVol.Value}");
                fader.Volume = _numberPickerVol.Value;
            };

            /// <summary>
            /// checkBoxMute
            /// </summary>
            _checkBoxMute = FindViewById<CheckBox>(Resource.Id.check_box_mute);
            _checkBoxMute.CheckedChange += (object sender, CheckBox.CheckedChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                fader.Sounds = !_checkBoxMute.Checked;
            };

            /// <summary>
            /// titleListView
            /// </summary>
            var titleList = new List<ListTitle>();
            titleList.Add(new ListTitle() { Name = "Name", Instrument = "Voice", Channel = "Ch" });
            _titleListView = FindViewById<ListView>(Resource.Id.list_view_title);
            _titleListView.Adapter = new ListTitleAdapter(this, 0, titleList);

            /// <summary>
            /// itemListView
            /// </summary>
            Enumerable.Range(MIDI_TRACK_BASE, MIDI_TRACK_COUNT).ToList().ForEach(x => {
                _itemList.Add(new ListItem() { Name = "------", Instrument = "------", Channel = "---" });
            });
            _itemListView = FindViewById<ListView>(Resource.Id.list_view_item);
            var listItemAdapter = new ListItemAdapter(this, 0, _itemList);
            _itemListView.Adapter = listItemAdapter;
            _itemListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                Log.Debug($"setected: {e.Position}");
                Mixer.Current = e.Position;
            };
        }
    }
}
