﻿
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

            // fader
            var numberPickerProg = FindViewById<NumberPicker>(Resource.Id.number_picker_prog);
            numberPickerProg.MinValue = 1;
            numberPickerProg.MaxValue = 128;
            var numberPickerPan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan);
            numberPickerPan.MinValue = 1;
            numberPickerPan.MaxValue = 128;
            numberPickerPan.Value = 65;
            var numberPickerVol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol);
            numberPickerVol.MinValue = 1;
            numberPickerVol.MaxValue = 128;
            numberPickerVol.Value = 104;
            var buttonSendSynth = FindViewById<Button>(Resource.Id.button_send_synth);
            buttonSendSynth.Click += buttonSendSynth_Click;

            // list view title
            var titleList = new List<ListTitle>();
            titleList.Add(new ListTitle() { Name = "Name", Instrument = "Voice" });
            var titleListView = FindViewById<ListView>(Resource.Id.list_view_title);
            titleListView.Adapter = new ListTitleAdapter(this, 0, titleList);

            // list view item
            for (var i = 0; i < 16; i++) {
                _itemList.Add(new ListItem() { Name = "------", Instrument = "------"});
            }
            var itemListView = FindViewById<ListView>(Resource.Id.list_view_item);
            var listItemAdapter = new ListItemAdapter(this, 0, _itemList);
            itemListView.Adapter = listItemAdapter;
            itemListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var item = itemListView.GetItemAtPosition(e.Position);
                ListItem listItem = item.Cast<ListItem>();
                // TODO:
                Log.Info($"setected: {e.Position}");
                Mixer.Current = e.Position;
            };
        }
    }
}
