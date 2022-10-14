
namespace MidiPlayer.Win64 {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this._button_load_soundfont = new System.Windows.Forms.Button();
            this._button_load_midi_file = new System.Windows.Forms.Button();
            this._button_start = new System.Windows.Forms.Button();
            this._button_stop = new System.Windows.Forms.Button();
            this._openfiledialog = new System.Windows.Forms.OpenFileDialog();
            this._listview = new MidiPlayer.Win64.BufferedListView();
            this.SuspendLayout();
            // 
            // _button_load_soundfont
            // 
            this._button_load_soundfont.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._button_load_soundfont.Location = new System.Drawing.Point(12, 12);
            this._button_load_soundfont.Name = "_button_load_soundfont";
            this._button_load_soundfont.Size = new System.Drawing.Size(84, 40);
            this._button_load_soundfont.TabIndex = 0;
            this._button_load_soundfont.Text = "SOUND";
            this._button_load_soundfont.UseVisualStyleBackColor = true;
            this._button_load_soundfont.Click += new System.EventHandler(this.buttonLoadSoundFont_Click);
            // 
            // _button_load_midi_file
            // 
            this._button_load_midi_file.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._button_load_midi_file.Location = new System.Drawing.Point(107, 12);
            this._button_load_midi_file.Name = "_button_load_midi_file";
            this._button_load_midi_file.Size = new System.Drawing.Size(84, 40);
            this._button_load_midi_file.TabIndex = 1;
            this._button_load_midi_file.Text = "SONG";
            this._button_load_midi_file.UseVisualStyleBackColor = true;
            this._button_load_midi_file.Click += new System.EventHandler(this.buttonLoadMidiFile_Click);
            // 
            // _button_start
            // 
            this._button_start.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._button_start.Location = new System.Drawing.Point(202, 12);
            this._button_start.Name = "_button_start";
            this._button_start.Size = new System.Drawing.Size(84, 40);
            this._button_start.TabIndex = 2;
            this._button_start.Text = "START";
            this._button_start.UseVisualStyleBackColor = true;
            this._button_start.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // _button_stop
            // 
            this._button_stop.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._button_stop.Location = new System.Drawing.Point(297, 12);
            this._button_stop.Name = "_button_stop";
            this._button_stop.Size = new System.Drawing.Size(84, 40);
            this._button_stop.TabIndex = 3;
            this._button_stop.Text = "STOP";
            this._button_stop.UseVisualStyleBackColor = true;
            this._button_stop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // _openfiledialog
            // 
            this._openfiledialog.FileName = "openFileDialog";
            // 
            // _listview
            // 
            this._listview.HideSelection = false;
            this._listview.Location = new System.Drawing.Point(12, 74);
            this._listview.Name = "_listview";
            this._listview.Size = new System.Drawing.Size(415, 423);
            this._listview.TabIndex = 4;
            this._listview.UseCompatibleStateImageBehavior = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 575);
            this.Controls.Add(this._listview);
            this.Controls.Add(this._button_stop);
            this.Controls.Add(this._button_start);
            this.Controls.Add(this._button_load_midi_file);
            this.Controls.Add(this._button_load_soundfont);
            this.Name = "MainForm";
            this.Text = "MIDIPlayer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _button_load_soundfont;
        private System.Windows.Forms.Button _button_load_midi_file;
        private System.Windows.Forms.Button _button_start;
        private System.Windows.Forms.Button _button_stop;
        private System.Windows.Forms.OpenFileDialog _openfiledialog;
        private BufferedListView _listview;
    }
}

