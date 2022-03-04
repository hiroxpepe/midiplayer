
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
        // static Fields [nouns, noun phrases]

        static Map<int, Queue<Data>> _queueMap = new Map<int, Queue<Data>>();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static EventQueue() {
            _queueMap = new Map<int, Queue<Data>>();
            Enumerable.Range(0, 16).ToList().ForEach(x => _queueMap.Add(x, new Queue<Data>()));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Enqueue(int idx, Data value) {
            _queueMap[idx].Enqueue(value);
        }

        public static Data Dequeue(int idx) {
            return _queueMap[idx].Count == 0 ? null : _queueMap[idx].Dequeue();
        }
    }

    /// <summary>
    /// data class to send synth
    /// </summary>
    public class Data {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        int _prog;

        int _pan;

        int _vol;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public int Prog {
            get => _prog - 1;
            set => _prog = value;
        }

        public int Pan {
            get => _pan - 1;
            set => _pan = value;
        }

        public int Vol {
            get => _vol - 1;
            set => _vol = value;
        }
    }
}
