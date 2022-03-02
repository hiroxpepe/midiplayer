
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
            var buttonloadSoundFont = FindViewById<Button>(Resource.Id.button_load_soundfont);
            buttonloadSoundFont.Click += buttonLoadSoundFont_Click;

            var buttonloadMidiFile = FindViewById<Button>(Resource.Id.button_load_midifile);
            buttonloadMidiFile.Click += buttonLoadMidiFile_Click;

            var buttonStart = FindViewById<Button>(Resource.Id.button_start);
            buttonStart.Click += buttonStart_Click;

            var buttonStop = FindViewById<Button>(Resource.Id.button_stop);
            buttonStop.Click += buttonStop_Click;

            var buttonAddPlaylist = FindViewById<Button>(Resource.Id.button_add_playlist);
            buttonAddPlaylist.Click += buttonAddPlaylist_Click;

            var buttonDeletePlaylist = FindViewById<Button>(Resource.Id.button_delete_playlist);
            buttonDeletePlaylist.Click += buttonDeletePlaylist_Click;

            // fader1
            var numberPickerProg_1 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_1);
            numberPickerProg_1.MinValue = 1;
            numberPickerProg_1.MaxValue = 128;
            var numberPickerPan_1 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_1);
            numberPickerPan_1.MinValue = 1;
            numberPickerPan_1.MaxValue = 128;
            numberPickerPan_1.Value = 65;
            var numberPickerVol_1 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_1);
            numberPickerVol_1.MinValue = 1;
            numberPickerVol_1.MaxValue = 128;
            numberPickerVol_1.Value = 104;
            var buttonSendSynth_1 = FindViewById<Button>(Resource.Id.button_send_synth_1);
            buttonSendSynth_1.Click += buttonSendSynth_1_Click;

            // fader2
            var numberPickerProg_2 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_2);
            numberPickerProg_2.MinValue = 1;
            numberPickerProg_2.MaxValue = 128;
            var numberPickerPan_2 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_2);
            numberPickerPan_2.MinValue = 1;
            numberPickerPan_2.MaxValue = 128;
            numberPickerPan_2.Value = 65;
            var numberPickerVol_2 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_2);
            numberPickerVol_2.MinValue = 1;
            numberPickerVol_2.MaxValue = 128;
            numberPickerVol_2.Value = 104;
            var buttonSendSynth_2 = FindViewById<Button>(Resource.Id.button_send_synth_2);
            buttonSendSynth_2.Click += buttonSendSynth_2_Click;

            // fader3
            var numberPickerProg_3 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_3);
            numberPickerProg_3.MinValue = 1;
            numberPickerProg_3.MaxValue = 128;
            var numberPickerPan_3 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_3);
            numberPickerPan_3.MinValue = 1;
            numberPickerPan_3.MaxValue = 128;
            numberPickerPan_3.Value = 65;
            var numberPickerVol_3 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_3);
            numberPickerVol_3.MinValue = 1;
            numberPickerVol_3.MaxValue = 128;
            numberPickerVol_3.Value = 104;
            var buttonSendSynth_3 = FindViewById<Button>(Resource.Id.button_send_synth_3);
            buttonSendSynth_3.Click += buttonSendSynth_3_Click;

            // fader4
            var numberPickerProg_4 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_4);
            numberPickerProg_4.MinValue = 1;
            numberPickerProg_4.MaxValue = 128;
            var numberPickerPan_4 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_4);
            numberPickerPan_4.MinValue = 1;
            numberPickerPan_4.MaxValue = 128;
            numberPickerPan_4.Value = 65;
            var numberPickerVol_4 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_4);
            numberPickerVol_4.MinValue = 1;
            numberPickerVol_4.MaxValue = 128;
            numberPickerVol_4.Value = 104;
            var buttonSendSynth_4 = FindViewById<Button>(Resource.Id.button_send_synth_4);
            buttonSendSynth_4.Click += buttonSendSynth_4_Click;

            // fader5
            var numberPickerProg_5 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_5);
            numberPickerProg_5.MinValue = 1;
            numberPickerProg_5.MaxValue = 128;
            var numberPickerPan_5 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_5);
            numberPickerPan_5.MinValue = 1;
            numberPickerPan_5.MaxValue = 128;
            numberPickerPan_5.Value = 65;
            var numberPickerVol_5 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_5);
            numberPickerVol_5.MinValue = 1;
            numberPickerVol_5.MaxValue = 128;
            numberPickerVol_5.Value = 104;
            var buttonSendSynth_5 = FindViewById<Button>(Resource.Id.button_send_synth_5);
            buttonSendSynth_5.Click += buttonSendSynth_5_Click;

            // fader6
            var numberPickerProg_6 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_6);
            numberPickerProg_6.MinValue = 1;
            numberPickerProg_6.MaxValue = 128;
            var numberPickerPan_6 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_6);
            numberPickerPan_6.MinValue = 1;
            numberPickerPan_6.MaxValue = 128;
            numberPickerPan_6.Value = 65;
            var numberPickerVol_6 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_6);
            numberPickerVol_6.MinValue = 1;
            numberPickerVol_6.MaxValue = 128;
            numberPickerVol_6.Value = 104;
            var buttonSendSynth_6 = FindViewById<Button>(Resource.Id.button_send_synth_6);
            buttonSendSynth_6.Click += buttonSendSynth_6_Click;

            // fader7
            var numberPickerProg_7 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_7);
            numberPickerProg_7.MinValue = 1;
            numberPickerProg_7.MaxValue = 128;
            var numberPickerPan_7 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_7);
            numberPickerPan_7.MinValue = 1;
            numberPickerPan_7.MaxValue = 128;
            numberPickerPan_7.Value = 65;
            var numberPickerVol_7 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_7);
            numberPickerVol_7.MinValue = 1;
            numberPickerVol_7.MaxValue = 128;
            numberPickerVol_7.Value = 104;
            var buttonSendSynth_7 = FindViewById<Button>(Resource.Id.button_send_synth_7);
            buttonSendSynth_7.Click += buttonSendSynth_7_Click;

            // fader8
            var numberPickerProg_8 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_8);
            numberPickerProg_8.MinValue = 1;
            numberPickerProg_8.MaxValue = 128;
            var numberPickerPan_8 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_8);
            numberPickerPan_8.MinValue = 1;
            numberPickerPan_8.MaxValue = 128;
            numberPickerPan_8.Value = 65;
            var numberPickerVol_8 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_8);
            numberPickerVol_8.MinValue = 1;
            numberPickerVol_8.MaxValue = 128;
            numberPickerVol_8.Value = 104;
            var buttonSendSynth_8 = FindViewById<Button>(Resource.Id.button_send_synth_8);
            buttonSendSynth_8.Click += buttonSendSynth_8_Click;

            // fader9
            var numberPickerProg_9 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_9);
            numberPickerProg_9.MinValue = 1;
            numberPickerProg_9.MaxValue = 128;
            var numberPickerPan_9 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_9);
            numberPickerPan_9.MinValue = 1;
            numberPickerPan_9.MaxValue = 128;
            numberPickerPan_9.Value = 65;
            var numberPickerVol_9 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_9);
            numberPickerVol_9.MinValue = 1;
            numberPickerVol_9.MaxValue = 128;
            numberPickerVol_9.Value = 104;
            var buttonSendSynth_9 = FindViewById<Button>(Resource.Id.button_send_synth_9);
            buttonSendSynth_9.Click += buttonSendSynth_9_Click;

            // fader10
            var numberPickerProg_10 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_10);
            numberPickerProg_10.MinValue = 1;
            numberPickerProg_10.MaxValue = 128;
            var numberPickerPan_10 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_10);
            numberPickerPan_10.MinValue = 1;
            numberPickerPan_10.MaxValue = 128;
            numberPickerPan_10.Value = 65;
            var numberPickerVol_10 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_10);
            numberPickerVol_10.MinValue = 1;
            numberPickerVol_10.MaxValue = 128;
            numberPickerVol_10.Value = 104;
            var buttonSendSynth_10 = FindViewById<Button>(Resource.Id.button_send_synth_10);
            buttonSendSynth_10.Click += buttonSendSynth_10_Click;

            // fader11
            var numberPickerProg_11 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_11);
            numberPickerProg_11.MinValue = 1;
            numberPickerProg_11.MaxValue = 128;
            var numberPickerPan_11 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_11);
            numberPickerPan_11.MinValue = 1;
            numberPickerPan_11.MaxValue = 128;
            numberPickerPan_11.Value = 65;
            var numberPickerVol_11 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_11);
            numberPickerVol_11.MinValue = 1;
            numberPickerVol_11.MaxValue = 128;
            numberPickerVol_11.Value = 104;
            var buttonSendSynth_11 = FindViewById<Button>(Resource.Id.button_send_synth_11);
            buttonSendSynth_11.Click += buttonSendSynth_11_Click;

            // fader12
            var numberPickerProg_12 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_12);
            numberPickerProg_12.MinValue = 1;
            numberPickerProg_12.MaxValue = 128;
            var numberPickerPan_12 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_12);
            numberPickerPan_12.MinValue = 1;
            numberPickerPan_12.MaxValue = 128;
            numberPickerPan_12.Value = 65;
            var numberPickerVol_12 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_12);
            numberPickerVol_12.MinValue = 1;
            numberPickerVol_12.MaxValue = 128;
            numberPickerVol_12.Value = 104;
            var buttonSendSynth_12 = FindViewById<Button>(Resource.Id.button_send_synth_12);
            buttonSendSynth_12.Click += buttonSendSynth_12_Click;

            // fader13
            var numberPickerProg_13 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_13);
            numberPickerProg_13.MinValue = 1;
            numberPickerProg_13.MaxValue = 128;
            var numberPickerPan_13 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_13);
            numberPickerPan_13.MinValue = 1;
            numberPickerPan_13.MaxValue = 128;
            numberPickerPan_13.Value = 65;
            var numberPickerVol_13 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_13);
            numberPickerVol_13.MinValue = 1;
            numberPickerVol_13.MaxValue = 128;
            numberPickerVol_13.Value = 104;
            var buttonSendSynth_13 = FindViewById<Button>(Resource.Id.button_send_synth_13);
            buttonSendSynth_13.Click += buttonSendSynth_13_Click;

            // fader14
            var numberPickerProg_14 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_14);
            numberPickerProg_14.MinValue = 1;
            numberPickerProg_14.MaxValue = 128;
            var numberPickerPan_14 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_14);
            numberPickerPan_14.MinValue = 1;
            numberPickerPan_14.MaxValue = 128;
            numberPickerPan_14.Value = 65;
            var numberPickerVol_14 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_14);
            numberPickerVol_14.MinValue = 1;
            numberPickerVol_14.MaxValue = 128;
            numberPickerVol_14.Value = 104;
            var buttonSendSynth_14 = FindViewById<Button>(Resource.Id.button_send_synth_14);
            buttonSendSynth_14.Click += buttonSendSynth_14_Click;

            // fader15
            var numberPickerProg_15 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_15);
            numberPickerProg_15.MinValue = 1;
            numberPickerProg_15.MaxValue = 128;
            var numberPickerPan_15 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_15);
            numberPickerPan_15.MinValue = 1;
            numberPickerPan_15.MaxValue = 128;
            numberPickerPan_15.Value = 65;
            var numberPickerVol_15 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_15);
            numberPickerVol_15.MinValue = 1;
            numberPickerVol_15.MaxValue = 128;
            numberPickerVol_15.Value = 104;
            var buttonSendSynth_15 = FindViewById<Button>(Resource.Id.button_send_synth_15);
            buttonSendSynth_15.Click += buttonSendSynth_15_Click;

            // fader16
            var numberPickerProg_16 = FindViewById<NumberPicker>(Resource.Id.number_picker_prog_16);
            numberPickerProg_16.MinValue = 1;
            numberPickerProg_16.MaxValue = 128;
            var numberPickerPan_16 = FindViewById<NumberPicker>(Resource.Id.number_picker_pan_16);
            numberPickerPan_16.MinValue = 1;
            numberPickerPan_16.MaxValue = 128;
            numberPickerPan_16.Value = 65;
            var numberPickerVol_16 = FindViewById<NumberPicker>(Resource.Id.number_picker_vol_16);
            numberPickerVol_16.MinValue = 1;
            numberPickerVol_16.MaxValue = 128;
            numberPickerVol_16.Value = 104;
            var buttonSendSynth_16 = FindViewById<Button>(Resource.Id.button_send_synth_16);
            buttonSendSynth_16.Click += buttonSendSynth_16_Click;
        }
    }
}
