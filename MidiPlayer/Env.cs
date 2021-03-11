
namespace MidiPlayer {
    /// <summary>
    /// environment value for app
    /// </summary>
    public class Env {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static string soundFontDir = "Music";

        static string midiFileDir = "Music";

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        public static string SoundFontDir {
            get {
                if (!Conf.Value.Synth.SoundFontDir.Equals("undefined")) {
                    soundFontDir = Conf.Value.Synth.SoundFontDir;
                }
                return soundFontDir.Replace("/", "%2F");
            }
            set {
                soundFontDir = value.Replace("/storage/emulated/0/", "");
                Conf.Value.Synth.SoundFontDir = soundFontDir;
            }
        }

        public static string MidiFileDir {
            get {
                if (!Conf.Value.Synth.MidiFileDir.Equals("undefined")) {
                    midiFileDir = Conf.Value.Synth.MidiFileDir;
                }
                return midiFileDir.Replace("/", "%2F");
            }
            set {
                midiFileDir = value.Replace("/storage/emulated/0/", "");
                Conf.Value.Synth.MidiFileDir = midiFileDir;
            }
        }
    }
}
