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
    /// thin adapter delegating to the existing static Mixer implementation.
    /// provides an IMixer instance that forwards calls to Mixer static members.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public sealed class StaticMixerAdapter : IMixer {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        /// <summary>
        /// singleton instance of the StaticMixerAdapter.
        /// </summary>
        public static readonly StaticMixerAdapter Instance = new StaticMixerAdapter();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// private constructor enforces singleton usage via Instance.
        /// </summary>
        private StaticMixerAdapter() { }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Events [verb, verb phrase]

        /// <summary>
        /// raised when the current fader selection changes; forwards to Mixer.Selected.
        /// </summary>
        public event PropertyChangedEventHandler? Selected {
            add => Mixer.Selected += value;
            remove => Mixer.Selected -= value;
        }

        /// <summary>
        /// raised when a fader property changes; forwards to Mixer.Updated.
        /// </summary>
        public event PropertyChangedEventHandler? Updated {
            add => Mixer.Updated += value;
            remove => Mixer.Updated -= value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective]

        /// <summary>
        /// the index of the currently selected fader; delegates to Mixer.Current.
        /// </summary>
        public int Current {
            get => Mixer.Current;
            set => Mixer.Current = value;
        }

        /// <summary>
        /// the currently selected fader index as a one-based value; delegates to Mixer.CurrentAsOneBased.
        /// </summary>
        public int CurrentAsOneBased => Mixer.CurrentAsOneBased;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// gets the currently selected fader; delegates to Mixer.GetCurrent().
        /// </summary>
        /// <returns>the current Fader instance.</returns>
        public Mixer.Fader GetCurrent() => Mixer.GetCurrent();

        /// <summary>
        /// gets the previously selected fader; delegates to Mixer.GetPrevious().
        /// </summary>
        /// <returns>the previous Fader instance.</returns>
        public Mixer.Fader GetPrevious() => Mixer.GetPrevious();

        /// <summary>
        /// gets the fader at the given zero-based index; delegates to Mixer.GetBy(index).
        /// </summary>
        /// <param name="index">zero-based fader index.</param>
        /// <returns>the Fader at the specified index.</returns>
        public Mixer.Fader GetBy(int index) => Mixer.GetBy(index);
    }
}
