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
using System.Linq;

namespace MidiPlayer {
    /// <summary>
    /// event queue class to send synth
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class EventQueue {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static Map<int, Queue<Data>> _queue_map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static EventQueue() {
            _queue_map = new();
            Enumerable.Range(start: MIDI_TRACK_BASE, count: MIDI_TRACK_COUNT).ToList().ForEach(
                track_index => _queue_map.Add(key: track_index, value: new())
            );
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Enqueue(int track_index, Data value) {
            _queue_map[track_index].Enqueue(item: value);
        }

        public static Data Dequeue(int track_index) {
            return _queue_map[track_index].Count == 0 ? null : _queue_map[track_index].Dequeue();
        }
    }

    /// <summary>
    /// data class to send synth
    /// </summary>
    public class Data {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        int _channel;

        int _program;

        int _pan;

        int _volume;

        bool _mute;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public int Channel {
            get => _channel;
            set => _channel = value;
        }

        public int Program {
            get => _program;
            set => _program = value;
        }

        public int Pan {
            get => _pan;
            set => _pan = value;
        }

        public int Volume {
            get => _volume;
            set => _volume = value;
        }

        public bool Mute {
            get => _mute;
            set => _mute = value;
        }
    }

    /// <summary>
    /// rename Dictionary to Map
    /// </summary>
    public class Map<K, V> : Dictionary<K, V> {
    }
}
