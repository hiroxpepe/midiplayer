
using Android.Support.V7.App;
using Android.Widget;

namespace MidiPlayer.Droid {
    /// <summary>
    ///  partial class for initialize the component
    /// </summary>
    public partial class MainActivity : AppCompatActivity {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

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
    }
}
