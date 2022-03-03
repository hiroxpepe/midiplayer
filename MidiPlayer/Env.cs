
using System.IO;

namespace MidiPlayer {
    /// <summary>
    /// environment value for the application.
    /// </summary>
    public class Env {
#nullable enable

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
                return _soundFontDir;
            }
            set {
                Conf.Value.Synth.SoundFontDir = value;
            }
        }

        public static string MidiFileDir {
            get {
                if (!Conf.Value.Synth.MidiFileDir.Equals("undefined")) {
                    _midiFileDir = Conf.Value.Synth.MidiFileDir;
                }
                return _midiFileDir;
            }
            set {
                Conf.Value.Synth.MidiFileDir = value;
            }
        }

        public static string SoundFontDirForIntent {
            get {
                return _soundFontDir.Replace("/storage/emulated/0/", "").Replace("/", "%2F");
            }
        }

        public static string MidiFileDirForIntent {
            get {
                return _midiFileDir.Replace("/storage/emulated/0/", "").Replace("/", "%2F");
            }
        }

        public static string SoundFontName {
            get {
                return Conf.Value.Synth.SoundFontName;
            }
            set {
                Conf.Value.Synth.SoundFontName = value;
            }
        }

        public static string MidiFileName {
            get {
                return Conf.Value.Synth.MidiFileName;
            }
            set {
                Conf.Value.Synth.MidiFileName = value;
            }
        }

        public static string SoundFontPath {
            get {
                return $"{SoundFontDir}/{SoundFontName}"; // TODO: Win64
            }
        }

        public static string MidiFilePath {
            get {
                return $"{MidiFileDir}/{MidiFileName}"; // TODO: Win64
            }
        }

        public static bool ExistsSoundFont {
            get {
                return File.Exists(SoundFontPath);
            }
        }

        public static bool ExistsMidiFile {
            get {
                return File.Exists(MidiFilePath);
            }
        }
    }
}
