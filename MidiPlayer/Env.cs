
namespace MidiPlayer {
    public class Env {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        static string soundFontDir = "Music";

        static string midiFileDir = "Music";

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, adjective] 

        public static string SoundFontDir {
            get => soundFontDir.Replace("/", "%2F");
            set => soundFontDir = value.Replace("/storage/emulated/0/", "");
        }

        public static string MidiFileDir {
            get => midiFileDir.Replace("/", "%2F");
            set => midiFileDir = value.Replace("/storage/emulated/0/", "");
        }
    }
}
