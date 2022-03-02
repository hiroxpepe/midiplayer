
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MidiPlayer.SoundFont.Tests {
    [TestClass()]
    public class SoundFontInfoTests {
        [TestMethod()]
        public void GetVoiceTest() {
            var target = new SoundFontInfo("../data/OmegaGMGS2.sf2");
            var result1 = target.GetVoice(0, 0);
            AreEqual("Grand Piano", result1); // bank:0, prog:0
            var result2 = target.GetVoice(8, 38);
            AreEqual("Synth Bass 3", result2); // bank:8, prog:38
            var result3 = target.GetVoice(128, 8);
            AreEqual("Room Kit", result3); // bank:128, prog:8
        }
    }
}
