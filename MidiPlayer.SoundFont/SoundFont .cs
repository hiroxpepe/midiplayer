
using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiPlayer.SoundFont {
    /// <summary>
    /// class for soundfont information
    /// </summary>
    public class SoundFontInfo {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        NAudio.SoundFont.SoundFont _soundFont;

        Map<int, List<Voice>> _map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public SoundFontInfo(string target) {
            try {
                _soundFont = new NAudio.SoundFont.SoundFont(target);
                var map = new Map<int, List<Voice>>();
                _soundFont.Presets.ToList().ForEach(x => {
                    if (!map.ContainsKey(x.Bank)) {
                        var newList = new List<Voice>();
                        newList.Add(new Voice() { Prog = x.PatchNumber, Name = x.Name });
                        map.Add(x.Bank, newList); // new bank and new voice
                    } else {
                        map[x.Bank].Add(new Voice() { Prog = x.PatchNumber, Name = x.Name }); // exists bank and new voice
                    }
                });
                this._map = new Map<int, List<Voice>>();
                map.OrderBy(x => x.Key).ToList().ForEach(x => { // sort bank
                    this._map.Add(x.Key, x.Value.OrderBy(_x => _x.Prog).ToList()); // sort prog
                });
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public string GetVoice(int bank, int prog) {
            var voice = _map[bank];
            var result = voice.Where(x => x.Prog == prog);
            if (result.Count() == 0) {
                return _map[0].Where(x => x.Prog == prog).First().Name; // return default bank's voice.
            } else {
                return voice.Where(x => x.Prog == prog).First().Name;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class Voice {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective] 

            public int Prog {
                get; set;
            }

            public string Name {
                get; set;
            }
        }
    }
}
