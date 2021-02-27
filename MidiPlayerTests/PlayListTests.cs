
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MidiPlayer.Test {
    [TestClass()]
    public class PlayListTests {
        [TestMethod()]
        public void NextTest1() {
            var _target = new PlayList();
            _target.Add("file1.mid");
            _target.Add("file2.mid");
            _target.Add("file3.mid");
            Assert.AreEqual("file1.mid", _target.Next);
            Assert.AreEqual("file2.mid", _target.Next);
            Assert.AreEqual("file3.mid", _target.Next);
            Assert.AreEqual("file1.mid", _target.Next);
            Assert.AreEqual("file2.mid", _target.Next);
            _target.Add("file4.mid");
            Assert.AreEqual("file3.mid", _target.Next);
            Assert.AreEqual("file4.mid", _target.Next);
            Assert.AreEqual("file1.mid", _target.Next);
            Assert.AreEqual("file2.mid", _target.Next);
            Assert.AreEqual("file3.mid", _target.Next);
            Assert.AreEqual("file4.mid", _target.Next);
        }
    }
}
