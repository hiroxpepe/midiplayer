
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MidiPlayer {
    /// <summary>
    /// config for app
    /// </summary>
    public class Conf {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static readonly string APP_CONF_FILE_PATH = "storage/emulated/0/Android/data/com.studio.meowtoon.midiplayer/files/app_conf.json";

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

        public static void Load() {
            if (File.Exists(APP_CONF_FILE_PATH)) {
                using (var stream = new StreamReader(APP_CONF_FILE_PATH)) {
                    _json = loadJson(stream.ReadToEnd().ToMemoryStream());
                }
            } else {
                Synth synth = new Synth();
                synth.SoundFontDir = "undefined";
                synth.MidiFileDir = "undefined";
                App app = new App();
                app.PlayList = null;
                app.Synth = synth;
                _json = new Json();
                _json.App = app;
            }
        }

        public static void Save() {
            using (var stream = new FileStream(APP_CONF_FILE_PATH, FileMode.Create, FileAccess.Write)) {
                saveJson(stream);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

        static Json loadJson(Stream target) {
            var serializer = new DataContractJsonSerializer(typeof(Json));
            return (Json) serializer.ReadObject(target);
        }

        static void saveJson(Stream target) {
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(target, Encoding.UTF8, true, true)) {
                var serializer = new DataContractJsonSerializer(typeof(Json));
                serializer.WriteObject(writer, _json);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

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
        }
    }
}
