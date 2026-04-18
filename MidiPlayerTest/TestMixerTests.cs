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
using System.ComponentModel;
using MidiPlayer;

namespace MidiPlayerTest
{
    /// <summary>
    /// unit tests for TestMixer (the DI-injectable IMixer adapter).
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    [TestClass]
    public class TestMixerTests
    {
        /// <summary>
        /// verifies that setting Current fires the Selected event and updates the current fader index.
        /// </summary>
        [TestMethod]
        public void CurrentChange_RaisesSelected()
        {
            var m = new TestMixer();
            bool called = false;
            m.Selected += (s, e) => { called = true; };

            m.Current = 3;

            Assert.IsTrue(called, "Selected event should be raised when Current changes");
            Assert.AreEqual(3, m.Current);
            Assert.AreEqual(3, m.GetCurrent().Index);
        }

        /// <summary>
        /// verifies that changing a Fader property raises the Updated event forwarded from the Fader's Updated event.
        /// </summary>
        [TestMethod]
        public void FaderChange_RaisesUpdated()
        {
            var m = new TestMixer();
            bool updated = false;
            m.Updated += (s, e) => { updated = true; };

            var f = m.GetCurrent();
            f.Volume = 55; // should trigger forwarding Updated event

            Assert.IsTrue(updated, "Updated should be forwarded from Fader to IMixer.Updated");
            Assert.AreEqual(55, f.Volume);
        }
    }
}
