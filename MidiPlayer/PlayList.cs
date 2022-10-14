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

        List<string> _target_list = new();

        int _index;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public PlayList() {
            _index = 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public bool Ready {
            get => _target_list.Count == 0 ? false : true;
        }

        public string[] List {
            get => _target_list.ToArray();
        }

        public string Current {
            get => _target_list[_index];
        }

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

        public void Add(string target) {
            _target_list.Add(target);
        }

        public void Clear() {
            _target_list.Clear();
        }
    }
}
