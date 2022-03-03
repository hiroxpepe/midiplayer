
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using static NativeFuncs.Fluidsynth;
using void_ptr = System.IntPtr;
using fluid_settings_t = System.IntPtr;
using fluid_synth_t = System.IntPtr;
using fluid_audio_driver_t = System.IntPtr;
using fluid_player_t = System.IntPtr;
using fluid_midi_event_t = System.IntPtr;
using MidiPlayer.Midi;
using MidiPlayer.SoundFont;

namespace MidiPlayer {

    public class Synth {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static fluid_settings_t _setting = IntPtr.Zero;

        static fluid_synth_t _synth = IntPtr.Zero;

        static fluid_player_t _player = IntPtr.Zero;

        static fluid_audio_driver_t _adriver = IntPtr.Zero;

        static handle_midi_event_func_t _event_callback;

        static Func<IntPtr, IntPtr, int> _onPlaybacking;

        static Action _onStarted;

        static Action _onEnded;

        static Action<object, PropertyChangedEventArgs> _onUpdated;

        static string _soundFontPath;

        static string _midiFilePath;

        static SoundFontInfo _soundFontInfo;

        static StandardMidiFile _standardMidiFile;

        static bool _ready = false;

        static bool _stopping = false;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static Synth() {
            _onPlaybacking += (void_ptr data, fluid_midi_event_t evt) => {
                Enumerable.Range(0, 16).ToList().ForEach(x => {
                    var eventData = EventQueue.Dequeue(x);
                    if (!(eventData is null)) {
                        fluid_synth_program_change(_synth, x, eventData.Prog);
                        fluid_synth_cc(_synth, x, (int) ControlChange.Pan, eventData.Pan);
                        fluid_synth_cc(_synth, x, (int) ControlChange.Volume, eventData.Vol);
                    }
                });
                var type = fluid_midi_event_get_type(evt);
                var channel = fluid_midi_event_get_channel(evt);
                var control = fluid_midi_event_get_control(evt);
                var value = fluid_midi_event_get_value(evt);
                var program = fluid_midi_event_get_program(evt);
                if (type != 128 && type != 144) { // not note on or note off
                    Log.Info($"_type: {type} _channel: {channel} _control: {control} _value: {value} _program: {program}");
                }
                Task.Run(() => {
                    if (type == 144) { // NOTE_ON = 144
                        Multi.ApplyNoteOn(channel);
                    } else if (type == 128) { // NOTE_OFF = 128
                        Multi.ApplyNoteOff(channel);
                    } else if (type == 192) { // PROGRAM_CHANGE = 192
                        Multi.ApplyProgramChange(channel, program);
                    } else if (type == 176) { // CONTROL_CHANGE = 176
                        Multi.ApplyControlChange(channel, control, value);
                    }
                });
                return fluid_synth_handle_midi_event(data, evt);
            };
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        public static string SoundFontPath {
            get => _soundFontPath;
            set {
                _soundFontPath = value;
                _soundFontInfo = new SoundFontInfo(_soundFontPath);
            }
        }

        public static string MidiFilePath {
            get => _midiFilePath;
            set {
                _midiFilePath = value;
                _standardMidiFile = new StandardMidiFile(_midiFilePath);
            }
        }

        public static List<int> MidiChannelList {
            get => _standardMidiFile.MidiChannelList;
        }

        public static int TrackCount {
            get => _standardMidiFile.TrackCount;
        }

        public static bool Playing {
            get => _ready;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Events [verb, verb phrase] 

        public static event Func<IntPtr, IntPtr, int> Playbacking {
            add {
                _onPlaybacking += value;
                _event_callback = new handle_midi_event_func_t(_onPlaybacking);
            }
            remove => _onPlaybacking -= value;
        }

        public static event Action Started {
            add => _onStarted += value;
            remove => _onStarted -= value;
        }

        public static event Action Ended {
            add => _onEnded += value;
            remove => _onEnded -= value;
        }

        public static event Action<object, PropertyChangedEventArgs> Updated {
            add => _onUpdated += value;
            remove => _onUpdated -= value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Init() {
            try {
                if (!SoundFontPath.HasValue() || !MidiFilePath.HasValue()) {
                    Log.Warn("no sound font or no midi file.");
                    return;
                }
                _setting = new_fluid_settings();
                _synth = new_fluid_synth(_setting);
                _player = new_fluid_player(_synth);
                Log.Info($"try to load the sound font: {SoundFontPath}");
                if (fluid_is_soundfont(SoundFontPath) != 1) {
                    Log.Error("not a sound font.");
                    return;
                }
                fluid_player_set_playback_callback(_player, _event_callback, _synth);
                int sfont_id = fluid_synth_sfload(_synth, SoundFontPath, true);
                if (sfont_id == FLUID_FAILED) {
                    Log.Error("failed to load the sound font.");
                    return;
                } else {
                    Log.Info($"loaded the sound font: {SoundFontPath}");
                }
                Log.Info($"try to load the midi file: {MidiFilePath}");
                if (fluid_is_midifile(MidiFilePath) != 1) {
                    Log.Error("not a midi file.");
                    return;
                }
                Multi.StandardMidiFile = _standardMidiFile;
                Enumerable.Range(0, 16).ToList().ForEach(x => {
                    Multi.Get(x).PropertyChanged += onPropertyChanged;
                });
                int result = fluid_player_add(_player, MidiFilePath);
                if (result == FLUID_FAILED) {
                    Log.Error("failed to load the midi file.");
                    return;
                } else {
                    Log.Info($"loaded the midi file: {MidiFilePath}");
                }
                _ready = true;
                Log.Info("init :)");
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        public static void Start() {
            try {
                if (!_ready) {
                    Init();
                    if (!_ready) {
                        Log.Error("failed to init.");
                        return;
                    }
                }
                _adriver = new_fluid_audio_driver(_setting, _synth);
                fluid_player_play(_player);
                Log.Info("start :)");
                _onStarted();
                fluid_player_join(_player);
                Log.Info("end :D");
                if (_stopping == false) {
                    _onEnded();
                }
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        public static void Stop() {
            try {
                if (!_player.IsZero()) {
                    _stopping = true;
                    fluid_player_stop(_player);
                }
                final();
                Log.Info("stop :|");
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        public static int HandleEvent(IntPtr data, IntPtr evt) {
            return fluid_synth_handle_midi_event(data, evt);
        }

        public static int GetChannel(IntPtr evt) {
            var channel = fluid_midi_event_get_channel(evt);
            return channel;
        }

        public static int GetChannel(int track) {
            var channel = Multi.Get(track).Channel;
            return channel;
        }

        public static int GetBank(int track) {
            var bank = Multi.Get(track).Bank;
            if (bank == -1) { // unset BANK_SELECT_LSB = 32
                bank = 0;
            }
            return bank;
        }

        public static int GetProgram(int track) {
            var program = Multi.Get(track).Program;
            return program;
        }

        public static string GetVoice(int track) {
            var bank = GetBank(track);
            var program = GetProgram(track);
            var voice = _soundFontInfo.GetVoice(bank, program); 
            return voice;
        }

        public static string GetTrackName(int track) {
            var name = Multi.Get(track).Name;
            return name;
        }

        public static bool IsSounded(int channel) {
            return Multi.Get(channel).Sounds;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

        static void final() {
            try {
                delete_fluid_audio_driver(_adriver);
                delete_fluid_player(_player);
                delete_fluid_synth(_synth);
                delete_fluid_settings(_setting);
                _adriver = IntPtr.Zero;
                _player = IntPtr.Zero;
                _synth = IntPtr.Zero;
                _setting = IntPtr.Zero;
                Log.Info("final :|");
            } catch (Exception ex) {
                Log.Error(ex.Message);
            } finally {
                _ready = false;
                _stopping = false;
            }
        }

        static void onPropertyChanged(object sender, PropertyChangedEventArgs e) {
            _onUpdated(sender, e);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        static class Multi {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Fields [nouns, noun phrases]

            static Map<int, Track> _trackMap;

            static StandardMidiFile _standardMidiFile;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Constructor

            static Multi() {
                _trackMap = new Map<int, Track>();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Properties [noun, noun phrase, adjective]

            public static List<Track> List {
                get => _trackMap.Select(x => x.Value).ToList();
            }

            public static StandardMidiFile StandardMidiFile {
                get => _standardMidiFile;
                set {
                    _standardMidiFile = value;
                    init();
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // public static Methods [verb, verb phrases]

            /// <summary>
            /// NOTE_ON = 144
            /// </summary>
            public static void ApplyNoteOn(int channel) {
                _trackMap.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Sounds = true);
            }

            /// <summary>
            /// NOTE_OFF = 128
            /// </summary>
            public static void ApplyNoteOff(int channel) {
                _trackMap.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Sounds = false);
            }

            /// <summary>
            /// PROGRAM_CHANGE = 192
            /// </summary>
            public static void ApplyProgramChange(int channel, int program) {
                _trackMap.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Program = program);
            }

            /// <summary>
            /// CONTROL_CHANGE = 176
            /// </summary>
            public static void ApplyControlChange(int channel, int control, int value) {
                // BANK_SELECT_MSB =  0 [-- drums: 127 --]
                //     _type: 176, _control:  0, _value: 127
                // BANK_SELECT_LSB = 32
                //     _type: 176, _control: 32, _value:   0
                // VOLUME_MSB      =  7
                //     _type: 176, _control:  7, _value:  90 
                // PAN_MSB         = 10
                //     _type: 176, _control: 10, _value:  64 
                switch (control) {
                    case 0: // BANK_SELECT_MSB
                        if (channel == 9) { // Drum
                            _trackMap.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Bank = value + 1); // 128
                        }
                        break;
                    case 32: // BANK_SELECT_LSB
                        if (channel != 9) { // not Drum
                            _trackMap.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Bank = value);
                        }
                        break;
                    case 7: // VOLUME_MSB
                        break;
                    case 10: // PAN_MSB
                        break;
                    default:
                        break;
                }
            }

            public static Track Get(int index) {
                var track = _trackMap[index];
                return track;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // private static Methods [verb, verb phrases]

            static void init() {
                _trackMap.Clear();
                Enumerable.Range(0, 16).ToList().ForEach(x => _trackMap.Add(x, new Track(x)));
                var list = _standardMidiFile.MidiChannelList;
                _trackMap[0].Name = _standardMidiFile.GetTrackName(0);
                for (var idx = 0; idx < MidiChannelList.Count; idx++) {
                    _trackMap[idx + 1].Channel = list[idx]; // exclude conductor track;
                    _trackMap[idx + 1].Name = _standardMidiFile.GetTrackName(idx + 1);
                }
            }
        }

        public class Track : INotifyPropertyChanged {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields [nouns, noun phrases]

            int _index = -1;

            bool _sounds = false;

            string _name = "undefined";

            int _channel = -1;

            int _bank = 0;

            int _program = 0;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            public Track(int index) {
                _index = index;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Events [verb, verb phrase] 

            /// <summary>
            /// implementation for INotifyPropertyChanged
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective]

            public int Index {
                get => _index;
                set {
                    _index = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Index)));
                }
            }

            public bool Sounds {
                get => _sounds;
                set {
                    _sounds = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sounds)));
                }
            }

            public string Name {
                get => _name;
                set {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }

            public int Channel {
                get => _channel;
                set {
                    _channel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Channel)));
                }
            }

            public int Bank {
                get {
                    if (_channel == 9 && _bank != 128) {
                        return 128; // Drum
                    }
                    return _bank;
                }
                set {
                    _bank = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bank)));
                }
            }

            public int Program {
                get => _program;
                set {
                    _program = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Program)));
                }
            }
        }
    }
}
