
using System.Collections.Generic;

namespace MidiPlayer {
    /// <summary>
    /// playlist for synth
    /// </summary>
    public class PlayList {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        List<string> targetList = new List<string>();

        int idx;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public PlayList() {
            idx = 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public bool Ready {
            get => targetList.Count == 0 ? false : true;
        }

        public string[] List {
            get => targetList.ToArray();
        }

        public string Current {
            get => targetList[idx];
        }

        public string Next {
            get {
                if (idx == targetList.Count) {
                    idx = 0;
                }
                return targetList[idx++];
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public void Add(string target) {
            targetList.Add(target);
        }

        public void Clear() {
            targetList.Clear();
        }
    }
}
