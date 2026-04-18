#nullable enable
using System;

namespace MidiPlayer
{
    /// <summary>
    /// Abstraction over the existing static EventQueue to allow DI and test replacements.
    /// </summary>
    public interface IEventQueue
    {
        /// <summary>
        /// Enqueue a Data command for the given track index.
        /// Mirrors EventQueue.Enqueue.
        /// </summary>
        void Enqueue(int trackIndex, Data value);

        /// <summary>
        /// Dequeue a Data command for the given track index. Returns null when empty.
        /// Mirrors EventQueue.Dequeue.
        /// </summary>
        Data? Dequeue(int trackIndex);
    }
}
