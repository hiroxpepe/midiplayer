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

namespace MidiPlayer {
    /// <summary>
    /// compatibility stub retained during refactor.
    /// the canonical implementation has moved to MidiPlayer.FluidSynth.IFluidSynth.
    /// this interface mirrors the FluidSynth native API for dependency injection.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public interface IFluidSynth {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>creates a new FluidSynth settings object.</summary>
        /// <returns>pointer to the new fluid_settings_t.</returns>
        IntPtr new_fluid_settings();

        /// <summary>destroys a FluidSynth settings object.</summary>
        /// <param name="settings">pointer to the fluid_settings_t to delete.</param>
        void delete_fluid_settings(IntPtr settings);

        /// <summary>creates a new FluidSynth synthesizer.</summary>
        /// <param name="settings">pointer to the settings object.</param>
        /// <returns>pointer to the new fluid_synth_t.</returns>
        IntPtr new_fluid_synth(IntPtr settings);

        /// <summary>destroys a FluidSynth synthesizer.</summary>
        /// <param name="synth">pointer to the fluid_synth_t to delete.</param>
        void delete_fluid_synth(IntPtr synth);

        /// <summary>creates a new FluidSynth audio driver.</summary>
        /// <param name="settings">pointer to the settings object.</param>
        /// <param name="synth">pointer to the synthesizer.</param>
        /// <returns>pointer to the new fluid_audio_driver_t.</returns>
        IntPtr new_fluid_audio_driver(IntPtr settings, IntPtr synth);

        /// <summary>destroys a FluidSynth audio driver.</summary>
        /// <param name="driver">pointer to the fluid_audio_driver_t to delete.</param>
        void delete_fluid_audio_driver(IntPtr driver);

        /// <summary>loads a SoundFont file into the synthesizer.</summary>
        /// <param name="synth">pointer to the synthesizer.</param>
        /// <param name="filename">path to the SoundFont file.</param>
        /// <param name="reset_presets">whether to reset all presets after loading.</param>
        /// <returns>SoundFont ID on success, FLUID_FAILED on error.</returns>
        int fluid_synth_sfload(IntPtr synth, string filename, bool reset_presets);

        /// <summary>checks whether a file is a valid SoundFont.</summary>
        /// <param name="filename">path to the file.</param>
        /// <returns>1 if valid, 0 otherwise.</returns>
        int fluid_is_soundfont(string filename);

        /// <summary>sends a MIDI note-on message.</summary>
        /// <param name="synth">pointer to the synthesizer.</param>
        /// <param name="chan">MIDI channel (0-based).</param>
        /// <param name="key">MIDI key number.</param>
        /// <param name="vel">velocity (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_noteon(IntPtr synth, int chan, int key, int vel);

        /// <summary>sends a MIDI note-off message.</summary>
        /// <param name="synth">pointer to the synthesizer.</param>
        /// <param name="chan">MIDI channel (0-based).</param>
        /// <param name="key">MIDI key number.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_noteoff(IntPtr synth, int chan, int key);

        /// <summary>sets the master gain of the synthesizer.</summary>
        /// <param name="synth">pointer to the synthesizer.</param>
        /// <param name="gain">gain value (0.0 to 10.0).</param>
        void fluid_synth_set_gain(IntPtr synth, float gain);

        /// <summary>creates a new MIDI player.</summary>
        /// <param name="synth">pointer to the synthesizer.</param>
        /// <returns>pointer to the new fluid_player_t.</returns>
        IntPtr new_fluid_player(IntPtr synth);

        /// <summary>destroys a MIDI player.</summary>
        /// <param name="player">pointer to the fluid_player_t to delete.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int delete_fluid_player(IntPtr player);

        /// <summary>adds a MIDI file to the player's playlist.</summary>
        /// <param name="player">pointer to the player.</param>
        /// <param name="midifile">path to the MIDI file.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_add(IntPtr player, string midifile);

        /// <summary>checks whether a file is a valid MIDI file.</summary>
        /// <param name="filename">path to the file.</param>
        /// <returns>1 if valid, 0 otherwise.</returns>
        int fluid_is_midifile(string filename);

        /// <summary>starts MIDI playback.</summary>
        /// <param name="player">pointer to the player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_play(IntPtr player);

        /// <summary>blocks until MIDI playback finishes.</summary>
        /// <param name="player">pointer to the player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_join(IntPtr player);

        /// <summary>stops MIDI playback.</summary>
        /// <param name="player">pointer to the player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_stop(IntPtr player);

        /// <summary>registers a MIDI event callback with the player.</summary>
        /// <param name="player">pointer to the player.</param>
        /// <param name="handler">the callback handler pointer.</param>
        /// <param name="handler_data">user data passed to the handler.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_player_set_playback_callback(IntPtr player, IntPtr handler, IntPtr handler_data);

        /// <summary>default MIDI event handler that routes events to the synthesizer.</summary>
        /// <param name="data">pointer to the synthesizer.</param>
        /// <param name="evt">pointer to the MIDI event.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_handle_midi_event(IntPtr data, IntPtr evt);

        /// <summary>sends a MIDI program change message.</summary>
        /// <param name="synth">pointer to the synthesizer.</param>
        /// <param name="chan">MIDI channel (0-based).</param>
        /// <param name="program">program number (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_program_change(IntPtr synth, int chan, int program);

        /// <summary>sends a MIDI control change (CC) message.</summary>
        /// <param name="synth">pointer to the synthesizer.</param>
        /// <param name="chan">MIDI channel (0-based).</param>
        /// <param name="ctrl">controller number.</param>
        /// <param name="val">controller value (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        int fluid_synth_cc(IntPtr synth, int chan, int ctrl, int val);

        /// <summary>returns the type field of a MIDI event.</summary>
        /// <param name="evt">pointer to the MIDI event.</param>
        /// <returns>MIDI event type number.</returns>
        int fluid_midi_event_get_type(IntPtr evt);

        /// <summary>returns the channel field of a MIDI event.</summary>
        /// <param name="evt">pointer to the MIDI event.</param>
        /// <returns>MIDI channel number (0-based).</returns>
        int fluid_midi_event_get_channel(IntPtr evt);

        /// <summary>returns the key field of a MIDI note event.</summary>
        /// <param name="evt">pointer to the MIDI event.</param>
        /// <returns>MIDI key number.</returns>
        int fluid_midi_event_get_key(IntPtr evt);

        /// <summary>returns the velocity field of a MIDI note event.</summary>
        /// <param name="evt">pointer to the MIDI event.</param>
        /// <returns>velocity value (0-127).</returns>
        int fluid_midi_event_get_velocity(IntPtr evt);

        /// <summary>returns the controller number field of a MIDI CC event.</summary>
        /// <param name="evt">pointer to the MIDI event.</param>
        /// <returns>controller number.</returns>
        int fluid_midi_event_get_control(IntPtr evt);

        /// <summary>returns the value field of a MIDI CC event.</summary>
        /// <param name="evt">pointer to the MIDI event.</param>
        /// <returns>controller value (0-127).</returns>
        int fluid_midi_event_get_value(IntPtr evt);

        /// <summary>returns the program number field of a MIDI program change event.</summary>
        /// <param name="evt">pointer to the MIDI event.</param>
        /// <returns>program number (0-127).</returns>
        int fluid_midi_event_get_program(IntPtr evt);
    }
}