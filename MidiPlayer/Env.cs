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
    public static class Env {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants [nouns]

        public const string MUSIC_FOLDER = "Music";
        public const string SOUNDFONT_FOLDER = "SoundFont";
        public const string MIDI_FOLDER = "MIDI";

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective]

        /// <summary>
        /// app-specific root directory (set by Android OnCreate before Conf.Load).
        /// </summary>
        public static string AppRootPath { get; set; } = string.Empty;

        public static string SoundFontDir {
            get {
                if (!string.IsNullOrEmpty(AppRootPath)) {
                    return Path.Combine(AppRootPath, MUSIC_FOLDER, SOUNDFONT_FOLDER);
                }
                return Conf.Value?.Synth?.SoundFontDir ?? "undefined";
            }
            set => Conf.Value.Synth.SoundFontDir = value;
        }

        public static string MidiFileDir {
            get {
                if (!string.IsNullOrEmpty(AppRootPath)) {
                    return Path.Combine(AppRootPath, MUSIC_FOLDER, MIDI_FOLDER);
                }
                return Conf.Value?.Synth?.MidiFileDir ?? "undefined";
            }
            set => Conf.Value.Synth.MidiFileDir = value;
        }

        public static string SoundFontName {
            get => Conf.Value?.Synth?.SoundFontName ?? "undefined";
            set => Conf.Value.Synth.SoundFontName = value;
        }

        public static string MidiFileName {
            get => Conf.Value?.Synth?.MidiFileName ?? "undefined";
            set => Conf.Value.Synth.MidiFileName = value;
        }

        public static string SoundFontPath {
            get => Path.Combine(SoundFontDir, SoundFontName);
            set {
                SoundFontDir = value.ToDirectoryName() ?? string.Empty;
                SoundFontName = value.ToFileName() ?? string.Empty;
            }
        }

        public static string MidiFilePath {
            get => Path.Combine(MidiFileDir, MidiFileName);
            set {
                MidiFileDir = value.ToDirectoryName() ?? string.Empty;
                MidiFileName = value.ToFileName() ?? string.Empty;
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
