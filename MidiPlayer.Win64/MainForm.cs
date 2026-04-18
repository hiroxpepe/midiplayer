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

        /// <summary>
        /// current sound font file path, loaded from the open-file dialog.
        /// </summary>
        string _soundfont_path = "undefined";

        /// <summary>
        /// current MIDI file path, loaded from the open-file dialog.
        /// </summary>
        string _midi_file_path = "undefined";

        /// <summary>
        /// playlist holding MIDI file paths to play sequentially.
        /// </summary>
        PlayList _playlist;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// initializes a new MainForm.
        /// </summary>
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
            /// <remarks>
            /// Ended fires on the LongRunning thread (Thread 11). Calling Stop()+Start() directly
            /// here caused a race: Thread 11 would be inside Init() loading the SF2 file while
            /// Thread 1 (buttonStop_Click) called final(), which deleted the native handles Thread
            /// 11 was using — causing new_fluid_audio_driver(IntPtr.Zero) to hang indefinitely.
            ///
            /// Fix: post the restart to the UI thread via BeginInvoke. This serializes the restart
            /// with buttonStop_Click on the same message queue, eliminating the race entirely.
            /// Thread 11 exits cleanly; the UI thread decides whether to restart.
            /// </remarks>
            Synth.Ended += () => {
                Log.Info("Ended called.");
                BeginInvoke((MethodInvoker) (() => {
                    if (!Synth.Playing) {
                        // Stop() was already called (user pressed Stop); do not restart.
                        return;
                    }
                    stopSong();
                    if (!_playlist.Ready) {
                        playSong();
                    } else {
                        Synth.MidiFilePath = _playlist.Next;
                        playSong();
                    }
                }));
            };

            /// <summary>
            /// add a callback function to be called when the synth updated.
            /// BeginInvoke (async) is used instead of Invoke (sync) because Updated fires on the
            /// native audio callback thread. Using Invoke here would block the callback thread
            /// while waiting for the UI thread, and Synth.Stop() calls delete_fluid_audio_driver
            /// (on the UI thread) which waits for all callbacks to finish — causing deadlock.
            /// </summary>
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
                var track = (Synth.Track) sender;
                BeginInvoke(updateList(track));
            };
        }

        /// <summary>
        /// button load sound font click event handler.
        /// </summary>
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

        /// <summary>
        /// button load MIDI file click event handler.
        /// </summary>
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

        /// <summary>
        /// button start click event handler.
        /// </summary>
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

        /// <summary>
        /// button stop click event handler.
        /// </summary>
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
        /// <remarks>
        /// TaskCreationOptions.LongRunning is used instead of Task.Run because Synth.Start()
        /// calls fluid_player_join() internally, which blocks until the song finishes.
        /// LongRunning signals the scheduler to allocate a dedicated thread outside the ThreadPool,
        /// preventing Thread Pool Starvation when multiple songs play consecutively.
        /// </remarks>
        void playSong() {
            try {
                Task.Factory.StartNew(() => {
                    try {
                        if (!_playlist.Ready) {
                            Synth.MidiFilePath = _midi_file_path;
                            Synth.Start();
                        } else {
                            Synth.MidiFilePath = _playlist.Next;
                            Synth.Start();
                        }
                    } catch (Exception ex) {
                        Log.Error(ex.Message);
                    }
                }, TaskCreationOptions.LongRunning);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// stop the song.
        /// </summary>
        /// <remarks>
        /// kept synchronous intentionally: Synth.Stop() calls fluid_player_stop + final(),
        /// both of which return quickly. making this async void would silently swallow any
        /// exception that escapes the try-catch, crashing the process without a call-site trace.
        /// </remarks>
        void stopSong() {
            try {
                Synth.Stop();
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
        /// <param name="track">the track whose state changed.</param>
        /// <returns>a MethodInvoker that updates the corresponding ListView row on the UI thread.</returns>
        MethodInvoker updateList(Synth.Track track) {
            const int COLUMN_1_INDEX = 0;
            var track_index = track.Index - 1; // exclude conductor track;
            return () => {
                // guard: listview is populated by the Started event; during Init() it may be empty.
                if (track_index < 0 || track_index >= _listview.Items.Count) {
                    return;
                }
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
