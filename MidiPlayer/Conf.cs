
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

        static Json json = null;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        public static bool Ready {
            get => !(json is null);
        }

        public static App Value {
            get {
                if (json is null) {
                    return null;
                }
                return json.App;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Load() {
            if (File.Exists(APP_CONF_FILE_PATH)) {
                using (var _stream = new StreamReader(APP_CONF_FILE_PATH)) {
                    json = loadJson(_stream.ReadToEnd().ToMemoryStream());
                }
            } else {
                Synth _synth = new Synth();
                _synth.SoundFontDir = "undefined";
                _synth.MidiFileDir = "undefined";
                App _app = new App();
                _app.PlayList = null;
                _app.Synth = _synth;
                json = new Json();
                json.App = _app;
            }
        }

        public static void Save() {
            using (var _stream = new FileStream(APP_CONF_FILE_PATH, FileMode.Create, FileAccess.Write)) {
                saveJson(_stream);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

        static Json loadJson(Stream target) {
            var _serializer = new DataContractJsonSerializer(typeof(Json));
            return (Json) _serializer.ReadObject(target);
        }

        static void saveJson(Stream target) {
            using (var _writer = JsonReaderWriterFactory.CreateJsonWriter(target, Encoding.UTF8, true, true)) {
                var _serializer = new DataContractJsonSerializer(typeof(Json));
                _serializer.WriteObject(_writer, json);
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
