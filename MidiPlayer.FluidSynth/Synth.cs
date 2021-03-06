
using System;
using System.Collections.Generic;
using System.Linq;

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

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields

        static fluid_settings_t setting = IntPtr.Zero;

        static fluid_synth_t synth = IntPtr.Zero;

        static fluid_player_t player = IntPtr.Zero;

        static fluid_audio_driver_t adriver = IntPtr.Zero;

        static handle_midi_event_func_t event_callback;

        static Func<IntPtr, IntPtr, int> onMessage;

        static Action onStart;

        static Action onEnd;

        static string soundFontPath;

        static string midiFilePath;

        static SoundFontInfo soundFontInfo;

        static StandardMidiFile standardMidiFile;

        static bool ready = false;

        static bool stopping = false;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static Synth() {
            onMessage += (void_ptr data, fluid_midi_event_t evt) => {
                Enumerable.Range(0, 16).ToList().ForEach(x => {
                    var _data = EventQueue.Dequeue(x);
                    if (!(_data is null)) {
                        fluid_synth_program_change(synth, x, _data.Prog);
                        fluid_synth_cc(synth, x, (int) ControlChange.Pan, _data.Pan);
                        fluid_synth_cc(synth, x, (int) ControlChange.Volume, _data.Vol);
                    }
                });
                var _type = fluid_midi_event_get_type(evt);
                var _channel = fluid_midi_event_get_channel(evt);
                var _control = fluid_midi_event_get_control(evt);
                var _value = fluid_midi_event_get_value(evt);
                var _program = fluid_midi_event_get_program(evt);
                if (_type != 128 && _type != 144) { // not note on or note off
                    Log.Info($"_type: {_type} _channel: {_channel} _control: {_control} _value: {_value} _program: {_program}");
                }
                if (_type == 144) { // NOTE_ON = 144
                    Multi.ApplyNoteOn(_channel);
                } else if (_type == 128) { // NOTE_OFF = 128
                    Multi.ApplyNoteOff(_channel);
                } else if (_type == 192) { // PROGRAM_CHANGE = 192
                    Multi.ApplyProgramChange(_channel, _program);
                } else if (_type == 176) { // CONTROL_CHANGE = 176
                    Multi.ApplyControlChange(_channel, _control, _value);
                }
                return fluid_synth_handle_midi_event(data, evt);
            };
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, adjective] 

        public static string SoundFontPath {
            get => soundFontPath;
            set {
                soundFontPath = value;
                soundFontInfo = new SoundFontInfo(soundFontPath);
            }
        }

        public static string MidiFilePath {
            get => midiFilePath;
            set {
                midiFilePath = value;
                standardMidiFile = new StandardMidiFile(midiFilePath);
            }
        }

        public static bool Playing {
            get => ready;
        }

        public static Func<IntPtr, IntPtr, int> OnMessage {
            get => onMessage;
            set {
                onMessage += value;
                event_callback = new handle_midi_event_func_t(onMessage);
            }
        }

        public static Action OnStart {
            get => onStart;
            set => onStart += value;
        }

        public static Action OnEnd {
            get => onEnd;
            set => onEnd += value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb]

        public static void Init() {
            try {
                if (!SoundFontPath.HasValue() || !MidiFilePath.HasValue()) {
                    Log.Warn("no sound font or no midi file.");
                    return;
                }
                setting = new_fluid_settings();
                synth = new_fluid_synth(setting);
                player = new_fluid_player(synth);
                Log.Info($"try to load the sound font: {SoundFontPath}");
                if (fluid_is_soundfont(SoundFontPath) != 1) {
                    Log.Error("not a sound font.");
                    return;
                }
                fluid_player_set_playback_callback(player, event_callback, synth);
                int _sfont_id = fluid_synth_sfload(synth, SoundFontPath, true);
                if (_sfont_id == FLUID_FAILED) {
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
                int _result = fluid_player_add(player, MidiFilePath);
                if (_result == FLUID_FAILED) {
                    Log.Error("failed to load the midi file.");
                    return;
                } else {
                    Log.Info($"loaded the midi file: {MidiFilePath}");
                }
                ready = true;
                Log.Info("init :)");
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        public static void Start() {
            try {
                if (!ready) {
                    Init();
                    if (!ready) {
                        Log.Error("failed to init.");
                        return;
                    }
                }
                adriver = new_fluid_audio_driver(setting, synth);
                fluid_player_play(player);
                Log.Info("start :)");
                onStart();
                fluid_player_join(player);
                Log.Info("end :D");
                if (stopping == false) {
                    onEnd();
                }
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        public static void Stop() {
            try {
                if (!player.IsZero()) {
                    stopping = true;
                    fluid_player_stop(player);
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
            var _channel = fluid_midi_event_get_channel(evt);
            return _channel;
        }

        public static int GetBank(int channel) {
            var _bank = Multi.Get(channel).Bank;
            if (_bank == -1) { // unset BANK_SELECT_LSB = 32
                _bank = 0;
            }
            return _bank;
        }

        public static int GetProgram(int channel) {
            var _program = Multi.Get(channel).Program;
            return _program;
        }

        public static string GetVoice(int channel) {
            var _bank = GetBank(channel);
            var _program = GetProgram(channel);
            var _voice = soundFontInfo.GetInstrumentName(_bank, _program); 
            return _voice;
        }

        public static string GetTrackName(int channel) {
            var _trackName = standardMidiFile.GetTrackName(channel);
            return _trackName;
        }

        public static bool IsSounded(int channel) {
            return Multi.Get(channel).Sounds;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb]

        static void final() {
            try {
                delete_fluid_audio_driver(adriver);
                delete_fluid_player(player);
                delete_fluid_synth(synth);
                delete_fluid_settings(setting);
                adriver = IntPtr.Zero;
                player = IntPtr.Zero;
                synth = IntPtr.Zero;
                setting = IntPtr.Zero;
                Log.Info("final :|");
            } catch (Exception ex) {
                Log.Error(ex.Message);
            } finally {
                ready = false;
                stopping = false;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        static class Multi {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Fields

            static Map<int, Track> trackMap;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Constructor

            static Multi() {
                trackMap = new Map<int, Track>();
                Enumerable.Range(0, 16).ToList().ForEach(x => trackMap.Add(x, new Track()));
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Properties [noun, adjective]

            public static List<Track> List {
                get => trackMap.Select(x => x.Value).ToList();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // public static Methods [verb]

            /// <summary>
            /// NOTE_ON = 144
            /// </summary>
            public static void ApplyNoteOn(int channel) {
                trackMap[channel].Sounds = true;
            }

            /// <summary>
            /// NOTE_OFF = 128
            /// </summary>
            public static void ApplyNoteOff(int channel) {
                trackMap[channel].Sounds = false;
            }

            /// <summary>
            /// PROGRAM_CHANGE = 192
            /// </summary>
            public static void ApplyProgramChange(int channel, int program) {
                trackMap[channel].Program = program;
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
                            trackMap[channel].Bank = value + 1; // 128
                        }
                        break;
                    case 32: // BANK_SELECT_LSB
                        if (channel != 9) { // not Drum
                            trackMap[channel].Bank = value;
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

            public static Track Get(int channel) {
                return trackMap[channel];
            }
        }

        class Track {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields

            bool sounds = false;

            int bank = -1;

            int program = -1;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjective]

            public bool Sounds {
                get => sounds;
                set => sounds = value;
            }

            public int Bank {
                get => bank;
                set => bank = value;
            }

            public int Program {
                get => program;
                set => program = value;
            }
        }
    }
}
