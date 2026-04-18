#nullable enable
using System.ComponentModel;

namespace MidiPlayer
{
    /// <summary>
    /// Adaptor interface for the static Mixer so callers can depend on an instance abstraction.
    /// Keeps the existing Mixer.Fader type to minimize changes.
    /// </summary>
    public interface IMixer
    {
        event PropertyChangedEventHandler? Selected;
        event PropertyChangedEventHandler? Updated;

        int Current { get; set; }
        int CurrentAsOneBased { get; }

        Mixer.Fader GetCurrent();
        Mixer.Fader GetPrevious();
        Mixer.Fader GetBy(int index);
    }
}
