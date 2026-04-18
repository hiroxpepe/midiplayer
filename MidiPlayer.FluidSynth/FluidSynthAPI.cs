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
    /// Static facade used by Synth.cs and other production code to call into FluidSynth.
    /// Tests may replace Instance with a fake implementation to simulate native calls.
    /// </summary>
    public static class FluidSynthAPI
    {
        /// <summary>
        /// The active IFluidSynth implementation. Set by tests to replace PInvokeFluidSynth with a fake.
        /// </summary>
        public static IFluidSynth Instance { get; set; } = new PInvokeFluidSynth();

        /// <summary>
        /// Forwards to Instance.new_fluid_settings().
        /// </summary>
        /// <returns>IntPtr to the new fluid_settings_t, or IntPtr.Zero on failure.</returns>
        public static IntPtr new_fluid_settings() => Instance.new_fluid_settings();

        /// <summary>
        /// Forwards to Instance.delete_fluid_settings().
        /// </summary>
        /// <param name="settings">The fluid_settings_t to delete.</param>
        public static void delete_fluid_settings(IntPtr settings) => Instance.delete_fluid_settings(settings);

        /// <summary>
        /// Forwards to Instance.new_fluid_synth().
        /// </summary>
        /// <param name="settings">The settings to use.</param>
        /// <returns>IntPtr to the new fluid_synth_t.</returns>
        public static IntPtr new_fluid_synth(IntPtr settings) => Instance.new_fluid_synth(settings);

        /// <summary>
        /// Forwards to Instance.delete_fluid_synth().
        /// </summary>
        /// <param name="synth">The fluid_synth_t to delete.</param>
        public static void delete_fluid_synth(IntPtr synth) => Instance.delete_fluid_synth(synth);

        /// <summary>
        /// Forwards to Instance.new_fluid_audio_driver().
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="synth">The synthesizer.</param>
        /// <returns>IntPtr to the new fluid_audio_driver_t.</returns>
        public static IntPtr new_fluid_audio_driver(IntPtr settings, IntPtr synth) => Instance.new_fluid_audio_driver(settings, synth);

        /// <summary>
        /// Forwards to Instance.delete_fluid_audio_driver().
        /// </summary>
        /// <param name="driver">The fluid_audio_driver_t to delete.</param>
        public static void delete_fluid_audio_driver(IntPtr driver) => Instance.delete_fluid_audio_driver(driver);

        /// <summary>
        /// Forwards to Instance.fluid_synth_sfload().
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="filename">Path to the .sf2 file.</param>
        /// <param name="reset_presets">Whether to reset all presets after loading.</param>
        /// <returns>SoundFont ID on success, FLUID_FAILED on error.</returns>
        public static int fluid_synth_sfload(IntPtr synth, string filename, bool reset_presets) => Instance.fluid_synth_sfload(synth, filename, reset_presets);

        /// <summary>
        /// Forwards to Instance.fluid_is_soundfont().
        /// </summary>
        /// <param name="filename">Path to check.</param>
        /// <returns>1 if valid SoundFont, 0 otherwise.</returns>
        public static int fluid_is_soundfont(string filename) => Instance.fluid_is_soundfont(filename);

        /// <summary>
        /// Forwards to Instance.fluid_synth_noteon().
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="chan">MIDI channel (0-15).</param>
        /// <param name="key">MIDI note number (0-127).</param>
        /// <param name="vel">Velocity (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_synth_noteon(IntPtr synth, int chan, int key, int vel) => Instance.fluid_synth_noteon(synth, chan, key, vel);

        /// <summary>
        /// Forwards to Instance.fluid_synth_noteoff().
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="chan">MIDI channel.</param>
        /// <param name="key">MIDI note number.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_synth_noteoff(IntPtr synth, int chan, int key) => Instance.fluid_synth_noteoff(synth, chan, key);

        /// <summary>
        /// Forwards to Instance.fluid_synth_set_gain().
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="gain">Gain value (0.0–10.0).</param>
        public static void fluid_synth_set_gain(IntPtr synth, float gain) => Instance.fluid_synth_set_gain(synth, gain);

        /// <summary>
        /// Forwards to Instance.new_fluid_player().
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <returns>IntPtr to the new fluid_player_t.</returns>
        public static IntPtr new_fluid_player(IntPtr synth) => Instance.new_fluid_player(synth);

        /// <summary>
        /// Forwards to Instance.delete_fluid_player().
        /// </summary>
        /// <param name="player">The fluid_player_t to delete.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int delete_fluid_player(IntPtr player) => Instance.delete_fluid_player(player);

        /// <summary>
        /// Forwards to Instance.fluid_player_add().
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="midifile">Path to the .mid file.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_player_add(IntPtr player, string midifile) => Instance.fluid_player_add(player, midifile);

        /// <summary>
        /// Forwards to Instance.fluid_is_midifile().
        /// </summary>
        /// <param name="filename">Path to check.</param>
        /// <returns>1 if valid MIDI file, 0 otherwise.</returns>
        public static int fluid_is_midifile(string filename) => Instance.fluid_is_midifile(filename);

        /// <summary>
        /// Forwards to Instance.fluid_player_play().
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_player_play(IntPtr player) => Instance.fluid_player_play(player);

        /// <summary>
        /// Forwards to Instance.fluid_player_join().
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_player_join(IntPtr player) => Instance.fluid_player_join(player);

        /// <summary>
        /// Forwards to Instance.fluid_player_stop().
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_player_stop(IntPtr player) => Instance.fluid_player_stop(player);

        /// <summary>
        /// Registers the playback callback. When Instance is a fake (IFakeFluidSynth), wraps the callback
        /// as a managed delegate and calls RegisterPlaybackHandler; otherwise calls the native P/Invoke.
        /// </summary>
        /// <param name="player">The fluid_player_t.</param>
        /// <param name="handler">The native callback delegate.</param>
        /// <param name="handler_data">User data passed through to the callback.</param>
        /// <returns>FLUID_OK.</returns>
        internal static int fluid_player_set_playback_callback(IntPtr player, NativeFuncs.Fluidsynth.handle_midi_event_func_t handler, IntPtr handler_data)
        {
            // If tests replaced Instance with a fake that implements IFakeFluidSynth, register a managed handler instead
            if (Instance is IFakeFluidSynth fake)
            {
                // Wrap the native handler into a managed Func so the public IFakeFluidSynth signature remains public-safe
                // For fakes, invoke the Synth's managed processing logic directly to avoid calling back into the fake's native handler and causing recursion.
                fake.RegisterPlaybackHandler((data, evt) => MidiPlayer.Synth.ProcessPlayback(data, evt));
                return 0;
            }

            return NativeFuncs.Fluidsynth.fluid_player_set_playback_callback(player, handler, handler_data);
        }

        /// <summary>
        /// Forwards to Instance.fluid_synth_handle_midi_event().
        /// </summary>
        /// <param name="data">User data pointer (typically fluid_synth_t).</param>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_synth_handle_midi_event(IntPtr data, IntPtr evt) => Instance.fluid_synth_handle_midi_event(data, evt);

        /// <summary>
        /// Forwards to Instance.fluid_synth_program_change().
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="chan">MIDI channel.</param>
        /// <param name="program">Program number (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_synth_program_change(IntPtr synth, int chan, int program) => Instance.fluid_synth_program_change(synth, chan, program);

        /// <summary>
        /// Forwards to Instance.fluid_synth_cc().
        /// </summary>
        /// <param name="synth">The synthesizer.</param>
        /// <param name="chan">MIDI channel.</param>
        /// <param name="ctrl">Controller number.</param>
        /// <param name="val">Controller value (0-127).</param>
        /// <returns>FLUID_OK or FLUID_FAILED.</returns>
        public static int fluid_synth_cc(IntPtr synth, int chan, int ctrl, int val) => Instance.fluid_synth_cc(synth, chan, ctrl, val);

        /// <summary>
        /// Forwards to Instance.fluid_midi_event_get_type().
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The MIDI status byte (e.g., 144=NOTE_ON, 128=NOTE_OFF).</returns>
        public static int fluid_midi_event_get_type(IntPtr evt) => Instance.fluid_midi_event_get_type(evt);

        /// <summary>
        /// Forwards to Instance.fluid_midi_event_get_channel().
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The MIDI channel (0-15).</returns>
        public static int fluid_midi_event_get_channel(IntPtr evt) => Instance.fluid_midi_event_get_channel(evt);

        /// <summary>
        /// Forwards to Instance.fluid_midi_event_get_key().
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The MIDI key number (0-127).</returns>
        public static int fluid_midi_event_get_key(IntPtr evt) => Instance.fluid_midi_event_get_key(evt);

        /// <summary>
        /// Forwards to Instance.fluid_midi_event_get_velocity().
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The velocity (0-127).</returns>
        public static int fluid_midi_event_get_velocity(IntPtr evt) => Instance.fluid_midi_event_get_velocity(evt);

        /// <summary>
        /// Forwards to Instance.fluid_midi_event_get_control().
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The controller number.</returns>
        public static int fluid_midi_event_get_control(IntPtr evt) => Instance.fluid_midi_event_get_control(evt);

        /// <summary>
        /// Forwards to Instance.fluid_midi_event_get_value().
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The controller value (0-127).</returns>
        public static int fluid_midi_event_get_value(IntPtr evt) => Instance.fluid_midi_event_get_value(evt);

        /// <summary>
        /// Forwards to Instance.fluid_midi_event_get_program().
        /// </summary>
        /// <param name="evt">The MIDI event.</param>
        /// <returns>The program number (0-127).</returns>
        public static int fluid_midi_event_get_program(IntPtr evt) => Instance.fluid_midi_event_get_program(evt);
    }
}
