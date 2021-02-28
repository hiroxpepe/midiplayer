
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MidiPlayer.Tests {
    [TestClass()]
    public class SoundFontInfoTests {
        [TestMethod()]
        public void GetInstrumentTest() {
            var _target = new SoundFont.SoundFontInfo("../data/OmegaGMGS2.sf2");
            var _result1 = _target.GetInstrumentName(0, 0);
            Assert.AreEqual("Grand Piano", _result1); // bank:0, prog:0
            var _result2 = _target.GetInstrumentName(8, 38);
            Assert.AreEqual("Synth Bass 3", _result2); // bank:8, prog:38
            var _result3 = _target.GetInstrumentName(128, 8);
            Assert.AreEqual("Room Kit", _result3); // bank:128, prog:8
        }
    }
}
