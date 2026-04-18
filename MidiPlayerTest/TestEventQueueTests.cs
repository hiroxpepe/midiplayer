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
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MidiPlayer;

namespace MidiPlayerTest
{
    /// <summary>
    /// unit tests for TestEventQueue (the thread-safe IEventQueue test adapter backed by ConcurrentQueue).
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    [TestClass]
    public class TestEventQueueTests
    {
        /// <summary>
        /// verifies that a single enqueued item can be dequeued with all fields intact.
        /// </summary>
        [TestMethod]
        public void Enqueue_Dequeue_Basic()
        {
            var q = new TestEventQueue();
            var d = new Data { Volume = 42, Program = 7, Channel = 1 };
            q.Enqueue(0, d);

            var outD = q.Dequeue(0);
            Assert.IsNotNull(outD);
            Assert.AreEqual(42, outD.Volume);
            Assert.AreEqual(7, outD.Program);
            Assert.AreEqual(1, outD.Channel);
        }

        /// <summary>
        /// verifies that Dequeue returns null when the queue is empty.
        /// </summary>
        [TestMethod]
        public void Dequeue_Empty_ReturnsNull()
        {
            var q = new TestEventQueue();
            var outD = q.Dequeue(0);
            Assert.IsNull(outD);
        }

        /// <summary>
        /// verifies that Clear removes all enqueued items from the queue.
        /// </summary>
        [TestMethod]
        public void Clear_EmptiesQueues()
        {
            var q = new TestEventQueue();
            q.Enqueue(0, new Data { Volume = 1 });
            q.Enqueue(0, new Data { Volume = 2 });
            q.Clear();
            Assert.IsNull(q.Dequeue(0));
        }

        /// <summary>
        /// verifies thread-safety by enqueueing 8 × 500 items concurrently and confirming all 4000 items are dequeued.
        /// </summary>
        [TestMethod]
        public void ConcurrentEnqueue_AllItemsPresent()
        {
            var q = new TestEventQueue();
            int producers = 8;
            int perProducer = 500;
            var tasks = new List<Task>();
            for (int p = 0; p < producers; p++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int i = 0; i < perProducer; i++)
                    {
                        q.Enqueue(0, new Data { Volume = i });
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            int count = 0;
            while (q.Dequeue(0) is not null)
            {
                count++;
            }

            Assert.AreEqual(producers * perProducer, count);
        }
    }
}
