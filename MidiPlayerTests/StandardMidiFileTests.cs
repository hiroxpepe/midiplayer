using MidiPlayer.Midi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Collections.Generic;

namespace MidiPlayer.Midi.Test {
    [TestClass()]
    public class StandardMidiFileTests {
        [TestMethod()]
        public void getTrackNameAndMidiChannelTest1() {
            var _target = new StandardMidiFile("../data/Cmon_v1.mid");
            var _private = new PrivateObject(_target);
            var _result0 = _private.Invoke("getTrackNameAndMidiChannel", 0);
            AreEqual(("Cmon", -1), _result0);
            var _result1 = _private.Invoke("getTrackNameAndMidiChannel", 1);
            AreEqual(("Vocal Main", 13), _result1);
            var _result2 = _private.Invoke("getTrackNameAndMidiChannel", 2);
            AreEqual(("Vocal Cho", 0), _result2);
            var _result3 = _private.Invoke("getTrackNameAndMidiChannel", 3);
            AreEqual(("Synth Sqe", 15), _result3);
            var _result4 = _private.Invoke("getTrackNameAndMidiChannel", 4);
            AreEqual(("Synth Pad", 14), _result4);
            var _result5 = _private.Invoke("getTrackNameAndMidiChannel", 5);
            AreEqual(("Guiter Riff", 12), _result5);
            var _result6 = _private.Invoke("getTrackNameAndMidiChannel", 6);
            AreEqual(("Bass", 11), _result6);
            var _result7 = _private.Invoke("getTrackNameAndMidiChannel", 7);
            AreEqual(("Drum OverTop", 9), _result7);
            var _result8 = _private.Invoke("getTrackNameAndMidiChannel", 8);
            AreEqual(("Durm SN & BD", 9), _result8);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void getTrackNameAndMidiChannelTest2() {
            var _target = new StandardMidiFile("../data/Cmon_v1.mid");
            var _private = new PrivateObject(_target);
            _private.Invoke("getTrackNameAndMidiChannel", 9);
        }

        [TestMethod()]
        public void TrackCountTest1() {
            var _target = new StandardMidiFile("../data/Cmon_v1.mid");
            var _result = _target.TrackCount;
            AreEqual(8, _result);
        }

        [TestMethod()]
        public void TrackCountTest2() {
            var _target = new StandardMidiFile("../data/ABC_v1.mid");
            var _result = _target.TrackCount;
            AreEqual(14, _result);
        }

        [TestMethod()]
        public void TrackCountTest3() {
            var _target = new StandardMidiFile("../data/DoYouSay_v4.mid");
            var _result = _target.TrackCount;
            AreEqual(8, _result);
        }

        [TestMethod()]
        public void trackCountIncludeConductorTrackTest1() {
            var _target = new StandardMidiFile("../data/Cmon_v1.mid");
            var _private = new PrivateObject(_target);
            var _result = _private.Invoke("trackCountIncludeConductorTrack");
            AreEqual(9, _result);
        }

        [TestMethod()]
        public void MidiChannelListTest1() {
            var _target = new StandardMidiFile("../data/Cmon_v1.mid");
            var _result = _target.MidiChannelList;
            CollectionAssert.AreEqual(new List<int>() { 13, 0, 15, 14, 12, 11, 9, 9 }, _result);
        }

        [TestMethod()]
        public void GetTrackNameTest1() {
            var _target = new StandardMidiFile("../data/Cmon_v1.mid");
            var _result1 = _target.GetTrackName(1);
            AreEqual("Vocal Main", _result1);
            var _result2 = _target.GetTrackName(2);
            AreEqual("Vocal Cho", _result2);
            var _result3 = _target.GetTrackName(3);
            AreEqual("Synth Sqe", _result3);
            var _result4 = _target.GetTrackName(4);
            AreEqual("Synth Pad", _result4);
            var _result5 = _target.GetTrackName(5);
            AreEqual("Guiter Riff", _result5);
            var _result6 = _target.GetTrackName(6);
            AreEqual("Bass", _result6);
            var _result7 = _target.GetTrackName(7);
            AreEqual("Drum OverTop", _result7);
            var _result8 = _target.GetTrackName(8);
            AreEqual("Durm SN & BD", _result8);
        }

        [TestMethod()]
        public void GetTrackNameTest2() {
            var _target = new StandardMidiFile("../data/Tornado_v2.mid");
            var _result0 = _target.GetTrackName(0);
            AreEqual("Tornado", _result0);
            var _result1 = _target.GetTrackName(1);
            AreEqual("Bass", _result1);
            var _result2 = _target.GetTrackName(2);
            AreEqual("Seque", _result2);
            var _result3 = _target.GetTrackName(3);
            AreEqual("Pad", _result3);
            var _result4 = _target.GetTrackName(4);
            AreEqual("Melody", _result4);
            var _result5 = _target.GetTrackName(5);
            AreEqual("Drum", _result5);
        }

        [TestMethod()]
        public void GetTrackNameTest3() {
            var _target = new StandardMidiFile("../data/ABC_v1.mid");
            var _result0 = _target.GetTrackName(0);
            AreEqual("ABC", _result0);
            var _result1 = _target.GetTrackName(1);
            AreEqual("Brass1", _result1);
            var _result2 = _target.GetTrackName(2);
            AreEqual("Brass2", _result2);
            var _result3 = _target.GetTrackName(3);
            AreEqual("Melody Main", _result3);
            var _result4 = _target.GetTrackName(4);
            AreEqual("Synth Reff", _result4);
            var _result5 = _target.GetTrackName(5);
            AreEqual("Synth Pad2", _result5);
            var _result6 = _target.GetTrackName(6);
            AreEqual("Synth Pad1", _result6);
            var _result7 = _target.GetTrackName(7);
            AreEqual("DX Reff", _result7);
            var _result8 = _target.GetTrackName(8);
            AreEqual("Drum Main", _result8);
            var _result9 = _target.GetTrackName(9);
            AreEqual("Percussion1", _result9);
            var _result10 = _target.GetTrackName(10);
            AreEqual("Percussion2", _result10);
            var _result11 = _target.GetTrackName(11);
            AreEqual("Bass", _result11);
            var _result12 = _target.GetTrackName(12);
            AreEqual("Bass over dub", _result12);
            var _result13 = _target.GetTrackName(13);
            AreEqual("DX Sequence", _result13);
            var _result14 = _target.GetTrackName(14);
            AreEqual("Orchestral Hit", _result14);
        }

        [TestMethod()]
        public void GetTrackNameTest4() {
            var _target = new StandardMidiFile("../data/DoYouSay_v4.mid");
            var _result0 = _target.GetTrackName(0);
            AreEqual("DoYouSay", _result0);
            var _result1 = _target.GetTrackName(1);
            AreEqual("Vocal Main", _result1);
            var _result2 = _target.GetTrackName(2);
            AreEqual("Vocal Cho", _result2);
            var _result3 = _target.GetTrackName(3);
            AreEqual("Synth Pad", _result3);
            var _result4 = _target.GetTrackName(4);
            AreEqual("Guiter Clean", _result4);
            var _result5 = _target.GetTrackName(5);
            AreEqual("Guiter Riff", _result5);
            var _result6 = _target.GetTrackName(6);
            AreEqual("Bass", _result6);
            var _result7 = _target.GetTrackName(7);
            AreEqual("Drum OverTop", _result7);
            var _result8 = _target.GetTrackName(8);
            AreEqual("Drum SN & BD", _result8);
        }
    }
}
