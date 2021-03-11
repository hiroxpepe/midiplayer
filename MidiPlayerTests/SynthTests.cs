
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
            var _result = Synth.GetBank(0);
            AreEqual(0, _result);
        }

        [TestMethod()]
        public void GetBankTest2() {
            var _result = Synth.GetBank(7);
            AreEqual(128, _result);
        }

        [TestMethod()]
        public void GetProgramTest1() {
            var _result = Synth.GetProgram(1);
            AreEqual(58, _result);
        }

        [TestMethod()]
        public void GetProgramTest2() {
            var _result = Synth.GetProgram(8);
            AreEqual(16, _result);
        }

        [TestMethod()]
        public void GetVoiceTest1() {
            var _result = Synth.GetVoice(1);
            AreEqual("Tuba", _result);
        }

        [TestMethod()]
        public void GetVoiceTest2() {
            var _result = Synth.GetVoice(8);
            AreEqual("Power Kit", _result);
        }

        [TestMethod()]
        public void GetVoiceTest3() {
            var _result = Synth.GetVoice(4);
            AreEqual("Warm Pad", _result);
        }

        [TestMethod()]
        public void GetTrackNameTest1() {
            var _result = Synth.GetTrackName(0);
            AreEqual("Cmon", _result);
        }

        [TestMethod()]
        public void GetTrackNameTest2() {
            var _result = Synth.GetTrackName(6);
            AreEqual("Bass", _result);
        }

        [TestMethod()]
        public void GetTrackNameTest3() {
            var _result = Synth.GetTrackName(7);
            AreEqual("Drum OverTop", _result);
        }

        [TestInitialize]
        public void TestInitialize() {
            Synth.SoundFontPath = "../data/OmegaGMGS2.sf2";
            Synth.MidiFilePath = "../data/Cmon_v1.mid";
            Synth.OnMessage += (IntPtr data, IntPtr evt) => {
                return Synth.HandleEvent(data, evt);
            };
            Synth.OnStart += () => {
            };
            Synth.OnEnd += () => {
            };
            Synth.OnUpdate += (object sender, PropertyChangedEventArgs e) => {
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
