
using System.Runtime.InteropServices;
using void_ptr = System.IntPtr;
using fluid_settings_t = System.IntPtr;
using fluid_synth_t = System.IntPtr;
using fluid_audio_driver_t = System.IntPtr;
using fluid_player_t = System.IntPtr;
using fluid_midi_event_t = System.IntPtr;

namespace NativeFuncs {

    internal static class Fluidsynth {

        const UnmanagedType LP_Str = UnmanagedType.LPStr;

        internal const int FLUID_OK = 0;

        internal const int FLUID_FAILED = -1;

        [DllImport("libfluidsynth.so")]
        internal static extern fluid_settings_t new_fluid_settings();

        [DllImport("libfluidsynth.so")]
        internal static extern void delete_fluid_settings(fluid_settings_t settings);

        [DllImport("libfluidsynth.so")]
        internal static extern fluid_synth_t new_fluid_synth(fluid_settings_t settings);

        [DllImport("libfluidsynth.so")]
        internal static extern void delete_fluid_synth(fluid_synth_t synth);

        [DllImport("libfluidsynth.so")]
        internal static extern fluid_audio_driver_t new_fluid_audio_driver(fluid_settings_t settings, fluid_synth_t synth);

        [DllImport("libfluidsynth.so")]
        internal static extern void delete_fluid_audio_driver(fluid_audio_driver_t driver);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_sfload(fluid_synth_t synth, [MarshalAs(LP_Str)] string filename, bool reset_presets);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_is_soundfont([MarshalAs(LP_Str)] string filename); // 1 or 0

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_noteon(fluid_synth_t synth, int chan, int key, int vel);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_noteoff(fluid_synth_t synth, int chan, int key);

        [DllImport("libfluidsynth.so")]
        internal static extern fluid_player_t new_fluid_player(fluid_synth_t synth);

        [DllImport("libfluidsynth.so")]
        internal static extern int delete_fluid_player(fluid_player_t player);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_add(fluid_player_t player, [MarshalAs(LP_Str)] string midifile);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_is_midifile([MarshalAs(LP_Str)] string filename); // 1 or 0

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_play(fluid_player_t player);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_join(fluid_player_t player);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_stop(fluid_player_t player);

        internal delegate int handle_midi_event_func_t(void_ptr data, fluid_midi_event_t evt);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_set_playback_callback(fluid_player_t player, handle_midi_event_func_t handler, void_ptr handler_data);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_handle_midi_event(void_ptr data, fluid_midi_event_t evt);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_program_change(fluid_synth_t synth, int chan, int program);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_cc(fluid_synth_t synth, int chan, int ctrl, int val);
    }
}
