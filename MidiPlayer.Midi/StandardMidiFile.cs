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

        /// <summary>
        /// the Sanford MIDI Sequence loaded from the file.
        /// </summary>
        Sequence _sequence;

        /// <summary>
        /// the map from track index to (name, MIDI channel) tuple, filtered to exclude empty and system tracks.
        /// </summary>
        Map<int, (string name, int channel)> _name_and_midi_channel_map;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// loads and parses a Standard MIDI File, building the track name and channel map.
        /// </summary>
        /// <param name="file_path">the full path to the .mid file to load.</param>
        public StandardMidiFile(string file_path) {
            try {
                _sequence = new();
                _sequence.Format = 1;
                _sequence.Load(file_path);
                Map<int, (string name, int channel)> name_and_midi_channel_map;
                name_and_midi_channel_map = new();
                Enumerable.Range(0, _sequence.Count).ToList().ForEach(x => {
                    name_and_midi_channel_map.Add(x, GetTrackNameAndMidiChannel(x));
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

        /// <summary>
        /// the number of non-conductor MIDI tracks (tracks with a valid MIDI channel).
        /// </summary>
        public int TrackCount {
            get => _name_and_midi_channel_map.Where(x => x.Value.channel != -1).Count(); // exclude conductor track;
        }

        /// <summary>
        /// the list of MIDI channel numbers for all non-conductor tracks, in track order.
        /// </summary>
        public List<int> MidiChannelList {
            get => _name_and_midi_channel_map.Where(x => x.Value.channel != -1).Select(x => x.Value.channel).ToList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// gets the MIDI track name for the given filtered track index.
        /// </summary>
        /// <param name="track_index">the zero-based index into the filtered track map.</param>
        /// <returns>the track name string.</returns>
        public string GetTrackName(int track_index) {
            return _name_and_midi_channel_map[track_index].name;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        /// <summary>
        /// reads the track name meta-message from the raw sequence track at the given index.
        /// </summary>
        /// <param name="track_index">the zero-based raw sequence track index.</param>
        /// <returns>the track name string, or "undefined" if no TrackName meta-message is found.</returns>
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

        /// <summary>
        /// reads the MIDI channel number from the first channel message in the raw sequence track.
        /// </summary>
        /// <param name="track_index">the zero-based raw sequence track index.</param>
        /// <returns>the MIDI channel (0-15), or -1 if the track contains no channel messages (conductor track).</returns>
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

        /// <summary>
        /// get the track name and MIDI channel number for the raw sequence track at the given index.
        /// </summary>
        /// <remarks>
        /// internal so that MidiPlayerTest can call it directly without reflection.
        /// </remarks>
        /// <param name="track_index">the zero-based raw sequence track index.</param>
        /// <returns>a tuple of (name, channel) where channel is -1 for conductor tracks.</returns>
        internal (string name, int channel) GetTrackNameAndMidiChannel(int track_index) {
            return (getTrackName(track_index), getMidiChannel(track_index));
        }
    }
}
