
using System;
using System.IO;
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

            int _count = 0;
            Synth.OnMessage += (IntPtr data, IntPtr evt) => {
                //Log.Info($"OnMessage count: {_count}");
                _count++;
                var _channel = Synth.GetChannel(evt);
                if (_channel == 9) {
                    Log.Info($"IsSounded:{Synth.IsSounded(_channel)}");
                }
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
    }
}
