#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MidiPlayer
{
    /// <summary>
    /// Instance-based mixer used for tests. Creates per-instance faders so parallel tests
    /// don't interfere with each other.
    /// </summary>
    public sealed class TestMixer : IMixer
    {
        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        readonly Dictionary<int, Mixer.Fader> _mixer;
        int _current;
        int _previous;

        PropertyChangedEventHandler? _on_selected;
        PropertyChangedEventHandler? _on_updated;

        public event PropertyChangedEventHandler? Selected
        {
            add => _on_selected += value;
            remove => _on_selected -= value;
        }

        public event PropertyChangedEventHandler? Updated
        {
            add => _on_updated += value;
            remove => _on_updated -= value;
        }

        public TestMixer(int trackCount = MIDI_TRACK_COUNT)
        {
            if (trackCount <= 0) trackCount = MIDI_TRACK_COUNT;
            _mixer = new Dictionary<int, Mixer.Fader>(trackCount);
            for (int i = 0; i < trackCount; i++)
            {
                var fader = new Mixer.Fader(i);
                // forward fader updates to listeners
                fader.Updated += (s, e) => _on_updated?.Invoke(s, e);
                _mixer.Add(i, fader);
            }
            _current = 0;
            _previous = 0;
        }

        public int Current
        {
            get => _current;
            set
            {
                _previous = _current;
                _current = value;
                _on_selected?.Invoke(null, new PropertyChangedEventArgs(nameof(Current)));
            }
        }

        public int CurrentAsOneBased => Current + 1;

        public Mixer.Fader GetCurrent() => _mixer[Current];

        public Mixer.Fader GetPrevious() => _mixer[_previous];

        public Mixer.Fader GetBy(int index) => _mixer[index];

        /// <summary>
        /// Clear and re-create internal faders (useful for test setup/teardown).
        /// </summary>
        public void Reset()
        {
            _mixer.Clear();
            for (int i = 0; i < MIDI_TRACK_COUNT; i++)
            {
                var fader = new Mixer.Fader(i);
                fader.Updated += (s, e) => _on_updated?.Invoke(s, e);
                _mixer.Add(i, fader);
            }
            _current = 0;
            _previous = 0;
        }
    }
}
