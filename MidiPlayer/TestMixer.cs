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
using System.Collections.Generic;
using System.ComponentModel;

namespace MidiPlayer {
    /// <summary>
    /// instance-based mixer used for tests. creates per-instance faders so parallel tests
    /// do not interfere with each other.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public sealed class TestMixer : IMixer {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        /// <summary>
        /// the first MIDI track index (zero-based).
        /// </summary>
        const int MIDI_TRACK_BASE = 0;

        /// <summary>
        /// the total number of MIDI tracks supported.
        /// </summary>
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        /// <summary>
        /// dictionary holding per-index Fader instances for this test mixer.
        /// </summary>
        readonly Dictionary<int, Mixer.Fader> _mixer;

        /// <summary>
        /// the index of the currently selected fader.
        /// </summary>
        int _current;

        /// <summary>
        /// the index of the previously selected fader.
        /// </summary>
        int _previous;

        /// <summary>
        /// backing delegate for the Selected event.
        /// </summary>
        PropertyChangedEventHandler? _on_selected;

        /// <summary>
        /// backing delegate for the Updated event.
        /// </summary>
        PropertyChangedEventHandler? _on_updated;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// creates a TestMixer with the given number of fader tracks.
        /// </summary>
        /// <param name="trackCount">number of faders to create; defaults to MIDI_TRACK_COUNT.</param>
        public TestMixer(int trackCount = MIDI_TRACK_COUNT) {
            if (trackCount <= 0) trackCount = MIDI_TRACK_COUNT;
            _mixer = new Dictionary<int, Mixer.Fader>(trackCount);
            for (int i = 0; i < trackCount; i++) {
                var fader = new Mixer.Fader(i);
                fader.Updated += (s, e) => _on_updated?.Invoke(s, e);
                _mixer.Add(i, fader);
            }
            _current = 0;
            _previous = 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Events [verb, verb phrase]

        /// <summary>
        /// raised when the current fader selection changes.
        /// </summary>
        public event PropertyChangedEventHandler? Selected {
            add => _on_selected += value;
            remove => _on_selected -= value;
        }

        /// <summary>
        /// raised when a fader property changes.
        /// </summary>
        public event PropertyChangedEventHandler? Updated {
            add => _on_updated += value;
            remove => _on_updated -= value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective]

        /// <summary>
        /// the index of the currently selected fader.
        /// </summary>
        public int Current {
            get => _current;
            set {
                _previous = _current;
                _current = value;
                _on_selected?.Invoke(null, new PropertyChangedEventArgs(nameof(Current)));
            }
        }

        /// <summary>
        /// the currently selected fader index as a one-based value.
        /// </summary>
        public int CurrentAsOneBased => Current + 1;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// gets the currently selected fader.
        /// </summary>
        /// <returns>the current Fader instance.</returns>
        public Mixer.Fader GetCurrent() => _mixer[Current];

        /// <summary>
        /// gets the previously selected fader.
        /// </summary>
        /// <returns>the previous Fader instance.</returns>
        public Mixer.Fader GetPrevious() => _mixer[_previous];

        /// <summary>
        /// gets the fader at the given zero-based index.
        /// </summary>
        /// <param name="index">zero-based fader index.</param>
        /// <returns>the Fader at the specified index.</returns>
        public Mixer.Fader GetBy(int index) => _mixer[index];

        /// <summary>
        /// clears and re-creates all internal faders; useful for test setup and teardown.
        /// </summary>
        public void Reset() {
            _mixer.Clear();
            for (int i = 0; i < MIDI_TRACK_COUNT; i++) {
                var fader = new Mixer.Fader(i);
                fader.Updated += (s, e) => _on_updated?.Invoke(s, e);
                _mixer.Add(i, fader);
            }
            _current = 0;
            _previous = 0;
        }
    }
}
