
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

        List<int> channelList;

        Map<int, string> trackNameMap;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public StandardMidiFile(string target) {
            sequence = new Sequence();
            sequence.Format = 1;
            sequence.Load(target);
            channelList = new List<int>();
            Enumerable.Range(0, trackCountIncludeConductorTrack).ToList().ForEach(x => {
                var _channel = getMidiChannel(x);
                if (_channel != -1) { // exclude conductor track;
                    channelList.Add(_channel);
                }
            });
            trackNameMap = new Map<int, string>();
            Enumerable.Range(0, trackCountIncludeConductorTrack).ToList().ForEach(x => {
                trackNameMap.Add(x, getTrackName(x));
            });
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, adjective] 

        public int TrackCount {
            get => sequence.Count - 1; // exclude conductor track;
        }

        public List<int> MidiChannelList {
            get => channelList;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public string GetTrackName(int track) {
            return trackNameMap[track];
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Properties [noun, adjective] 

        int trackCountIncludeConductorTrack {
            get => sequence.Count;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        string getTrackName(int track) {
            var _trackName = "undefined";
            var _track = sequence[track];
            for (var _idx = 0; _idx < _track.Count; _idx++) {
                var _evt = _track.GetMidiEvent(_idx);
                var _msg = _evt.MidiMessage;
                if (_msg.MessageType == MessageType.Meta) {
                    var _metaMsg = (MetaMessage) _msg;
                    if (_metaMsg.MetaType == MetaType.TrackName) {
                        var _data = _metaMsg.GetBytes();
                        var _text = Encoding.UTF8.GetString(_data);
                        _trackName = _text;
                        break;
                    }
                }
            }
            return _trackName;
        }

        int getMidiChannel(int track) {
            var _channel = -1; // conductor track gets -1;
            var _track = sequence[track];
            for (var _idx = 0; _idx < _track.Count; _idx++) {
                var _evt = _track.GetMidiEvent(_idx);
                var _msg = _evt.MidiMessage;
                if (_msg.MessageType == MessageType.Channel) {
                    var _chanMsg = (ChannelMessage) _msg;
                    _channel = _chanMsg.MidiChannel;
                    break;
                }
            }
            return _channel;
        }

        /// <summary>
        /// NOTE: use only from unit-test.
        /// </summary>
        (string, int) getTrackNameAndMidiChannel(int track) {
            return (getTrackName(track), getMidiChannel(track));
        }
    }
}
