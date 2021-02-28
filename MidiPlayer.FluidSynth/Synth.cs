
using System;
using System.Linq;

using NativeFuncs;
using void_ptr = System.IntPtr;
using fluid_settings_t = System.IntPtr;
using fluid_synth_t = System.IntPtr;
using fluid_audio_driver_t = System.IntPtr;
using fluid_player_t = System.IntPtr;
using fluid_midi_event_t = System.IntPtr;

namespace MidiPlayer {

    public class Synth {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields

        static fluid_settings_t setting = IntPtr.Zero;

        static fluid_synth_t synth = IntPtr.Zero;

        static fluid_player_t player = IntPtr.Zero;

        static fluid_audio_driver_t adriver = IntPtr.Zero;

        static bool ready = false;

        static bool stopping = false;

        static Fluidsynth.handle_midi_event_func_t event_callback;

        static Func<IntPtr, IntPtr, int> onMessage;

        static Action onStart;

        static Action onEnd;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, adjective] 

        public static string SoundFontPath {
            get; set;
        }

        public static string MidiFilePath {
            get; set;
        }

        public static bool Playing {
            get => ready;
        }

        public static Func<IntPtr, IntPtr, int> OnMessage {
            get => onMessage;
            set {
                onMessage += value;
                onMessage += (void_ptr data, fluid_midi_event_t evt) => {
                    Enumerable.Range(0, 15).ToList().ForEach(x => {
                        var _data = EventQueue.Dequeue(x);
                        if (!(_data is null)) {
                            Fluidsynth.fluid_synth_program_change(synth, x, _data.Prog);
                            Fluidsynth.fluid_synth_cc(synth, x, (int) ControlChange.Pan, _data.Pan);
                            Fluidsynth.fluid_synth_cc(synth, x, (int) ControlChange.Volume, _data.Vol);
                        }
                    });
                    var _type = Fluidsynth.fluid_midi_event_get_type(evt);
                    var _channel = Fluidsynth.fluid_midi_event_get_channel(evt);
                    var _control = Fluidsynth.fluid_midi_event_get_control(evt);
                    var _value = Fluidsynth.fluid_midi_event_get_value(evt);
                    var _program = Fluidsynth.fluid_midi_event_get_program(evt);
                    // PROGRAM_CHANGE = 192 (merged drum trucks)
                    //     _type: 192, _program: 16 
                    // BANK_SELECT_MSB =  0 [-- drums: 127 --]
                    //     _type: 176, _control:  0, _value: 127
                    // BANK_SELECT_LSB = 32
                    //     _type: 176, _control: 32, _value:   0
                    // VOLUME_MSB      =  7
                    //     _type: 176, _control:  7, _value:  90 
                    // PAN_MSB         = 10
                    //     _type: 176, _control: 10, _value:  64 
                    if (_type != 128 && _type != 144) { // not note on or note off
                        Log.Info($"_type: {_type} _channel: {_channel} _control: {_control} _value: {_value} _program: {_program}");
                    }
                    return Fluidsynth.fluid_synth_handle_midi_event(data, evt);
                };
                event_callback = new Fluidsynth.handle_midi_event_func_t(onMessage);
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
                setting = Fluidsynth.new_fluid_settings();
                synth = Fluidsynth.new_fluid_synth(setting);
                player = Fluidsynth.new_fluid_player(synth);
                Log.Info($"try to load the sound font: {SoundFontPath}");
                if (Fluidsynth.fluid_is_soundfont(SoundFontPath) != 1) {
                    Log.Error("not a sound font.");
                    return;
                }
                Fluidsynth.fluid_player_set_playback_callback(player, event_callback, synth);
                int _sfont_id = Fluidsynth.fluid_synth_sfload(synth, SoundFontPath, true);
                if (_sfont_id == Fluidsynth.FLUID_FAILED) {
                    Log.Error("failed to load the sound font.");
                    return;
                } else {
                    Log.Info($"loaded the sound font: {SoundFontPath}");
                }
                Log.Info($"try to load the midi file: {MidiFilePath}");
                if (Fluidsynth.fluid_is_midifile(MidiFilePath) != 1) {
                    Log.Error("not a midi file.");
                    return;
                }
                int _result = Fluidsynth.fluid_player_add(player, MidiFilePath);
                if (_result == Fluidsynth.FLUID_FAILED) {
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
                adriver = Fluidsynth.new_fluid_audio_driver(setting, synth);
                Fluidsynth.fluid_player_play(player);
                Log.Info("start :)");
                onStart();
                Fluidsynth.fluid_player_join(player);
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
                    Fluidsynth.fluid_player_stop(player);
                }
                final();
                Log.Info("stop :|");
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        public static int HandleEvent(IntPtr data, IntPtr evt) {
            return Fluidsynth.fluid_synth_handle_midi_event(data, evt);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb]

        static void final() {
            try {
                Fluidsynth.delete_fluid_audio_driver(adriver);
                Fluidsynth.delete_fluid_player(player);
                Fluidsynth.delete_fluid_synth(synth);
                Fluidsynth.delete_fluid_settings(setting);
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
    }
}
