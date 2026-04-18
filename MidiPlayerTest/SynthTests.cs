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
using System.ComponentModel;
using System.Threading.Tasks;

using MidiPlayer;

namespace MidiPlayerTest {
    /// <summary>
    /// unit tests for Synth using FakeFluidSynth to avoid native FluidSynth calls.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    [TestClass()]
    public class SynthTests {
#nullable enable
        /// <summary>
        /// verifies that GetBank returns 0 for track index 0 (default bank).
        /// </summary>
        [TestMethod()]
        public void GetBankTest1() {
            var result = Synth.GetBank(0);
            AreEqual(0, result);
        }

        /// <summary>
        /// verifies that GetBank returns 128 (drum bank) for track index 7.
        /// </summary>
        [TestMethod()]
        public void GetBankTest2() {
            var result = Synth.GetBank(7);
            AreEqual(128, result);
        }

        /// <summary>
        /// verifies that GetProgram returns 58 for track index 1 after MIDI file loading.
        /// </summary>
        [TestMethod()]
        public void GetProgramTest1() {
            var result = Synth.GetProgram(1);
            AreEqual(58, result);
        }

        /// <summary>
        /// verifies that GetProgram returns 16 for track index 8 (drum track) after loading.
        /// </summary>
        [TestMethod()]
        public void GetProgramTest2() {
            var result = Synth.GetProgram(8);
            AreEqual(16, result);
        }

        /// <summary>
        /// verifies that GetVoice returns "Tuba" for track index 1.
        /// </summary>
        [TestMethod()]
        public void GetVoiceTest1() {
            var result = Synth.GetVoice(1);
            AreEqual("Tuba", result);
        }

        /// <summary>
        /// verifies that GetVoice returns "Power Kit" for track index 8.
        /// </summary>
        [TestMethod()]
        public void GetVoiceTest2() {
            var result = Synth.GetVoice(8);
            AreEqual("Power Kit", result);
        }

        /// <summary>
        /// verifies that GetVoice returns "Warm Pad" for track index 4.
        /// </summary>
        [TestMethod()]
        public void GetVoiceTest3() {
            var result = Synth.GetVoice(4);
            AreEqual("Warm Pad", result);
        }

        /// <summary>
        /// verifies that GetTrackName returns "Cmon" for track index 0 (song title track).
        /// </summary>
        [TestMethod()]
        public void GetTrackNameTest1() {
            var result = Synth.GetTrackName(0);
            AreEqual("Cmon", result);
        }

        /// <summary>
        /// verifies that GetTrackName returns "Bass" for track index 6.
        /// </summary>
        [TestMethod()]
        public void GetTrackNameTest2() {
            var result = Synth.GetTrackName(6);
            AreEqual("Bass", result);
        }

        /// <summary>
        /// verifies that GetTrackName returns "Drum OverTop" for track index 7.
        /// </summary>
        [TestMethod()]
        public void GetTrackNameTest3() {
            var result = Synth.GetTrackName(7);
            AreEqual("Drum OverTop", result);
        }

        /// <summary>
        /// sets up the FakeFluidSynth, loads test SoundFont and MIDI file, registers event handlers, and runs Synth.Start() to populate managed state.
        /// </summary>
        /// <returns>A Task awaited by the test runner.</returns>
        [TestInitialize]
        public async Task TestInitialize() {
            // Use the in-process fake FluidSynth to avoid native library calls during tests
            MidiPlayer.FluidSynth.FluidSynthAPI.Instance = new MidiPlayer.FluidSynth.FakeFluidSynth();

            Synth.SoundFontPath = "../data/OmegaGMGS2.sf2";
            Synth.MidiFilePath = "../data/Cmon_v1.mid";
            Synth.Playbacking += (IntPtr data, IntPtr evt) => {
                return Synth.HandleEvent(data, evt);
            };
            Synth.Started += () => {
            };
            Synth.Ended += () => {
            };
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
            };
            await Task.Run(() => Synth.Start());
        }

        /// <summary>
        /// stops the synth after each test to clean up native/managed state.
        /// </summary>
        /// <returns>A Task awaited by the test runner.</returns>
        [TestCleanup]
        public async Task TestCelean() {
            await Task.Run(() => Synth.Stop());
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

    }
}
