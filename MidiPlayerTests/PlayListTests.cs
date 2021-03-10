
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MidiPlayer.Test {
    [TestClass()]
    public class PlayListTests {
        [TestMethod()]
        public void NextTest1() {
            var _target = new PlayList();
            _target.Add("file1.mid");
            _target.Add("file2.mid");
            _target.Add("file3.mid");
            AreEqual("file1.mid", _target.Next);
            AreEqual("file2.mid", _target.Next);
            AreEqual("file3.mid", _target.Next);
            AreEqual("file1.mid", _target.Next);
            AreEqual("file2.mid", _target.Next);
            _target.Add("file4.mid");
            AreEqual("file3.mid", _target.Next);
            AreEqual("file4.mid", _target.Next);
            AreEqual("file1.mid", _target.Next);
            AreEqual("file2.mid", _target.Next);
            AreEqual("file3.mid", _target.Next);
            AreEqual("file4.mid", _target.Next);
        }
    }
}
