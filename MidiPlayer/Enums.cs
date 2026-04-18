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

namespace MidiPlayer {
    /// <summary>
    /// common enums for app
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>

    /// <summary>
    /// activity result request codes used with Android StartActivityForResult.
    /// </summary>
    public enum Request {
        /// <summary>request code for selecting a SoundFont file.</summary>
        SoundFont = 128,
        /// <summary>request code for selecting a MIDI file.</summary>
        MidiFile = 256,
        /// <summary>request code for adding a file to the playlist.</summary>
        AddPlayList = 384,
    }

    /// <summary>
    /// MIDI control change controller numbers used in this application.
    /// </summary>
    public enum ControlChange {
        /// <summary>MIDI CC 7: channel volume (0-127).</summary>
        Volume = 7,
        /// <summary>MIDI CC 10: stereo pan position (0=left, 64=center, 127=right).</summary>
        Pan = 10,
    }

    /// <summary>
    /// MIDI channel numbers as zero-based enumeration values.
    /// </summary>
    public enum MidiChannel {
        /// <summary>MIDI channel 1 (zero-based index 0).</summary>
        ch1 = 0,
        /// <summary>MIDI channel 2 (zero-based index 1).</summary>
        ch2 = 1,
        /// <summary>MIDI channel 3 (zero-based index 2).</summary>
        ch3 = 2,
        /// <summary>MIDI channel 4 (zero-based index 3).</summary>
        ch4 = 3,
        /// <summary>MIDI channel 5 (zero-based index 4).</summary>
        ch5 = 4,
        /// <summary>MIDI channel 6 (zero-based index 5).</summary>
        ch6 = 5,
        /// <summary>MIDI channel 7 (zero-based index 6).</summary>
        ch7 = 6,
        /// <summary>MIDI channel 8 (zero-based index 7).</summary>
        ch8 = 7,
        /// <summary>MIDI channel 9 (zero-based index 8).</summary>
        ch9 = 8,
        /// <summary>MIDI channel 10 (percussion; zero-based index 9).</summary>
        ch10 = 9,
        /// <summary>MIDI channel 11 (zero-based index 10).</summary>
        ch11 = 10,
        /// <summary>MIDI channel 12 (zero-based index 11).</summary>
        ch12 = 11,
        /// <summary>MIDI channel 13 (zero-based index 12).</summary>
        ch13 = 12,
        /// <summary>MIDI channel 14 (zero-based index 13).</summary>
        ch14 = 13,
        /// <summary>MIDI channel 15 (zero-based index 14).</summary>
        ch15 = 14,
        /// <summary>MIDI channel 16 (zero-based index 15).</summary>
        ch16 = 15,
        /// <summary>sentinel value used by extension methods to iterate over all channels.</summary>
        Enum = -128,
    }
}
