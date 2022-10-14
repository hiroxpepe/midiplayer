/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Android.Support.V7.App;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiPlayer.Droid {
    /// <summary>
    ///  partial class for initialize the component
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public partial class MainActivity : AppCompatActivity {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        Button _button_load_soundfont;
        Button _button_load_midi_file;
        Button _button_start;
        Button _button_stop;
        Button _button_add_playlist;
        Button _button_delete_playlist;
        Button _button_send_synth;
        TextView _textview_no;
        TextView _textview_channel;
        NumberPicker _numberpicker_prog;
        NumberPicker _numberpicker_pan;
        NumberPicker _numberpicker_vol;
        CheckBox _checkbox_mute;
        ListView _listview_title;
        ListView _listview_item;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        /// <summary>
        /// initialize the component.
        /// </summary>
        void initializeComponent() {

            /// <summary>
            /// buttonLoadSoundFont
            /// </summary>
            _button_load_soundfont = FindViewById<Button>(Resource.Id.button_load_soundfont);
            _button_load_soundfont.Click += (object sender, EventArgs e) => {
                Log.Info("_button_load_soundfont clicked.");
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
            _button_load_midi_file = FindViewById<Button>(Resource.Id.button_load_midi_file);
            _button_load_midi_file.Click += (object sender, EventArgs e) => {
                Log.Info("_button_load_midi_file clicked.");
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
            _button_start = FindViewById<Button>(Resource.Id.button_start);
            _button_start.Click += (object sender, EventArgs e) => {
                Log.Info("_button_start clicked.");
                try {
                    if (!_midi_file_path.HasValue()) { // FIXME: case sounFdont
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
            _button_stop = FindViewById<Button>(Resource.Id.button_stop);
            _button_stop.Click += (object sender, EventArgs e) => {
                Log.Info("_button_stop clicked.");
                try {
                    stopSong();
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// buttonAddPlaylist
            /// </summary>
            _button_add_playlist = FindViewById<Button>(Resource.Id.button_add_playlist);
            _button_add_playlist.Click += (object sender, EventArgs e) => {
                Log.Info("_button_add_playlist clicked.");
                try {
                    callIntent(Env.MidiFileDir, (int) Request.AddPlayList);
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// buttonDeletePlaylist
            /// </summary>
            _button_delete_playlist = FindViewById<Button>(Resource.Id.button_delete_playlist);
            _button_delete_playlist.Click += (object sender, EventArgs e) => {
                Log.Info("_button_delete_playlist clicked.");
                try {
                    _playlist.Clear();
                } catch (Exception ex) {
                    Log.Error(ex.Message);
                }
            };

            /// <summary>
            /// buttonSendSynth
            /// </summary>
            _button_send_synth = FindViewById<Button>(Resource.Id.button_send_synth);
            _button_send_synth.Click += (object sender, EventArgs e) => {
                Log.Info("_button_send_synth clicked.");
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
            _textview_no = FindViewById<TextView>(Resource.Id.textview_no);
            _textview_channel = FindViewById<TextView>(Resource.Id.textview_channel);

            /// <summary>
            /// numberPickerProg
            /// </summary>
            _numberpicker_prog = FindViewById<NumberPicker>(Resource.Id.numberpicker_prog);
            _numberpicker_prog.MinValue = 1;
            _numberpicker_prog.MaxValue = 128;
            _numberpicker_prog.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                Log.Debug($"_numberpicker_prog.Value: {_numberpicker_prog.Value}");
                fader.ProgramAsOneBased = _numberpicker_prog.Value;
            };

            /// <summary>
            /// _numberpicker_pan
            /// </summary>
            _numberpicker_pan = FindViewById<NumberPicker>(Resource.Id.numberpicker_pan);
            _numberpicker_pan.MinValue = 1;
            _numberpicker_pan.MaxValue = 128;
            _numberpicker_pan.Value = 65;
            _numberpicker_pan.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                Log.Debug($"_numberpicker_pan.Value: {_numberpicker_pan.Value}");
                fader.Pan = _numberpicker_pan.Value;
            };

            /// <summary>
            /// _numberpicker_vol
            /// </summary>
            _numberpicker_vol = FindViewById<NumberPicker>(Resource.Id.numberpicker_vol);
            _numberpicker_vol.MinValue = 1;
            _numberpicker_vol.MaxValue = 128;
            _numberpicker_vol.Value = 104;
            _numberpicker_vol.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                Log.Debug($"_numberpicker_vol.Value: {_numberpicker_vol.Value}");
                fader.Volume = _numberpicker_vol.Value;
            };

            /// <summary>
            /// _checkbox_mute
            /// </summary>
            _checkbox_mute = FindViewById<CheckBox>(Resource.Id.checkbox_mute);
            _checkbox_mute.CheckedChange += (object sender, CheckBox.CheckedChangeEventArgs e) => {
                var fader = Mixer.GetCurrent();
                fader.Sounds = !_checkbox_mute.Checked;
            };

            /// <summary>
            /// _listview_title
            /// </summary>
            var listtitle_list = new List<ListTitle>();
            listtitle_list.Add(new ListTitle() { Name = "Name", Instrument = "Voice", Channel = "Ch" });
            _listview_title = FindViewById<ListView>(Resource.Id.listview_title);
            _listview_title.Adapter = new ListTitleAdapter(this, 0, listtitle_list);

            /// <summary>
            /// _listview_item
            /// </summary>
            Enumerable.Range(MIDI_TRACK_BASE, MIDI_TRACK_COUNT).ToList().ForEach(x => {
                _listitem_list.Add(new ListItem() { Name = "------", Instrument = "------", Channel = "---" });
            });
            _listview_item = FindViewById<ListView>(Resource.Id.listview_item);
            var listitem_adapter = new ListItemAdapter(this, 0, _listitem_list);
            _listview_item.Adapter = listitem_adapter;
            _listview_item.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                Log.Debug($"setected: {e.Position}");
                Mixer.Current = e.Position;
            };
        }
    }
}
