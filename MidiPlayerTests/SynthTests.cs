
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using static System.Threading.Thread;

namespace MidiPlayer.Test {
    [TestClass()]
    public class SynthTests {
        [TestMethod()]
        public void GetBankTest1() {
            var result = Synth.GetBank(0);
            AreEqual(0, result);
        }

        [TestMethod()]
        public void GetBankTest2() {
            var result = Synth.GetBank(7);
            AreEqual(128, result);
        }

        [TestMethod()]
        public void GetProgramTest1() {
            var result = Synth.GetProgram(1);
            AreEqual(58, result);
        }

        [TestMethod()]
        public void GetProgramTest2() {
            var result = Synth.GetProgram(8);
            AreEqual(16, result);
        }

        [TestMethod()]
        public void GetVoiceTest1() {
            var result = Synth.GetVoice(1);
            AreEqual("Tuba", result);
        }

        [TestMethod()]
        public void GetVoiceTest2() {
            var result = Synth.GetVoice(8);
            AreEqual("Power Kit", result);
        }

        [TestMethod()]
        public void GetVoiceTest3() {
            var result = Synth.GetVoice(4);
            AreEqual("Warm Pad", result);
        }

        [TestMethod()]
        public void GetTrackNameTest1() {
            var result = Synth.GetTrackName(0);
            AreEqual("Cmon", result);
        }

        [TestMethod()]
        public void GetTrackNameTest2() {
            var result = Synth.GetTrackName(6);
            AreEqual("Bass", result);
        }

        [TestMethod()]
        public void GetTrackNameTest3() {
            var result = Synth.GetTrackName(7);
            AreEqual("Drum OverTop", result);
        }

        [TestInitialize]
        public void TestInitialize() {
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
            playSong();
            Sleep(3000);
        }

        [TestCleanup]
        public void TestCelean() {
            stopSong();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        async void playSong() {
            try {
                await Task.Run(() => Synth.Start());
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        async void stopSong() {
            try {
                await Task.Run(() => Synth.Stop());
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }
    }
}
