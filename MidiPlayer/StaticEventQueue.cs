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
    /// thin adapter delegating to the existing static EventQueue implementation.
    /// use this for production; tests can inject TestEventQueue instead.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public sealed class StaticEventQueue : IEventQueue {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        /// <summary>
        /// singleton instance of the StaticEventQueue adapter.
        /// </summary>
        public static readonly StaticEventQueue Instance = new StaticEventQueue();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// private constructor enforces singleton usage via Instance.
        /// </summary>
        private StaticEventQueue() { }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// enqueues data for the given track index in the static EventQueue.
        /// </summary>
        /// <param name="trackIndex">zero-based track index.</param>
        /// <param name="value">event data to enqueue.</param>
        public void Enqueue(int trackIndex, Data value)
            => EventQueue.Enqueue(trackIndex, value);

        /// <summary>
        /// dequeues and returns data for the given track index from the static EventQueue.
        /// </summary>
        /// <param name="trackIndex">zero-based track index.</param>
        /// <returns>the next Data item, or null if the queue is empty.</returns>
        public Data? Dequeue(int trackIndex)
            => EventQueue.Dequeue(trackIndex);
    }
}
