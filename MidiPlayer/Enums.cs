
namespace MidiPlayer {
    /// <summary>
    /// common enums for app
    /// </summary>

    public enum Request {
        SoundFont = 128,
        MidiFile = 256,
        AddPlayList = 384,
    }

    public enum ControlChange {
        Volume = 7,
        Pan = 10,
    }

    public enum MidiChannel {
        ch1 = 0,
        ch2 = 1,
        ch3 = 2,
        ch4 = 3,
        ch5 = 4,
        ch6 = 5,
        ch7 = 6,
        ch8 = 7,
        ch9 = 8,
        ch10 = 9,
        ch11 = 10,
        ch12 = 11,
        ch13 = 12,
        ch14 = 13,
        ch15 = 14,
        ch16 = 15,
        // for Extension Method
        Enum = -128,
    }
}
