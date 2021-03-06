
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MidiPlayer.Midi.Test {
    [TestClass()]
    public class StandardMidiFileTests {
        [TestMethod()]
        public void GetTrackNameAndMidiChannelTest1() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _result0 = _target.GetTrackNameAndMidiChannel(0);
            Assert.AreEqual(("Cmon", -1), _result0);
            var _result1 = _target.GetTrackNameAndMidiChannel(1);
            Assert.AreEqual(("Vocal Main", 13), _result1);
            var _result2 = _target.GetTrackNameAndMidiChannel(2);
            Assert.AreEqual(("Vocal Cho", 0), _result2);
            var _result3 = _target.GetTrackNameAndMidiChannel(3);
            Assert.AreEqual(("Synth Sqe", 15), _result3);
            var _result4 = _target.GetTrackNameAndMidiChannel(4);
            Assert.AreEqual(("Synth Pad", 14), _result4);
            var _result5 = _target.GetTrackNameAndMidiChannel(5);
            Assert.AreEqual(("Guiter Riff", 12), _result5);
            var _result6 = _target.GetTrackNameAndMidiChannel(6);
            Assert.AreEqual(("Bass", 11), _result6);
            var _result7 = _target.GetTrackNameAndMidiChannel(7);
            Assert.AreEqual(("Drum OverTop", 9), _result7);
            var _result8 = _target.GetTrackNameAndMidiChannel(8);
            Assert.AreEqual(("Durm SN & BD", 9), _result8);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void GetTrackNameAndMidiChannelTest2() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            _target.GetTrackNameAndMidiChannel(9);
        }

        [TestMethod()]
        public void TrackCountTest1() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _result = _target.TrackCount;
            Assert.AreEqual(8, _result);
        }

        [TestMethod()]
        public void TrackCountIncludeConductorTrackTest1() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _result = _target.TrackCountIncludeConductorTrack;
            Assert.AreEqual(9, _result);
        }

        [TestMethod()]
        public void MidiChannelListTest1() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _result = _target.MidiChannelList;
            CollectionAssert.AreEqual(new List<int>() { 13, 0, 15, 14, 12, 11, 9, 9 }, _result);
        }

        [TestMethod()]
        public void GetTrackNameTest1() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _result1 = _target.GetTrackName(1, 13);
            Assert.AreEqual("Vocal Main", _result1);
            var _result2 = _target.GetTrackName(2, 0);
            Assert.AreEqual("Vocal Cho", _result2);
            var _result3 = _target.GetTrackName(3, 15);
            Assert.AreEqual("Synth Sqe", _result3);
            var _result4 = _target.GetTrackName(4, 14);
            Assert.AreEqual("Synth Pad", _result4);
            var _result5 = _target.GetTrackName(5, 12);
            Assert.AreEqual("Guiter Riff", _result5);
            var _result6 = _target.GetTrackName(6, 11);
            Assert.AreEqual("Bass", _result6);
            var _result7 = _target.GetTrackName(7, 9);
            Assert.AreEqual("Drum OverTop", _result7);
            var _result8 = _target.GetTrackName(8, 9);
            Assert.AreEqual("Durm SN & BD", _result8);
        }
    }
}
