
using System.Collections.Generic;
using System.Linq;

namespace MidiPlayer {
    /// <summary>
    /// rename Dictionary to Map
    /// </summary>
    public class Map<K, V> : Dictionary<K, V> {
    }

    /// <summary>
    /// event queue class to send synth
    /// </summary>
    public class EventQueue {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static Map<int, Queue<Data>> _queueMap;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static EventQueue() {
            _queueMap = new();
            Enumerable.Range(MIDI_TRACK_BASE, MIDI_TRACK_COUNT).ToList().ForEach(
                trackIdx => _queueMap.Add(trackIdx, new())
            );
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Enqueue(int trackIdx, Data value) {
            _queueMap[trackIdx].Enqueue(value);
        }

        public static Data Dequeue(int trackIdx) {
            return _queueMap[trackIdx].Count == 0 ? null : _queueMap[trackIdx].Dequeue();
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
}
