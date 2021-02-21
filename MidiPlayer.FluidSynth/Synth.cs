
using System;

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
        // Fields

        static fluid_settings_t setting = IntPtr.Zero;

        static fluid_synth_t synth = IntPtr.Zero;

        static fluid_player_t player = IntPtr.Zero;

        static fluid_audio_driver_t adriver = IntPtr.Zero;

        static bool ready = false;

        static int cont = 0;
        static Fluidsynth.handle_midi_event_func_t event_callback = (void_ptr data, fluid_midi_event_t evt) => {
            Log.Info(cont.ToString());
            cont++;
            return Fluidsynth.fluid_synth_handle_midi_event(synth, evt);
        };

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, adjective] 

        public static string SoundFontPath {
            get; set;
        }

        public static string MidiFilePath {
            get; set;
        }

        public static bool Playing {
            get => ready;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

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
                }
                adriver = Fluidsynth.new_fluid_audio_driver(setting, synth); // start the synthesizer thread
                Fluidsynth.fluid_player_play(player); // play the midi files, if any
                Log.Info("start :)");
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        public static void Stop() {
            try {
                if (!player.IsZero()) {
                    Fluidsynth.fluid_player_stop(player);
                }
                final();
                Log.Info("stop :|");
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

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
            }
        }
    }
}
