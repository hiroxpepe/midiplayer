
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

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        Sequence sequence;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public StandardMidiFile(string target) {
            sequence = new Sequence();
            sequence.Format = 1;
            sequence.Load(target);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, adjective] 

        public int TrackCount {
            get => sequence.Count - 1; // exclude conductor track;
        }

        public int TrackCountIncludeConductorTrack {
            get => sequence.Count;
        }

        public List<int> MidiChannelList {
            get {
                var _list = new List<int>();
                Enumerable.Range(0, TrackCountIncludeConductorTrack).ToList().ForEach(x => {
                    (string name, int channel) _result = GetTrackNameAndMidiChannel(x);
                    var _channel = _result.channel;
                    if (_channel != -1) { // exclude conductor track;
                        _list.Add(_result.channel);
                    }
                });
                return _list;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public (string, int) GetTrackNameAndMidiChannel(int index) {
            var _trackCount = sequence.Count;
            if (index > _trackCount) {
                throw new ArgumentOutOfRangeException("index is out of range.");
            }
            var _trackName = "undefined";
            var _channel = -1; // conductor track gets -1;
            var _track = sequence[index];
            for (var _idx = 0; _idx < _track.Count; _idx++) {
                var _evt = _track.GetMidiEvent(_idx);
                var _msg = _evt.MidiMessage;
                // search trackName from meta message.
                if (_msg.MessageType == MessageType.Meta) {
                    var _metaMsg = (MetaMessage) _msg;
                    if (_metaMsg.MetaType == MetaType.TrackName) {
                        var _data = _metaMsg.GetBytes();
                        var _text = Encoding.UTF8.GetString(_data);
                        _trackName = _text;
                    }
                }
                // search midi ch from channel message.
                if (_msg.MessageType == MessageType.Channel) {
                    var _chanMsg = (ChannelMessage) _msg;
                    _channel = _chanMsg.MidiChannel;
                }
                if (_channel != -1) {
                    break;
                }
            }
            return (_trackName, _channel);
        }

        public string GetTrackName(int index, int channel) {
            if (!MidiChannelList.Contains(channel) && channel != -1) { // conductor track gets -1;
                throw new ArgumentOutOfRangeException("index is out of range.");
            }
            var _trackName = "undefined";
            Enumerable.Range(0, TrackCountIncludeConductorTrack).ToList().ForEach(x => {
                (string name, int channel) _result = GetTrackNameAndMidiChannel(x);
                if (index == x && channel == _result.channel) {
                    _trackName = _result.name;
                }
            });
            return _trackName;
        }
    }
}
