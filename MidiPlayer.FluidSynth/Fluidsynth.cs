/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Runtime.InteropServices;
using void_ptr = System.IntPtr;
using fluid_settings_t = System.IntPtr;
using fluid_synth_t = System.IntPtr;
using fluid_audio_driver_t = System.IntPtr;
using fluid_player_t = System.IntPtr;
using fluid_midi_event_t = System.IntPtr;

namespace NativeFuncs {
    /// <summary>
    /// class for Fluidsynth API definitions.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    internal static class Fluidsynth {
#nullable enable

        // must set to MidiPlayer.FluidSynth.csproj
        /// <summary>the native FluidSynth shared library filename. Resolved at compile-time based on RUNTIME_LINUX or RUNTIME_WINDOWS compile symbols.</summary>
#if RUNTIME_LINUX
        const string LIBLARY = "libfluidsynth.so";
#elif RUNTIME_WINDOWS
        const string LIBLARY = "libs/libfluidsynth-2.dll";
#endif
        /// <summary>the UnmanagedType used to marshal C# strings to native null-terminated char* (LPStr).</summary>
        const UnmanagedType LP_Str = UnmanagedType.LPStr;

        /// <summary>return code indicating a successful FluidSynth operation (value: 0).</summary>
        internal const int FLUID_OK = 0;

        /// <summary>return code indicating a failed FluidSynth operation (value: -1).</summary>
        internal const int FLUID_FAILED = -1;

        /// <summary>P/Invoke: creates and returns a new fluid_settings_t object.</summary>
        /// <returns>a new fluid_settings_t handle.</returns>
        [DllImport(LIBLARY)]
        internal static extern fluid_settings_t new_fluid_settings();

        /// <summary>P/Invoke: frees the memory of a given fluid_settings_t object.</summary>
        /// <param name="settings">the fluid_settings_t object to delete.</param>
        [DllImport(LIBLARY)]
        internal static extern void delete_fluid_settings(fluid_settings_t settings);

        /// <summary>P/Invoke: creates a new synthesizer object.</summary>
        /// <param name="settings">a pointer to the settings object.</param>
        /// <returns>a new fluid_synth_t handle.</returns>
        [DllImport(LIBLARY)]
        internal static extern fluid_synth_t new_fluid_synth(fluid_settings_t settings);

        /// <summary>P/Invoke: deletes the synthesizer and frees associated memory.</summary>
        /// <param name="synth">the synthesizer object to delete.</param>
        [DllImport(LIBLARY)]
        internal static extern void delete_fluid_synth(fluid_synth_t synth);

        /// <summary>P/Invoke: creates an audio driver for the synthesizer.</summary>
        /// <param name="settings">the settings object.</param>
        /// <param name="synth">the synthesizer object.</param>
        /// <returns>a new fluid_audio_driver_t handle.</returns>
        [DllImport(LIBLARY)]
        internal static extern fluid_audio_driver_t new_fluid_audio_driver(fluid_settings_t settings, fluid_synth_t synth);

        /// <summary>P/Invoke: destroys the audio driver.</summary>
        /// <param name="driver">the audio driver to delete.</param>
        [DllImport(LIBLARY)]
        internal static extern void delete_fluid_audio_driver(fluid_audio_driver_t driver);

        /// <summary>P/Invoke: loads a SoundFont file into the synthesizer.</summary>
        /// <param name="synth">the synthesizer object.</param>
        /// <param name="filename">path to the .sf2 file.</param>
        /// <param name="reset_presets">if true, resets all program changes.</param>
        /// <returns>FLUID_OK (0) on success, FLUID_FAILED on error.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_sfload(fluid_synth_t synth, [MarshalAs(LP_Str)] string filename, bool reset_presets);

        /// <summary>P/Invoke: checks whether the given file is a SoundFont.</summary>
        /// <param name="filename">the path to check.</param>
        /// <returns>1 if the file is a valid SoundFont, 0 otherwise.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_is_soundfont([MarshalAs(LP_Str)] string filename); // 1 or 0

        /// <summary>P/Invoke: sends a note-on event to the synthesizer.</summary>
        /// <param name="synth">the synthesizer object.</param>
        /// <param name="chan">the MIDI channel.</param>
        /// <param name="key">the MIDI key number.</param>
        /// <param name="vel">the velocity.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_noteon(fluid_synth_t synth, int chan, int key, int vel);

        /// <summary>P/Invoke: sends a note-off event to the synthesizer.</summary>
        /// <param name="synth">the synthesizer object.</param>
        /// <param name="chan">the MIDI channel.</param>
        /// <param name="key">the MIDI key number.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_noteoff(fluid_synth_t synth, int chan, int key);

        /// <summary>P/Invoke: sets the gain (master volume) of the synthesizer.</summary>
        /// <param name="synth">the synthesizer object.</param>
        /// <param name="gain">the gain value.</param>
        [DllImport(LIBLARY)]
        internal static extern void fluid_synth_set_gain(fluid_synth_t synth, float gain);

        /// <summary>P/Invoke: creates a new MIDI player.</summary>
        /// <param name="synth">the synthesizer object.</param>
        /// <returns>a new fluid_player_t handle.</returns>
        [DllImport(LIBLARY)]
        internal static extern fluid_player_t new_fluid_player(fluid_synth_t synth);

        /// <summary>P/Invoke: deletes the MIDI player.</summary>
        /// <param name="player">the player to delete.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int delete_fluid_player(fluid_player_t player);

        /// <summary>P/Invoke: adds a MIDI file to the player's play list.</summary>
        /// <param name="player">the player object.</param>
        /// <param name="midifile">path to the .mid file.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_player_add(fluid_player_t player, [MarshalAs(LP_Str)] string midifile);

        /// <summary>P/Invoke: checks whether the given file is a MIDI file.</summary>
        /// <param name="filename">the path to check.</param>
        /// <returns>1 if the file is a valid MIDI file, 0 otherwise.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_is_midifile([MarshalAs(LP_Str)] string filename); // 1 or 0

        /// <summary>P/Invoke: starts playback of the MIDI player.</summary>
        /// <param name="player">the player to start.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_player_play(fluid_player_t player);

        /// <summary>P/Invoke: waits for the player to finish playing.</summary>
        /// <param name="player">the player to join.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_player_join(fluid_player_t player);

        /// <summary>P/Invoke: stops the MIDI player.</summary>
        /// <param name="player">the player to stop.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_player_stop(fluid_player_t player);

        /// <summary>delegate type for the native MIDI event callback used with fluid_player_set_playback_callback.</summary>
        /// <param name="data">user-data pointer (typically the fluid_synth_t).</param>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>an int result code.</returns>
        internal delegate int handle_midi_event_func_t(void_ptr data, fluid_midi_event_t evt);

        /// <summary>P/Invoke: registers a callback for MIDI playback events.</summary>
        /// <param name="player">the player object.</param>
        /// <param name="handler">the MIDI event callback delegate.</param>
        /// <param name="handler_data">user data to pass to the handler.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_player_set_playback_callback(fluid_player_t player, handle_midi_event_func_t handler, void_ptr handler_data);

        /// <summary>P/Invoke: default MIDI event handler that passes events directly to the synthesizer.</summary>
        /// <param name="data">user data pointer (typically the fluid_synth_t).</param>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_handle_midi_event(void_ptr data, fluid_midi_event_t evt);

        /// <summary>P/Invoke: sends a program-change message to the synthesizer.</summary>
        /// <param name="synth">the synthesizer object.</param>
        /// <param name="chan">the MIDI channel.</param>
        /// <param name="program">the program number.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_program_change(fluid_synth_t synth, int chan, int program);

        /// <summary>P/Invoke: sends a control-change (CC) message to the synthesizer.</summary>
        /// <param name="synth">the synthesizer object.</param>
        /// <param name="chan">the MIDI channel.</param>
        /// <param name="ctrl">the controller number.</param>
        /// <param name="val">the controller value.</param>
        /// <returns>FLUID_OK (0) on success.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_cc(fluid_synth_t synth, int chan, int ctrl, int val);

        /// <summary>P/Invoke: returns the MIDI event type stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>the MIDI status byte.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_type(fluid_midi_event_t evt);

        /// <summary>P/Invoke: returns the MIDI channel stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>the MIDI channel (0-15).</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_channel(fluid_midi_event_t evt);

        /// <summary>P/Invoke: returns the note key stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>the MIDI key number (0-127).</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_key(fluid_midi_event_t evt);

        /// <summary>P/Invoke: returns the velocity stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>the velocity (0-127).</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_velocity(fluid_midi_event_t evt);

        /// <summary>P/Invoke: returns the controller number stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>the controller number.</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_control(fluid_midi_event_t evt);

        /// <summary>P/Invoke: returns the controller value stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>the controller value (0-127).</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_value(fluid_midi_event_t evt);

        /// <summary>P/Invoke: returns the program number stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the MIDI event handle.</param>
        /// <returns>the program number (0-127).</returns>
        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_program(fluid_midi_event_t evt);
    }
}

// fluid_midi_event_type
// NOTE_OFF       = 128
// NOTE_ON        = 144
// CONTROL_CHANGE = 176
// PROGRAM_CHANGE = 192

// fluid_midi_control_chang
// BANK_SELECT_MSB =  0
// DATA_ENTRY_MSB  =  6
// VOLUME_MSB      =  7
// PAN_MSB         = 10
// EXPRESSION_MSB  = 11
// BANK_SELECT_LSB = 32
// DATA_ENTRY_LSB  = 38