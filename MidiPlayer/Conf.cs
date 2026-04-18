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

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MidiPlayer {
    /// <summary>
    /// config file for for the application.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class Conf {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        /// <summary>
        /// the deserialized JSON object; null until Load() is called.
        /// </summary>
        static Json _json = null;

        /// <summary>
        /// the external files directory injected by the Android platform at runtime.
        /// set via <see cref="SetAndroidFilesDir"/> before calling <see cref="Load"/>.
        /// </summary>
        static string? _android_files_dir;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // internal static Properties [noun, noun phrase, adjective] 

        /// <summary>
        /// returns true when the configuration has been loaded (i.e., _json is not null).
        /// </summary>
        internal static bool Ready {
            get => !(_json is null);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Properties [noun, noun phrase, adjective] 

        /// <summary>
        /// returns the root App configuration object, or null if the configuration has not been loaded.
        /// </summary>
        public static App Value {
            get {
                if (_json is null) {
                    return null;
                }
                return _json.App;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        /// <summary>
        /// set the external files directory for Android at runtime.
        /// must be called before <see cref="Load"/> on Android.
        /// </summary>
        /// <remarks>
        /// use Activity.GetExternalFilesDir(null).AbsolutePath to obtain the path.
        /// this avoids the Scoped Storage restriction introduced in Android 10 (API 29)
        /// that forbids hardcoded /storage/emulated/0/ paths.
        /// </remarks>
        public static void SetAndroidFilesDir(string path) {
            _android_files_dir = path;
        }

        /// <summary>
        /// load the app_conf.json file.
        /// </summary>
        public static void Load() {
            if (File.Exists(ConfEnv.ConfPath)) {
                using var stream = new StreamReader(ConfEnv.ConfPath);
                _json = loadJson(stream.ReadToEnd().ToMemoryStream());
                Log.Info("Conf loaded.");
                Log.Debug("Conf soundFontDir: " + _json.App.Synth.SoundFontDir);
                Log.Debug("Conf soundFontName: " + _json.App.Synth.SoundFontName);
                Log.Debug("Conf midiFileDir: " + _json.App.Synth.MidiFileDir);
                Log.Debug("Conf midiFileName: " + _json.App.Synth.MidiFileName);
            } else {
                Synth synth = new();
                synth.SoundFontDir = "undefined";
                synth.MidiFileDir = "undefined";
                App app = new();
                app.PlayList = null;
                app.Synth = synth;
                _json = new();
                _json.App = app;
            }
        }

        /// <summary>
        /// save the app_conf.json file.
        /// </summary>
        public static void Save() {
            if (!Directory.Exists(ConfEnv.ConfDir)) {
                Directory.CreateDirectory(ConfEnv.ConfDir);
            }
            using var stream = new FileStream(ConfEnv.ConfPath, FileMode.Create, FileAccess.Write);
            saveJson(stream);
            Log.Info("Conf saved.");
            Log.Debug("Conf soundFontDir: " + _json.App.Synth.SoundFontDir);
            Log.Debug("Conf soundFontName: " + _json.App.Synth.SoundFontName);
            Log.Debug("Conf midiFileDir: " + _json.App.Synth.MidiFileDir);
            Log.Debug("Conf midiFileName: " + _json.App.Synth.MidiFileName);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

        /// <summary>
        /// deserializes a JSON stream into a Json object.
        /// </summary>
        /// <param name="target">the stream containing JSON data to read.</param>
        /// <returns>the deserialized Json instance.</returns>
        static Json loadJson(Stream target) {
            var serializer = new DataContractJsonSerializer(typeof(Json));
            return (Json) serializer.ReadObject(target);
        }

        /// <summary>
        /// serializes the current Json object to the given stream.
        /// </summary>
        /// <param name="target">the stream to write the JSON data to.</param>
        static void saveJson(Stream target) {
            using var writer = JsonReaderWriterFactory.CreateJsonWriter(target, Encoding.UTF8, true, true);
            var serializer = new DataContractJsonSerializer(typeof(Json));
            serializer.WriteObject(writer, _json);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        /// <summary>
        /// resolves the configuration file path and directory for the current platform (Win64 or Android).
        /// </summary>
        class ConfEnv {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Const [nouns]

            /// <summary>
            /// the relative path to the configuration file on Windows 64-bit.
            /// </summary>
            const string WIN64_PATH = "conf\\app_conf.json";

            /// <summary>
            /// the configuration file name used on Android.
            /// </summary>
            const string ANDROID_CONF_FILE = "app_conf.json";

            /// <summary>
            /// fallback directory used when <see cref="Conf._android_files_dir"/> has not been injected.
            /// kept for reference only; production code must call SetAndroidFilesDir before Load.
            /// </summary>
            const string ANDROID_FALLBACK_DIR = "storage/emulated/0/Android/data/com.studio.meowtoon.midiplayer/files";

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective]

            /// <summary>
            /// the full path to the configuration file for the current platform.
            /// </summary>
            public static string ConfPath {
                get {
                    var os = Environment.OSVersion;
                    if (os.Platform == PlatformID.Win32NT) {
                        return WIN64_PATH;
                    } else if (os.Platform == PlatformID.Unix) {
                        var dir = _android_files_dir ?? ANDROID_FALLBACK_DIR;
                        return $"{dir}/{ANDROID_CONF_FILE}";
                    }
                    return string.Empty;
                }
            }

            /// <summary>
            /// the directory that contains the configuration file for the current platform.
            /// </summary>
            public static string ConfDir {
                get {
                    var os = Environment.OSVersion;
                    if (os.Platform == PlatformID.Win32NT) {
                        return WIN64_PATH.Replace("\\app_conf.json", string.Empty);
                    } else if (os.Platform == PlatformID.Unix) {
                        return _android_files_dir ?? ANDROID_FALLBACK_DIR;
                    }
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// the root JSON container that wraps the App configuration object.
        /// </summary>
        [DataContract]
        class Json {
            /// <summary>
            /// the application configuration data.
            /// </summary>
            [DataMember(Name = "app")]
            public App App {
                get; set;
            }
        }

        /// <summary>
        /// the application-level configuration data contract, containing synth settings and playlist.
        /// </summary>
        [DataContract]
        public class App {
            /// <summary>
            /// the synthesizer configuration settings.
            /// </summary>
            [DataMember(Name = "synth")]
            public Synth Synth {
                get; set;
            }
            /// <summary>
            /// the saved playlist of MIDI file paths.
            /// </summary>
            [DataMember(Name = "play_list")]
            public string[] PlayList {
                get; set;
            }
        }

        /// <summary>
        /// the synthesizer-specific configuration data contract holding SoundFont and MIDI file paths.
        /// </summary>
        [DataContract]
        public class Synth {
            /// <summary>
            /// the directory path of the last used SoundFont file.
            /// </summary>
            [DataMember(Name = "sound_font_dir")]
            public string SoundFontDir {
                get; set;
            }
            /// <summary>
            /// the directory path of the last used MIDI file.
            /// </summary>
            [DataMember(Name = "midi_file_dir")]
            public string MidiFileDir {
                get; set;
            }
            /// <summary>
            /// the file name of the last used SoundFont file.
            /// </summary>
            [DataMember(Name = "sound_font_name")]
            public string SoundFontName {
                get; set;
            }
            /// <summary>
            /// the file name of the last used MIDI file.
            /// </summary>
            [DataMember(Name = "midi_file_name")]
            public string MidiFileName {
                get; set;
            }
        }
    }
}
