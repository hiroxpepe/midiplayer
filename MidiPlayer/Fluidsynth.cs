
using System.Runtime.InteropServices;
using fluid_settings_t_ptr = System.IntPtr;
using fluid_synth_t_ptr = System.IntPtr;
using fluid_audio_driver_t_ptr = System.IntPtr;
using fluid_player_t_ptr = System.IntPtr;

namespace NativeFuncs {

    internal static class Fluidsynth {

        const UnmanagedType LP_Str = UnmanagedType.LPStr;

        internal const int FLUID_OK = 0;

        internal const int FLUID_FAILED = -1;

        [DllImport("libfluidsynth.so")]
        internal static extern fluid_settings_t_ptr new_fluid_settings();

        [DllImport("libfluidsynth.so")]
        internal static extern void delete_fluid_settings(fluid_settings_t_ptr settings);

        [DllImport("libfluidsynth.so")]
        internal static extern fluid_synth_t_ptr new_fluid_synth(fluid_settings_t_ptr settings);

        [DllImport("libfluidsynth.so")]
        internal static extern void delete_fluid_synth(fluid_synth_t_ptr synth);

        [DllImport("libfluidsynth.so")]
        internal static extern fluid_audio_driver_t_ptr new_fluid_audio_driver(fluid_settings_t_ptr settings, fluid_synth_t_ptr synth);

        [DllImport("libfluidsynth.so")]
        internal static extern void delete_fluid_audio_driver(fluid_audio_driver_t_ptr driver);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_sfload(fluid_synth_t_ptr synth, [MarshalAs(LP_Str)] string filename, bool reset_presets);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_is_soundfont([MarshalAs(LP_Str)] string filename); // 1 or 0

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_noteon(fluid_synth_t_ptr synth, int chan, int key, int vel);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_noteoff(fluid_synth_t_ptr synth, int chan, int key);

        [DllImport("libfluidsynth.so")]
        internal static extern fluid_player_t_ptr new_fluid_player(fluid_synth_t_ptr synth);

        [DllImport("libfluidsynth.so")]
        internal static extern int delete_fluid_player(fluid_player_t_ptr player);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_add(fluid_player_t_ptr player, [MarshalAs(LP_Str)] string midifile);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_is_midifile([MarshalAs(LP_Str)] string filename); // 1 or 0

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_play(fluid_player_t_ptr player);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_join(fluid_player_t_ptr player);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_player_stop(fluid_player_t_ptr player);
    }
}