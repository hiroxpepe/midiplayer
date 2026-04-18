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

using System.IO;

namespace MidiPlayer {
    /// <summary>
    /// environment value for the application.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class Env {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        /// <summary>
        /// the SoundFont directory path from the active configuration.
        /// </summary>
        public static string SoundFontDir {
            get => Conf.Value.Synth.SoundFontDir;
            set => Conf.Value.Synth.SoundFontDir = value;
        }

        /// <summary>
        /// the MIDI file directory path from the active configuration.
        /// </summary>
        public static string MidiFileDir {
            get => Conf.Value.Synth.MidiFileDir;
            set => Conf.Value.Synth.MidiFileDir = value;
        }

        /// <summary>
        /// the SoundFont directory path in URL-encoded form suitable for use in Android Intents.
        /// returns "Music" when no SoundFont is configured.
        /// </summary>
        public static string SoundFontDirForIntent {
            get {
                if (!ExistsSoundFont) {
                    return "Music";
                }
                return SoundFontDir.Replace("/storage/emulated/0/", string.Empty).Replace("/", "%2F");
            }
        }

        /// <summary>
        /// the MIDI file directory path in URL-encoded form suitable for use in Android Intents.
        /// returns "Music" when no MIDI file is configured.
        /// </summary>
        public static string MidiFileDirForIntent {
            get {
                if (!ExistsMidiFile) {
                    return "Music";
                }
                return MidiFileDir.Replace("/storage/emulated/0/", string.Empty).Replace("/", "%2F");
            }
        }

        /// <summary>
        /// the SoundFont file name from the active configuration.
        /// </summary>
        public static string SoundFontName {
            get => Conf.Value.Synth.SoundFontName;
            set => Conf.Value.Synth.SoundFontName = value;
        }

        /// <summary>
        /// the MIDI file name from the active configuration.
        /// </summary>
        public static string MidiFileName {
            get => Conf.Value.Synth.MidiFileName;
            set => Conf.Value.Synth.MidiFileName = value;
        }

        /// <summary>
        /// the full path to the SoundFont file (directory + name).
        /// setting this property updates SoundFontDir and SoundFontName separately.
        /// </summary>
        public static string SoundFontPath {
            get => $"{SoundFontDir}/{SoundFontName}";
            set {
                SoundFontDir = value.ToDirectoryName();
                SoundFontName = value.ToFileName();
            }
        }

        /// <summary>
        /// the full path to the MIDI file (directory + name).
        /// setting this property updates MidiFileDir and MidiFileName separately.
        /// </summary>
        public static string MidiFilePath {
            get => $"{MidiFileDir}/{MidiFileName}";
            set {
                MidiFileDir = value.ToDirectoryName();
                MidiFileName = value.ToFileName();
            }
        }

        /// <summary>
        /// returns true if the SoundFont file exists at SoundFontPath.
        /// </summary>
        public static bool ExistsSoundFont {
            get => File.Exists(SoundFontPath);
        }

        /// <summary>
        /// returns true if the MIDI file exists at MidiFilePath.
        /// </summary>
        public static bool ExistsMidiFile {
            get => File.Exists(MidiFilePath);
        }
    }
}
