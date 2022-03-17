
using System.ComponentModel;
using System.Linq;

namespace MidiPlayer {

    /// <summary>
    /// Mixer object.
    /// </summary>
    public static class Mixer {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        /// <summary>
        /// a Dictionary object that holds faders.
        /// </summary>
        static Map<int, Fader> _mixer;

        /// <summary>
        /// the current index value of the selected fader.
        /// </summary>
        static int _current;

        /// <summary>
        /// the index value of the previously selected fader.
        /// </summary>
        static int _previous;

        /// <summary>
        /// func object to be called when a fader is selected.
        /// </summary>
        static PropertyChangedEventHandler _onSelected;

        /// <summary>
        /// func object to be called when a fader is updated.
        /// </summary>
        static PropertyChangedEventHandler _onUpdated;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        /// <summary>
        /// static constructor.
        /// </summary>
        static Mixer() {
            _mixer = new();
            _current = 0;
            Enumerable.Range(MIDI_TRACK_BASE, MIDI_TRACK_COUNT).ToList().ForEach(x => {
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
        /// <remarks>
        /// called when Mixer's channel is clicked.<br/>
        /// </remarks>
        public static event PropertyChangedEventHandler? Selected {
            add => _onSelected += value;
            remove => _onSelected -= value;
        }

        /// <summary>
        /// updated event handler.
        /// </summary>
        /// <remarks>
        /// called when Fader's properties change.<br/>
        /// </remarks>
        public static event PropertyChangedEventHandler? Updated {
            add => _onUpdated += value;
            remove => _onUpdated -= value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Properties [noun, noun phrase, adjective]

        /// <summary>
        /// get selected fader number.
        /// </summary>
        /// <remarks>
        /// base is 0 value.
        /// </remarks>
        public static int Current {
            get => _current;
            set {
                _previous = _current;
                _current = value;
                Log.Info($"current: {_current}");
                _onSelected(null, new(nameof(Current)));
            }
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
        /// get a fader by index.
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
            _onUpdated(sender, e);
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
            /// an index value of a fader.
            /// </summary>
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
            /// 
            /// </summary>
            int _channel = -1;

            /// <summary>
            /// a midi bank number of a fader.
            /// </summary>
            int _bank = 0;

            /// <summary>
            /// a midi program number of a fader.
            /// </summary>
            int _program = 0;

            /// <summary>
            /// a midi volume value of a fader.
            /// </summary>
            int _volume = 104;

            /// <summary>
            /// a midi pan value of a fader.
            /// </summary>
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
            /// an index value of a fader.
            /// </summary>
            public int Index {
                get => _index;
                set {
                    if (value != _index) {
                        _index = value;
                        Updated?.Invoke(this, new(nameof(Index)));
                    }
                }
            }

            /// <summary>
            /// a value of whether the fader is on or off.
            /// </summary>
            public bool Sounds {
                get => _sounds;
                set {
                    if (value != _sounds) {
                        _sounds = value;
                        Updated?.Invoke(this, new(nameof(Sounds)));
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
                        Updated?.Invoke(this, new(nameof(Name)));
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
                        Updated?.Invoke(this, new(nameof(Channel)));
                    }
                }
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
                        Updated?.Invoke(this, new(nameof(Bank)));
                    }
                }
            }

            /// <summary>
            /// a midi program number of a fader.
            /// </summary>
            public int Program {
                get => _program;
                set {
                    if (value != _program) {
                        _program = value;
                        Updated?.Invoke(this, new(nameof(Program)));
                    }
                }
            }

            /// <summary>
            /// a midi volume value of a fader.
            /// </summary>
            public int Volume {
                get => _volume;
                set {
                    if (value != _volume) {
                        _volume = value;
                        Updated?.Invoke(this, new(nameof(Volume)));
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
                        Updated?.Invoke(this, new(nameof(Pan)));
                    }
                }
            }
        }
    }
}
