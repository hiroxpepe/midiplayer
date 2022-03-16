
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

        static Map<int, Fader> _mixer;

        static int _current;

        static int _previous;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static Mixer() {
            _mixer = new();
            _current = 0;
            Enumerable.Range(MIDI_TRACK_BASE, MIDI_TRACK_COUNT).ToList().ForEach(x => {
                Fader fader = new(x);
                fader.Updated += Updated;
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
        public static event PropertyChangedEventHandler? Selected;

        /// <summary>
        /// updated event handler.
        /// </summary>
        /// <remarks>
        /// called when Fader's properties change.<br/>
        /// </remarks>
        public static event PropertyChangedEventHandler? Updated;

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
                Selected?.Invoke(null, new(nameof(Current)));
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

        /// <summary>
        /// Fader class.
        /// </summary>
        public class Fader {
#nullable enable

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields [nouns, noun phrases]

            int _index = -1;

            bool _sounds = true; // mute param.

            string _name = "undefined";

            int _channel = -1;

            int _bank = 0;

            int _program = 0;

            int _volume = 104;

            int _pan = 64; // center

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            public Fader(int index) {
                _index = index;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Events [verb, verb phrase] 

            /// <summary>
            /// updated event handler.
            /// </summary>
            public event PropertyChangedEventHandler? Updated;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective]

            public int Index {
                get => _index;
                set {
                    _index = value; // TODO: Compare if the value is the same value as the previous.
                    Updated?.Invoke(this, new(nameof(Index))); // FIXME: not Invoke if two values are the same.
                }
            }

            public bool Sounds {
                get => _sounds;
                set {
                    _sounds = value; // TODO: Compare if the value is the same value as the previous.
                    Updated?.Invoke(this, new(nameof(Sounds))); // FIXME: not Invoke if two values are the same.
                }
            }

            public string Name {
                get => _name;
                set {
                    _name = value; // TODO: Compare if the value is the same value as the previous.
                    Updated?.Invoke(this, new(nameof(Name))); // FIXME: not Invoke if two values are the same.
                }
            }

            public int Channel {
                get => _channel;
                set {
                    _channel = value; // TODO: Compare if the value is the same value as the previous.
                    Updated?.Invoke(this, new(nameof(Channel))); // FIXME: not Invoke if two values are the same.
                }
            }

            public int Bank {
                get {
                    if (_channel == 9 && _bank != 128) {
                        return 128; // Drum
                    }
                    return _bank;
                }
                set {
                    _bank = value; // TODO: Compare if the value is the same value as the previous.
                    Updated?.Invoke(this, new(nameof(Bank))); // FIXME: not Invoke if two values are the same.
                }
            }

            public int Program {
                get => _program;
                set {
                    _program = value; // TODO: Compare if the value is the same value as the previous.
                    Updated?.Invoke(this, new(nameof(Program))); // FIXME: not Invoke if two values are the same.
                }
            }

            public int Volume {
                get => _volume;
                set {
                    _volume = value; // TODO: Compare if the value is the same value as the previous.
                    Updated?.Invoke(this, new(nameof(Volume))); // FIXME: not Invoke if two values are the same.
                }
            }

            public int Pan {
                get => _pan;
                set {
                    _pan = value; // TODO: Compare if the value is the same value as the previous.
                    Updated?.Invoke(this, new(nameof(Pan))); // FIXME: not Invoke if two values are the same.
                }
            }
        }
    }
}
