# Aggregated C# sources
Repository: C:\Users\hiroxpepe\Projects\midiplayer
Date: 2026-04-18 17:57:09Z



## MidiPlayer.FluidSynth\Fluidsynth.cs

```csharp
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

using System.Runtime.InteropServices;
using void_ptr = System.IntPtr;
using fluid_settings_t = System.IntPtr;
using fluid_synth_t = System.IntPtr;
using fluid_audio_driver_t = System.IntPtr;
using fluid_player_t = System.IntPtr;
using fluid_midi_event_t = System.IntPtr;

namespace NativeFuncs {
    /// <summary>
    /// class for Fluidsynth API definitions.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    internal static class Fluidsynth {
#nullable enable

        // must set to MidiPlayer.FluidSynth.csproj
#if RUNTIME_LINUX
        const string LIBLARY = "libfluidsynth.so";
#elif RUNTIME_WINDOWS
        const string LIBLARY = "libs/libfluidsynth-2.dll";
#endif
        const UnmanagedType LP_Str = UnmanagedType.LPStr;

        internal const int FLUID_OK = 0;

        internal const int FLUID_FAILED = -1;

        [DllImport(LIBLARY)]
        internal static extern fluid_settings_t new_fluid_settings();

        [DllImport(LIBLARY)]
        internal static extern void delete_fluid_settings(fluid_settings_t settings);

        [DllImport(LIBLARY)]
        internal static extern fluid_synth_t new_fluid_synth(fluid_settings_t settings);

        [DllImport(LIBLARY)]
        internal static extern void delete_fluid_synth(fluid_synth_t synth);

        [DllImport(LIBLARY)]
        internal static extern fluid_audio_driver_t new_fluid_audio_driver(fluid_settings_t settings, fluid_synth_t synth);

        [DllImport(LIBLARY)]
        internal static extern void delete_fluid_audio_driver(fluid_audio_driver_t driver);

        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_sfload(fluid_synth_t synth, [MarshalAs(LP_Str)] string filename, bool reset_presets);

        [DllImport(LIBLARY)]
        internal static extern int fluid_is_soundfont([MarshalAs(LP_Str)] string filename); // 1 or 0

        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_noteon(fluid_synth_t synth, int chan, int key, int vel);

        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_noteoff(fluid_synth_t synth, int chan, int key);

        [DllImport(LIBLARY)]
        internal static extern void fluid_synth_set_gain(fluid_synth_t synth, float gain);

        [DllImport(LIBLARY)]
        internal static extern fluid_player_t new_fluid_player(fluid_synth_t synth);

        [DllImport(LIBLARY)]
        internal static extern int delete_fluid_player(fluid_player_t player);

        [DllImport(LIBLARY)]
        internal static extern int fluid_player_add(fluid_player_t player, [MarshalAs(LP_Str)] string midifile);

        [DllImport(LIBLARY)]
        internal static extern int fluid_is_midifile([MarshalAs(LP_Str)] string filename); // 1 or 0

        [DllImport(LIBLARY)]
        internal static extern int fluid_player_play(fluid_player_t player);

        [DllImport(LIBLARY)]
        internal static extern int fluid_player_join(fluid_player_t player);

        [DllImport(LIBLARY)]
        internal static extern int fluid_player_stop(fluid_player_t player);

        internal delegate int handle_midi_event_func_t(void_ptr data, fluid_midi_event_t evt);

        [DllImport(LIBLARY)]
        internal static extern int fluid_player_set_playback_callback(fluid_player_t player, handle_midi_event_func_t handler, void_ptr handler_data);

        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_handle_midi_event(void_ptr data, fluid_midi_event_t evt);

        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_program_change(fluid_synth_t synth, int chan, int program);

        [DllImport(LIBLARY)]
        internal static extern int fluid_synth_cc(fluid_synth_t synth, int chan, int ctrl, int val);

        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_type(fluid_midi_event_t evt);

        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_channel(fluid_midi_event_t evt);

        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_key(fluid_midi_event_t evt);

        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_velocity(fluid_midi_event_t evt);

        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_control(fluid_midi_event_t evt);

        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_value(fluid_midi_event_t evt);

        [DllImport(LIBLARY)]
        internal static extern int fluid_midi_event_get_program(fluid_midi_event_t evt);
    }
}

// fluid_midi_event_type
// NOTE_OFF       = 128
// NOTE_ON        = 144
// CONTROL_CHANGE = 176
// PROGRAM_CHANGE = 192

// fluid_midi_control_chang
// BANK_SELECT_MSB =  0
// DATA_ENTRY_MSB  =  6
// VOLUME_MSB      =  7
// PAN_MSB         = 10
// EXPRESSION_MSB  = 11
// BANK_SELECT_LSB = 32
// DATA_ENTRY_LSB  = 38
```



## MidiPlayer.FluidSynth\Synth.cs

```csharp
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
using System.Threading.Tasks;

using static NativeFuncs.Fluidsynth;
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

        const float SYNTH_GAIN = 0.5f;

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        const int NOTE_ON = 144;
        const int NOTE_OFF = 128;
        const int PROGRAM_CHANGE = 192;
        const int CONTROL_CHANGE = 176;

        const int BANK_SELECT_MSB = 0;
        const int BANK_SELECT_LSB = 32;
        const int VOLUME_MSB = 7;
        const int PAN_MSB = 10;

        const int MUTE_VOLUME = 0;
        const int TO_ONE_BASED = 1;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static fluid_settings_t _setting = IntPtr.Zero;

        static fluid_synth_t _synth = IntPtr.Zero;

        static fluid_player_t _player = IntPtr.Zero;

        static fluid_audio_driver_t _adriver = IntPtr.Zero;

        static handle_midi_event_func_t _event_callback;

        static Func<IntPtr, IntPtr, int> _on_playbacking;

        static Action _on_started;

        static Action _on_ended;

        static PropertyChangedEventHandler _on_updated;

        static string _sound_font_path = string.Empty;

        static string _midi_file_path = string.Empty;

        static SoundFontInfo _sound_font_info;

        static StandardMidiFile _standard_midi_file;

        static bool _ready = false;

        static bool _stopping = false;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static Synth() {
            _on_playbacking += (void_ptr data, fluid_midi_event_t evt) => {
                var type = fluid_midi_event_get_type(evt);
                var channel = fluid_midi_event_get_channel(evt);
                var control = fluid_midi_event_get_control(evt);
                var value = fluid_midi_event_get_value(evt);
                var program = fluid_midi_event_get_program(evt);
                if (type != NOTE_ON && type != NOTE_OFF) { // not note on or note off
                    //Log.Debug($"type: {type} channel: {channel} control: {control} value: {value} program: {program}");
                }
                Task.Run(() => {
                    if (type == NOTE_ON) { // NOTE_ON = 144
                        Multi.ApplyNoteOn(channel);
                    } else if (type == NOTE_OFF) { // NOTE_OFF = 128
                        Multi.ApplyNoteOff(channel);
                    } else if (type == PROGRAM_CHANGE) { // PROGRAM_CHANGE = 192
                        Multi.ApplyProgramChange(channel, program);
                    } else if (type == CONTROL_CHANGE) { // CONTROL_CHANGE = 176
                        Multi.ApplyControlChange(channel, control, value);
                    }
                });
                Enumerable.Range(start: MIDI_TRACK_BASE, count: MIDI_TRACK_COUNT).ToList().ForEach(track_index => {
                    var event_data = EventQueue.Dequeue(track_index);
                    if (event_data is not null) {
                        fluid_synth_program_change(_synth, event_data.Channel, event_data.Program);
                        fluid_synth_cc(_synth, event_data.Channel, (int) ControlChange.Pan, event_data.Pan);
                        if (event_data.Mute) {
                            fluid_synth_cc(_synth, event_data.Channel, (int) ControlChange.Volume, MUTE_VOLUME);
                        } else {
                            fluid_synth_cc(_synth, event_data.Channel, (int) ControlChange.Volume, event_data.Volume);
                        }
                        Task.Run(() => {
                            Multi.ApplyProgramChange(event_data.Channel, event_data.Program);
                        });
                    }
                });
                return fluid_synth_handle_midi_event(data, evt);
            };
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        public static string SoundFontPath {
            get => _sound_font_path;
            set {
                _sound_font_path = value;
                _sound_font_info = new SoundFontInfo(_sound_font_path);
                Log.Info("Synth set soundFontPath.");
            }
        }

        public static string MidiFilePath {
            get => _midi_file_path;
            set {
                _midi_file_path = value;
                _standard_midi_file = new StandardMidiFile(_midi_file_path);
                Log.Info("Synth set midiFilePath.");
            }
        }

        public static List<int> MidiChannelList {
            get => _standard_midi_file.MidiChannelList;
        }

        public static int TrackCount {
            get => _standard_midi_file.TrackCount;
        }

        public static bool Playing {
            get => _ready;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Events [verb, verb phrase] 

        public static event Func<IntPtr, IntPtr, int> Playbacking {
            add {
                _on_playbacking += value;
                _event_callback = new handle_midi_event_func_t(_on_playbacking);
            }
            remove => _on_playbacking -= value;
        }

        public static event Action Started {
            add => _on_started += value;
            remove => _on_started -= value;
        }

        public static event Action Ended {
            add => _on_ended += value;
            remove => _on_ended -= value;
        }

        public static event PropertyChangedEventHandler Updated {
            add => _on_updated += value;
            remove => _on_updated -= value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Init() {
            try {
                if (!SoundFontPath.HasValue() || !MidiFilePath.HasValue()) {
                    Log.Warn("no sound font or no midi file.");
                    return;
                }
                _setting = new_fluid_settings();
                _synth = new_fluid_synth(_setting);
                fluid_synth_set_gain(_synth, SYNTH_GAIN);
                _player = new_fluid_player(_synth);
                Log.Info($"try to load the sound font: {SoundFontPath}");
                if (fluid_is_soundfont(SoundFontPath) != 1) {
                    Log.Error("not a sound font.");
                    return;
                }
                fluid_player_set_playback_callback(_player, _event_callback, _synth);
                int sfont_id = fluid_synth_sfload(_synth, SoundFontPath, true);
                if (sfont_id == FLUID_FAILED) {
                    Log.Error("failed to load the sound font.");
                    return;
                } else {
                    Log.Info($"loaded the sound font: {SoundFontPath}");
                }
                Log.Info($"try to load the midi file: {MidiFilePath}");
                if (fluid_is_midifile(MidiFilePath) != 1) {
                    Log.Error("not a midi file.");
                    return;
                }
                Multi.StandardMidiFile = _standard_midi_file;
                int result = fluid_player_add(_player, MidiFilePath);
                if (result == FLUID_FAILED) {
                    Log.Error("failed to load the midi file.");
                    return;
                } else {
                    Log.Info($"loaded the midi file: {MidiFilePath}");
                }
                _adriver = new_fluid_audio_driver(_setting, _synth);
                _ready = true;
                Log.Info("init :)");
            } catch (Exception ex) {
                Log.Error(ex.Message);
                // FIXME: terminate Fluidsynth.
            }
        }

        public static void Start() {
            try {
                if (!_ready) {
                    Init();
                    if (!_ready) {
                        Log.Error("failed to init.");
                        return;
                    }
                }
                fluid_player_play(_player);
                Log.Info("start :)");
                _on_started();
                fluid_player_join(_player);
                Log.Info("end :D");
                if (_stopping == false) {
                    _on_ended();
                }
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

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
                Log.Error(ex.Message);
            }
        }

        public static int HandleEvent(IntPtr data, IntPtr evt) {
            return fluid_synth_handle_midi_event(data, evt);
        }

        public static int GetChannel(IntPtr evt) {
            int channel = fluid_midi_event_get_channel(evt);
            return channel;
        }

        public static int GetChannel(int track_index) {
            int channel = Multi.GetBy(track_index).Channel;
            return channel;
        }

        public static int GetBank(int track_index) {
            int bank = Multi.GetBy(track_index).Bank;
            if (bank == -1) { // unset BANK_SELECT_LSB = 32
                bank = 0;
            }
            return bank;
        }

        public static int GetProgram(int track_index) {
            int program = Multi.GetBy(track_index).Program;
            return program;
        }

        public static string GetVoice(int track_index) {
            int bank = GetBank(track_index);
            int program = GetProgram(track_index);
            string voice = _sound_font_info.GetVoice(bank, program); 
            return voice;
        }

        public static string GetTrackName(int track_index) {
            string name = Multi.GetBy(track_index).Name;
            return name;
        }

        public static bool IsSounded(int track_index) {
            bool sounds = Multi.GetBy(track_index).Sounds;
            return sounds;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

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
                Log.Error(ex.Message);
            } finally {
                _ready = false;
                _stopping = false;
            }
        }

        static void onPropertyChanged(object sender, PropertyChangedEventArgs e) {
            _on_updated(sender, e);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        static class Multi {
#nullable enable

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Fields [nouns, noun phrases]

            static Map<int, Track> _track_map;

            static StandardMidiFile _standard_midi_file;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Constructor

            static Multi() {
                _track_map = new();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // internal static Properties [noun, noun phrase, adjective]

            internal static List<Track> List {
                get => _track_map.Select(x => x.Value).ToList();
            }

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
            internal static void ApplyNoteOn(int channel) {
                _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Sounds = true);
            }

            /// <summary>
            /// NOTE_OFF = 128
            /// </summary>
            internal static void ApplyNoteOff(int channel) {
                _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Sounds = false);
            }

            /// <summary>
            /// PROGRAM_CHANGE = 192
            /// </summary>
            internal static void ApplyProgramChange(int channel, int program) {
                _track_map.Where(x => x.Value.Channel == channel).ToList().ForEach(x => x.Value.Program = program);
            }

            /// <summary>
            /// CONTROL_CHANGE = 176
            /// </summary>
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
            internal static Track GetBy(int index) {
                Track track = _track_map[index];
                return track;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // private static Methods [verb, verb phrases]

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

        public class Track {
#nullable enable

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields [nouns, noun phrases]

            int _index = -1;

            bool _sounds = false;

            string _name = "undefined";

            int _channel = -1;

            int _bank = 0;

            int _program = 0;

            int _volume = 104;

            int _pan = 64;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

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

            public bool Sounds {
                get => _sounds;
                set {
                    _sounds = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Sounds)));
                }
            }

            public string Name {
                get => _name;
                set {
                    _name = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Name)));
                }
            }

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

            public int Program {
                get => _program;
                set {
                    _program = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Program)));
                }
            }

            public int Volume {
                get => _volume;
                set {
                    _volume = value;
                    Updated?.Invoke(sender: this, e: new(nameof(Volume)));
                }
            }

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

```



## MidiPlayer.Midi\StandardMidiFile.cs

```csharp
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
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace MidiPlayer.Midi {
    /// <summary>
    /// class for standard midi file
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class StandardMidiFile {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        Sequence _sequence;

        Map<int, (string name, int channel)> _name_and_midi_channel_map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public StandardMidiFile(string file_path) {
            try {
                _sequence = new();
                _sequence.Format = 1;
                _sequence.Load(file_path);
                Map<int, (string name, int channel)> name_and_midi_channel_map;
                name_and_midi_channel_map = new();
                Enumerable.Range(0, _sequence.Count).ToList().ForEach(x => {
                    name_and_midi_channel_map.Add(x, getTrackNameAndMidiChannel(x));
                });
                _name_and_midi_channel_map = new();
                var index = 0;
                name_and_midi_channel_map.ToList().ForEach(x => {
                    if (!x.Value.name.Equals("System Setup") && !(x.Value.name.Equals(string.Empty) && x.Value.channel == -1)) { // no need track
                        _name_and_midi_channel_map.Add(index++, x.Value);
                    }
                });
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public int TrackCount {
            get => _name_and_midi_channel_map.Where(x => x.Value.channel != -1).Count(); // exclude conductor track;
        }

        public List<int> MidiChannelList {
            get => _name_and_midi_channel_map.Where(x => x.Value.channel != -1).Select(x => x.Value.channel).ToList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public string GetTrackName(int track_index) {
            return _name_and_midi_channel_map[track_index].name;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        string getTrackName(int track_index) {
            var track_name = "undefined";
            var track = _sequence[track_index];
            for (var index = 0; index < track.Count; index++) {
                var evt = track.GetMidiEvent(index);
                var msg = evt.MidiMessage;
                if (msg.MessageType == MessageType.Meta) {
                    var meta_msg = (MetaMessage) msg;
                    if (meta_msg.MetaType == MetaType.TrackName) {
                        var data = meta_msg.GetBytes();
                        var text = Encoding.UTF8.GetString(data);
                        track_name = text;
                        break;
                    }
                }
            }
            return track_name;
        }

        int getMidiChannel(int track_index) {
            var channel = -1; // conductor track gets -1;
            var track = _sequence[track_index];
            for (var index = 0; index < track.Count; index++) {
                var evt = track.GetMidiEvent(index);
                var msg = evt.MidiMessage;
                if (msg.MessageType == MessageType.Channel) {
                    var chan_msg = (ChannelMessage) msg;
                    channel = chan_msg.MidiChannel;
                    break;
                }
            }
            return channel;
        }

        (string name, int channel) getTrackNameAndMidiChannel(int track_index) {
            return (getTrackName(track_index), getMidiChannel(track_index));
        }
    }
}

```



## MidiPlayer.SoundFont\SoundFont .cs

```csharp
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
using System.Linq;

namespace MidiPlayer.SoundFont {
    /// <summary>
    /// class for soundfont information
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class SoundFontInfo {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        NAudio.SoundFont.SoundFont _sound_font;

        Map<int, List<Voice>> _map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public SoundFontInfo(string file_path) {
            try {
                _sound_font = new NAudio.SoundFont.SoundFont(file_path);
                Map<int, List<Voice>> map = new();
                _sound_font.Presets.ToList().ForEach(x => {
                    if (!map.ContainsKey(key: x.Bank)) {
                        List<Voice> new_list = new();
                        new_list.Add(item: new Voice() { Prog = x.PatchNumber, Name = x.Name });
                        map.Add(key: x.Bank, value: new_list); // new bank and new voice
                    } else {
                        map[x.Bank].Add(item: new Voice() { Prog = x.PatchNumber, Name = x.Name }); // exists bank and new voice
                    }
                });
                _map = new();
                map.OrderBy(x => x.Key).ToList().ForEach(x => { // sort bank
                    _map.Add(key: x.Key, value: x.Value.OrderBy(_x => _x.Prog).ToList()); // sort prog
                });
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public string GetVoice(int bank, int prog) {
            var voice = _map[bank];
            var result = voice.Where(x => x.Prog == prog);
            if (result.Count() == 0) {
                return _map[0].Where(x => x.Prog == prog).First().Name; // return default bank's voice.
            } else {
                return voice.Where(x => x.Prog == prog).First().Name;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class Voice {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // internal Properties [noun, noun phrase, adjective] 

            internal int Prog {
                get; set;
            }

            internal string Name {
                get; set;
            }
        }
    }
}

```



## MidiPlayer\Conf.cs

```csharp
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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MidiPlayer {
    /// <summary>
    /// config file for for the application.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class Conf {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static Json _json = null;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // internal static Properties [noun, noun phrase, adjective] 

        internal static bool Ready {
            get => !(_json is null);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Properties [noun, noun phrase, adjective] 

        public static App Value {
            get {
                if (_json is null) {
                    return null;
                }
                return _json.App;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        /// <summary>
        /// load the app_conf.json file.
        /// </summary>
        public static void Load() {
            if (File.Exists(ConfEnv.ConfPath)) {
                using var stream = new StreamReader(ConfEnv.ConfPath);
                _json = loadJson(stream.ReadToEnd().ToMemoryStream());
                Log.Info("Conf loaded.");
                Log.Debug("Conf soundFontDir: " + _json.App.Synth.SoundFontDir);
                Log.Debug("Conf soundFontName: " + _json.App.Synth.SoundFontName);
                Log.Debug("Conf midiFileDir: " + _json.App.Synth.MidiFileDir);
                Log.Debug("Conf midiFileName: " + _json.App.Synth.MidiFileName);
            } else {
                Synth synth = new();
                synth.SoundFontDir = "undefined";
                synth.MidiFileDir = "undefined";
                App app = new();
                app.PlayList = null;
                app.Synth = synth;
                _json = new();
                _json.App = app;
            }
        }

        /// <summary>
        /// save the app_conf.json file.
        /// </summary>
        public static void Save() {
            if (!Directory.Exists(ConfEnv.ConfDir)) {
                Directory.CreateDirectory(ConfEnv.ConfDir);
            }
            using var stream = new FileStream(ConfEnv.ConfPath, FileMode.Create, FileAccess.Write);
            saveJson(stream);
            Log.Info("Conf saved.");
            Log.Debug("Conf soundFontDir: " + _json.App.Synth.SoundFontDir);
            Log.Debug("Conf soundFontName: " + _json.App.Synth.SoundFontName);
            Log.Debug("Conf midiFileDir: " + _json.App.Synth.MidiFileDir);
            Log.Debug("Conf midiFileName: " + _json.App.Synth.MidiFileName);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

        static Json loadJson(Stream target) {
            var serializer = new DataContractJsonSerializer(typeof(Json));
            return (Json) serializer.ReadObject(target);
        }

        static void saveJson(Stream target) {
            using var writer = JsonReaderWriterFactory.CreateJsonWriter(target, Encoding.UTF8, true, true);
            var serializer = new DataContractJsonSerializer(typeof(Json));
            serializer.WriteObject(writer, _json);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class ConfEnv {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // static Fields [nouns, noun phrases]

            const string WIN64_PATH = "conf\\app_conf.json";//"conf\\app_conf.json";

            const string ANDROID_PATH = "storage/emulated/0/Android/data/com.studio.meowtoon.midiplayer/files/app_conf.json";

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective] 

            public static string ConfPath {
                get {
                    var os = Environment.OSVersion;
                    if (os.Platform == PlatformID.Win32NT) {
                        return WIN64_PATH;
                    } else if (os.Platform == PlatformID.Unix) {
                        return ANDROID_PATH;
                    }
                    return string.Empty;
                }
            }

            public static string ConfDir {
                get {
                    var os = Environment.OSVersion;
                    if (os.Platform == PlatformID.Win32NT) {
                        return WIN64_PATH.Replace("\\app_conf.json", string.Empty);
                    } else if (os.Platform == PlatformID.Unix) {
                        return ANDROID_PATH.Replace("/app_conf.json", string.Empty);
                    }
                    return string.Empty;
                }
            }
        }

        [DataContract]
        class Json {
            [DataMember(Name = "app")]
            public App App {
                get; set;
            }
        }

        [DataContract]
        public class App {
            [DataMember(Name = "synth")]
            public Synth Synth {
                get; set;
            }
            [DataMember(Name = "play_list")]
            public string[] PlayList {
                get; set;
            }
        }

        [DataContract]
        public class Synth {
            [DataMember(Name = "sound_font_dir")]
            public string SoundFontDir {
                get; set;
            }
            [DataMember(Name = "midi_file_dir")]
            public string MidiFileDir {
                get; set;
            }
            [DataMember(Name = "sound_font_name")]
            public string SoundFontName {
                get; set;
            }
            [DataMember(Name = "midi_file_name")]
            public string MidiFileName {
                get; set;
            }
        }
    }
}

```



## MidiPlayer\DIAppSample.cs

```csharp
// DIAppSample.cs removed per user request (2026-04-18).
// Original sample/host wiring deleted to keep repository focused on Xamarin.Android adapters and tests.
// If you need this snippet later, restore from session history or ask the assistant to recreate it.

```



## MidiPlayer\DIRegistrationSample.cs

```csharp
// DIRegistrationSample.cs removed per user request (2026-04-18).
// Keep IEventQueue/IMixer adapters and Test implementations in place; global registration sample removed.
// Restore from session history if needed.

```



## MidiPlayer\Enums.cs

```csharp
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

namespace MidiPlayer {
    /// <summary>
    /// common enums for app
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>

    public enum Request {
        SoundFont = 128,
        MidiFile = 256,
        AddPlayList = 384,
    }

    public enum ControlChange {
        Volume = 7,
        Pan = 10,
    }

    public enum MidiChannel {
        ch1 = 0,
        ch2 = 1,
        ch3 = 2,
        ch4 = 3,
        ch5 = 4,
        ch6 = 5,
        ch7 = 6,
        ch8 = 7,
        ch9 = 8,
        ch10 = 9,
        ch11 = 10,
        ch12 = 11,
        ch13 = 12,
        ch14 = 13,
        ch15 = 14,
        ch16 = 15,
        // for Extension Method
        Enum = -128,
    }
}

```



## MidiPlayer\Env.cs

```csharp
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

using System.IO;

namespace MidiPlayer {
    /// <summary>
    /// environment value for the application.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class Env {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Properties [noun, noun phrase, adjective] 

        public static string SoundFontDir {
            get => Conf.Value.Synth.SoundFontDir;
            set => Conf.Value.Synth.SoundFontDir = value;
        }

        public static string MidiFileDir {
            get => Conf.Value.Synth.MidiFileDir;
            set => Conf.Value.Synth.MidiFileDir = value;
        }

        public static string SoundFontDirForIntent {
            get {
                if (!ExistsSoundFont) {
                    return "Music";
                }
                return SoundFontDir.Replace("/storage/emulated/0/", string.Empty).Replace("/", "%2F");
            }
        }

        public static string MidiFileDirForIntent {
            get {
                if (!ExistsMidiFile) {
                    return "Music";
                }
                return MidiFileDir.Replace("/storage/emulated/0/", string.Empty).Replace("/", "%2F");
            }
        }

        public static string SoundFontName {
            get => Conf.Value.Synth.SoundFontName;
            set => Conf.Value.Synth.SoundFontName = value;
        }

        public static string MidiFileName {
            get => Conf.Value.Synth.MidiFileName;
            set => Conf.Value.Synth.MidiFileName = value;
        }

        public static string SoundFontPath {
            get => $"{SoundFontDir}/{SoundFontName}";
            set {
                SoundFontDir = value.ToDirectoryName();
                SoundFontName = value.ToFileName();
            }
        }

        public static string MidiFilePath {
            get => $"{MidiFileDir}/{MidiFileName}";
            set {
                MidiFileDir = value.ToDirectoryName();
                MidiFileName = value.ToFileName();
            }
        }

        public static bool ExistsSoundFont {
            get => File.Exists(SoundFontPath);
        }

        public static bool ExistsMidiFile {
            get => File.Exists(MidiFilePath);
        }
    }
}

```



## MidiPlayer\EventQueue.cs

```csharp
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

using System.Collections.Generic;
using System.Linq;

namespace MidiPlayer {
    /// <summary>
    /// event queue class to send synth
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class EventQueue {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static Map<int, Queue<Data>> _queue_map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static EventQueue() {
            _queue_map = new();
            Enumerable.Range(start: MIDI_TRACK_BASE, count: MIDI_TRACK_COUNT).ToList().ForEach(
                track_index => _queue_map.Add(key: track_index, value: new())
            );
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Enqueue(int track_index, Data value) {
            _queue_map[track_index].Enqueue(item: value);
        }

        public static Data Dequeue(int track_index) {
            return _queue_map[track_index].Count == 0 ? null : _queue_map[track_index].Dequeue();
        }
    }

    /// <summary>
    /// data class to send synth
    /// </summary>
    public class Data {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        int _channel;

        int _program;

        int _pan;

        int _volume;

        bool _mute;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public int Channel {
            get => _channel;
            set => _channel = value;
        }

        public int Program {
            get => _program;
            set => _program = value;
        }

        public int Pan {
            get => _pan;
            set => _pan = value;
        }

        public int Volume {
            get => _volume;
            set => _volume = value;
        }

        public bool Mute {
            get => _mute;
            set => _mute = value;
        }
    }

    /// <summary>
    /// rename Dictionary to Map
    /// </summary>
    public class Map<K, V> : Dictionary<K, V> {
    }
}

```



## MidiPlayer\Extensions.cs

```csharp
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
using System.IO;
using System.Text;

namespace MidiPlayer {
    /// <summary>
    /// common extension method
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class Extensions {
#nullable enable

        /// <summary>
        /// to memory stream
        /// </summary>
        public static MemoryStream ToMemoryStream(this string source) {
            return new MemoryStream(buffer: Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// to directory name
        /// </summary>
        public static string ToDirectoryName(this string source) {
            return Path.GetDirectoryName(path: source);
        }

        /// <summary>
        /// to file name
        /// </summary>
        public static string ToFileName(this string source) {
            return Path.GetFileName(path: source);
        }

        /// <summary>
        /// bytes to megabytes.
        /// </summary>
        public static long ToMegabytes(this long source) {
            return source / (1024 * 1024);
        }

        /// <summary>
        /// returns true if the string is not null or an empty string "" or "undefined".
        /// </summary>
        public static bool HasValue(this string source) {
            return !(source is null || source.Equals(string.Empty) || source.Equals("undefined"));
        }

        /// <summary>
        /// returns true if IntPtr is IntPtr.Zero.
        /// </summary>
        public static bool IsZero(this IntPtr source) {
            return source == IntPtr.Zero;
        }
    }
}

```



## MidiPlayer\IEventQueue.cs

```csharp
#nullable enable
using System;

namespace MidiPlayer
{
    /// <summary>
    /// Abstraction over the existing static EventQueue to allow DI and test replacements.
    /// </summary>
    public interface IEventQueue
    {
        /// <summary>
        /// Enqueue a Data command for the given track index.
        /// Mirrors EventQueue.Enqueue.
        /// </summary>
        void Enqueue(int trackIndex, Data value);

        /// <summary>
        /// Dequeue a Data command for the given track index. Returns null when empty.
        /// Mirrors EventQueue.Dequeue.
        /// </summary>
        Data? Dequeue(int trackIndex);
    }
}

```



## MidiPlayer\IMixer.cs

```csharp
#nullable enable
using System.ComponentModel;

namespace MidiPlayer
{
    /// <summary>
    /// Adaptor interface for the static Mixer so callers can depend on an instance abstraction.
    /// Keeps the existing Mixer.Fader type to minimize changes.
    /// </summary>
    public interface IMixer
    {
        event PropertyChangedEventHandler? Selected;
        event PropertyChangedEventHandler? Updated;

        int Current { get; set; }
        int CurrentAsOneBased { get; }

        Mixer.Fader GetCurrent();
        Mixer.Fader GetPrevious();
        Mixer.Fader GetBy(int index);
    }
}

```



## MidiPlayer\Log.cs

```csharp
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

using NLog;

namespace MidiPlayer {
    /// <summary>
    /// Facade class for log
    /// NOTE: using NLog
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class Log {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [nouns, noun phrases]

        static Logger _logger = LogManager.GetCurrentClassLogger();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        public static void Fatal(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Fatal, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Error(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Error, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Warn(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Warn, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Info(string target) {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
        }

        public static void Debug(string target) {
#if DEBUG
            var eventInfo = new LogEventInfo(LogLevel.Debug, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
#endif
        }

        public static void Trace(string target) {
#if DEBUG
            var eventInfo = new LogEventInfo(LogLevel.Trace, _logger.Name, target);
            _logger.Log(typeof(Log), eventInfo);
#endif
        }
    }
}

```



## MidiPlayer\Mixer.cs

```csharp
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

using System.ComponentModel;
using System.Linq;

namespace MidiPlayer {
    /// <summary>
    /// Mixer object.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class Mixer {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Const [nouns]

        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        const int TO_ONE_BASED = 1;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        /// <summary>
        /// a Dictionary object that holds faders.
        /// </summary>
        static Map<int, Fader> _mixer;

        /// <summary>
        /// the current index value of the selected fader.
        /// </summary>
        /// <remarks>
        /// base index value is 0.
        /// </remarks>
        static int _current;

        /// <summary>
        /// the index value of the previously selected fader.
        /// </summary>
        static int _previous;

        /// <summary>
        /// func object to be called when a fader is selected.
        /// </summary>
        static PropertyChangedEventHandler? _on_selected;

        /// <summary>
        /// func object to be called when a fader is updated.
        /// </summary>
        static PropertyChangedEventHandler? _on_updated;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        /// <summary>
        /// static constructor.
        /// </summary>
        static Mixer() {
            _mixer = new();
            _current = 0;
            Enumerable.Range(start: MIDI_TRACK_BASE, count: MIDI_TRACK_COUNT).ToList().ForEach(x => {
                Fader fader = new(x);
                fader.Updated += onUpdate;
                _mixer.Add(x, fader);
            });
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Events [verb, verb phrase] 

        /// <summary>
        /// selected event handler.
        /// </summary>
        /// <note>
        /// called when Mixer's channel is clicked.<br/>
        /// </note>
        public static event PropertyChangedEventHandler? Selected {
            add => _on_selected += value;
            remove => _on_selected -= value;
        }

        /// <summary>
        /// updated event handler.
        /// </summary>
        /// <note>
        /// called when Fader's properties change.<br/>
        /// </note>
        public static event PropertyChangedEventHandler? Updated {
            add => _on_updated += value;
            remove => _on_updated -= value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Properties [noun, noun phrase, adjective]

        /// <summary>
        /// get selected fader number.
        /// </summary>
        /// <note>
        /// base index value is 0.
        /// </note>
        public static int Current {
            get => _current;
            set {
                _previous = _current;
                _current = value;
                Log.Info($"current: {_current}");
                _on_selected(null, new(nameof(Current)));
            }
        }

        /// <summary>
        /// get selected fader number as one-based value.
        /// </summary>
        public static int CurrentAsOneBased {
            get => Current + TO_ONE_BASED;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb, verb phrases]

        /// <summary>
        /// get the current fader.
        /// </summary>
        public static Fader GetCurrent() {
            return _mixer[Current];
        }

        /// <summary>
        /// get the previous fader.
        /// </summary>
        public static Fader GetPrevious() {
            return _mixer[_previous];
        }

        /// <summary>
        /// get a fader by 0 based index value.
        /// </summary>
        public static Fader GetBy(int index) {
            return _mixer[index];
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private static Methods [verb, verb phrases]

        /// <summary>
        /// called when a fader value is updated.
        /// </summary>
        static void onUpdate(object sender, PropertyChangedEventArgs e) {
            _on_updated(sender, e);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        /// <summary>
        /// Fader class.
        /// </summary>
        public class Fader {
#nullable enable

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields [nouns, noun phrases]

            /// <summary>
            /// a track index value of a fader.
            /// </summary>
            /// <note>
            /// base index value is 0, maxim value is MIDI_TRACK_COUNT.
            /// </note>
            int _index = -1;

            /// <summary>
            /// a value of whether the fader is on or off.
            /// </summary>
            bool _sounds = true; // mute parameter.

            /// <summary>
            /// a midi channel number of a fader.
            /// </summary>
            string _name = "undefined";

            /// <summary>
            /// a midi channel number of a fader.
            /// </summary>
            /// <note>
            /// base index value is 0, maxim value is MIDI_TRACK_COUNT.
            /// </note>
            int _channel = -1;

            /// <summary>
            /// a midi bank number of a fader.
            /// </summary>
            /// <note>
            /// minimum value is 0, maxim value is 127.
            /// </note>
            int _bank = -1;

            /// <summary>
            /// a midi program number of a fader.
            /// </summary>
            /// <note>
            /// minimum value is 0, maxim value is 127.
            /// </note>
            int _program = 0;

            /// <summary>
            /// a midi volume value of a fader.
            /// </summary>
            /// <note>
            /// minimum value is 0, maxim value is 127.
            /// </note>
            int _volume = 104;

            /// <summary>
            /// a midi pan value of a fader.
            /// </summary>
            /// <note>
            /// full left value is 0, center value is 64, full right value is 127.
            /// </note>
            int _pan = 64; // center

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            /// <summary>
            /// internal constructor.
            /// </summary>
            internal Fader(int index) {
                _index = index;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Events [verb, verb phrase] 

            /// <summary>
            /// updated event handler.
            /// </summary>
            internal event PropertyChangedEventHandler? Updated;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, noun phrase, adjective]

            /// <summary>
            /// a track index value of a fader.
            /// </summary>
            public int Index {
                get => _index;
                set {
                    if (value != _index) {
                        _index = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Index)));
                    }
                }
            }

            /// <summary>
            /// a track index value of a fader.
            /// </summary>
            public int IndexAsOneBased {
                get => Index + TO_ONE_BASED;
                set => Index = value - TO_ONE_BASED;
            }

            /// <summary>
            /// a value of whether the fader is on or off.
            /// </summary>
            public bool Sounds {
                get => _sounds;
                set {
                    if (value != _sounds) {
                        _sounds = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Sounds)));
                    }
                }
            }

            /// <summary>
            /// a midi track name of a fader.
            /// </summary>
            public string Name {
                get => _name;
                set {
                    if (value != _name) {
                        _name = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Name)));
                    }
                }
            }

            /// <summary>
            /// a midi channel number of a fader.
            /// </summary>
            public int Channel {
                get => _channel;
                set {
                    if (value != _channel) {
                        _channel = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Channel)));
                    }
                }
            }

            /// <summary>
            /// a midi channel number of a fader.
            /// </summary>
            public int ChannelAsOneBased {
                get => Channel + TO_ONE_BASED;
                set => Channel = value - TO_ONE_BASED;
            }

            /// <summary>
            /// a midi bank number of a fader.
            /// </summary>
            public int Bank {
                get {
                    if (_channel == 9 && _bank != 128) {
                        return 128; // Drum
                    }
                    return _bank;
                }
                set {
                    if (value != _bank) {
                        _bank = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Bank)));
                    }
                }
            }

            /// <summary>
            /// a midi bank number of a fader.
            /// </summary>
            public int BankAsOneBased {
                get => Bank + TO_ONE_BASED;
                set => Bank = value - TO_ONE_BASED;
            }

            /// <summary>
            /// a midi program number of a fader.
            /// </summary>
            public int Program {
                get => _program;
                set {
                    if (value != _program) {
                        _program = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Program)));
                    }
                }
            }

            /// <summary>
            /// a midi program number of a fader.
            /// </summary>
            public int ProgramAsOneBased {
                get => Program + TO_ONE_BASED;
                set => Program = value - TO_ONE_BASED;
            }

            /// <summary>
            /// a midi volume value of a fader.
            /// </summary>
            public int Volume {
                get => _volume;
                set {
                    if (value != _volume) {
                        _volume = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Volume)));
                    }
                }
            }

            /// <summary>
            /// a midi pan value of a fader.
            /// </summary>
            public int Pan {
                get => _pan;
                set {
                    if (value != _pan) {
                        _pan = value;
                        Updated?.Invoke(sender: this, e: new(nameof(Pan)));
                    }
                }
            }
        }
    }
}

```



## MidiPlayer\PlayList.cs

```csharp
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

using System.Collections.Generic;

namespace MidiPlayer {
    /// <summary>
    /// playlist for synth
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class PlayList {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        List<string> _target_list = new();

        int _index;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public PlayList() {
            _index = 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public bool Ready {
            get => _target_list.Count == 0 ? false : true;
        }

        public string[] List {
            get => _target_list.ToArray();
        }

        public string Current {
            get => _target_list[_index];
        }

        public string Next {
            get {
                if (_index == _target_list.Count) {
                    _index = 0;
                }
                return _target_list[_index++];
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public void Add(string target) {
            _target_list.Add(target);
        }

        public void Clear() {
            _target_list.Clear();
        }
    }
}

```



## MidiPlayer\StaticEventQueue.cs

```csharp
#nullable enable
using System;

namespace MidiPlayer
{
    /// <summary>
    /// Thin adapter delegating to the existing static EventQueue implementation.
    /// Use this for production; tests can inject TestEventQueue instead.
    /// </summary>
    public sealed class StaticEventQueue : IEventQueue
    {
        public static readonly StaticEventQueue Instance = new StaticEventQueue();

        private StaticEventQueue() { }

        public void Enqueue(int trackIndex, Data value)
            => EventQueue.Enqueue(trackIndex, value);

        public Data? Dequeue(int trackIndex)
            => EventQueue.Dequeue(trackIndex);
    }
}

```



## MidiPlayer\StaticMixerAdapter.cs

```csharp
#nullable enable
using System.ComponentModel;

namespace MidiPlayer
{
    /// <summary>
    /// Thin adapter delegating to the existing static Mixer implementation.
    /// Provides an IMixer instance that forwards calls to Mixer static members.
    /// </summary>
    public sealed class StaticMixerAdapter : IMixer
    {
        public static readonly StaticMixerAdapter Instance = new StaticMixerAdapter();

        private StaticMixerAdapter() { }

        public event PropertyChangedEventHandler? Selected
        {
            add => Mixer.Selected += value;
            remove => Mixer.Selected -= value;
        }

        public event PropertyChangedEventHandler? Updated
        {
            add => Mixer.Updated += value;
            remove => Mixer.Updated -= value;
        }

        public int Current
        {
            get => Mixer.Current;
            set => Mixer.Current = value;
        }

        public int CurrentAsOneBased => Mixer.CurrentAsOneBased;

        public Mixer.Fader GetCurrent() => Mixer.GetCurrent();
        public Mixer.Fader GetPrevious() => Mixer.GetPrevious();
        public Mixer.Fader GetBy(int index) => Mixer.GetBy(index);
    }
}

```



## MidiPlayer\TestEventQueue.cs

```csharp
#nullable enable
using System.Collections.Generic;

namespace MidiPlayer
{
    /// <summary>
    /// Simple in-memory event queue for tests. Not optimized for real-time; intended for unit tests
    /// where allocations are acceptable.
    /// </summary>
    public sealed class TestEventQueue : IEventQueue
    {
        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        readonly System.Collections.Concurrent.ConcurrentQueue<Data>[] _queues;

        public TestEventQueue(int trackCount = MIDI_TRACK_COUNT)
        {
            if (trackCount <= 0) trackCount = MIDI_TRACK_COUNT;
            _queues = new System.Collections.Concurrent.ConcurrentQueue<Data>[trackCount];
            for (int i = 0; i < _queues.Length; i++)
                _queues[i] = new System.Collections.Concurrent.ConcurrentQueue<Data>();
        }

        public void Enqueue(int trackIndex, Data value)
        {
            int idx = trackIndex - MIDI_TRACK_BASE;
            if ((uint)idx >= (uint)_queues.Length) throw new System.ArgumentOutOfRangeException(nameof(trackIndex));
            _queues[idx].Enqueue(value);
        }

        public Data? Dequeue(int trackIndex)
        {
            int idx = trackIndex - MIDI_TRACK_BASE;
            if ((uint)idx >= (uint)_queues.Length) throw new System.ArgumentOutOfRangeException(nameof(trackIndex));
            return _queues[idx].TryDequeue(out var d) ? d : null;
        }

        /// <summary>
        /// Clear all queues (useful for test setup/teardown).
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _queues.Length; i++)
            {
                while (_queues[i].TryDequeue(out _)) { }
            }
        }
    }
}

```



## MidiPlayer\TestMixer.cs

```csharp
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MidiPlayer
{
    /// <summary>
    /// Instance-based mixer used for tests. Creates per-instance faders so parallel tests
    /// don't interfere with each other.
    /// </summary>
    public sealed class TestMixer : IMixer
    {
        const int MIDI_TRACK_BASE = 0;
        const int MIDI_TRACK_COUNT = 16;

        readonly Dictionary<int, Mixer.Fader> _mixer;
        int _current;
        int _previous;

        PropertyChangedEventHandler? _on_selected;
        PropertyChangedEventHandler? _on_updated;

        public event PropertyChangedEventHandler? Selected
        {
            add => _on_selected += value;
            remove => _on_selected -= value;
        }

        public event PropertyChangedEventHandler? Updated
        {
            add => _on_updated += value;
            remove => _on_updated -= value;
        }

        public TestMixer(int trackCount = MIDI_TRACK_COUNT)
        {
            if (trackCount <= 0) trackCount = MIDI_TRACK_COUNT;
            _mixer = new Dictionary<int, Mixer.Fader>(trackCount);
            for (int i = 0; i < trackCount; i++)
            {
                var fader = new Mixer.Fader(i);
                // forward fader updates to listeners
                fader.Updated += (s, e) => _on_updated?.Invoke(s, e);
                _mixer.Add(i, fader);
            }
            _current = 0;
            _previous = 0;
        }

        public int Current
        {
            get => _current;
            set
            {
                _previous = _current;
                _current = value;
                _on_selected?.Invoke(null, new PropertyChangedEventArgs(nameof(Current)));
            }
        }

        public int CurrentAsOneBased => Current + 1;

        public Mixer.Fader GetCurrent() => _mixer[Current];

        public Mixer.Fader GetPrevious() => _mixer[_previous];

        public Mixer.Fader GetBy(int index) => _mixer[index];

        /// <summary>
        /// Clear and re-create internal faders (useful for test setup/teardown).
        /// </summary>
        public void Reset()
        {
            _mixer.Clear();
            for (int i = 0; i < MIDI_TRACK_COUNT; i++)
            {
                var fader = new Mixer.Fader(i);
                fader.Updated += (s, e) => _on_updated?.Invoke(s, e);
                _mixer.Add(i, fader);
            }
            _current = 0;
            _previous = 0;
        }
    }
}

```



## MidiPlayerTest\PlayListTests.cs

```csharp

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

using MidiPlayer;

namespace MidiPlayerTest {
    [TestClass()]
    public class PlayListTests {
#nullable enable
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

```



## MidiPlayerTest\PrivateObject.cs

```csharp

using System;
using System.Reflection;

namespace MidiPlayerTest {
    /// <summary>
    /// PrivateObject for MSTest
    /// </summary>
    public class PrivateObject {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        private readonly object _obj;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public PrivateObject(object obj) {
            _obj = obj;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public object Invoke(string methodName, params object[] args) {
            var type = _obj.GetType();
            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance;
            try {
                return type.InvokeMember(methodName, bindingFlags, null, _obj, args);
            } catch (Exception ex) {
                throw ex.InnerException;
            }
        }

        public object Invoke(string methodName) {
            var type = _obj.GetType();
            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance;
            try {
                return type.InvokeMember(methodName, bindingFlags, null, _obj, null);
            } catch (Exception ex) {
                throw ex.InnerException;
            }
        }
    }
}

```



## MidiPlayerTest\SoundFontTests.cs

```csharp

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

using MidiPlayer.SoundFont;

namespace MidiPlayerTest.SoundFont {
    [TestClass()]
    public class SoundFontInfoTests {
#nullable enable
        [TestMethod()]
        public void GetVoiceTest() {
            var target = new SoundFontInfo("../data/OmegaGMGS2.sf2");
            var result1 = target.GetVoice(0, 0);
            AreEqual("Grand Piano", result1); // bank:0, prog:0
            var result2 = target.GetVoice(8, 38);
            AreEqual("Synth Bass 3", result2); // bank:8, prog:38
            var result3 = target.GetVoice(128, 8);
            AreEqual("Room Kit", result3); // bank:128, prog:8
        }
    }
}

```



## MidiPlayerTest\StandardMidiFileTests.cs

```csharp
using MidiPlayer.Midi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Collections.Generic;

namespace MidiPlayerTest.Midi {
    [TestClass()]
    public class StandardMidiFileTests {
#nullable enable
        [TestMethod()]
        public void getTrackNameAndMidiChannelTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var privateObj = new PrivateObject(target);
            var result0 = privateObj.Invoke("getTrackNameAndMidiChannel", 0);
            AreEqual(("Cmon", -1), result0);
            var result1 = privateObj.Invoke("getTrackNameAndMidiChannel", 1);
            AreEqual(("Vocal Main", 13), result1);
            var result2 = privateObj.Invoke("getTrackNameAndMidiChannel", 2);
            AreEqual(("Vocal Cho", 0), result2);
            var result3 = privateObj.Invoke("getTrackNameAndMidiChannel", 3);
            AreEqual(("Synth Sqe", 15), result3);
            var result4 = privateObj.Invoke("getTrackNameAndMidiChannel", 4);
            AreEqual(("Synth Pad", 14), result4);
            var result5 = privateObj.Invoke("getTrackNameAndMidiChannel", 5);
            AreEqual(("Guiter Riff", 12), result5);
            var result6 = privateObj.Invoke("getTrackNameAndMidiChannel", 6);
            AreEqual(("Bass", 11), result6);
            var result7 = privateObj.Invoke("getTrackNameAndMidiChannel", 7);
            AreEqual(("Drum OverTop", 9), result7);
            var result8 = privateObj.Invoke("getTrackNameAndMidiChannel", 8);
            AreEqual(("Durm SN & BD", 9), result8);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void getTrackNameAndMidiChannelTest2() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var privateObj = new PrivateObject(target);
            privateObj.Invoke("getTrackNameAndMidiChannel", 9);
        }

        [TestMethod()]
        public void TrackCountTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var result = target.TrackCount;
            AreEqual(8, result);
        }

        [TestMethod()]
        public void TrackCountTest2() {
            var target = new StandardMidiFile("../data/ABC_v1.mid");
            var result = target.TrackCount;
            AreEqual(14, result);
        }

        [TestMethod()]
        public void TrackCountTest3() {
            var target = new StandardMidiFile("../data/DoYouSay_v4.mid");
            var result = target.TrackCount;
            AreEqual(8, result);
        }

        [TestMethod()]
        public void MidiChannelListTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var result = target.MidiChannelList;
            CollectionAssert.AreEqual(new List<int>() { 13, 0, 15, 14, 12, 11, 9, 9 }, result);
        }

        [TestMethod()]
        public void GetTrackNameTest1() {
            var target = new StandardMidiFile("../data/Cmon_v1.mid");
            var result1 = target.GetTrackName(1);
            AreEqual("Vocal Main", result1);
            var result2 = target.GetTrackName(2);
            AreEqual("Vocal Cho", result2);
            var result3 = target.GetTrackName(3);
            AreEqual("Synth Sqe", result3);
            var result4 = target.GetTrackName(4);
            AreEqual("Synth Pad", result4);
            var result5 = target.GetTrackName(5);
            AreEqual("Guiter Riff", result5);
            var result6 = target.GetTrackName(6);
            AreEqual("Bass", result6);
            var result7 = target.GetTrackName(7);
            AreEqual("Drum OverTop", result7);
            var result8 = target.GetTrackName(8);
            AreEqual("Durm SN & BD", result8);
        }

        [TestMethod()]
        public void GetTrackNameTest2() {
            var target = new StandardMidiFile("../data/Tornado_v2.mid");
            var result0 = target.GetTrackName(0);
            AreEqual("Tornado", result0);
            var result1 = target.GetTrackName(1);
            AreEqual("Bass", result1);
            var result2 = target.GetTrackName(2);
            AreEqual("Seque", result2);
            var result3 = target.GetTrackName(3);
            AreEqual("Pad", result3);
            var result4 = target.GetTrackName(4);
            AreEqual("Melody", result4);
            var result5 = target.GetTrackName(5);
            AreEqual("Drum", result5);
        }

        [TestMethod()]
        public void GetTrackNameTest3() {
            var target = new StandardMidiFile("../data/ABC_v1.mid");
            var result0 = target.GetTrackName(0);
            AreEqual("ABC", result0);
            var result1 = target.GetTrackName(1);
            AreEqual("Brass1", result1);
            var result2 = target.GetTrackName(2);
            AreEqual("Brass2", result2);
            var result3 = target.GetTrackName(3);
            AreEqual("Melody Main", result3);
            var result4 = target.GetTrackName(4);
            AreEqual("Synth Reff", result4);
            var result5 = target.GetTrackName(5);
            AreEqual("Synth Pad2", result5);
            var result6 = target.GetTrackName(6);
            AreEqual("Synth Pad1", result6);
            var result7 = target.GetTrackName(7);
            AreEqual("DX Reff", result7);
            var result8 = target.GetTrackName(8);
            AreEqual("Drum Main", result8);
            var result9 = target.GetTrackName(9);
            AreEqual("Percussion1", result9);
            var result10 = target.GetTrackName(10);
            AreEqual("Percussion2", result10);
            var result11 = target.GetTrackName(11);
            AreEqual("Bass", result11);
            var result12 = target.GetTrackName(12);
            AreEqual("Bass over dub", result12);
            var result13 = target.GetTrackName(13);
            AreEqual("DX Sequence", result13);
            var result14 = target.GetTrackName(14);
            AreEqual("Orchestral Hit", result14);
        }

        [TestMethod()]
        public void GetTrackNameTest4() {
            var target = new StandardMidiFile("../data/DoYouSay_v4.mid");
            var result0 = target.GetTrackName(0);
            AreEqual("DoYouSay", result0);
            var result1 = target.GetTrackName(1);
            AreEqual("Vocal Main", result1);
            var result2 = target.GetTrackName(2);
            AreEqual("Vocal Cho", result2);
            var result3 = target.GetTrackName(3);
            AreEqual("Synth Pad", result3);
            var result4 = target.GetTrackName(4);
            AreEqual("Guiter Clean", result4);
            var result5 = target.GetTrackName(5);
            AreEqual("Guiter Riff", result5);
            var result6 = target.GetTrackName(6);
            AreEqual("Bass", result6);
            var result7 = target.GetTrackName(7);
            AreEqual("Drum OverTop", result7);
            var result8 = target.GetTrackName(8);
            AreEqual("Drum SN & BD", result8);
        }
    }
}

```



## MidiPlayerTest\SynthTests.cs

```csharp

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using static System.Threading.Thread;

using MidiPlayer;

namespace MidiPlayerTest {
    [TestClass()]
    public class SynthTests {
#nullable enable
        [TestMethod()]
        public void GetBankTest1() {
            var result = Synth.GetBank(0);
            AreEqual(0, result);
        }

        [TestMethod()]
        public void GetBankTest2() {
            var result = Synth.GetBank(7);
            AreEqual(128, result);
        }

        [TestMethod()]
        public void GetProgramTest1() {
            var result = Synth.GetProgram(1);
            AreEqual(58, result);
        }

        [TestMethod()]
        public void GetProgramTest2() {
            var result = Synth.GetProgram(8);
            AreEqual(16, result);
        }

        [TestMethod()]
        public void GetVoiceTest1() {
            var result = Synth.GetVoice(1);
            AreEqual("Tuba", result);
        }

        [TestMethod()]
        public void GetVoiceTest2() {
            var result = Synth.GetVoice(8);
            AreEqual("Power Kit", result);
        }

        [TestMethod()]
        public void GetVoiceTest3() {
            var result = Synth.GetVoice(4);
            AreEqual("Warm Pad", result);
        }

        [TestMethod()]
        public void GetTrackNameTest1() {
            var result = Synth.GetTrackName(0);
            AreEqual("Cmon", result);
        }

        [TestMethod()]
        public void GetTrackNameTest2() {
            var result = Synth.GetTrackName(6);
            AreEqual("Bass", result);
        }

        [TestMethod()]
        public void GetTrackNameTest3() {
            var result = Synth.GetTrackName(7);
            AreEqual("Drum OverTop", result);
        }

        [TestInitialize]
        public void TestInitialize() {
            Synth.SoundFontPath = "../data/OmegaGMGS2.sf2";
            Synth.MidiFilePath = "../data/Cmon_v1.mid";
            Synth.Playbacking += (IntPtr data, IntPtr evt) => {
                return Synth.HandleEvent(data, evt);
            };
            Synth.Started += () => {
            };
            Synth.Ended += () => {
            };
            Synth.Updated += (object sender, PropertyChangedEventArgs e) => {
            };
            playSong();
            Sleep(3000);
        }

        [TestCleanup]
        public void TestCelean() {
            stopSong();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

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

```



