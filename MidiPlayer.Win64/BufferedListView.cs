
using System.Windows.Forms;

namespace MidiPlayer.Win64 {
    /// <summary>
    /// double buffered ListView
    /// </summary>
    public class BufferedListView : ListView {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // protected Methods [verb]

        protected override bool DoubleBuffered {
            get {
                return true;
            }
            set {
            }
        }
    }
}
