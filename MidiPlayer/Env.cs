
using System.IO;

namespace MidiPlayer {
    /// <summary>
    /// environment value for the application.
    /// </summary>
    public class Env {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        public static string SoundFontDir {
            get => Conf.Value.Synth.SoundFontDir;
            set => Conf.Value.Synth.SoundFontDir = value;
        }

        public static string MidiFileDir {
            get => Conf.Value.Synth.MidiFileDir;
            set => Conf.Value.Synth.MidiFileDir = value;
        }

        public static string SoundFontDirForIntent {
            get {
                if (!ExistsSoundFont) {
                    return "Music";
                }
                return SoundFontDir.Replace("/storage/emulated/0/", "").Replace("/", "%2F");
            }
        }

        public static string MidiFileDirForIntent {
            get {
                if (!ExistsMidiFile) {
                    return "Music";
                }
                return MidiFileDir.Replace("/storage/emulated/0/", "").Replace("/", "%2F");
            }
        }

        public static string SoundFontName {
            get => Conf.Value.Synth.SoundFontName;
            set => Conf.Value.Synth.SoundFontName = value;
        }

        public static string MidiFileName {
            get => Conf.Value.Synth.MidiFileName;
            set => Conf.Value.Synth.MidiFileName = value;
        }

        public static string SoundFontPath {
            get => $"{SoundFontDir}/{SoundFontName}";
            set {
                SoundFontDir = value.ToDirectoryName();
                SoundFontName = value.ToFileName();
            }
        }

        public static string MidiFilePath {
            get => $"{MidiFileDir}/{MidiFileName}";
            set {
                MidiFileDir = value.ToDirectoryName();
                MidiFileName = value.ToFileName();
            }
        }

        public static bool ExistsSoundFont {
            get => File.Exists(SoundFontPath);
        }

        public static bool ExistsMidiFile {
            get => File.Exists(MidiFilePath);
        }
    }
}
