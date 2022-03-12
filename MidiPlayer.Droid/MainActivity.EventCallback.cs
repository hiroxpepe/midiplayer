
using Android.Support.V7.App;
using Android.Widget;

using System;

namespace MidiPlayer.Droid {
    /// <summary>
    ///  partial class for event callback functions.
    /// </summary>
    public partial class MainActivity : AppCompatActivity {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        /// <summary>
        /// buttonLoadSoundFont Click.
        /// </summary>
        void buttonLoadSoundFont_Click(object sender, EventArgs e) {
            Log.Info("buttonLoadSoundFont clicked.");
            try {
                if (Synth.Playing) {
                    stopSong();
                }
                callIntent(Env.SoundFontDirForIntent, (int) Request.SoundFont);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// buttonLoadMidiFile Click.
        /// </summary>
        void buttonLoadMidiFile_Click(object sender, EventArgs e) {
            Log.Info("buttonLoadMidiFile clicked.");
            try {
                if (Synth.Playing) {
                    stopSong();
                }
                callIntent(Env.MidiFileDirForIntent, (int) Request.MidiFile);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// buttonStart Click.
        /// </summary>
        void buttonStart_Click(object sender, EventArgs e) {
            Log.Info("buttonStart clicked.");
            try {
                if (!_midiFilePath.HasValue()) { // FIXME: case sounFdont
                    Log.Warn("midiFilePath has no value.");
                    return;
                }
                playSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// buttonStop Click.
        /// </summary>
        void buttonStop_Click(object sender, EventArgs e) {
            Log.Info("buttonStop clicked.");
            try {
                stopSong();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// buttonAddPlaylist Click.
        /// </summary>
        void buttonAddPlaylist_Click(object sender, EventArgs e) {
            Log.Info("buttonAddPlaylist clicked.");
            try {
                callIntent(Env.MidiFileDir, (int) Request.AddPlayList);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// buttonDeletePlaylist Click.
        /// </summary>
        void buttonDeletePlaylist_Click(object sender, EventArgs e) {
            Log.Info("buttonDeletePlaylist clicked.");
            try {
                _playList.Clear();
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// buttonSendSynth Click.
        /// </summary>
        void buttonSendSynth_Click(object sender, EventArgs e) {
            Log.Info("buttonSendSynth clicked.");
            try {
                Data data = new() {
                    Prog = FindViewById<NumberPicker>(Resource.Id.number_picker_prog).Value,
                    Pan = FindViewById<NumberPicker>(Resource.Id.number_picker_pan).Value,
                    Vol = FindViewById<NumberPicker>(Resource.Id.number_picker_vol).Value,
                };
                EventQueue.Enqueue((int) MidiChannel.ch1, data);
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }
    }
}
