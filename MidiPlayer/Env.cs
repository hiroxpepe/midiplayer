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
    /// manages environment paths for the application.
    /// provides a hybrid path strategy: on Android (when <see cref="AppRootPath"/> is set), paths are
    /// resolved from the app-specific external files directory following the Meowziq folder hierarchy
    /// (<c>AppRootPath/Music/SoundFont</c> and <c>AppRootPath/Music/MIDI</c>);
    /// on Win64 (when <see cref="AppRootPath"/> is not set), paths delegate to the active configuration,
    /// preserving all existing Win64 behaviour without modification.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class Env {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants [nouns]

        /// <summary>
        /// the top-level music folder name used in the Android directory hierarchy.
        /// </summary>
        public const string MUSIC_FOLDER = "Music";

        /// <summary>
        /// the subfolder name under <see cref="MUSIC_FOLDER"/> that holds SoundFont files.
        /// </summary>
        public const string SOUNDFONT_FOLDER = "SoundFont";

        /// <summary>
        /// the subfolder name under <see cref="MUSIC_FOLDER"/> that holds MIDI files.
        /// </summary>
        public const string MIDI_FOLDER = "MIDI";

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective]

        /// <summary>
        /// the app-specific root directory injected at runtime by the Android platform.
        /// set this to <c>Activity.GetExternalFilesDir(null)?.AbsolutePath ?? string.Empty</c> in <c>OnCreate</c>
        /// before calling <see cref="Conf.Load"/>.
        /// when empty, all path properties fall back to the Win64 configuration values,
        /// so this property must never be set on Win64.
        /// </summary>
        /// <value>
        /// the absolute path to the app-specific external files directory on Android,
        /// or <see cref="string.Empty"/> on Win64 or when external storage is unavailable.
        /// </value>
        public static string AppRootPath { get; set; } = string.Empty;

        /// <summary>
        /// the SoundFont directory path.
        /// on Android (when <see cref="AppRootPath"/> is set) returns
        /// <c>AppRootPath/Music/SoundFont</c> built with <see cref="Path.Combine"/>;
        /// on Win64 returns the directory stored in the active configuration.
        /// the setter always writes to the active configuration regardless of platform.
        /// </summary>
        /// <value>the absolute path to the directory that contains SoundFont files.</value>
        public static string SoundFontDir {
            get {
                if (!string.IsNullOrEmpty(AppRootPath)) {
                    return Path.Combine(AppRootPath, MUSIC_FOLDER, SOUNDFONT_FOLDER);
                }
                return Conf.Value?.Synth?.SoundFontDir ?? "undefined";
            }
            set => Conf.Value.Synth.SoundFontDir = value;
        }

        /// <summary>
        /// the MIDI file directory path.
        /// on Android (when <see cref="AppRootPath"/> is set) returns
        /// <c>AppRootPath/Music/MIDI</c> built with <see cref="Path.Combine"/>;
        /// on Win64 returns the directory stored in the active configuration.
        /// the setter always writes to the active configuration regardless of platform.
        /// </summary>
        /// <value>the absolute path to the directory that contains MIDI files.</value>
        public static string MidiFileDir {
            get {
                if (!string.IsNullOrEmpty(AppRootPath)) {
                    return Path.Combine(AppRootPath, MUSIC_FOLDER, MIDI_FOLDER);
                }
                return Conf.Value?.Synth?.MidiFileDir ?? "undefined";
            }
            set => Conf.Value.Synth.MidiFileDir = value;
        }

        /// <summary>
        /// the SoundFont file name stored in the active configuration.
        /// returns <c>"undefined"</c> when the configuration value is <c>null</c>
        /// (e.g. on first launch before <c>app_conf.json</c> is created).
        /// </summary>
        /// <value>the file name (without directory) of the currently selected SoundFont.</value>
        public static string SoundFontName {
            get => Conf.Value?.Synth?.SoundFontName ?? "undefined";
            set => Conf.Value.Synth.SoundFontName = value;
        }

        /// <summary>
        /// the MIDI file name stored in the active configuration.
        /// returns <c>"undefined"</c> when the configuration value is <c>null</c>
        /// (e.g. on first launch before <c>app_conf.json</c> is created).
        /// </summary>
        /// <value>the file name (without directory) of the currently selected MIDI file.</value>
        public static string MidiFileName {
            get => Conf.Value?.Synth?.MidiFileName ?? "undefined";
            set => Conf.Value.Synth.MidiFileName = value;
        }

        /// <summary>
        /// the full path to the currently selected SoundFont file.
        /// combines <see cref="SoundFontDir"/> and <see cref="SoundFontName"/> with
        /// <see cref="Path.Combine"/> so the path separator is correct on every platform.
        /// setting this property splits the value and updates <see cref="SoundFontDir"/>
        /// and <see cref="SoundFontName"/> individually.
        /// </summary>
        /// <value>the absolute path to the SoundFont file.</value>
        public static string SoundFontPath {
            get => Path.Combine(SoundFontDir, SoundFontName);
            set {
                SoundFontDir = value.ToDirectoryName();
                SoundFontName = value.ToFileName();
            }
        }

        /// <summary>
        /// the full path to the currently selected MIDI file.
        /// combines <see cref="MidiFileDir"/> and <see cref="MidiFileName"/> with
        /// <see cref="Path.Combine"/> so the path separator is correct on every platform.
        /// setting this property splits the value and updates <see cref="MidiFileDir"/>
        /// and <see cref="MidiFileName"/> individually.
        /// </summary>
        /// <value>the absolute path to the MIDI file.</value>
        public static string MidiFilePath {
            get => Path.Combine(MidiFileDir, MidiFileName);
            set {
                MidiFileDir = value.ToDirectoryName();
                MidiFileName = value.ToFileName();
            }
        }

        /// <summary>
        /// returns <c>true</c> if the file at <see cref="SoundFontPath"/> exists on disk.
        /// </summary>
        /// <value><c>true</c> when the SoundFont file is present; otherwise <c>false</c>.</value>
        public static bool ExistsSoundFont => File.Exists(SoundFontPath);

        /// <summary>
        /// returns <c>true</c> if the file at <see cref="MidiFilePath"/> exists on disk.
        /// </summary>
        /// <value><c>true</c> when the MIDI file is present; otherwise <c>false</c>.</value>
        public static bool ExistsMidiFile => File.Exists(MidiFilePath);
    }
}
