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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

using System;
using MidiPlayer.SoundFont;

namespace MidiPlayerTest.SoundFont {
    /// <summary>
    /// unit tests for SoundFontInfo.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    [TestClass()]
    public class SoundFontInfoTests {
#nullable enable
        /// <summary>
        /// verifies that GetVoice correctly returns voice names for bank 0 prog 0, bank 8 prog 38, and bank 128 prog 8.
        /// </summary>
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
