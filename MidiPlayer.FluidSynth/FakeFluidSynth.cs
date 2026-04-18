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
using System.Collections.Concurrent;
using System.Collections.Generic;
using NativeFuncs;
using Sanford.Multimedia.Midi;

namespace MidiPlayer.FluidSynth
{
    /// <summary>
    /// Test/fake implementation of IFluidSynth to allow unit tests to simulate
    /// playback callbacks without requiring the native FluidSynth library.
    /// This class is intended for use in tests only and lives under MidiPlayer.FluidSynth
    /// so production code can depend on the interface while tests replace the Instance.
    /// </summary>
    public sealed class FakeFluidSynth : IFluidSynth, IFakeFluidSynth
    {
        /// <summary>the thread-safe queue of pending fake event handles.</summary>
        private readonly ConcurrentQueue<IntPtr> _events = new();
        /// <summary>maps each fake event handle to its MIDI event parameter tuple.</summary>
        private readonly System.Collections.Concurrent.ConcurrentDictionary<IntPtr, (int type, int channel, int key, int velocity, int control, int value, int program)> _eventMap = new();
        /// <summary>the incrementing counter used to generate unique fake event handles.</summary>
        private int _nextHandle;
        /// <summary>the path of the last MIDI file added via fluid_player_add; used by fluid_player_play to parse MIDI events.</summary>
        private string? _lastMidiFilePath;

        /// <summary>the native callback delegate registered via fluid_player_set_playback_callback; used as fallback when no managed handler is set.</summary>
        private NativeFuncs.Fluidsynth.handle_midi_event_func_t? _nativeHandler;
        /// <summary>the user data pointer passed alongside _nativeHandler.</summary>
        private IntPtr _nativeHandlerData = IntPtr.Zero;

        /// <summary>test stub: returns IntPtr.Zero (no native settings object is created).</summary>
        /// <returns>IntPtr.Zero.</returns>
        public IntPtr new_fluid_settings() => IntPtr.Zero;
        /// <summary>test stub: no-op; native settings cleanup is not needed in tests.</summary>
        /// <param name="settings">ignored.</param>
        public void delete_fluid_settings(IntPtr settings) { }
        /// <summary>test stub: returns IntPtr.Zero (no native synthesizer is created).</summary>
        /// <param name="settings">ignored.</param>
        /// <returns>IntPtr.Zero.</returns>
        public IntPtr new_fluid_synth(IntPtr settings) => IntPtr.Zero;
        /// <summary>test stub: no-op; native synthesizer cleanup is not needed in tests.</summary>
        /// <param name="synth">ignored.</param>
        public void delete_fluid_synth(IntPtr synth) { }
        /// <summary>test stub: returns IntPtr.Zero (no audio driver is created).</summary>
        /// <param name="settings">ignored.</param>
        /// <param name="synth">ignored.</param>
        /// <returns>IntPtr.Zero.</returns>
        public IntPtr new_fluid_audio_driver(IntPtr settings, IntPtr synth) => IntPtr.Zero;
        /// <summary>test stub: no-op; audio driver cleanup is not needed in tests.</summary>
        /// <param name="driver">ignored.</param>
        public void delete_fluid_audio_driver(IntPtr driver) { }

        /// <summary>test stub: loads an SF2 file via NAudio and registers its presets into Synth via RegisterTestVoice.</summary>
        /// <param name="synth">ignored.</param>
        /// <param name="filename">path to the .sf2 file.</param>
        /// <param name="reset_presets">ignored.</param>
        /// <returns>0 on success.</returns>
        public int fluid_synth_sfload(IntPtr synth, string filename, bool reset_presets)
        {
            try {
                if (!string.IsNullOrEmpty(filename)) {
                    var sf = new NAudio.SoundFont.SoundFont(filename);
                    foreach (var p in sf.Presets) {
                        if (p != null && p.Name != null) {
                            MidiPlayer.Synth.RegisterTestVoice(p.Bank, p.PatchNumber, p.Name);
                        }
                    }
                }
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
            return 0;
        }
        /// <summary>test stub: always returns 1 (any path is treated as a valid SoundFont).</summary>
        /// <param name="filename">ignored.</param>
        /// <returns>1.</returns>
        public int fluid_is_soundfont(string filename) => 1;

        /// <summary>test stub: always returns 0.</summary>
        /// <param name="synth">ignored.</param>
        /// <param name="chan">ignored.</param>
        /// <param name="key">ignored.</param>
        /// <param name="vel">ignored.</param>
        /// <returns>0.</returns>
        public int fluid_synth_noteon(IntPtr synth, int chan, int key, int vel) => 0;
        /// <summary>test stub: always returns 0.</summary>
        /// <param name="synth">ignored.</param>
        /// <param name="chan">ignored.</param>
        /// <param name="key">ignored.</param>
        /// <returns>0.</returns>
        public int fluid_synth_noteoff(IntPtr synth, int chan, int key) => 0;
        /// <summary>test stub: no-op.</summary>
        /// <param name="synth">ignored.</param>
        /// <param name="gain">ignored.</param>
        public void fluid_synth_set_gain(IntPtr synth, float gain) { }

        /// <summary>test stub: returns (IntPtr)1 as a sentinel player handle.</summary>
        /// <param name="synth">ignored.</param>
        /// <returns>(IntPtr)1.</returns>
        public IntPtr new_fluid_player(IntPtr synth) => (IntPtr)1;
        /// <summary>test stub: always returns 0.</summary>
        /// <param name="player">ignored.</param>
        /// <returns>0.</returns>
        public int delete_fluid_player(IntPtr player) => 0;

        /// <summary>records the MIDI file path for later use by fluid_player_play.</summary>
        /// <param name="player">ignored.</param>
        /// <param name="midifile">path to the .mid file to play.</param>
        /// <returns>0.</returns>
        public int fluid_player_add(IntPtr player, string midifile)
        {
            _lastMidiFilePath = midifile;
            return 0;
        }

        /// <summary>test stub: always returns 1 (any path is treated as a valid MIDI file).</summary>
        /// <param name="filename">ignored.</param>
        /// <returns>1.</returns>
        public int fluid_is_midifile(string filename) => 1;

        /// <summary>parses the last registered MIDI file using Sanford, enqueues program-change, controller, note-on and note-off events, then calls TriggerPlaybackLoopOnce to deliver them to the handler. All events are delivered without time delay (time-collapsed).</summary>
        /// <param name="player">ignored.</param>
        /// <returns>0.</returns>
        public int fluid_player_play(IntPtr player)
        {
            // Parse midi and enqueue program-change events so Synth's playback handler updates Multi
            try
            {
                if (!string.IsNullOrEmpty(_lastMidiFilePath))
                {
                    var seq = new Sequence();
                    seq.Load(_lastMidiFilePath);
                    for (int t = 0; t < seq.Count; t++)
                    {
                        var track = seq[t];
                        for (int i = 0; i < track.Count; i++)
                        {
                            var evt = track.GetMidiEvent(i);
                            if (evt.MidiMessage.MessageType == MessageType.Channel)
                            {
                                var msg = (ChannelMessage)evt.MidiMessage;
                                if (msg.Command == ChannelCommand.ProgramChange)
                                {
                                    // Apply initial program mapping directly to managed state so tests can inspect Program values deterministically.
                                    MidiPlayer.Synth.ApplyInitialProgramChange(msg.MidiChannel, msg.Data1);
                                    EnqueueFakeEvent(type: 192, channel: msg.MidiChannel, program: msg.Data1);
                                }
                                else if (msg.Command == ChannelCommand.Controller)
                                {
                                    // Controller events (e.g., bank select, volume, pan)
                                    MidiPlayer.Synth.ApplyInitialControlChange(msg.MidiChannel, msg.Data1, msg.Data2);
                                    EnqueueFakeEvent(type: 176, channel: msg.MidiChannel, control: msg.Data1, value: msg.Data2);
                                }
                                else if (msg.Command == ChannelCommand.NoteOn)
                                {
                                    EnqueueFakeEvent(type: 144, channel: msg.MidiChannel, key: msg.Data1, velocity: msg.Data2);
                                }
                                else if (msg.Command == ChannelCommand.NoteOff)
                                {
                                    EnqueueFakeEvent(type: 128, channel: msg.MidiChannel, key: msg.Data1);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            // Deliver queued events to the registered playback handler synchronously.
            // NOTE: All events are fired with no time delay (time-collapsed); MIDI delta times are ignored.
            // This is acceptable for unit tests that verify state transitions only.
            // For E2E timing tests, a timer-based loop with MIDI tick delays would be needed.
            TriggerPlaybackLoopOnce();
            return 0;
        }

        /// <summary>test stub: returns immediately since fake playback is synchronous.</summary>
        /// <param name="player">ignored.</param>
        /// <returns>0.</returns>
        public int fluid_player_join(IntPtr player) => 0;
        /// <summary>test stub: always returns 0.</summary>
        /// <param name="player">ignored.</param>
        /// <returns>0.</returns>
        public int fluid_player_stop(IntPtr player) => 0;

        // allow native-style registration (kept for parity)
        /// <summary>stores the native handler and its data for use when no managed handler is registered.</summary>
        /// <param name="player">ignored.</param>
        /// <param name="handler">the native MIDI callback delegate.</param>
        /// <param name="handler_data">user data to pass to the handler.</param>
        /// <returns>0.</returns>
        internal int fluid_player_set_playback_callback(IntPtr player, NativeFuncs.Fluidsynth.handle_midi_event_func_t handler, IntPtr handler_data)
        {
            _nativeHandler = handler;
            _nativeHandlerData = handler_data;
            return 0;
        }

        /// <summary>thread-local reentrancy guard; non-zero while a handler invocation is in progress.</summary>
        [ThreadStatic]
        private static int _inHandler;

        /// <summary>dispatches a MIDI event to the registered managed or native handler with a thread-local reentrancy guard to prevent infinite recursion.</summary>
        /// <param name="data">user data pointer passed to the handler.</param>
        /// <param name="evt">the fake MIDI event handle.</param>
        /// <returns>the handler's return value, or 0 if no handler is registered.</returns>
        public int fluid_synth_handle_midi_event(IntPtr data, IntPtr evt)
        {
            // Reentrancy guard: if we're already invoking the managed/native handler on this thread, avoid re-entering to prevent infinite recursion.
            if (_inHandler != 0) {
                return 0;
            }
            try {
                _inHandler = 1;
                // Prefer managed handler if present, otherwise invoke the stored native handler
                if (_managedHandler != null)
                {
                    return _managedHandler(data, evt);
                }
                if (_nativeHandler != null)
                {
                    return _nativeHandler(data, evt);
                }
                return 0;
            } finally {
                _inHandler = 0;
            }
        }

        /// <summary>the managed callback registered via RegisterPlaybackHandler; takes priority over _nativeHandler.</summary>
        private Func<IntPtr, IntPtr, int>? _managedHandler;

        /// <summary>stores the managed playback callback; takes priority over any previously registered native handler.</summary>
        /// <param name="handler">the managed delegate accepting (data, evt) and returning a result code.</param>
        public void RegisterPlaybackHandler(Func<IntPtr, IntPtr, int> handler)
        {
            // store the managed wrapper provided by FluidSynthAPI that invokes the Synth delegate
            _managedHandler = handler;
        }

        /// <summary>test stub: always returns 0.</summary>
        /// <param name="synth">ignored.</param>
        /// <param name="chan">ignored.</param>
        /// <param name="program">ignored.</param>
        /// <returns>0.</returns>
        public int fluid_synth_program_change(IntPtr synth, int chan, int program) => 0;
        /// <summary>test stub: always returns 0.</summary>
        /// <param name="synth">ignored.</param>
        /// <param name="chan">ignored.</param>
        /// <param name="ctrl">ignored.</param>
        /// <param name="val">ignored.</param>
        /// <returns>0.</returns>
        public int fluid_synth_cc(IntPtr synth, int chan, int ctrl, int val) => 0;

        /// <summary>returns the MIDI event type stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the fake event handle.</param>
        /// <returns>the MIDI status byte.</returns>
        public int fluid_midi_event_get_type(IntPtr evt) => _eventMap.TryGetValue(evt, out var v) ? v.type : 0;
        /// <summary>returns the MIDI channel stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the fake event handle.</param>
        /// <returns>the MIDI channel (0-15).</returns>
        public int fluid_midi_event_get_channel(IntPtr evt) => _eventMap.TryGetValue(evt, out var v) ? v.channel : 0;
        /// <summary>returns the note key stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the fake event handle.</param>
        /// <returns>the MIDI key number (0-127).</returns>
        public int fluid_midi_event_get_key(IntPtr evt) => _eventMap.TryGetValue(evt, out var v) ? v.key : 0;
        /// <summary>returns the velocity stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the fake event handle.</param>
        /// <returns>the velocity (0-127).</returns>
        public int fluid_midi_event_get_velocity(IntPtr evt) => _eventMap.TryGetValue(evt, out var v) ? v.velocity : 0;
        /// <summary>returns the controller number stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the fake event handle.</param>
        /// <returns>the controller number.</returns>
        public int fluid_midi_event_get_control(IntPtr evt) => _eventMap.TryGetValue(evt, out var v) ? v.control : 0;
        /// <summary>returns the controller value stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the fake event handle.</param>
        /// <returns>the controller value (0-127).</returns>
        public int fluid_midi_event_get_value(IntPtr evt) => _eventMap.TryGetValue(evt, out var v) ? v.value : 0;
        /// <summary>returns the program number stored for the given handle, or 0 if not found.</summary>
        /// <param name="evt">the fake event handle.</param>
        /// <returns>the program number (0-127).</returns>
        public int fluid_midi_event_get_program(IntPtr evt) => _eventMap.TryGetValue(evt, out var v) ? v.program : 0;

        /// <summary>creates a fake MIDI event with the given parameters, stores it in _eventMap under a new unique handle, and enqueues that handle in _events.</summary>
        /// <param name="type">the MIDI event type (status byte).</param>
        /// <param name="channel">the MIDI channel (0-15).</param>
        /// <param name="key">the MIDI key number (0-127).</param>
        /// <param name="velocity">the velocity (0-127).</param>
        /// <param name="control">the controller number.</param>
        /// <param name="value">the controller value (0-127).</param>
        /// <param name="program">the program number (0-127).</param>
        /// <returns>the IntPtr handle for the new event.</returns>
        public IntPtr EnqueueFakeEvent(int type = 0, int channel = 0, int key = 0, int velocity = 0, int control = 0, int value = 0, int program = 0)
        {
            var handle = System.Threading.Interlocked.Increment(ref _nextHandle);
            var ptr = (IntPtr)handle;
            _events.Enqueue(ptr);
            _eventMap[ptr] = (type, channel, key, velocity, control, value, program);
            return ptr;
        }

        /// <summary>dequeues the next handle from _events.</summary>
        /// <returns>IntPtr.Zero when the queue is empty.</returns>
        public IntPtr DequeueFakeEvent() => _events.TryDequeue(out var ptr) ? ptr : IntPtr.Zero;

        /// <summary>drains _events and calls the managed or native handler for each handle in order.</summary>
        public void TriggerPlaybackLoopOnce()
        {
            while (_events.TryDequeue(out var h))
            {
                if (_managedHandler != null)
                {
                    _managedHandler(_nativeHandlerData, h);
                }
                else if (_nativeHandler != null)
                {
                    _nativeHandler(_nativeHandlerData, h);
                }
            }
        }
    }
}
