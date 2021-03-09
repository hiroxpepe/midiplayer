
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

        Map<int, (string name, int channel)> nameAndMidiChannelMap;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public StandardMidiFile(string target) {
            sequence = new Sequence();
            sequence.Format = 1;
            sequence.Load(target);
            Map<int, (string name, int channel)> _nameAndMidiChannelMap;
            _nameAndMidiChannelMap = new Map<int, (string name, int channel)>();
            Enumerable.Range(0, trackCountIncludeConductorTrack).ToList().ForEach(x => {
                _nameAndMidiChannelMap.Add(x, getTrackNameAndMidiChannel(x));
            });
            nameAndMidiChannelMap = new Map<int, (string name, int channel)>();
            var _idx = 0;
            _nameAndMidiChannelMap.ToList().ForEach(x => {
                if (!x.Value.name.Equals("System Setup") && !(x.Value.name.Equals("") && x.Value.channel == -1)) { // no need track
                    nameAndMidiChannelMap.Add(_idx++, x.Value);
                }
            });
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties [noun, adjective] 

        public int TrackCount {
            get => sequence.Count - 1; // exclude conductor track;
        }

        public List<int> MidiChannelList {
            get => nameAndMidiChannelMap.Where(x => x.Value.channel != -1).Select(x => x.Value.channel).ToList();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public string GetTrackName(int track) {
            return nameAndMidiChannelMap[track].name;
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

        (string name, int channel) getTrackNameAndMidiChannel(int track) {
            return (getTrackName(track), getMidiChannel(track));
        }
    }
}
