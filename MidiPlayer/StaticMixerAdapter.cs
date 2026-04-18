#nullable enable
using System.ComponentModel;

namespace MidiPlayer
{
    /// <summary>
    /// Thin adapter delegating to the existing static Mixer implementation.
    /// Provides an IMixer instance that forwards calls to Mixer static members.
    /// </summary>
    public sealed class StaticMixerAdapter : IMixer
    {
        public static readonly StaticMixerAdapter Instance = new StaticMixerAdapter();

        private StaticMixerAdapter() { }

        public event PropertyChangedEventHandler? Selected
        {
            add => Mixer.Selected += value;
            remove => Mixer.Selected -= value;
        }

        public event PropertyChangedEventHandler? Updated
        {
            add => Mixer.Updated += value;
            remove => Mixer.Updated -= value;
        }

        public int Current
        {
            get => Mixer.Current;
            set => Mixer.Current = value;
        }

        public int CurrentAsOneBased => Mixer.CurrentAsOneBased;

        public Mixer.Fader GetCurrent() => Mixer.GetCurrent();
        public Mixer.Fader GetPrevious() => Mixer.GetPrevious();
        public Mixer.Fader GetBy(int index) => Mixer.GetBy(index);
    }
}
