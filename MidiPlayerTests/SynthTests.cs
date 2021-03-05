
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using static System.Threading.Thread;

namespace MidiPlayer.Test {
    [TestClass()]
    public class SynthTests {
        [TestMethod()]
        public void GetBankTest1() {
            var _result = Synth.GetBank(0);
            Assert.AreEqual(0, _result);
        }

        [TestMethod()]
        public void GetBankTest2() {
            var _result = Synth.GetBank(9);
            Assert.AreEqual(128, _result);
        }

        [TestMethod()]
        public void GetProgramTest1() {
            var _result = Synth.GetProgram(0);
            Assert.AreEqual(58, _result);
        }

        [TestMethod()]
        public void GetProgramTest2() {
            var _result = Synth.GetProgram(9);
            Assert.AreEqual(16, _result);
        }

        [TestInitialize]
        public void TestInitialize() {
            Synth.SoundFontPath = "../data/OmegaGMGS2.sf2";
            Synth.MidiFilePath = "../data/Cmon.mid";
            Synth.OnMessage += (IntPtr data, IntPtr evt) => {
                return Synth.HandleEvent(data, evt);
            };
            Synth.OnStart += () => {
            };
            Synth.OnEnd += () => {
            };
            playSong();
            Sleep(3000);
        }

        [TestCleanup]
        public void TestCelean() {
            stopSong();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

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
