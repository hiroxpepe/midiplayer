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
using System.ComponentModel;

namespace MidiPlayer {
    /// <summary>
    /// adaptor interface for the static Mixer so callers can depend on an instance abstraction.
    /// keeps the existing Mixer.Fader type to minimize changes.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public interface IMixer {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Events [verb, verb phrase]

        /// <summary>
        /// raised when the current fader selection changes.
        /// </summary>
        event PropertyChangedEventHandler? Selected;

        /// <summary>
        /// raised when a fader property changes.
        /// </summary>
        event PropertyChangedEventHandler? Updated;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective]

        /// <summary>
        /// the index of the currently selected fader.
        /// </summary>
        int Current { get; set; }

        /// <summary>
        /// the currently selected fader index as a one-based value.
        /// </summary>
        int CurrentAsOneBased { get; }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// gets the currently selected fader.
        /// </summary>
        /// <returns>the current Fader instance.</returns>
        Mixer.Fader GetCurrent();

        /// <summary>
        /// gets the previously selected fader.
        /// </summary>
        /// <returns>the previous Fader instance.</returns>
        Mixer.Fader GetPrevious();

        /// <summary>
        /// gets the fader at the given zero-based index.
        /// </summary>
        /// <param name="index">zero-based fader index.</param>
        /// <returns>the Fader at the specified index.</returns>
        Mixer.Fader GetBy(int index);
    }
}
