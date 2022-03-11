
using Android.Support.V7.App;
using Android.Widget;

using System.Collections.Generic;

namespace MidiPlayer.Droid {
    /// <summary>
    ///  partial class for initialize the component
    /// </summary>
    public partial class MainActivity : AppCompatActivity {
#nullable enable

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

            // list view title
            var titleList = new List<ListTitle>();
            titleList.Add(new ListTitle() { Name = "Name", Instrument = "Voice" });
            var titleListView = FindViewById<ListView>(Resource.Id.list_view_title);
            titleListView.Adapter = new ListTitleAdapter(this, 0, titleList);

            // list view truck
            //var truckList = new List<ListItem>();
            for (var i = 0; i < 16; i++) {
                _truckList.Add(new ListItem() { Name = "------", Instrument = "------"});
            }
            var truckListView = FindViewById<ListView>(Resource.Id.list_view_truck);
            var listItemAdapter = new ListItemAdapter(this, 0, _truckList);
            truckListView.Adapter = listItemAdapter;
            truckListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var item = truckListView.GetItemAtPosition(e.Position);
                ListItem listItem = item.Cast<ListItem>();
                Log.Info("e.Position: " + e.Position);
                Log.Info("selected Name: " + listItem.Name);
            };
        }
    }
}
