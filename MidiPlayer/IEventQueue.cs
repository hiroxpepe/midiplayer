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

namespace MidiPlayer {
    /// <summary>
    /// abstraction over the static EventQueue to allow dependency injection and test doubles.
    /// </summary>
    /// <remarks>
    /// the production implementation is the static EventQueue class.
    /// the test implementation is TestEventQueue which uses ConcurrentQueue per track.
    /// </remarks>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public interface IEventQueue {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// enqueue a data value for the specified track.
        /// mirrors EventQueue.Enqueue.
        /// </summary>
        /// <param name="trackIndex">zero-based MIDI track index.</param>
        /// <param name="value">mixer state data to deliver to the audio callback.</param>
        void Enqueue(int trackIndex, Data value);

        /// <summary>
        /// dequeue the next data value for the specified track.
        /// returns null when the queue is empty.
        /// mirrors EventQueue.Dequeue.
        /// </summary>
        /// <param name="trackIndex">zero-based MIDI track index.</param>
        /// <returns>the next Data value, or null if the queue is empty.</returns>
        Data? Dequeue(int trackIndex);
    }
}
