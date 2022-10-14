/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiPlayer.SoundFont {
    /// <summary>
    /// class for soundfont information
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class SoundFontInfo {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        NAudio.SoundFont.SoundFont _sound_font;

        Map<int, List<Voice>> _map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public SoundFontInfo(string file_path) {
            try {
                _sound_font = new NAudio.SoundFont.SoundFont(file_path);
                Map<int, List<Voice>> map = new();
                _sound_font.Presets.ToList().ForEach(x => {
                    if (!map.ContainsKey(key: x.Bank)) {
                        List<Voice> new_list = new();
                        new_list.Add(item: new Voice() { Prog = x.PatchNumber, Name = x.Name });
                        map.Add(key: x.Bank, value: new_list); // new bank and new voice
                    } else {
                        map[x.Bank].Add(item: new Voice() { Prog = x.PatchNumber, Name = x.Name }); // exists bank and new voice
                    }
                });
                _map = new();
                map.OrderBy(x => x.Key).ToList().ForEach(x => { // sort bank
                    _map.Add(key: x.Key, value: x.Value.OrderBy(_x => _x.Prog).ToList()); // sort prog
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
            // internal Properties [noun, noun phrase, adjective] 

            internal int Prog {
                get; set;
            }

            internal string Name {
                get; set;
            }
        }
    }
}
