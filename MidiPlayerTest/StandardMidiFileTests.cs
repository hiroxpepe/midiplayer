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

using MidiPlayer.Midi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Collections.Generic;

namespace MidiPlayerTest.Midi {
    /// <summary>
    /// unit tests for StandardMidiFile.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    [TestClass()]
    public class StandardMidiFileTests {
#nullable enable
        /// <summary>
        /// verifies track name and MIDI channel for all 9 raw sequence tracks in Cmon_v1.mid.
        /// </summary>
        [TestMethod()]
        public void GetTrackNameAndMidiChannelTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            AreEqual(("Cmon", -1),       target.GetTrackNameAndMidiChannel(0));
            AreEqual(("Vocal Main", 13), target.GetTrackNameAndMidiChannel(1));
            AreEqual(("Vocal Cho", 0),   target.GetTrackNameAndMidiChannel(2));
            AreEqual(("Synth Sqe", 15),  target.GetTrackNameAndMidiChannel(3));
            AreEqual(("Synth Pad", 14),  target.GetTrackNameAndMidiChannel(4));
            AreEqual(("Guiter Riff", 12),target.GetTrackNameAndMidiChannel(5));
            AreEqual(("Bass", 11),       target.GetTrackNameAndMidiChannel(6));
            AreEqual(("Drum OverTop", 9),target.GetTrackNameAndMidiChannel(7));
            AreEqual(("Durm SN & BD", 9),target.GetTrackNameAndMidiChannel(8));
        }

        /// <summary>
        /// verifies that accessing index 9 (out of range) throws ArgumentOutOfRangeException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void GetTrackNameAndMidiChannelTest2() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            target.GetTrackNameAndMidiChannel(9);
        }

        /// <summary>
        /// verifies that TrackCount is 8 for Cmon_v1.mid.
        /// </summary>
        [TestMethod()]
        public void TrackCountTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var result = target.TrackCount;
            AreEqual(8, result);
        }

        /// <summary>
        /// verifies that TrackCount is 14 for ABC_v1.mid.
        /// </summary>
        [TestMethod()]
        public void TrackCountTest2() {
            var target = new StandardMidiFile("../data/ABC_v1.mid");
            var result = target.TrackCount;
            AreEqual(14, result);
        }

        /// <summary>
        /// verifies that TrackCount is 8 for DoYouSay_v4.mid.
        /// </summary>
        [TestMethod()]
        public void TrackCountTest3() {
            var target = new StandardMidiFile("../data/DoYouSay_v4.mid");
            var result = target.TrackCount;
            AreEqual(8, result);
        }

        /// <summary>
        /// verifies the full MIDI channel list for Cmon_v1.mid.
        /// </summary>
        [TestMethod()]
        public void MidiChannelListTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var result = target.MidiChannelList;
            CollectionAssert.AreEqual(new List<int>() { 13, 0, 15, 14, 12, 11, 9, 9 }, result);
        }

        /// <summary>
        /// verifies all non-conductor track names in Cmon_v1.mid by index.
        /// </summary>
        [TestMethod()]
        public void GetTrackNameTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            AreEqual("Vocal Main",   target.GetTrackName(1));
            AreEqual("Vocal Cho",    target.GetTrackName(2));
            AreEqual("Synth Sqe",    target.GetTrackName(3));
            AreEqual("Synth Pad",    target.GetTrackName(4));
            AreEqual("Guiter Riff",  target.GetTrackName(5));
            AreEqual("Bass",         target.GetTrackName(6));
            AreEqual("Drum OverTop", target.GetTrackName(7));
            AreEqual("Durm SN & BD", target.GetTrackName(8));
        }

        /// <summary>
        /// verifies all track names in Tornado_v2.mid.
        /// </summary>
        [TestMethod()]
        public void GetTrackNameTest2() {
            var target = new StandardMidiFile("../data/Tornado_v2.mid");
            AreEqual("Tornado", target.GetTrackName(0));
            AreEqual("Bass",    target.GetTrackName(1));
            AreEqual("Seque",   target.GetTrackName(2));
            AreEqual("Pad",     target.GetTrackName(3));
            AreEqual("Melody",  target.GetTrackName(4));
            AreEqual("Drum",    target.GetTrackName(5));
        }

        /// <summary>
        /// verifies all 15 track names in ABC_v1.mid.
        /// </summary>
        [TestMethod()]
        public void GetTrackNameTest3() {
            var target = new StandardMidiFile("../data/ABC_v1.mid");
            AreEqual("ABC",            target.GetTrackName(0));
            AreEqual("Brass1",         target.GetTrackName(1));
            AreEqual("Brass2",         target.GetTrackName(2));
            AreEqual("Melody Main",    target.GetTrackName(3));
            AreEqual("Synth Reff",     target.GetTrackName(4));
            AreEqual("Synth Pad2",     target.GetTrackName(5));
            AreEqual("Synth Pad1",     target.GetTrackName(6));
            AreEqual("DX Reff",        target.GetTrackName(7));
            AreEqual("Drum Main",      target.GetTrackName(8));
            AreEqual("Percussion1",    target.GetTrackName(9));
            AreEqual("Percussion2",    target.GetTrackName(10));
            AreEqual("Bass",           target.GetTrackName(11));
            AreEqual("Bass over dub",  target.GetTrackName(12));
            AreEqual("DX Sequence",    target.GetTrackName(13));
            AreEqual("Orchestral Hit", target.GetTrackName(14));
        }

        /// <summary>
        /// verifies all track names in DoYouSay_v4.mid.
        /// </summary>
        [TestMethod()]
        public void GetTrackNameTest4() {
            var target = new StandardMidiFile("../data/DoYouSay_v4.mid");
            AreEqual("DoYouSay",     target.GetTrackName(0));
            AreEqual("Vocal Main",   target.GetTrackName(1));
            AreEqual("Vocal Cho",    target.GetTrackName(2));
            AreEqual("Synth Pad",    target.GetTrackName(3));
            AreEqual("Guiter Clean", target.GetTrackName(4));
            AreEqual("Guiter Riff",  target.GetTrackName(5));
            AreEqual("Bass",         target.GetTrackName(6));
            AreEqual("Drum OverTop", target.GetTrackName(7));
            AreEqual("Drum SN & BD", target.GetTrackName(8));
        }
    }
}
