
using Android.Support.V7.App;
using Android.Widget;

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
            _buttonloadSoundFont = FindViewById<Button>(Resource.Id.button_load_soundfont);
            _buttonloadSoundFont.Click += buttonLoadSoundFont_Click;

            _buttonloadMidiFile = FindViewById<Button>(Resource.Id.button_load_midifile);
            _buttonloadMidiFile.Click += buttonLoadMidiFile_Click;

            _buttonStart = FindViewById<Button>(Resource.Id.button_start);
            _buttonStart.Click += buttonStart_Click;

            _buttonStop = FindViewById<Button>(Resource.Id.button_stop);
            _buttonStop.Click += buttonStop_Click;

            _buttonAddPlaylist = FindViewById<Button>(Resource.Id.button_add_playlist);
            _buttonAddPlaylist.Click += buttonAddPlaylist_Click;

            _buttonDeletePlaylist = FindViewById<Button>(Resource.Id.button_delete_playlist);
            _buttonDeletePlaylist.Click += buttonDeletePlaylist_Click;

            // fader
            _textViewNo = FindViewById<TextView>(Resource.Id.text_view_no);
            _textViewChannel = FindViewById<TextView>(Resource.Id.text_view_channel);
            _numberPickerProg = FindViewById<NumberPicker>(Resource.Id.number_picker_prog);
            _numberPickerProg.MinValue = 1;
            _numberPickerProg.MaxValue = 128;
            _numberPickerProg.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                fader.Program = _numberPickerProg.Value;
            };
            _numberPickerPan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan);
            _numberPickerPan.MinValue = 1;
            _numberPickerPan.MaxValue = 128;
            _numberPickerPan.Value = 65;
            _numberPickerPan.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                fader.Pan = _numberPickerPan.Value;
            };
            _numberPickerVol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol);
            _numberPickerVol.MinValue = 1;
            _numberPickerVol.MaxValue = 128;
            _numberPickerVol.Value = 104;
            _numberPickerVol.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                fader.Volume = _numberPickerVol.Value;
            };
            _checkBoxMute = FindViewById<CheckBox>(Resource.Id.check_box_mute);
            _checkBoxMute.CheckedChange += (object sender, CheckBox.CheckedChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                fader.Sounds = !_checkBoxMute.Checked;
            };
            _buttonSendSynth = FindViewById<Button>(Resource.Id.button_send_synth);
            _buttonSendSynth.Click += buttonSendSynth_Click;

            // list view title
            var titleList = new List<ListTitle>();
            titleList.Add(new ListTitle() { Name = "Name", Instrument = "Voice", Channel = "Ch" });
            _titleListView = FindViewById<ListView>(Resource.Id.list_view_title);
            _titleListView.Adapter = new ListTitleAdapter(this, 0, titleList);

            // list view item
            Enumerable.Range(MIDI_TRACK_BASE, MIDI_TRACK_COUNT).ToList().ForEach(x => {
                _itemList.Add(new ListItem() { Name = "------", Instrument = "------", Channel = "---" });
            });
            _itemListView = FindViewById<ListView>(Resource.Id.list_view_item);
            var listItemAdapter = new ListItemAdapter(this, 0, _itemList);
            _itemListView.Adapter = listItemAdapter;
            _itemListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var item = _itemListView.GetItemAtPosition(e.Position);
                ListItem listItem = item.Cast<ListItem>();
                Log.Info($"setected: {e.Position}");
                Mixer.Current = e.Position;
            };
        }
    }
}
