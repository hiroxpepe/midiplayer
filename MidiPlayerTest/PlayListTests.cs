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

using MidiPlayer;

namespace MidiPlayerTest {
    /// <summary>
    /// unit tests for PlayList.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    [TestClass()]
    public class PlayListTests {
#nullable enable
        /// <summary>
        /// verifies that Next advances through items in order and wraps around, including after Add is called mid-iteration.
        /// </summary>
        [TestMethod()]
        public void NextTest1() {
            PlayList target = new();
            target.Add("file1.mid");
            target.Add("file2.mid");
            target.Add("file3.mid");
            AreEqual("file1.mid", target.Next);
            AreEqual("file2.mid", target.Next);
            AreEqual("file3.mid", target.Next);
            AreEqual("file1.mid", target.Next);
            AreEqual("file2.mid", target.Next);
            target.Add("file4.mid");
            AreEqual("file3.mid", target.Next);
            AreEqual("file4.mid", target.Next);
            AreEqual("file1.mid", target.Next);
            AreEqual("file2.mid", target.Next);
            AreEqual("file3.mid", target.Next);
            AreEqual("file4.mid", target.Next);
        }
    }
}
