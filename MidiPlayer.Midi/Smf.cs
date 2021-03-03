
using System.Text;
using Sanford.Multimedia.Midi;

namespace MidiPlayer.Midi {
    /// <summary>
    /// class for standard midi file
    /// </summary>
    public class Smf {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        Sequence sequence;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public Smf(string target) {
            sequence = new Sequence();
            sequence.Format = 1;
            sequence.Load(target);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public string GetTrackName(int channel) {
            var _track = sequence[channel];
            for (var _idx = 0; _idx < _track.Count; _idx++) {
                var _evt = _track.GetMidiEvent(_idx);
                var _msg = _evt.MidiMessage;
                if (_msg.MessageType == MessageType.Meta) {
                    var _meta = (MetaMessage) _msg;
                    if (_meta.MetaType == MetaType.TrackName) {
                        var _data = _meta.GetBytes();
                        var _text = Encoding.UTF8.GetString(_data);
                        return _text;
                    }
                }
            }
            return "undefined";
        }
    }
}
