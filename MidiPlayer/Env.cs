
namespace MidiPlayer {
    /// <summary>
    /// environment value for app
    /// </summary>
    public class Env {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static string _soundFontDir = "Music";

        static string _midiFileDir = "Music";

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        public static string SoundFontDir {
            get {
                if (!Conf.Value.Synth.SoundFontDir.Equals("undefined")) {
                    _soundFontDir = Conf.Value.Synth.SoundFontDir;
                }
                return _soundFontDir.Replace("/", "%2F");
            }
            set {
                _soundFontDir = value.Replace("/storage/emulated/0/", "");
                Conf.Value.Synth.SoundFontDir = _soundFontDir;
            }
        }

        public static string MidiFileDir {
            get {
                if (!Conf.Value.Synth.MidiFileDir.Equals("undefined")) {
                    _midiFileDir = Conf.Value.Synth.MidiFileDir;
                }
                return _midiFileDir.Replace("/", "%2F");
            }
            set {
                _midiFileDir = value.Replace("/storage/emulated/0/", "");
                Conf.Value.Synth.MidiFileDir = _midiFileDir;
            }
        }
    }
}
