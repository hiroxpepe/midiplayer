﻿
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MidiPlayer.SoundFont.Tests {
    [TestClass()]
    public class SoundFontInfoTests {
        [TestMethod()]
        public void GetVoiceTest() {
            var _target = new SoundFontInfo("../data/OmegaGMGS2.sf2");
            var _result1 = _target.GetVoice(0, 0);
            AreEqual("Grand Piano", _result1); // bank:0, prog:0
            var _result2 = _target.GetVoice(8, 38);
            AreEqual("Synth Bass 3", _result2); // bank:8, prog:38
            var _result3 = _target.GetVoice(128, 8);
            AreEqual("Room Kit", _result3); // bank:128, prog:8
        }
    }
}
