
using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiPlayer.SoundFont {
    /// <summary>
    /// class for soundfont information
    /// </summary>
    public class SoundFontInfo {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        NAudio.SoundFont.SoundFont soundFont;

        Map<int, List<Voice>> map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public SoundFontInfo(string target) {
            try {
                soundFont = new NAudio.SoundFont.SoundFont(target);
                var _map = new Map<int, List<Voice>>();
                soundFont.Presets.ToList().ForEach(x => {
                    if (!_map.ContainsKey(x.Bank)) {
                        var _newList = new List<Voice>();
                        _newList.Add(new Voice() { Prog = x.PatchNumber, Name = x.Name });
                        _map.Add(x.Bank, _newList); // new bank and new voice
                    } else {
                        _map[x.Bank].Add(new Voice() { Prog = x.PatchNumber, Name = x.Name }); // exists bank and new voice
                    }
                });
                map = new Map<int, List<Voice>>();
                _map.OrderBy(x => x.Key).ToList().ForEach(x => { // sort bank
                    map.Add(x.Key, x.Value.OrderBy(_x => _x.Prog).ToList()); // sort prog
                });
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public string GetInstrumentName(int bank, int prog) {
            var _voice = map[bank];
            var _result = _voice.Where(x => x.Prog == prog);
            if (_result.Count() == 0) {
                return map[0].Where(x => x.Prog == prog).First().Name; // return default bank's voice.
            } else {
                return _voice.Where(x => x.Prog == prog).First().Name;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class Voice {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjective] 

            public int Prog {
                get; set;
            }

            public string Name {
                get; set;
            }
        }
    }
}
