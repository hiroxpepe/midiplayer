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

using System.ComponentModel;
using System.Linq;

namespace MidiPlayer {
    /// <summary>
    /// Mixer object.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class Mixer {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        const int TO_ONE_BASED = 1;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        /// <summary>
        /// a Dictionary object that holds faders.
        /// </summary>
        static Map<int, Fader> _mixer;

        /// <summary>
        /// the current index value of the selected fader.
        /// </summary>
        /// <remarks>
        /// base index value is 0.
        /// </remarks>
        static int _current;

        /// <summary>
        /// the index value of the previously selected fader.
        /// </summary>
        static int _previous;

        /// <summary>
        /// func object to be called when a fader is selected.
        /// </summary>
        static PropertyChangedEventHandler? _on_selected;

        /// <summary>
        /// func object to be called when a fader is updated.
        /// </summary>
        static PropertyChangedEventHandler? _on_updated;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        /// <summary>
        /// static constructor.
        /// </summary>
        static Mixer() {
            _mixer = new();
            _current = 0;
            Enumerable.Range(start: MIDI_TRACK_BASE, count: MIDI_TRACK_COUNT).ToList().ForEach(x => {
                Fader fader = new(x);
                fader.Updated += onUpdate;
                _mixer.Add(x, fader);
            });
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Events [verb, verb phrase] 

        /// <summary>
        /// selected event handler.
        /// </summary>
        /// <note>
        /// called when Mixer's channel is clicked.<br/>
        /// </note>
        public static event PropertyChangedEventHandler? Selected {
            add => _on_selected += value;
            remove => _on_selected -= value;
        }

        /// <summary>
        /// updated event handler.
        /// </summary>
        /// <note>
        /// called when Fader's properties change.<br/>
        /// </note>
        public static event PropertyChangedEventHandler? Updated {
            add => _on_updated += value;
            remove => _on_updated -= value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Properties [noun, noun phrase, adjective]

        /// <summary>
        /// get selected fader number.
        /// </summary>
        /// <note>
        /// base index value is 0.
        /// </note>
        public static int Current {
            get => _current;
            set {
                _previous = _current;
                _current = value;
                Log.Info($"current: {_current}");
                _on_selected(null, new(nameof(Current)));
            }
        }

        /// <summary>
        /// get selected fader number as one-based value.
        /// </summary>
        public static int CurrentAsOneBased {
            get => Current + TO_ONE_BASED;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        /// <summary>
        /// get the current fader.
        /// </summary>
        public static Fader GetCurrent() {
            return _mixer[Current];
        }

        /// <summary>
        /// get the previous fader.
        /// </summary>
        public static Fader GetPrevious() {
            return _mixer[_previous];
        }

        /// <summary>
        /// get a fader by 0 based index value.
        /// </summary>
        public static Fader GetBy(int index) {
            return _mixer[index];
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

        /// <summary>
        /// called when a fader value is updated.
        /// </summary>
        static void onUpdate(object sender, PropertyChangedEventArgs e) {
            _on_updated(sender, e);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        /// <summary>
        /// Fader class.
        /// </summary>
        public class Fader {
#nullable enable

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields [nouns, noun phrases]

            /// <summary>
            /// a track index value of a fader.
            /// </summary>
            /// <note>
            /// base index value is 0, maxim value is MIDI_TRACK_COUNT.
            /// </note>
            int _index = -1;

            /// <summary>
            /// a value of whether the fader is on or off.
            /// </summary>
            bool _sounds = true; // mute parameter.

            /// <summary>
            /// a midi channel number of a fader.
            /// </summary>
            string _name = "undefined";

            /// <summary>
            /// a midi channel number of a fader.
            /// </summary>
            /// <note>
            /// base index value is 0, maxim value is MIDI_TRACK_COUNT.
            /// </note>
            int _channel = -1;

            /// <summary>
            /// a midi bank number of a fader.
            /// </summary>
            /// <note>
            /// minimum value is 0, maxim value is 127.
            /// </note>
            int _bank = -1;

            /// <summary>
            /// a midi program number of a fader.
            /// </summary>
            /// <note>
            /// minimum value is 0, maxim value is 127.
            /// </note>
            int _program = 0;

            /// <summary>
            /// a midi volume value of a fader.
            /// </summary>
            /// <note>
            /// minimum value is 0, maxim value is 127.
            /// </note>
            int _volume = 104;

            /// <summary>
            /// a midi pan value of a fader.
            /// </summary>
            /// <note>
            /// full left value is 0, center value is 64, full right value is 127.
            /// </note>
            int _pan = 64; // center

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            /// <summary>
            /// internal constructor.
            /// </summary>
            internal Fader(int index) {
                _index = index;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Events [verb, verb phrase] 

            /// <summary>
            /// updated event handler.
            /// </summary>
            internal event PropertyChangedEventHandler? Updated;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective]

            /// <summary>
            /// a track index value of a fader.
            /// </summary>
            public int Index {
                get => _index;
                set {
                    if (value != _index) {
                        _index = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Index)));
                    }
                }
            }

            /// <summary>
            /// a track index value of a fader.
            /// </summary>
            public int IndexAsOneBased {
                get => Index + TO_ONE_BASED;
                set => Index = value - TO_ONE_BASED;
            }

            /// <summary>
            /// a value of whether the fader is on or off.
            /// </summary>
            public bool Sounds {
                get => _sounds;
                set {
                    if (value != _sounds) {
                        _sounds = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Sounds)));
                    }
                }
            }

            /// <summary>
            /// a midi track name of a fader.
            /// </summary>
            public string Name {
                get => _name;
                set {
                    if (value != _name) {
                        _name = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Name)));
                    }
                }
            }

            /// <summary>
            /// a midi channel number of a fader.
            /// </summary>
            public int Channel {
                get => _channel;
                set {
                    if (value != _channel) {
                        _channel = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Channel)));
                    }
                }
            }

            /// <summary>
            /// a midi channel number of a fader.
            /// </summary>
            public int ChannelAsOneBased {
                get => Channel + TO_ONE_BASED;
                set => Channel = value - TO_ONE_BASED;
            }

            /// <summary>
            /// a midi bank number of a fader.
            /// </summary>
            public int Bank {
                get {
                    if (_channel == 9 && _bank != 128) {
                        return 128; // Drum
                    }
                    return _bank;
                }
                set {
                    if (value != _bank) {
                        _bank = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Bank)));
                    }
                }
            }

            /// <summary>
            /// a midi bank number of a fader.
            /// </summary>
            public int BankAsOneBased {
                get => Bank + TO_ONE_BASED;
                set => Bank = value - TO_ONE_BASED;
            }

            /// <summary>
            /// a midi program number of a fader.
            /// </summary>
            public int Program {
                get => _program;
                set {
                    if (value != _program) {
                        _program = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Program)));
                    }
                }
            }

            /// <summary>
            /// a midi program number of a fader.
            /// </summary>
            public int ProgramAsOneBased {
                get => Program + TO_ONE_BASED;
                set => Program = value - TO_ONE_BASED;
            }

            /// <summary>
            /// a midi volume value of a fader.
            /// </summary>
            public int Volume {
                get => _volume;
                set {
                    if (value != _volume) {
                        _volume = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Volume)));
                    }
                }
            }

            /// <summary>
            /// a midi pan value of a fader.
            /// </summary>
            public int Pan {
                get => _pan;
                set {
                    if (value != _pan) {
                        _pan = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Pan)));
                    }
                }
            }
        }
    }
}
