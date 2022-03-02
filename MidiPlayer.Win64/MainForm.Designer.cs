
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
            this._buttonLoadSoundFont = new System.Windows.Forms.Button();
            this._buttonLoadMidiFile = new System.Windows.Forms.Button();
            this._buttonStart = new System.Windows.Forms.Button();
            this._buttonStop = new System.Windows.Forms.Button();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._listView = new BufferedListView();
            this.SuspendLayout();
            // 
            // buttonLoadSoundFont
            // 
            this._buttonLoadSoundFont.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._buttonLoadSoundFont.Location = new System.Drawing.Point(12, 12);
            this._buttonLoadSoundFont.Name = "buttonLoadSoundFont";
            this._buttonLoadSoundFont.Size = new System.Drawing.Size(84, 40);
            this._buttonLoadSoundFont.TabIndex = 0;
            this._buttonLoadSoundFont.Text = "SOUND";
            this._buttonLoadSoundFont.UseVisualStyleBackColor = true;
            this._buttonLoadSoundFont.Click += new System.EventHandler(this.buttonLoadSoundFont_Click);
            // 
            // buttonLoadMidiFile
            // 
            this._buttonLoadMidiFile.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._buttonLoadMidiFile.Location = new System.Drawing.Point(107, 12);
            this._buttonLoadMidiFile.Name = "buttonLoadMidiFile";
            this._buttonLoadMidiFile.Size = new System.Drawing.Size(84, 40);
            this._buttonLoadMidiFile.TabIndex = 1;
            this._buttonLoadMidiFile.Text = "SONG";
            this._buttonLoadMidiFile.UseVisualStyleBackColor = true;
            this._buttonLoadMidiFile.Click += new System.EventHandler(this.buttonLoadMidiFile_Click);
            // 
            // buttonStart
            // 
            this._buttonStart.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._buttonStart.Location = new System.Drawing.Point(202, 12);
            this._buttonStart.Name = "buttonStart";
            this._buttonStart.Size = new System.Drawing.Size(84, 40);
            this._buttonStart.TabIndex = 2;
            this._buttonStart.Text = "START";
            this._buttonStart.UseVisualStyleBackColor = true;
            this._buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this._buttonStop.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._buttonStop.Location = new System.Drawing.Point(297, 12);
            this._buttonStop.Name = "buttonStop";
            this._buttonStop.Size = new System.Drawing.Size(84, 40);
            this._buttonStop.TabIndex = 3;
            this._buttonStop.Text = "STOP";
            this._buttonStop.UseVisualStyleBackColor = true;
            this._buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // openFileDialog
            // 
            this._openFileDialog.FileName = "openFileDialog";
            // 
            // listView
            // 
            this._listView.HideSelection = false;
            this._listView.Location = new System.Drawing.Point(12, 74);
            this._listView.Name = "listView";
            this._listView.Size = new System.Drawing.Size(604, 423);
            this._listView.TabIndex = 4;
            this._listView.UseCompatibleStateImageBehavior = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 508);
            this.Controls.Add(this._listView);
            this.Controls.Add(this._buttonStop);
            this.Controls.Add(this._buttonStart);
            this.Controls.Add(this._buttonLoadMidiFile);
            this.Controls.Add(this._buttonLoadSoundFont);
            this.Name = "MainForm";
            this.Text = "MIDIPlayer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _buttonLoadSoundFont;
        private System.Windows.Forms.Button _buttonLoadMidiFile;
        private System.Windows.Forms.Button _buttonStart;
        private System.Windows.Forms.Button _buttonStop;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.ListView _listView;
    }
}

