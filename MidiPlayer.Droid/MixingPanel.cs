
using System.ComponentModel;
using System.Linq;

namespace MidiPlayer.Droid {

    /// <summary>
    /// Mixer object.
    /// </summary>
    static class Mixer {
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
                _mixer.Add(x, new Fader(x));
            });
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Events [verb, verb phrase] 

        /// <summary>
        /// static selected event handler.
        /// </summary>
        public static event PropertyChangedEventHandler? Selected;

        ///////////////////////////////////////////////////////////////////////////////////////////
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

        ///////////////////////////////////////////////////////////////////////////////////////////
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
    }

    /// <summary>
    /// Fader class.
    /// </summary>
    class Fader {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
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
                _index = value;
                Updated?.Invoke(this, new(nameof(Index)));
            }
        }

        public bool Sounds {
            get => _sounds;
            set {
                _sounds = value;
                Updated?.Invoke(this, new(nameof(Sounds)));
            }
        }

        public string Name {
            get => _name;
            set {
                _name = value;
                Updated?.Invoke(this, new(nameof(Name)));
            }
        }

        public int Channel {
            get => _channel;
            set {
                _channel = value;
                Updated?.Invoke(this, new(nameof(Channel)));
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
                _bank = value;
                Updated?.Invoke(this, new(nameof(Bank)));
            }
        }

        public int Program {
            get => _program;
            set {
                _program = value;
                Updated?.Invoke(this, new(nameof(Program)));
            }
        }

        public int Volume {
            get => _volume;
            set {
                _volume = value;
                Updated?.Invoke(this, new(nameof(Volume)));
            }
        }

        public int Pan {
            get => _pan;
            set {
                _pan = value;
                Updated?.Invoke(this, new(nameof(Pan)));
            }
        }
    }
}
