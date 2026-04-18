#nullable enable
using System.Collections.Generic;

namespace MidiPlayer
{
    /// <summary>
    /// Simple in-memory event queue for tests. Not optimized for real-time; intended for unit tests
    /// where allocations are acceptable.
    /// </summary>
    public sealed class TestEventQueue : IEventQueue
    {
        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        readonly System.Collections.Concurrent.ConcurrentQueue<Data>[] _queues;

        public TestEventQueue(int trackCount = MIDI_TRACK_COUNT)
        {
            if (trackCount <= 0) trackCount = MIDI_TRACK_COUNT;
            _queues = new System.Collections.Concurrent.ConcurrentQueue<Data>[trackCount];
            for (int i = 0; i < _queues.Length; i++)
                _queues[i] = new System.Collections.Concurrent.ConcurrentQueue<Data>();
        }

        public void Enqueue(int trackIndex, Data value)
        {
            int idx = trackIndex - MIDI_TRACK_BASE;
            if ((uint)idx >= (uint)_queues.Length) throw new System.ArgumentOutOfRangeException(nameof(trackIndex));
            _queues[idx].Enqueue(value);
        }

        public Data? Dequeue(int trackIndex)
        {
            int idx = trackIndex - MIDI_TRACK_BASE;
            if ((uint)idx >= (uint)_queues.Length) throw new System.ArgumentOutOfRangeException(nameof(trackIndex));
            return _queues[idx].TryDequeue(out var d) ? d : null;
        }

        /// <summary>
        /// Clear all queues (useful for test setup/teardown).
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _queues.Length; i++)
            {
                while (_queues[i].TryDequeue(out _)) { }
            }
        }
    }
}
