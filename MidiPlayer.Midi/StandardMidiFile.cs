
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace MidiPlayer.Midi {
    /// <summary>
    /// class for standard midi file
    /// </summary>
    public class StandardMidiFile {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        Sequence _sequence;

        Map<int, (string name, int channel)> _nameAndMidiChannelMap;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public StandardMidiFile(string target) {
            try {
                _sequence = new Sequence();
                _sequence.Format = 1;
                _sequence.Load(target);
                Map<int, (string name, int channel)> nameAndMidiChannelMap;
                nameAndMidiChannelMap = new Map<int, (string name, int channel)>();
                Enumerable.Range(0, _sequence.Count).ToList().ForEach(x => {
                    nameAndMidiChannelMap.Add(x, getTrackNameAndMidiChannel(x));
                });
                this._nameAndMidiChannelMap = new Map<int, (string name, int channel)>();
                var idx = 0;
                nameAndMidiChannelMap.ToList().ForEach(x => {
                    if (!x.Value.name.Equals("System Setup") && !(x.Value.name.Equals("") && x.Value.channel == -1)) { // no need track
                        this._nameAndMidiChannelMap.Add(idx++, x.Value);
                    }
                });
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, noun phrase, adjective] 

        public int TrackCount {
            get => _nameAndMidiChannelMap.Where(x => x.Value.channel != -1).Count(); // exclude conductor track;
        }

        public List<int> MidiChannelList {
            get => _nameAndMidiChannelMap.Where(x => x.Value.channel != -1).Select(x => x.Value.channel).ToList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public string GetTrackName(int trackIndex) {
            return _nameAndMidiChannelMap[trackIndex].name;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb, verb phrases]

        string getTrackName(int trackIndex) {
            var trackName = "undefined";
            var track = _sequence[trackIndex];
            for (var idx = 0; idx < track.Count; idx++) {
                var evt = track.GetMidiEvent(idx);
                var msg = evt.MidiMessage;
                if (msg.MessageType == MessageType.Meta) {
                    var metaMsg = (MetaMessage) msg;
                    if (metaMsg.MetaType == MetaType.TrackName) {
                        var data = metaMsg.GetBytes();
                        var text = Encoding.UTF8.GetString(data);
                        trackName = text;
                        break;
                    }
                }
            }
            return trackName;
        }

        int getMidiChannel(int trackIndex) {
            var channel = -1; // conductor track gets -1;
            var track = _sequence[trackIndex];
            for (var idx = 0; idx < track.Count; idx++) {
                var evt = track.GetMidiEvent(idx);
                var msg = evt.MidiMessage;
                if (msg.MessageType == MessageType.Channel) {
                    var chanMsg = (ChannelMessage) msg;
                    channel = chanMsg.MidiChannel;
                    break;
                }
            }
            return channel;
        }

        (string name, int channel) getTrackNameAndMidiChannel(int trackIndex) {
            return (getTrackName(trackIndex), getMidiChannel(trackIndex));
        }
    }
}
