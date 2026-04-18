#nullable enable
using System;

namespace MidiPlayer
{
    /// <summary>
    /// Thin adapter delegating to the existing static EventQueue implementation.
    /// Use this for production; tests can inject TestEventQueue instead.
    /// </summary>
    public sealed class StaticEventQueue : IEventQueue
    {
        public static readonly StaticEventQueue Instance = new StaticEventQueue();

        private StaticEventQueue() { }

        public void Enqueue(int trackIndex, Data value)
            => EventQueue.Enqueue(trackIndex, value);

        public Data? Dequeue(int trackIndex)
            => EventQueue.Dequeue(trackIndex);
    }
}
