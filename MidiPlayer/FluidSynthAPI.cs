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

// This file is a compatibility stub retained during refactor.
// The canonical implementation has moved to MidiPlayer.FluidSynth.FluidSynthAPI.
#nullable enable
using System;

namespace MidiPlayer {
    /// <summary>
    /// compatibility stub retained during refactor.
    /// always throws NotImplementedException; use MidiPlayer.FluidSynth.FluidSynthAPI for production.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class FluidSynthAPI {
        /// <summary>
        /// placeholder instance property; not used by production code.
        /// </summary>
        public static object Instance { get; set; } = null!;

        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static IntPtr new_fluid_settings() => throw new NotImplementedException("Use MidiPlayer.FluidSynth.FluidSynthAPI instead");
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static void delete_fluid_settings(IntPtr settings) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static IntPtr new_fluid_synth(IntPtr settings) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static void delete_fluid_synth(IntPtr synth) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static IntPtr new_fluid_audio_driver(IntPtr settings, IntPtr synth) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static void delete_fluid_audio_driver(IntPtr driver) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_synth_sfload(IntPtr synth, string filename, bool reset_presets) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_is_soundfont(string filename) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_synth_noteon(IntPtr synth, int chan, int key, int vel) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_synth_noteoff(IntPtr synth, int chan, int key) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static void fluid_synth_set_gain(IntPtr synth, float gain) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static IntPtr new_fluid_player(IntPtr synth) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int delete_fluid_player(IntPtr player) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_player_add(IntPtr player, string midifile) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_is_midifile(string filename) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_player_play(IntPtr player) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_player_join(IntPtr player) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_player_stop(IntPtr player) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_player_set_playback_callback(IntPtr player, IntPtr handler, IntPtr handler_data) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_synth_handle_midi_event(IntPtr data, IntPtr evt) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_synth_program_change(IntPtr synth, int chan, int program) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_synth_cc(IntPtr synth, int chan, int ctrl, int val) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_midi_event_get_type(IntPtr evt) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_midi_event_get_channel(IntPtr evt) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_midi_event_get_key(IntPtr evt) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_midi_event_get_velocity(IntPtr evt) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_midi_event_get_control(IntPtr evt) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_midi_event_get_value(IntPtr evt) => throw new NotImplementedException();
        /// <summary>stub — always throws. use MidiPlayer.FluidSynth.FluidSynthAPI.</summary>
        public static int fluid_midi_event_get_program(IntPtr evt) => throw new NotImplementedException();
    }
}
