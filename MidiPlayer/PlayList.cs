
using System.Collections.Generic;

namespace MidiPlayer {
    /// <summary>
    /// playlist for synth
    /// </summary>
    public class PlayList {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        List<string> _targetList = new List<string>();

        int _idx;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public PlayList() {
            _idx = 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public bool Ready {
            get => _targetList.Count == 0 ? false : true;
        }

        public string[] List {
            get => _targetList.ToArray();
        }

        public string Current {
            get => _targetList[_idx];
        }

        public string Next {
            get {
                if (_idx == _targetList.Count) {
                    _idx = 0;
                }
                return _targetList[_idx++];
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public void Add(string target) {
            _targetList.Add(target);
        }

        public void Clear() {
            _targetList.Clear();
        }
    }
}
