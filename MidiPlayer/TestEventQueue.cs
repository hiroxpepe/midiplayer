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
using System.Collections.Concurrent;

namespace MidiPlayer {
    /// <summary>
    /// in-memory event queue for unit tests.
    /// </summary>
    /// <remarks>
    /// uses one ConcurrentQueue per track, matching the thread-safety contract of the
    /// production EventQueue without requiring the static class to be injected.
    /// allocations are acceptable here because this implementation is test-only.
    /// </remarks>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public sealed class TestEventQueue : IEventQueue {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        /// <summary>
        /// the first MIDI track index used as the base offset for the queue array.
        /// </summary>
        const int MIDI_TRACK_BASE = 0;

        /// <summary>
        /// default number of MIDI tracks when none is specified by the caller.
        /// </summary>
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        /// <summary>
        /// one lock-free concurrent queue per MIDI track; indexed by trackIndex - MIDI_TRACK_BASE.
        /// </summary>
        readonly ConcurrentQueue<Data>[] _queues;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// initializes a new TestEventQueue with the specified number of track queues.
        /// </summary>
        /// <param name="trackCount">number of track queues to create; defaults to MIDI_TRACK_COUNT.</param>
        public TestEventQueue(int trackCount = MIDI_TRACK_COUNT) {
            if (trackCount <= 0) { trackCount = MIDI_TRACK_COUNT; }
            _queues = new ConcurrentQueue<Data>[trackCount];
            for (int i = 0; i < _queues.Length; i++) {
                _queues[i] = new ConcurrentQueue<Data>();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// enqueue a data value for the specified track.
        /// </summary>
        /// <param name="trackIndex">zero-based MIDI track index.</param>
        /// <param name="value">mixer state data to deliver to the audio callback.</param>
        public void Enqueue(int trackIndex, Data value) {
            int idx = trackIndex - MIDI_TRACK_BASE;
            if ((uint) idx >= (uint) _queues.Length) {
                throw new System.ArgumentOutOfRangeException(nameof(trackIndex));
            }
            _queues[idx].Enqueue(value);
        }

        /// <summary>
        /// dequeue the next data value for the specified track.
        /// returns null when the queue is empty.
        /// </summary>
        /// <param name="trackIndex">zero-based MIDI track index.</param>
        /// <returns>the next Data value, or null if the queue is empty.</returns>
        public Data? Dequeue(int trackIndex) {
            int idx = trackIndex - MIDI_TRACK_BASE;
            if ((uint) idx >= (uint) _queues.Length) {
                throw new System.ArgumentOutOfRangeException(nameof(trackIndex));
            }
            return _queues[idx].TryDequeue(out var d) ? d : null;
        }

        /// <summary>
        /// clear all track queues.
        /// useful in test setup and teardown to reset state between test cases.
        /// </summary>
        public void Clear() {
            for (int i = 0; i < _queues.Length; i++) {
                while (_queues[i].TryDequeue(out _)) { }
            }
        }
    }
}
