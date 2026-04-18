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

#nullable enable
using System;

namespace MidiPlayer.FluidSynth
{
    /// <summary>
    /// Public test helper interface implemented by FakeFluidSynth.
    /// Tests may cast FluidSynthAPI.Instance to IFakeFluidSynth to enqueue
    /// and trigger fake playback events without the native FluidSynth.
    /// </summary>
    public interface IFakeFluidSynth
    {
        /// <summary>
        /// Enqueues a fake MIDI event with the given parameters for test-controlled playback.
        /// </summary>
        /// <param name="type">MIDI event type (e.g. 144=NOTE_ON).</param>
        /// <param name="channel">MIDI channel (0-15).</param>
        /// <param name="key">MIDI note number.</param>
        /// <param name="velocity">Note velocity.</param>
        /// <param name="control">Controller number.</param>
        /// <param name="value">Controller value.</param>
        /// <param name="program">Program number.</param>
        /// <returns>An IntPtr handle to the fake event.</returns>
        IntPtr EnqueueFakeEvent(int type = 0, int channel = 0, int key = 0, int velocity = 0, int control = 0, int value = 0, int program = 0);

        /// <summary>
        /// Dequeues the next fake MIDI event handle from the internal queue.
        /// </summary>
        /// <returns>IntPtr.Zero when the queue is empty.</returns>
        IntPtr DequeueFakeEvent();

        /// <summary>
        /// Drains the fake event queue and fires the registered playback handler for each event.
        /// Used by tests to simulate a full playback pass.
        /// </summary>
        void TriggerPlaybackLoopOnce();

        /// <summary>
        /// Registers the managed playback handler delegate that will be invoked for each fake event.
        /// </summary>
        /// <param name="handler">The callback accepting (data, evt) and returning an int result code.</param>
        void RegisterPlaybackHandler(Func<IntPtr, IntPtr, int> handler);
    }
}
