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

#nullable enable
using System;

namespace MidiPlayer.FluidSynth
{
    /// <summary>
    /// Interface for native FluidSynth calls. Implementations live in MidiPlayer.FluidSynth
    /// so they can reference the P/Invoke definitions in NativeFuncs.
    /// </summary>
    public interface IFluidSynth
    {
        /// <summary>
        /// Creates a new FluidSynth settings object.
        /// </summary>
        /// <returns>IntPtr to the new fluid_settings_t, or IntPtr.Zero on failure.</returns>
        IntPtr new_fluid_settings();

        /// <summary>
        /// Deletes a FluidSynth settings object.
        /// </summary>
        /// <param name="settings">The fluid_settings_t to delete.</param>
        void delete_fluid_settings(IntPtr settings);

        /// <summary>
        /// Creates a new FluidSynth synthesizer.
        /// </summary>
        /// <param name="settings">The settings to use.</param>
        /// <returns>IntPtr to the new fluid_synth_t.</returns>
        IntPtr new_fluid_synth(IntPtr settings);

        /// <summary>
        /// Deletes a FluidSynth synthesizer.
        /// </summary>
        /// <param name="synth">The fluid_synth_t to delete.</param>
        void delete_fluid_synth(IntPtr synth);

        /// <summary>
        /// Creates a new audio driver.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="synth">The synthesizer.</param>
        /// <returns>IntPtr to the new fluid_audio_driver_t.</returns>
        IntPtr new_fluid_audio_driver(IntPtr settings, IntPtr synth);

        /// <summary>
        /// Deletes a FluidSynth audio driver.
        /// </summary>
        /// <param name="driver">The fluid_audio_driver_t to delete.</param>
        void delete_fluid_audio_driver(IntPtr driver);

        /// <summary>
        /// Loads a SoundFont file.
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="filename">Path to the .sf2 file.</param>
        /// <param name="reset_presets">Whether to reset all presets after loading.</param>
        /// <returns>SoundFont ID on success, FLUID_FAILED on error.</returns>
        int fluid_synth_sfload(IntPtr synth, string filename, bool reset_presets);

        /// <summary>
        /// Checks whether a file is a valid SoundFont.
        /// </summary>
        /// <param name="filename">Path to check.</param>
        /// <returns>1 if valid SoundFont, 0 otherwise.</returns>
        int fluid_is_soundfont(string filename);

        /// <summary>
        /// Sends a note-on MIDI event.
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="chan">MIDI channel (0-15).</param>
        /// <param name="key">MIDI note number (0-127).</param>
        /// <param name="vel">Velocity (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_noteon(IntPtr synth, int chan, int key, int vel);

        /// <summary>
        /// Sends a note-off MIDI event.
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="chan">MIDI channel.</param>
        /// <param name="key">MIDI note number.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_noteoff(IntPtr synth, int chan, int key);

        /// <summary>
        /// Sets the master gain of the synthesizer.
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="gain">Gain value (0.0–10.0).</param>
        void fluid_synth_set_gain(IntPtr synth, float gain);

        /// <summary>
        /// Creates a new MIDI player.
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <returns>IntPtr to the new fluid_player_t.</returns>
        IntPtr new_fluid_player(IntPtr synth);

        /// <summary>
        /// Deletes a MIDI player.
        /// </summary>
        /// <param name="player">The fluid_player_t to delete.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int delete_fluid_player(IntPtr player);

        /// <summary>
        /// Adds a MIDI file to the player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="midifile">Path to the .mid file.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_add(IntPtr player, string midifile);

        /// <summary>
        /// Checks whether a file is a valid MIDI file.
        /// </summary>
        /// <param name="filename">Path to check.</param>
        /// <returns>1 if valid MIDI file, 0 otherwise.</returns>
        int fluid_is_midifile(string filename);

        /// <summary>
        /// Starts MIDI playback.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_play(IntPtr player);

        /// <summary>
        /// Blocks the calling thread until playback ends.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_join(IntPtr player);

        /// <summary>
        /// Stops MIDI playback.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_stop(IntPtr player);

        /// <summary>
        /// The default FluidSynth MIDI event handler.
        /// </summary>
        /// <param name="data">User data pointer (typically fluid_synth_t).</param>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_handle_midi_event(IntPtr data, IntPtr evt);

        /// <summary>
        /// Sends a program change on a channel.
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="chan">MIDI channel.</param>
        /// <param name="program">Program number (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_program_change(IntPtr synth, int chan, int program);

        /// <summary>
        /// Sends a control change (CC) on a channel.
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="chan">MIDI channel.</param>
        /// <param name="ctrl">Controller number.</param>
        /// <param name="val">Controller value (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_cc(IntPtr synth, int chan, int ctrl, int val);

        /// <summary>
        /// Gets the MIDI event type.
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The MIDI status byte (e.g., 144=NOTE_ON, 128=NOTE_OFF).</returns>
        int fluid_midi_event_get_type(IntPtr evt);

        /// <summary>
        /// Gets the MIDI channel from an event.
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The MIDI channel (0-15).</returns>
        int fluid_midi_event_get_channel(IntPtr evt);

        /// <summary>
        /// Gets the note key from a MIDI event.
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The MIDI key number (0-127).</returns>
        int fluid_midi_event_get_key(IntPtr evt);

        /// <summary>
        /// Gets the velocity from a MIDI note event.
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The velocity (0-127).</returns>
        int fluid_midi_event_get_velocity(IntPtr evt);

        /// <summary>
        /// Gets the controller number from a MIDI CC event.
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The controller number.</returns>
        int fluid_midi_event_get_control(IntPtr evt);

        /// <summary>
        /// Gets the value from a MIDI CC event.
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The controller value (0-127).</returns>
        int fluid_midi_event_get_value(IntPtr evt);

        /// <summary>
        /// Gets the program number from a MIDI program change event.
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The program number (0-127).</returns>
        int fluid_midi_event_get_program(IntPtr evt);
    }
}
