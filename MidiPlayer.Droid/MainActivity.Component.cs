
using Android.Support.V7.App;
using Android.Widget;

namespace MidiPlayer.Droid {
    /// <summary>
    ///  partial class for initialize the component
    /// </summary>
    public partial class MainActivity : AppCompatActivity {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        /// <summary>
        /// initialize the component.
        /// </summary>
        void initializeComponent() {
            var _buttonloadSoundFont = FindViewById<Button>(Resource.Id.button_load_soundfont);
            _buttonloadSoundFont.Click += buttonLoadSoundFont_Click;

            var _buttonloadMidiFile = FindViewById<Button>(Resource.Id.button_load_midifile);
            _buttonloadMidiFile.Click += buttonLoadMidiFile_Click;

            var _buttonStart = FindViewById<Button>(Resource.Id.button_start);
            _buttonStart.Click += buttonStart_Click;

            var _buttonStop = FindViewById<Button>(Resource.Id.button_stop);
            _buttonStop.Click += buttonStop_Click;

            var _buttonAddPlaylist = FindViewById<Button>(Resource.Id.button_add_playlist);
            _buttonAddPlaylist.Click += buttonAddPlaylist_Click;

            var _buttonDeletePlaylist = FindViewById<Button>(Resource.Id.button_delete_playlist);
            _buttonDeletePlaylist.Click += buttonDeletePlaylist_Click;

            // fader1
            var _numberPickerProg_1 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_1);
            _numberPickerProg_1.MinValue = 1;
            _numberPickerProg_1.MaxValue = 128;
            var _numberPickerPan_1 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_1);
            _numberPickerPan_1.MinValue = 1;
            _numberPickerPan_1.MaxValue = 128;
            _numberPickerPan_1.Value = 65;
            var _numberPickerVol_1 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_1);
            _numberPickerVol_1.MinValue = 1;
            _numberPickerVol_1.MaxValue = 128;
            _numberPickerVol_1.Value = 104;
            var _buttonSendSynth_1 = FindViewById<Button>(Resource.Id.button_send_synth_1);
            _buttonSendSynth_1.Click += buttonSendSynth_1_Click;

            // fader2
            var _numberPickerProg_2 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_2);
            _numberPickerProg_2.MinValue = 1;
            _numberPickerProg_2.MaxValue = 128;
            var _numberPickerPan_2 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_2);
            _numberPickerPan_2.MinValue = 1;
            _numberPickerPan_2.MaxValue = 128;
            _numberPickerPan_2.Value = 65;
            var _numberPickerVol_2 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_2);
            _numberPickerVol_2.MinValue = 1;
            _numberPickerVol_2.MaxValue = 128;
            _numberPickerVol_2.Value = 104;
            var _buttonSendSynth_2 = FindViewById<Button>(Resource.Id.button_send_synth_2);
            _buttonSendSynth_2.Click += buttonSendSynth_2_Click;

            // fader3
            var _numberPickerProg_3 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_3);
            _numberPickerProg_3.MinValue = 1;
            _numberPickerProg_3.MaxValue = 128;
            var _numberPickerPan_3 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_3);
            _numberPickerPan_3.MinValue = 1;
            _numberPickerPan_3.MaxValue = 128;
            _numberPickerPan_3.Value = 65;
            var _numberPickerVol_3 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_3);
            _numberPickerVol_3.MinValue = 1;
            _numberPickerVol_3.MaxValue = 128;
            _numberPickerVol_3.Value = 104;
            var _buttonSendSynth_3 = FindViewById<Button>(Resource.Id.button_send_synth_3);
            _buttonSendSynth_3.Click += buttonSendSynth_3_Click;

            // fader4
            var _numberPickerProg_4 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_4);
            _numberPickerProg_4.MinValue = 1;
            _numberPickerProg_4.MaxValue = 128;
            var _numberPickerPan_4 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_4);
            _numberPickerPan_4.MinValue = 1;
            _numberPickerPan_4.MaxValue = 128;
            _numberPickerPan_4.Value = 65;
            var _numberPickerVol_4 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_4);
            _numberPickerVol_4.MinValue = 1;
            _numberPickerVol_4.MaxValue = 128;
            _numberPickerVol_4.Value = 104;
            var _buttonSendSynth_4 = FindViewById<Button>(Resource.Id.button_send_synth_4);
            _buttonSendSynth_4.Click += buttonSendSynth_4_Click;

            // fader5
            var _numberPickerProg_5 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_5);
            _numberPickerProg_5.MinValue = 1;
            _numberPickerProg_5.MaxValue = 128;
            var _numberPickerPan_5 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_5);
            _numberPickerPan_5.MinValue = 1;
            _numberPickerPan_5.MaxValue = 128;
            _numberPickerPan_5.Value = 65;
            var _numberPickerVol_5 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_5);
            _numberPickerVol_5.MinValue = 1;
            _numberPickerVol_5.MaxValue = 128;
            _numberPickerVol_5.Value = 104;
            var _buttonSendSynth_5 = FindViewById<Button>(Resource.Id.button_send_synth_5);
            _buttonSendSynth_5.Click += buttonSendSynth_5_Click;

            // fader6
            var _numberPickerProg_6 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_6);
            _numberPickerProg_6.MinValue = 1;
            _numberPickerProg_6.MaxValue = 128;
            var _numberPickerPan_6 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_6);
            _numberPickerPan_6.MinValue = 1;
            _numberPickerPan_6.MaxValue = 128;
            _numberPickerPan_6.Value = 65;
            var _numberPickerVol_6 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_6);
            _numberPickerVol_6.MinValue = 1;
            _numberPickerVol_6.MaxValue = 128;
            _numberPickerVol_6.Value = 104;
            var _buttonSendSynth_6 = FindViewById<Button>(Resource.Id.button_send_synth_6);
            _buttonSendSynth_6.Click += buttonSendSynth_6_Click;

            // fader7
            var _numberPickerProg_7 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_7);
            _numberPickerProg_7.MinValue = 1;
            _numberPickerProg_7.MaxValue = 128;
            var _numberPickerPan_7 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_7);
            _numberPickerPan_7.MinValue = 1;
            _numberPickerPan_7.MaxValue = 128;
            _numberPickerPan_7.Value = 65;
            var _numberPickerVol_7 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_7);
            _numberPickerVol_7.MinValue = 1;
            _numberPickerVol_7.MaxValue = 128;
            _numberPickerVol_7.Value = 104;
            var _buttonSendSynth_7 = FindViewById<Button>(Resource.Id.button_send_synth_7);
            _buttonSendSynth_7.Click += buttonSendSynth_7_Click;

            // fader8
            var _numberPickerProg_8 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_8);
            _numberPickerProg_8.MinValue = 1;
            _numberPickerProg_8.MaxValue = 128;
            var _numberPickerPan_8 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_8);
            _numberPickerPan_8.MinValue = 1;
            _numberPickerPan_8.MaxValue = 128;
            _numberPickerPan_8.Value = 65;
            var _numberPickerVol_8 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_8);
            _numberPickerVol_8.MinValue = 1;
            _numberPickerVol_8.MaxValue = 128;
            _numberPickerVol_8.Value = 104;
            var _buttonSendSynth_8 = FindViewById<Button>(Resource.Id.button_send_synth_8);
            _buttonSendSynth_8.Click += buttonSendSynth_8_Click;

            // fader9
            var _numberPickerProg_9 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_9);
            _numberPickerProg_9.MinValue = 1;
            _numberPickerProg_9.MaxValue = 128;
            var _numberPickerPan_9 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_9);
            _numberPickerPan_9.MinValue = 1;
            _numberPickerPan_9.MaxValue = 128;
            _numberPickerPan_9.Value = 65;
            var _numberPickerVol_9 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_9);
            _numberPickerVol_9.MinValue = 1;
            _numberPickerVol_9.MaxValue = 128;
            _numberPickerVol_9.Value = 104;
            var _buttonSendSynth_9 = FindViewById<Button>(Resource.Id.button_send_synth_9);
            _buttonSendSynth_9.Click += buttonSendSynth_9_Click;

            // fader10
            var _numberPickerProg_10 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_10);
            _numberPickerProg_10.MinValue = 1;
            _numberPickerProg_10.MaxValue = 128;
            var _numberPickerPan_10 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_10);
            _numberPickerPan_10.MinValue = 1;
            _numberPickerPan_10.MaxValue = 128;
            _numberPickerPan_10.Value = 65;
            var _numberPickerVol_10 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_10);
            _numberPickerVol_10.MinValue = 1;
            _numberPickerVol_10.MaxValue = 128;
            _numberPickerVol_10.Value = 104;
            var _buttonSendSynth_10 = FindViewById<Button>(Resource.Id.button_send_synth_10);
            _buttonSendSynth_10.Click += buttonSendSynth_10_Click;

            // fader11
            var _numberPickerProg_11 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_11);
            _numberPickerProg_11.MinValue = 1;
            _numberPickerProg_11.MaxValue = 128;
            var _numberPickerPan_11 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_11);
            _numberPickerPan_11.MinValue = 1;
            _numberPickerPan_11.MaxValue = 128;
            _numberPickerPan_11.Value = 65;
            var _numberPickerVol_11 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_11);
            _numberPickerVol_11.MinValue = 1;
            _numberPickerVol_11.MaxValue = 128;
            _numberPickerVol_11.Value = 104;
            var _buttonSendSynth_11 = FindViewById<Button>(Resource.Id.button_send_synth_11);
            _buttonSendSynth_11.Click += buttonSendSynth_11_Click;

            // fader12
            var _numberPickerProg_12 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_12);
            _numberPickerProg_12.MinValue = 1;
            _numberPickerProg_12.MaxValue = 128;
            var _numberPickerPan_12 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_12);
            _numberPickerPan_12.MinValue = 1;
            _numberPickerPan_12.MaxValue = 128;
            _numberPickerPan_12.Value = 65;
            var _numberPickerVol_12 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_12);
            _numberPickerVol_12.MinValue = 1;
            _numberPickerVol_12.MaxValue = 128;
            _numberPickerVol_12.Value = 104;
            var _buttonSendSynth_12 = FindViewById<Button>(Resource.Id.button_send_synth_12);
            _buttonSendSynth_12.Click += buttonSendSynth_12_Click;

            // fader13
            var _numberPickerProg_13 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_13);
            _numberPickerProg_13.MinValue = 1;
            _numberPickerProg_13.MaxValue = 128;
            var _numberPickerPan_13 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_13);
            _numberPickerPan_13.MinValue = 1;
            _numberPickerPan_13.MaxValue = 128;
            _numberPickerPan_13.Value = 65;
            var _numberPickerVol_13 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_13);
            _numberPickerVol_13.MinValue = 1;
            _numberPickerVol_13.MaxValue = 128;
            _numberPickerVol_13.Value = 104;
            var _buttonSendSynth_13 = FindViewById<Button>(Resource.Id.button_send_synth_13);
            _buttonSendSynth_13.Click += buttonSendSynth_13_Click;

            // fader14
            var _numberPickerProg_14 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_14);
            _numberPickerProg_14.MinValue = 1;
            _numberPickerProg_14.MaxValue = 128;
            var _numberPickerPan_14 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_14);
            _numberPickerPan_14.MinValue = 1;
            _numberPickerPan_14.MaxValue = 128;
            _numberPickerPan_14.Value = 65;
            var _numberPickerVol_14 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_14);
            _numberPickerVol_14.MinValue = 1;
            _numberPickerVol_14.MaxValue = 128;
            _numberPickerVol_14.Value = 104;
            var _buttonSendSynth_14 = FindViewById<Button>(Resource.Id.button_send_synth_14);
            _buttonSendSynth_14.Click += buttonSendSynth_14_Click;

            // fader15
            var _numberPickerProg_15 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_15);
            _numberPickerProg_15.MinValue = 1;
            _numberPickerProg_15.MaxValue = 128;
            var _numberPickerPan_15 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_15);
            _numberPickerPan_15.MinValue = 1;
            _numberPickerPan_15.MaxValue = 128;
            _numberPickerPan_15.Value = 65;
            var _numberPickerVol_15 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_15);
            _numberPickerVol_15.MinValue = 1;
            _numberPickerVol_15.MaxValue = 128;
            _numberPickerVol_15.Value = 104;
            var _buttonSendSynth_15 = FindViewById<Button>(Resource.Id.button_send_synth_15);
            _buttonSendSynth_15.Click += buttonSendSynth_15_Click;

            // fader16
            var _numberPickerProg_16 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_16);
            _numberPickerProg_16.MinValue = 1;
            _numberPickerProg_16.MaxValue = 128;
            var _numberPickerPan_16 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_16);
            _numberPickerPan_16.MinValue = 1;
            _numberPickerPan_16.MaxValue = 128;
            _numberPickerPan_16.Value = 65;
            var _numberPickerVol_16 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_16);
            _numberPickerVol_16.MinValue = 1;
            _numberPickerVol_16.MaxValue = 128;
            _numberPickerVol_16.Value = 104;
            var _buttonSendSynth_16 = FindViewById<Button>(Resource.Id.button_send_synth_16);
            _buttonSendSynth_16.Click += buttonSendSynth_16_Click;
        }
    }
}
