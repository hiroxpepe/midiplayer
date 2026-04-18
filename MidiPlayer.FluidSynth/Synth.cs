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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using static MidiPlayer.FluidSynth.FluidSynthAPI;

using void_ptr = System.IntPtr;
using fluid_settings_t = System.IntPtr;
using fluid_synth_t = System.IntPtr;
using fluid_audio_driver_t = System.IntPtr;
using fluid_player_t = System.IntPtr;
using fluid_midi_event_t = System.IntPtr;
using MidiPlayer.Midi;
using MidiPlayer.SoundFont;

namespace MidiPlayer {
    /// <summary>
    /// the synth class.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class Synth {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        /// <summary>the default master gain applied to the FluidSynth synthesizer on initialization.</summary>
        const float SYNTH_GAIN = 0.5f;

        /// <summary>the base (zero) index for MIDI track slots.</summary>
        const int MIDI_TRACK_BASE = 0;
        /// <summary>the total number of MIDI track slots (0-15).</summary>
        const int MIDI_TRACK_COUNT = 16;

        /// <summary>MIDI event type for note-on (value: 144).</summary>
        const int NOTE_ON = 144;
        /// <summary>MIDI event type for note-off (value: 128).</summary>
        const int NOTE_OFF = 128;
        /// <summary>MIDI event type for program change (value: 192).</summary>
        const int PROGRAM_CHANGE = 192;
        /// <summary>MIDI event type for control change (value: 176).</summary>
        const int CONTROL_CHANGE = 176;

        /// <summary>MIDI CC number for bank select MSB (value: 0).</summary>
        const int BANK_SELECT_MSB = 0;
        /// <summary>MIDI CC number for bank select LSB (value: 32).</summary>
        const int BANK_SELECT_LSB = 32;
        /// <summary>MIDI CC number for channel volume (value: 7).</summary>
        const int VOLUME_MSB = 7;
        /// <summary>MIDI CC number for stereo pan (value: 10).</summary>
        const int PAN_MSB = 10;

        /// <summary>the volume value used to silence a muted channel (value: 0).</summary>
        const int MUTE_VOLUME = 0;
        /// <summary>the offset added to convert a zero-based index to a one-based value (value: 1).</summary>
        const int TO_ONE_BASED = 1;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        /// <summary>the native FluidSynth settings handle (fluid_settings_t).</summary>
        static fluid_settings_t _setting = IntPtr.Zero;

        /// <summary>the native FluidSynth synthesizer handle (fluid_synth_t).</summary>
        static fluid_synth_t _synth = IntPtr.Zero;

        /// <summary>the native FluidSynth MIDI player handle (fluid_player_t).</summary>
        static fluid_player_t _player = IntPtr.Zero;

        /// <summary>the native FluidSynth audio driver handle (fluid_audio_driver_t).</summary>
        static fluid_audio_driver_t _adriver = IntPtr.Zero;

        /// <summary>the native MIDI event callback delegate registered with fluid_player_set_playback_callback.</summary>
        static NativeFuncs.Fluidsynth.handle_midi_event_func_t _event_callback;

        /// <summary>the managed multicast delegate invoked for each incoming MIDI playback event.</summary>
        static Func<IntPtr, IntPtr, int> _on_playbacking;

        /// <summary>the managed action invoked when playback starts.</summary>
        static Action _on_started;

        /// <summary>the managed action invoked when playback ends naturally.</summary>
        static Action _on_ended;

        /// <summary>the managed property-changed event handler invoked when a Track property changes.</summary>
        static PropertyChangedEventHandler _on_updated;

        /// <summary>the full path to the currently loaded SoundFont file.</summary>
        static string _sound_font_path = string.Empty;

        /// <summary>the full path to the currently loaded MIDI file.</summary>
        static string _midi_file_path = string.Empty;

        /// <summary>the parsed SoundFont metadata for the currently loaded SoundFont.</summary>
        static SoundFontInfo _sound_font_info;

        /// <summary>the parsed Standard MIDI File metadata for the currently loaded MIDI file.</summary>
        static StandardMidiFile _standard_midi_file;

        /// <summary>true after Init() succeeds; false after Stop() cleans up.</summary>
        static bool _ready = false;

        /// <summary>true when a stop request is in progress, suppressing the natural Ended callback.</summary>
        static bool _stopping = false;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        /// <summary>initializes the _on_playbacking multicast delegate with the default handler that calls ProcessPlayback and, for the real P/Invoke implementation, also forwards the event to the native FluidSynth handler.</summary>
        static Synth() {
            _on_playbacking += (void_ptr data, fluid_midi_event_t evt) => {
                // Run the managed processing logic for both production and tests. Delegate through
                // FluidSynthAPI.Instance so FakeFluidSynth returns 0 in tests (no recursion) and
                // PInvokeFluidSynth forwards to the native library in production.
                ProcessPlayback(data, evt);
                return fluid_synth_handle_midi_event(data, evt);
            };
            // GC-root the callback delegate here so it can never be collected between Init() calls.
            // Previously this was done inside the Playbacking event add accessor, which meant the
            // delegate was only rooted after OnCreate subscribed — too late on Android where the
            // native audio thread can start before the UI finishes initialization.
            _event_callback = new NativeFuncs.Fluidsynth.handle_midi_event_func_t(_on_playbacking);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        /// <summary>the full path to the SoundFont file. Setting this property also loads the SoundFont metadata.</summary>
        public static string SoundFontPath {
            get => _sound_font_path;
            set {
                _sound_font_path = value;
                _sound_font_info = new SoundFontInfo(_sound_font_path);
                Log.Info("Synth set soundFontPath.");
            }
        }

        /// <summary>the full path to the MIDI file. Setting this property also parses the MIDI file metadata.</summary>
        public static string MidiFilePath {
            get => _midi_file_path;
            set {
                _midi_file_path = value;
                _standard_midi_file = new StandardMidiFile(_midi_file_path);
                Log.Info("Synth set midiFilePath.");
            }
        }

        /// <summary>the list of MIDI channel numbers used by the current MIDI file.</summary>
        public static List<int> MidiChannelList {
            get => _standard_midi_file.MidiChannelList;
        }

        /// <summary>the number of non-conductor MIDI tracks in the current MIDI file.</summary>
        public static int TrackCount {
            get => _standard_midi_file.TrackCount;
        }

        /// <summary>true when the synth has been initialized and playback is active.</summary>
        public static bool Playing {
            get => _ready;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Events [verb, verb phrase] 

        /// <summary>fired for each incoming MIDI event during playback.</summary>
        public static event Func<IntPtr, IntPtr, int> Playbacking {
            add => _on_playbacking += value;
            remove => _on_playbacking -= value;
        }

        /// <summary>fired when MIDI playback starts.</summary>
        public static event Action Started {
            add => _on_started += value;
            remove => _on_started -= value;
        }

        /// <summary>fired when MIDI playback ends naturally (not when Stop() is called).</summary>
        public static event Action Ended {
            add => _on_ended += value;
            remove => _on_ended -= value;
        }

        /// <summary>fired when a Track property changes (forwarded from Track.Updated via Multi).</summary>
        public static event PropertyChangedEventHandler Updated {
            add => _on_updated += value;
            remove => _on_updated -= value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        /// <summary>initializes all FluidSynth handles, loads the SoundFont and MIDI file, and creates the audio driver. Sets _ready = true on success.</summary>
        public static void Init() {
            try {
                if (!SoundFontPath.HasValue() || !MidiFilePath.HasValue()) {
                    Log.Warn("no sound font or no midi file path specified.");
                    return;
                }
                // Guard: reject paths that do not exist on disk before touching any native API.
                // fluid_player_add and fluid_is_soundfont silently pass SIGSEGV/abort when fed
                // a nonexistent path, which kills the process without throwing a managed exception.
                if (!System.IO.File.Exists(SoundFontPath)) {
                    Log.Error($"SoundFont file NOT FOUND on disk: {SoundFontPath}");
                    return;
                }
                if (!System.IO.File.Exists(MidiFilePath)) {
                    Log.Error($"MIDI file NOT FOUND on disk: {MidiFilePath}");
                    return;
                }
                _setting = new_fluid_settings();
                _synth = new_fluid_synth(_setting);
                fluid_synth_set_gain(_synth, SYNTH_GAIN);
                _player = new_fluid_player(_synth);
                Log.Info($"try to load the sound font: {SoundFontPath}");
                if (fluid_is_soundfont(SoundFontPath) != 1) {
                    Log.Error("not a valid sound font file.");
                    return;
                }
                fluid_player_set_playback_callback(_player, _event_callback, _synth);
                int sfont_id = fluid_synth_sfload(_synth, SoundFontPath, true);
                if (sfont_id == NativeFuncs.Fluidsynth.FLUID_FAILED) {
                    Log.Error("failed to load the sound font.");
                    return;
                } else {
                    Log.Info($"loaded the sound font: {SoundFontPath}");
                }
                Log.Info($"try to load the midi file: {MidiFilePath}");
                if (fluid_is_midifile(MidiFilePath) != 1) {
                    Log.Error("not a valid midi file.");
                    return;
                }
                Multi.StandardMidiFile = _standard_midi_file;
                int result = fluid_player_add(_player, MidiFilePath);
                if (result == NativeFuncs.Fluidsynth.FLUID_FAILED) {
                    Log.Error("failed to add the midi file to player.");
                    return;
                } else {
                    Log.Info($"added the midi file: {MidiFilePath}");
                }
                // Guard: if Stop() ran concurrently and cleared native handles while this
                // Init() was in the slow fluid_synth_sfload call, abort here. Calling
                // new_fluid_audio_driver with IntPtr.Zero arguments hangs indefinitely.
                // Only _player is checked alongside _stopping: settings and synth stubs in
                // FakeFluidSynth intentionally return IntPtr.Zero and must not be tested here.
                if (_stopping || _player.IsZero()) {
                    Log.Warn("Init() aborted: native handles were cleared by a concurrent Stop().");
                    return;
                }
                _adriver = new_fluid_audio_driver(_setting, _synth);
                _ready = true;
                Log.Info("init :)");
            } catch (Exception ex) {
                Log.Error($"[Init] {ex}");
            }
        }

        /// <summary>starts MIDI playback. Calls Init() if not yet ready, then fluid_player_play followed by a blocking fluid_player_join. Fires Started on play and Ended on natural completion.</summary>
        public static void Start() {
            try {
                if (!_ready) {
                    Init();
                    if (!_ready) {
                        Log.Error("failed to init.");
                        return;
                    }
                }
                Log.Info("Start: calling fluid_player_play...");
                fluid_player_play(_player);
                Log.Info("Start: fluid_player_play done, firing Started...");
                _on_started();
                Log.Info("Start: Started fired, calling fluid_player_join...");
                fluid_player_join(_player);
                Log.Info("end :D");
                if (_stopping == false) {
                    _on_ended();
                }
            } catch (Exception ex) {
                Log.Error($"[Start] {ex}");
            }
        }

        /// <summary>stops playback, calls final() to release all native handles, and requests GC collection.</summary>
        public static void Stop() {
            try {
                if (!_player.IsZero()) {
                    _stopping = true;
                    fluid_player_stop(_player);
                }
                final();
                Log.Info("stop :|");
                GC.Collect();
                Log.Info("GC.Collect.");
            } catch (Exception ex) {
                Log.Error($"[Stop] {ex}");
            }
        }

        /// <summary>forwards a MIDI event directly to FluidSynth's default handler.</summary>
        /// <param name="data">user data pointer.</param>
        /// <param name="evt">the MIDI event.</param>
        /// <returns>the handler's return code.</returns>
        public static int HandleEvent(IntPtr data, IntPtr evt) {
            return fluid_synth_handle_midi_event(data, evt);
        }

        /// <summary>
        /// Test helper: apply initial program change directly into managed state (Multi).
        /// Allows fakes to populate Multi without invoking native callbacks.
        /// </summary>
        public static void ApplyInitialProgramChange(int channel, int program) {
            Multi.ApplyProgramChange(channel, program);
        }

        /// <summary>
        /// Test helper: apply initial control change directly into managed state (Multi).
        /// </summary>
        public static void ApplyInitialControlChange(int channel, int control, int value) {
            Multi.ApplyControlChange(channel, control, value);
        }

        /// <summary>gets the MIDI channel from a raw MIDI event pointer.</summary>
        /// <param name="evt">the MIDI event.</param>
        /// <returns>the channel (0-15).</returns>
        public static int GetChannel(IntPtr evt) {
            int channel = fluid_midi_event_get_channel(evt);
            return channel;
        }

        /// <summary>gets the MIDI channel assigned to the given track slot.</summary>
        /// <param name="track_index">zero-based track index.</param>
        /// <returns>the channel (0-15).</returns>
        public static int GetChannel(int track_index) {
            int channel = Multi.GetBy(track_index).Channel;
            return channel;
        }

        /// <summary>gets the MIDI bank number for the given track slot. Unset bank (-1) is returned as 0.</summary>
        /// <param name="track_index">zero-based track index.</param>
        /// <returns>the bank number.</returns>
        public static int GetBank(int track_index) {
            int bank = Multi.GetBy(track_index).Bank;
            if (bank == -1) { // unset BANK_SELECT_LSB = 32
                bank = 0;
            }
            return bank;
        }

        /// <summary>gets the MIDI program number for the given track slot.</summary>
        /// <param name="track_index">zero-based track index.</param>
        /// <returns>the program number (0-127).</returns>
        public static int GetProgram(int track_index) {
            int program = Multi.GetBy(track_index).Program;
            return program;
        }

        /// <summary>the test-only map from "bank:program" key strings to voice names, populated by RegisterTestVoice.</summary>
        static Map<string, string> _test_voice_map = new();

        /// <summary>gets the voice (instrument) name for the given track slot. Checks _test_voice_map first, then falls back to SoundFontInfo.</summary>
        /// <param name="track_index">zero-based track index.</param>
        /// <returns>the voice name string.</returns>
        public static string GetVoice(int track_index) {
            int bank = GetBank(track_index);
            int program = GetProgram(track_index);
            string key = $"{bank}:{program}";
            if (_test_voice_map.ContainsKey(key)) {
                return _test_voice_map[key];
            }
            string voice = _sound_font_info.GetVoice(bank, program); 
            return voice;
        }

        /// <summary>
        /// Register a test-only voice name mapping (bank,program) -> name. Used by FakeFluidSynth to mirror SF2 presets
        /// into the managed Synth for deterministic tests.
        /// </summary>
        public static void RegisterTestVoice(int bank, int program, string name) {
            string key = $"{bank}:{program}";
            if (!_test_voice_map.ContainsKey(key)) {
                _test_voice_map.Add(key, name);
            }
        }

        /// <summary>gets the MIDI track name for the given track slot.</summary>
        /// <param name="track_index">zero-based track index.</param>
        /// <returns>the track name string.</returns>
        public static string GetTrackName(int track_index) {
            string name = Multi.GetBy(track_index).Name;
            return name;
        }

        /// <summary>returns whether the given track is currently sounding a note.</summary>
        /// <param name="track_index">zero-based track index.</param>
        /// <returns>true if the track is sounding.</returns>
        public static bool IsSounded(int track_index) {
            bool sounds = Multi.GetBy(track_index).Sounds;
            return sounds;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

        /// <summary>releases all native FluidSynth handles, resets all handle fields to IntPtr.Zero, and clears _ready and _stopping.</summary>
        static void final() {
            try {
                delete_fluid_audio_driver(_adriver);
                delete_fluid_player(_player);
                delete_fluid_synth(_synth);
                delete_fluid_settings(_setting);
                _adriver = IntPtr.Zero;
                _player = IntPtr.Zero;
                _synth = IntPtr.Zero;
                _setting = IntPtr.Zero;
                Log.Info("final :|");
            } catch (Exception ex) {
                Log.Error($"[final] {ex}");
            } finally {
                _ready = false;
                _stopping = false;
            }
        }

        /// <summary>
        /// Process a playback event's managed side-effects (update Multi and EventQueue) without invoking native fluidsynth handlers.
        /// This method is callable by fakes to avoid recursive native -> managed -> native loops.
        /// Wrapped in try-catch so that no C# exception can escape to the native JNI audio callback thread,
        /// which would otherwise manifest as Android.Runtime.JavaProxyThrowable and kill the process.
        /// </summary>
        public static int ProcessPlayback(IntPtr data, IntPtr evt) {
            try {
                var type = fluid_midi_event_get_type(evt);
                var channel = fluid_midi_event_get_channel(evt);
                var control = fluid_midi_event_get_control(evt);
                var value = fluid_midi_event_get_value(evt);
                var program = fluid_midi_event_get_program(evt);
                if (type == NOTE_ON) { // NOTE_ON = 144
                    Multi.ApplyNoteOn(channel);
                } else if (type == NOTE_OFF) { // NOTE_OFF = 128
                    Multi.ApplyNoteOff(channel);
                } else if (type == PROGRAM_CHANGE) { // PROGRAM_CHANGE = 192
                    Multi.ApplyProgramChange(channel, program);
                } else if (type == CONTROL_CHANGE) { // CONTROL_CHANGE = 176
                    Multi.ApplyControlChange(channel, control, value);
                }
                for (int track_index = MIDI_TRACK_BASE; track_index < MIDI_TRACK_BASE + MIDI_TRACK_COUNT; track_index++) {
                    var event_data = EventQueue.Dequeue(track_index);
                    if (event_data is not null) {
                        fluid_synth_program_change(_synth, event_data.Channel, event_data.Program);
                        fluid_synth_cc(_synth, event_data.Channel, (int) ControlChange.Pan, event_data.Pan);
                        if (event_data.Mute) {
                            fluid_synth_cc(_synth, event_data.Channel, (int) ControlChange.Volume, MUTE_VOLUME);
                        } else {
                            fluid_synth_cc(_synth, event_data.Channel, (int) ControlChange.Volume, event_data.Volume);
                        }
                        Multi.ApplyProgramChange(event_data.Channel, event_data.Program);
                    }
                }
                return 0;
            } catch (Exception ex) {
                //Log.Error($"[ProcessPlayback] {ex}");
                return 0;
            }
        }

        /// <summary>
        /// forwards property-change notifications from Track objects to the outer _on_updated event.
        /// Uses null-safe invoke and a try-catch so exceptions never escape into the native audio callback thread.
        /// </summary>
        /// <param name="sender">the Track that changed.</param>
        /// <param name="e">the property-change args.</param>
        static void onPropertyChanged(object sender, PropertyChangedEventArgs e) {
            try {
                _on_updated?.Invoke(sender, e);
            } catch (Exception ex) {
                //Log.Error($"[onPropertyChanged] {ex}");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        /// <summary>manages the collection of Track objects that represent all 16 MIDI channels during playback. Initialized by setting StandardMidiFile.</summary>
        static class Multi {
#nullable enable

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Fields [nouns, noun phrases]

            /// <summary>the map of zero-based track-slot index to Track instance.</summary>
            static Map<int, Track> _track_map;

            /// <summary>the parsed MIDI file whose track names and channels initialize the track map.</summary>
            static StandardMidiFile _standard_midi_file;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Constructor

            /// <summary>initializes the _track_map dictionary.</summary>
            static Multi() {
                _track_map = new();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // internal static Properties [noun, noun phrase, adjective]

            /// <summary>all Track instances in insertion order.</summary>
            internal static List<Track> List {
                get => _track_map.Select(x => x.Value).ToList();
            }

            /// <summary>the StandardMidiFile to use. Setting this property re-initializes _track_map via init().</summary>
            internal static StandardMidiFile StandardMidiFile {
                get => _standard_midi_file;
                set {
                    _standard_midi_file = value;
                    init();
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // internal static Methods [verb, verb phrases]

            /// <summary>
            /// NOTE_ON = 144
            /// </summary>
            /// <param name="channel">the MIDI channel (0-15) that received the note-on event.</param>
            internal static void ApplyNoteOn(int channel) {
                _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Sounds = true);
            }

            /// <summary>
            /// NOTE_OFF = 128
            /// </summary>
            /// <param name="channel">the MIDI channel (0-15) that received the note-off event.</param>
            internal static void ApplyNoteOff(int channel) {
                _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Sounds = false);
            }

            /// <summary>
            /// PROGRAM_CHANGE = 192
            /// </summary>
            /// <param name="channel">the MIDI channel (0-15).</param>
            /// <param name="program">the program number (0-127).</param>
            internal static void ApplyProgramChange(int channel, int program) {
                _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Program = program);
            }

            /// <summary>
            /// CONTROL_CHANGE = 176
            /// </summary>
            /// <param name="channel">the MIDI channel (0-15).</param>
            /// <param name="control">the controller number.</param>
            /// <param name="value">the controller value (0-127).</param>
            internal static void ApplyControlChange(int channel, int control, int value) {
                // BANK_SELECT_MSB =  0 [-- drums: 127 --]
                //     _type: 176, _control:  0, _value: 127
                // BANK_SELECT_LSB = 32
                //     _type: 176, _control: 32, _value:   0
                // VOLUME_MSB      =  7
                //     _type: 176, _control:  7, _value:  90 
                // PAN_MSB         = 10
                //     _type: 176, _control: 10, _value:  64 
                switch (control) {
                    case BANK_SELECT_MSB: // BANK_SELECT_MSB
                        if (channel == 9) { // Drum
                            _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Bank = value + 1); // 128
                        }
                        break;
                    case BANK_SELECT_LSB: // BANK_SELECT_LSB
                        if (channel != 9) { // not Drum
                            _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Bank = value);
                        }
                        break;
                    case VOLUME_MSB: // VOLUME_MSB
                        _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Volume = value);
                        break;
                    case PAN_MSB: // PAN_MSB
                        _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Pan = value);
                        break;
                    default:
                        break;
                }
            }

            /// <summary>
            /// gets a trak by index.
            /// </summary>
            /// <param name="index">zero-based slot index.</param>
            /// <returns>the Track at that index.</returns>
            internal static Track GetBy(int index) {
                Track track = _track_map[index];
                return track;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // private static Methods [verb, verb phrases]

            /// <summary>clears and rebuilds _track_map from the current _standard_midi_file, assigning track names and channels to each Track slot.</summary>
            static void init() {
                _track_map.Clear();
                Enumerable.Range(start: MIDI_TRACK_BASE, count: MIDI_TRACK_COUNT).ToList().ForEach(x => _track_map.Add(x, new Track(index: x)));
                _track_map[0].Name = _standard_midi_file.GetTrackName(track_index: 0); // a song name.
                Enumerable.Range(start: MIDI_TRACK_BASE, count: MIDI_TRACK_COUNT).ToList().ForEach(x => GetBy(index: x).Updated += onPropertyChanged);
                var list = _standard_midi_file.MidiChannelList;
                for (var index = 0; index < MidiChannelList.Count; index++) {
                    _track_map[index + 1].Channel = list[index]; // exclude conductor track;
                    _track_map[index + 1].Name = _standard_midi_file.GetTrackName(track_index: index + 1);
                }
            }
        }

        /// <summary>represents a single MIDI track slot, holding the channel, bank, program, volume, pan, name, and sounding state for one of the 16 MIDI channel slots.</summary>
        public class Track {
#nullable enable

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields [nouns, noun phrases]

            /// <summary>the zero-based track slot index.</summary>
            int _index = -1;

            /// <summary>true when this track is currently sounding a note.</summary>
            bool _sounds = false;

            /// <summary>the MIDI track name from the SMF metadata.</summary>
            string _name = "undefined";

            /// <summary>the MIDI channel assigned to this track slot (-1 if unset).</summary>
            int _channel = -1;

            /// <summary>the MIDI bank number (0-127; 128 for drum channel).</summary>
            int _bank = 0;

            /// <summary>the MIDI program number (0-127).</summary>
            int _program = 0;

            /// <summary>the MIDI volume (0-127; default 104).</summary>
            int _volume = 104;

            /// <summary>the stereo pan (0=left, 64=center, 127=right; default 64).</summary>
            int _pan = 64;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            /// <summary>initializes a new Track for the given slot index.</summary>
            /// <param name="index">the zero-based track slot index to assign.</param>
            internal Track(int index) {
                _index = index;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            //internal  Events [verb, verb phrase] 

            /// <summary>
            /// implementation for INotifyPropertyChanged
            /// </summary>
            internal event PropertyChangedEventHandler? Updated;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective]

            /// <summary>
            /// a track index value of an smf file.
            /// </summary>
            public int Index {
                get => _index;
                set {
                    _index = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Index)));
                }
            }

            /// <summary>
            /// a track index value of an smf file exclude conductor track.
            /// </summary>
            public int IndexWithExcludingConductor {
                get => Index - 1;
            }

            /// <summary>true when the track is currently playing a note (set by NOTE_ON/NOTE_OFF events).</summary>
            public bool Sounds {
                get => _sounds;
                set {
                    _sounds = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Sounds)));
                }
            }

            /// <summary>the MIDI track name read from the Standard MIDI File metadata.</summary>
            public string Name {
                get => _name;
                set {
                    _name = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Name)));
                }
            }

            /// <summary>the MIDI channel number assigned to this track (0-15; -1 if unset).</summary>
            public int Channel {
                get => _channel;
                set {
                    _channel = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Channel)));
                }
            }

            /// <summary>
            /// a midi channel number of a track as one-based value.
            /// </summary>
            public int ChannelAsOneBased {
                get => Channel + TO_ONE_BASED;
            }

            /// <summary>the MIDI bank number; automatically returns 128 (drum bank) for channel 9.</summary>
            public int Bank {
                get {
                    if (_channel == 9 && _bank != 128) {
                        return 128; // Drum
                    }
                    return _bank;
                }
                set {
                    _bank = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Bank)));
                }
            }

            /// <summary>the MIDI program (instrument) number (0-127).</summary>
            public int Program {
                get => _program;
                set {
                    _program = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Program)));
                }
            }

            /// <summary>the MIDI channel volume (0-127).</summary>
            public int Volume {
                get => _volume;
                set {
                    _volume = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Volume)));
                }
            }

            /// <summary>the stereo pan position (0=left, 64=center, 127=right).</summary>
            public int Pan {
                get => _pan;
                set {
                    _pan = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Pan)));
                }
            }
        }
    }
}
