
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiPlayer.Win64 {
    /// <summary>
    /// main form for app
    /// </summary>
    public partial class MainForm : Form {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        string soundFontPath = "undefined";

        string midiFilePath = "undefined";

        PlayList playList;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainForm() {
            InitializeComponent();

            playList = new PlayList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // EventHandler

        void MainForm_Load(object sender, EventArgs e) {
            Conf.Load();
            initializeControl();

            int _count = 0;
            Synth.OnMessage += (IntPtr data, IntPtr evt) => {
                //Log.Info($"OnMessage count: {_count}");
                _count++;
                var _channel = Synth.GetChannel(evt);
                if (_channel == 9) {
                    Log.Info($"IsSounded:{Synth.IsSounded(_channel)}");
                }
                Invoke(updateList());
                return Synth.HandleEvent(data, evt);
            };

            Synth.OnStart += () => {
                Log.Info("OnStart called.");
                Invoke((MethodInvoker) (() => {
                    this.Text = $"MidiPlayer: {Synth.MidiFilePath.ToFileName()} {Synth.SoundFontPath.ToFileName()}";
                }));
            };

            Synth.OnEnd += () => {
                Log.Info("OnEnd called.");
                if (!playList.Ready) {
                    Synth.Stop();
                    Synth.Start();
                } else {
                    Synth.Stop();
                    Synth.MidiFilePath = playList.Next;
                    Synth.Start();
                }
            };
        }

        void buttonLoadSoundFont_Click(object sender, EventArgs e) {
            Log.Info("buttonLoadSoundFont clicked.");
            try {
                if (Synth.Playing) {
                    stopSong();
                }
                var _dialog = openFileDialog.ShowDialog();
                if (_dialog == DialogResult.OK) {
                    soundFontPath = Path.GetFullPath(openFileDialog.FileName);
                    Synth.SoundFontPath = soundFontPath;
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
                var _dialog = openFileDialog.ShowDialog();
                if (_dialog == DialogResult.OK) {
                    midiFilePath = Path.GetFullPath(openFileDialog.FileName);
                    Synth.MidiFilePath = midiFilePath;
                }
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        void buttonStart_Click(object sender, EventArgs e) {
            Log.Info("buttonStart clicked.");
            try {
                if (!midiFilePath.HasValue()) {
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
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        async void playSong() {
            try {
                await Task.Run(() => {
                    if (!playList.Ready) {
                        Synth.MidiFilePath = midiFilePath;
                        Synth.Start();
                    } else {
                        Synth.MidiFilePath = playList.Next;
                        Synth.Start();
                    }
                });
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        async void stopSong() {
            try {
                await Task.Run(() => Synth.Stop());
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        MethodInvoker updateList() {
            return () => {
                Enumerable.Range(0, 16).ToList().ForEach(x => {
                    string[] _item = {
                        Synth.IsSounded(x).ToString(),
                        "",//Synth.GetTrackName(x),
                        Synth.GetVoice(x),
                        x.ToString(),
                        Synth.GetBank(x).ToString(),
                        Synth.GetProgram(x).ToString()
                    };
                    var _listViewItem = new ListViewItem(_item);
                    listView.Items.Add(_listViewItem);
                });
            };
        }

        void initializeControl() {
            // initialize ListView
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.Sorting = SortOrder.None; // do not sort automatically.
            listView.View = View.Details;
            var _column1 = new ColumnHeader();
            _column1.Text = "On";
            _column1.Width = 50;
            var _column2 = new ColumnHeader();
            _column2.Text = "Name";
            _column2.Width = 200;
            var _column3 = new ColumnHeader();
            _column3.Text = "Voice";
            _column3.Width = 200;
            var _column4 = new ColumnHeader();
            _column4.Text = "Ch";
            _column4.Width = 50;
            var _column5 = new ColumnHeader();
            _column5.Text = "Bank";
            _column5.Width = 50;
            var _column6 = new ColumnHeader();
            _column6.Text = "Prog";
            _column6.Width = 50;
            ColumnHeader[] _columnHeaderArray = { _column1, _column2, _column3, _column4, _column5, _column6 };
            listView.Columns.AddRange(_columnHeaderArray);
        }
    }
}
