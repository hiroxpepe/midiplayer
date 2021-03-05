
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MidiPlayer.Midi.Test {
    [TestClass()]
    public class StandardMidiFileTests {
        [TestMethod()]
        public void GetTrackNameTest() {
            var _target = new StandardMidiFile("../data/Cmon.mid");
            var _result0 = _target.GetTrackName(0);
            Assert.AreEqual("Cmon", _result0);
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
