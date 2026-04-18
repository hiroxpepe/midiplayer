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

using System.Collections.Generic;

namespace MidiPlayer {
    /// <summary>
    /// playlist for synth
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class PlayList {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        /// <summary>
        /// the internal list of MIDI file paths in the playlist.
        /// </summary>
        List<string> _target_list = new();

        /// <summary>
        /// the current playback position index within _target_list.
        /// </summary>
        int _index;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// creates a new empty PlayList.
        /// </summary>
        public PlayList() {
            _index = 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        /// <summary>
        /// returns true when the playlist contains at least one item.
        /// </summary>
        public bool Ready {
            get => _target_list.Count == 0 ? false : true;
        }

        /// <summary>
        /// returns all file paths in the playlist as an array.
        /// </summary>
        public string[] List {
            get => _target_list.ToArray();
        }

        /// <summary>
        /// returns the file path at the current playback position without advancing.
        /// </summary>
        public string Current {
            get => _target_list[_index];
        }

        /// <summary>
        /// returns the next file path in the playlist and advances the position.
        /// wraps around to the beginning when the end is reached.
        /// </summary>
        public string Next {
            get {
                if (_index == _target_list.Count) {
                    _index = 0;
                }
                return _target_list[_index++];
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// adds a file path to the end of the playlist.
        /// </summary>
        /// <param name="target">the MIDI file path to add.</param>
        public void Add(string target) {
            _target_list.Add(target);
        }

        /// <summary>
        /// removes all file paths from the playlist and resets the position.
        /// </summary>
        public void Clear() {
            _target_list.Clear();
        }
    }
}
