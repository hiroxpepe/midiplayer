using MidiPlayer.Midi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Collections.Generic;

namespace MidiPlayerTest.Midi {
    [TestClass()]
    public class StandardMidiFileTests {
#nullable enable
        [TestMethod()]
        public void getTrackNameAndMidiChannelTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var privateObj = new PrivateObject(target);
            var result0 = privateObj.Invoke("getTrackNameAndMidiChannel", 0);
            AreEqual(("Cmon", -1), result0);
            var result1 = privateObj.Invoke("getTrackNameAndMidiChannel", 1);
            AreEqual(("Vocal Main", 13), result1);
            var result2 = privateObj.Invoke("getTrackNameAndMidiChannel", 2);
            AreEqual(("Vocal Cho", 0), result2);
            var result3 = privateObj.Invoke("getTrackNameAndMidiChannel", 3);
            AreEqual(("Synth Sqe", 15), result3);
            var result4 = privateObj.Invoke("getTrackNameAndMidiChannel", 4);
            AreEqual(("Synth Pad", 14), result4);
            var result5 = privateObj.Invoke("getTrackNameAndMidiChannel", 5);
            AreEqual(("Guiter Riff", 12), result5);
            var result6 = privateObj.Invoke("getTrackNameAndMidiChannel", 6);
            AreEqual(("Bass", 11), result6);
            var result7 = privateObj.Invoke("getTrackNameAndMidiChannel", 7);
            AreEqual(("Drum OverTop", 9), result7);
            var result8 = privateObj.Invoke("getTrackNameAndMidiChannel", 8);
            AreEqual(("Durm SN & BD", 9), result8);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void getTrackNameAndMidiChannelTest2() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var privateObj = new PrivateObject(target);
            privateObj.Invoke("getTrackNameAndMidiChannel", 9);
        }

        [TestMethod()]
        public void TrackCountTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var result = target.TrackCount;
            AreEqual(8, result);
        }

        [TestMethod()]
        public void TrackCountTest2() {
            var target = new StandardMidiFile("../data/ABC_v1.mid");
            var result = target.TrackCount;
            AreEqual(14, result);
        }

        [TestMethod()]
        public void TrackCountTest3() {
            var target = new StandardMidiFile("../data/DoYouSay_v4.mid");
            var result = target.TrackCount;
            AreEqual(8, result);
        }

        [TestMethod()]
        public void MidiChannelListTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var result = target.MidiChannelList;
            CollectionAssert.AreEqual(new List<int>() { 13, 0, 15, 14, 12, 11, 9, 9 }, result);
        }

        [TestMethod()]
        public void GetTrackNameTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var result1 = target.GetTrackName(1);
            AreEqual("Vocal Main", result1);
            var result2 = target.GetTrackName(2);
            AreEqual("Vocal Cho", result2);
            var result3 = target.GetTrackName(3);
            AreEqual("Synth Sqe", result3);
            var result4 = target.GetTrackName(4);
            AreEqual("Synth Pad", result4);
            var result5 = target.GetTrackName(5);
            AreEqual("Guiter Riff", result5);
            var result6 = target.GetTrackName(6);
            AreEqual("Bass", result6);
            var result7 = target.GetTrackName(7);
            AreEqual("Drum OverTop", result7);
            var result8 = target.GetTrackName(8);
            AreEqual("Durm SN & BD", result8);
        }

        [TestMethod()]
        public void GetTrackNameTest2() {
            var target = new StandardMidiFile("../data/Tornado_v2.mid");
            var result0 = target.GetTrackName(0);
            AreEqual("Tornado", result0);
            var result1 = target.GetTrackName(1);
            AreEqual("Bass", result1);
            var result2 = target.GetTrackName(2);
            AreEqual("Seque", result2);
            var result3 = target.GetTrackName(3);
            AreEqual("Pad", result3);
            var result4 = target.GetTrackName(4);
            AreEqual("Melody", result4);
            var result5 = target.GetTrackName(5);
            AreEqual("Drum", result5);
        }

        [TestMethod()]
        public void GetTrackNameTest3() {
            var target = new StandardMidiFile("../data/ABC_v1.mid");
            var result0 = target.GetTrackName(0);
            AreEqual("ABC", result0);
            var result1 = target.GetTrackName(1);
            AreEqual("Brass1", result1);
            var result2 = target.GetTrackName(2);
            AreEqual("Brass2", result2);
            var result3 = target.GetTrackName(3);
            AreEqual("Melody Main", result3);
            var result4 = target.GetTrackName(4);
            AreEqual("Synth Reff", result4);
            var result5 = target.GetTrackName(5);
            AreEqual("Synth Pad2", result5);
            var result6 = target.GetTrackName(6);
            AreEqual("Synth Pad1", result6);
            var result7 = target.GetTrackName(7);
            AreEqual("DX Reff", result7);
            var result8 = target.GetTrackName(8);
            AreEqual("Drum Main", result8);
            var result9 = target.GetTrackName(9);
            AreEqual("Percussion1", result9);
            var result10 = target.GetTrackName(10);
            AreEqual("Percussion2", result10);
            var result11 = target.GetTrackName(11);
            AreEqual("Bass", result11);
            var result12 = target.GetTrackName(12);
            AreEqual("Bass over dub", result12);
            var result13 = target.GetTrackName(13);
            AreEqual("DX Sequence", result13);
            var result14 = target.GetTrackName(14);
            AreEqual("Orchestral Hit", result14);
        }

        [TestMethod()]
        public void GetTrackNameTest4() {
            var target = new StandardMidiFile("../data/DoYouSay_v4.mid");
            var result0 = target.GetTrackName(0);
            AreEqual("DoYouSay", result0);
            var result1 = target.GetTrackName(1);
            AreEqual("Vocal Main", result1);
            var result2 = target.GetTrackName(2);
            AreEqual("Vocal Cho", result2);
            var result3 = target.GetTrackName(3);
            AreEqual("Synth Pad", result3);
            var result4 = target.GetTrackName(4);
            AreEqual("Guiter Clean", result4);
            var result5 = target.GetTrackName(5);
            AreEqual("Guiter Riff", result5);
            var result6 = target.GetTrackName(6);
            AreEqual("Bass", result6);
            var result7 = target.GetTrackName(7);
            AreEqual("Drum OverTop", result7);
            var result8 = target.GetTrackName(8);
            AreEqual("Drum SN & BD", result8);
        }
    }
}
