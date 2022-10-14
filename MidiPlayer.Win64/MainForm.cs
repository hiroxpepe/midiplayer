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

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiPlayer.Win64 {
    /// <summary>
    /// main form for the application.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public partial class MainForm : Form {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        string _soundfont_path = "undefined";

        string _midi_file_path = "undefined";

        PlayList _playlist;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainForm() {
            InitializeComponent();
            DoubleBuffered = true;
            _playlist = new();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // EventHandler

        /// <summary>
        /// MainForm load.
        /// </summary>
        void MainForm_Load(object sender, EventArgs e) {
            Conf.Load();
            initializeControl();

            // load previous setting.
            if (Env.ExistsSoundFont && Env.ExistsMidiFile) {
                Synth.SoundFontPath = Env.SoundFontPath;
                Synth.MidiFilePath = Env.MidiFilePath;
                _soundfont_path = Env.SoundFontPath;
                _midi_file_path = Env.MidiFilePath;
            }

            /// <summary>
            /// add a callback function to be called when the synth is playback.
            /// </summary>
            Synth.Playbacking += (IntPtr data, IntPtr evt) => {
                return Synth.HandleEvent(data, evt);
            };

            /// <summary>
            /// add a callback function to be called when the synth started.
            /// </summary>
            Synth.Started += () => {
                Log.Info("Started called.");
                Invoke((MethodInvoker) (() => {
                    Text = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                    _listview.Items.Clear();
                    Enumerable.Range(0, Synth.TrackCount).ToList().ForEach(x => {
                        _listview.Items.Add(new ListViewItem(new string[] { "  ●", "--", "--", "--", "--", "--" }));
                    });
                }));
            };

            /// <summary>
            /// add a callback function to be called when the synth ended.
            /// </summary>
            Synth.Ended += () => {
                Log.Info("Ended called.");
                if (!_playlist.Ready) {
                    Synth.Stop();
                    Synth.Start();
                } else {
                    Synth.Stop();
                    Synth.MidiFilePath = _playlist.Next;
                    Synth.Start();
                }
            };

            /// <summary>
            /// add a callback function to be called when the synth updated.
            /// </summary>
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
                var track = (Synth.Track) sender;
                Invoke(updateList(track));
            };
        }

        void buttonLoadSoundFont_Click(object sender, EventArgs e) {
            Log.Info("buttonLoadSoundFont clicked.");
            try {
                if (Synth.Playing) {
                    stopSong();
                }
                _openfiledialog.InitialDirectory = Env.SoundFontDir;
                var dialog = _openfiledialog.ShowDialog();
                if (dialog == DialogResult.OK) {
                    _soundfont_path = Path.GetFullPath(_openfiledialog.FileName);
                    Synth.SoundFontPath = _soundfont_path;
                    Env.SoundFontPath = _soundfont_path;
                }
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
                _openfiledialog.InitialDirectory = Env.MidiFileDir;
                var dialog = _openfiledialog.ShowDialog();
                if (dialog == DialogResult.OK) {
                    _midi_file_path = Path.GetFullPath(_openfiledialog.FileName);
                    Synth.MidiFilePath = _midi_file_path;
                    Env.MidiFilePath = _midi_file_path;
                }
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonStart_Click(object sender, EventArgs e) {
            Log.Info("buttonStart clicked.");
            try {
                if (!_midi_file_path.HasValue()) {
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
                Conf.Save();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        /// <summary>
        /// play the song.
        /// </summary>
        async void playSong() {
            try {
                await Task.Run(() => {
                    if (!_playlist.Ready) {
                        Synth.MidiFilePath = _midi_file_path;
                        Synth.Start();
                    } else {
                        Synth.MidiFilePath = _playlist.Next;
                        Synth.Start();
                    }
                });
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// stop the song.
        /// </summary>
        async void stopSong() {
            try {
                await Task.Run(() => Synth.Stop());
                if (_listview.Items.Count != 0) {
                    Invoke((MethodInvoker) (() => {
                        Enumerable.Range(0, Synth.TrackCount).ToList().ForEach(x => {
                            _listview.Items[x].SubItems[0].ForeColor = Color.Black;
                        });
                    }));
                }
                Conf.Save(); // TODO: save
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// a callback function to be called when the synth updated.
        /// </summary>
        MethodInvoker updateList(Synth.Track track) {
            const int COLUMN_1_INDEX = 0;
            var track_index = track.Index - 1; // exclude conductor track;
            return () => {
                var listview_item = new ListViewItem(new string[] {
                    "  ●",
                    track.Name,
                    Synth.GetVoice(track.Index),
                    track.Channel.ToString(),
                    track.Bank.ToString(),
                    track.Program.ToString()
                });
                _listview.BeginUpdate();
                _listview.Items[track_index] = listview_item;
                _listview.Items[track_index].UseItemStyleForSubItems = false;
                if (track.Sounds) {
                    _listview.Items[track_index].SubItems[COLUMN_1_INDEX].ForeColor = Color.Lime;
                }
                else {
                    _listview.Items[track_index].SubItems[COLUMN_1_INDEX].ForeColor = Color.Black;
                }
                _listview.EndUpdate();
            };
        }

        /// <summary>
        /// initialize UI control.
        /// </summary>
        void initializeControl() {
            // initialize ListView
            _listview.FullRowSelect = true;
            _listview.GridLines = true;
            _listview.Sorting = SortOrder.None; // do not sort automatically.
            _listview.View = View.Details;
            ColumnHeader column1 = new();
            column1.Text = "On";
            column1.Width = 35;
            ColumnHeader column2 = new();
            column2.Text = "Name";
            column2.Width = 115;
            ColumnHeader column3 = new();
            column3.Text = "Voice";
            column3.Width = 115;
            ColumnHeader column4 = new();
            column4.Text = "Chan";
            column4.Width = 45;
            ColumnHeader column5 = new();
            column5.Text = "Bank";
            column5.Width = 45;
            ColumnHeader column6 = new();
            column6.Text = "Prog";
            column6.Width = 45;
            ColumnHeader[] columnHeaderArray = { column1, column2, column3, column4, column5, column6 };
            _listview.Columns.AddRange(columnHeaderArray);
        }
    }
}
