
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MidiPlayer.Midi.Test {
    [TestClass()]
    public class StandardMidiFileTests {
        [TestMethod()]
        public void getTrackNameAndMidiChannelTest1() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _private = new PrivateObject(_target);
            var _result0 = _private.Invoke("getTrackNameAndMidiChannel", 0);
            Assert.AreEqual(("Cmon", -1), _result0);
            var _result1 = _private.Invoke("getTrackNameAndMidiChannel", 1);
            Assert.AreEqual(("Vocal Main", 13), _result1);
            var _result2 = _private.Invoke("getTrackNameAndMidiChannel", 2);
            Assert.AreEqual(("Vocal Cho", 0), _result2);
            var _result3 = _private.Invoke("getTrackNameAndMidiChannel", 3);
            Assert.AreEqual(("Synth Sqe", 15), _result3);
            var _result4 = _private.Invoke("getTrackNameAndMidiChannel", 4);
            Assert.AreEqual(("Synth Pad", 14), _result4);
            var _result5 = _private.Invoke("getTrackNameAndMidiChannel", 5);
            Assert.AreEqual(("Guiter Riff", 12), _result5);
            var _result6 = _private.Invoke("getTrackNameAndMidiChannel", 6);
            Assert.AreEqual(("Bass", 11), _result6);
            var _result7 = _private.Invoke("getTrackNameAndMidiChannel", 7);
            Assert.AreEqual(("Drum OverTop", 9), _result7);
            var _result8 = _private.Invoke("getTrackNameAndMidiChannel", 8);
            Assert.AreEqual(("Durm SN & BD", 9), _result8);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void getTrackNameAndMidiChannelTest2() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _private = new PrivateObject(_target);
            _private.Invoke("getTrackNameAndMidiChannel", 9);
        }

        [TestMethod()]
        public void TrackCountTest1() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _result = _target.TrackCount;
            Assert.AreEqual(8, _result);
        }

        [TestMethod()]
        public void trackCountIncludeConductorTrackTest1() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _private = new PrivateObject(_target);
            var _result = _private.Invoke("trackCountIncludeConductorTrack");
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
            var _result1 = _target.GetTrackName(1);
            Assert.AreEqual("Vocal Main", _result1);
            var _result2 = _target.GetTrackName(2);
            Assert.AreEqual("Vocal Cho", _result2);
            var _result3 = _target.GetTrackName(3);
            Assert.AreEqual("Synth Sqe", _result3);
            var _result4 = _target.GetTrackName(4);
            Assert.AreEqual("Synth Pad", _result4);
            var _result5 = _target.GetTrackName(5);
            Assert.AreEqual("Guiter Riff", _result5);
            var _result6 = _target.GetTrackName(6);
            Assert.AreEqual("Bass", _result6);
            var _result7 = _target.GetTrackName(7);
            Assert.AreEqual("Drum OverTop", _result7);
            var _result8 = _target.GetTrackName(8);
            Assert.AreEqual("Durm SN & BD", _result8);
        }
    }
}
