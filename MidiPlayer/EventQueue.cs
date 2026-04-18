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

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MidiPlayer {
    /// <summary>
    /// event queue class to send synth.
    /// </summary>
    /// <remarks>
    /// uses ConcurrentQueue per track so that UI-thread Enqueue and audio-callback Dequeue
    /// can run simultaneously without corrupting internal state.
    /// </remarks>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class EventQueue {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        /// <summary>
        /// the first MIDI track index used as the base offset for the queue array.
        /// </summary>
        const int MIDI_TRACK_BASE = 0;

        /// <summary>
        /// number of MIDI tracks (and therefore the size of the queue array).
        /// </summary>
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        /// <summary>
        /// one lock-free concurrent queue per MIDI track; indexed by track_index - MIDI_TRACK_BASE.
        /// </summary>
        static ConcurrentQueue<Data>[] _queue_map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        /// <summary>
        /// initializes the per-track concurrent queue array on first use.
        /// </summary>
        static EventQueue() {
            _queue_map = new ConcurrentQueue<Data>[MIDI_TRACK_COUNT];
            for (int i = 0; i < MIDI_TRACK_COUNT; i++) {
                _queue_map[i] = new ConcurrentQueue<Data>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        /// <summary>
        /// enqueue a data value for the specified track.
        /// called from the UI thread when mixer settings change.
        /// </summary>
        public static void Enqueue(int track_index, Data value) {
            _queue_map[track_index - MIDI_TRACK_BASE].Enqueue(value);
        }

        /// <summary>
        /// dequeue the next data value for the specified track.
        /// called from the native audio callback; returns null when the queue is empty.
        /// </summary>
        public static Data? Dequeue(int track_index) {
            return _queue_map[track_index - MIDI_TRACK_BASE].TryDequeue(out var data) ? data : null;
        }
    }

    /// <summary>
    /// data value enqueued for a single MIDI track, carrying mixer state to the audio callback.
    /// </summary>
    public class Data {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        /// <summary>
        /// MIDI channel number (0-based).
        /// </summary>
        int _channel;

        /// <summary>
        /// MIDI program number (0-based).
        /// </summary>
        int _program;

        /// <summary>
        /// MIDI pan control value (0–127, center = 64).
        /// </summary>
        int _pan;

        /// <summary>
        /// MIDI volume control value (0–127).
        /// </summary>
        int _volume;

        /// <summary>
        /// whether the track is muted (true = muted, volume forced to 0 in the callback).
        /// </summary>
        bool _mute;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective]

        /// <summary>
        /// MIDI channel number (0-based).
        /// </summary>
        public int Channel {
            get => _channel;
            set => _channel = value;
        }

        /// <summary>
        /// MIDI program number (0-based).
        /// </summary>
        public int Program {
            get => _program;
            set => _program = value;
        }

        /// <summary>
        /// MIDI pan control value (0–127, center = 64).
        /// </summary>
        public int Pan {
            get => _pan;
            set => _pan = value;
        }

        /// <summary>
        /// MIDI volume control value (0–127).
        /// </summary>
        public int Volume {
            get => _volume;
            set => _volume = value;
        }

        /// <summary>
        /// whether the track is muted.
        /// </summary>
        public bool Mute {
            get => _mute;
            set => _mute = value;
        }
    }

    /// <summary>
    /// type alias: rename Dictionary to Map for consistency with the project naming convention.
    /// </summary>
    public class Map<K, V> : Dictionary<K, V> {
    }
}
