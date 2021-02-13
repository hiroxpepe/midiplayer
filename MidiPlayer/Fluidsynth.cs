
using System.Runtime.InteropServices;
using fluid_settings_t_ptr = System.IntPtr;
using fluid_synth_t_ptr = System.IntPtr;
using fluid_audio_driver_t_ptr = System.IntPtr;

namespace NativeFuncs {

    internal static class Fluidsynth {

        const UnmanagedType LP_Str = UnmanagedType.LPStr;

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
        internal static extern int fluid_synth_noteon(fluid_synth_t_ptr synth, int chan, int key, int vel);

        [DllImport("libfluidsynth.so")]
        internal static extern int fluid_synth_noteoff(fluid_synth_t_ptr synth, int chan, int key);
    }
}