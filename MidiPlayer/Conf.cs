
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MidiPlayer {
    /// <summary>
    /// config file for for the application.
    /// </summary>
    public class Conf {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static Json _json = null;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        public static bool Ready {
            get => !(_json is null);
        }

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

        static Json loadJson(Stream target) {
            var serializer = new DataContractJsonSerializer(typeof(Json));
            return (Json) serializer.ReadObject(target);
        }

        static void saveJson(Stream target) {
            using var writer = JsonReaderWriterFactory.CreateJsonWriter(target, Encoding.UTF8, true, true);
            var serializer = new DataContractJsonSerializer(typeof(Json));
            serializer.WriteObject(writer, _json);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class ConfEnv {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Fields [nouns, noun phrases]

            const string WIN64_PATH = "conf\\app_conf.json";//"conf\\app_conf.json";

            const string ANDROID_PATH = "storage/emulated/0/Android/data/com.studio.meowtoon.midiplayer/files/app_conf.json";

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective] 

            public static string ConfPath {
                get {
                    var os = Environment.OSVersion;
                    if (os.Platform == PlatformID.Win32NT) {
                        return WIN64_PATH;
                    } else if (os.Platform == PlatformID.Unix) {
                        return ANDROID_PATH;
                    }
                    return string.Empty;
                }
            }

            public static string ConfDir {
                get {
                    var os = Environment.OSVersion;
                    if (os.Platform == PlatformID.Win32NT) {
                        return WIN64_PATH.Replace("\\app_conf.json", "");
                    } else if (os.Platform == PlatformID.Unix) {
                        return ANDROID_PATH.Replace("/app_conf.json", "");
                    }
                    return string.Empty;
                }
            }
        }

        [DataContract]
        class Json {
            [DataMember(Name = "app")]
            public App App {
                get; set;
            }
        }

        [DataContract]
        public class App {
            [DataMember(Name = "synth")]
            public Synth Synth {
                get; set;
            }
            [DataMember(Name = "play_list")]
            public string[] PlayList {
                get; set;
            }
        }

        [DataContract]
        public class Synth {
            [DataMember(Name = "sound_font_dir")]
            public string SoundFontDir {
                get; set;
            }
            [DataMember(Name = "midi_file_dir")]
            public string MidiFileDir {
                get; set;
            }
            [DataMember(Name = "sound_font_name")]
            public string SoundFontName {
                get; set;
            }
            [DataMember(Name = "midi_file_name")]
            public string MidiFileName {
                get; set;
            }
        }
    }
}
