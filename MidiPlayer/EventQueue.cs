
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

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static Map<int, Queue<Data>> queueMap = new Map<int, Queue<Data>>();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static EventQueue() {
            queueMap = new Map<int, Queue<Data>>();
            Enumerable.Range(0, 16).ToList().ForEach(x => queueMap.Add(x, new Queue<Data>()));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Enqueue(int idx, Data value) {
            queueMap[idx].Enqueue(value);
        }

        public static Data Dequeue(int idx) {
            return queueMap[idx].Count == 0 ? null : queueMap[idx].Dequeue();
        }
    }

    /// <summary>
    /// data class to send synth
    /// </summary>
    public class Data {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        int prog;

        int pan;

        int vol;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public int Prog {
            get => prog - 1;
            set => prog = value;
        }

        public int Pan {
            get => pan - 1;
            set => pan = value;
        }

        public int Vol {
            get => vol - 1;
            set => vol = value;
        }
    }
}
