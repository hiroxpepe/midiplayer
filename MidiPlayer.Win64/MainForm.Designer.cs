
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
            this.buttonLoadSoundFont = new System.Windows.Forms.Button();
            this.buttonLoadMidiFile = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.listView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // buttonLoadSoundFont
            // 
            this.buttonLoadSoundFont.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonLoadSoundFont.Location = new System.Drawing.Point(12, 12);
            this.buttonLoadSoundFont.Name = "buttonLoadSoundFont";
            this.buttonLoadSoundFont.Size = new System.Drawing.Size(84, 40);
            this.buttonLoadSoundFont.TabIndex = 0;
            this.buttonLoadSoundFont.Text = "SOUND";
            this.buttonLoadSoundFont.UseVisualStyleBackColor = true;
            this.buttonLoadSoundFont.Click += new System.EventHandler(this.buttonLoadSoundFont_Click);
            // 
            // buttonLoadMidiFile
            // 
            this.buttonLoadMidiFile.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonLoadMidiFile.Location = new System.Drawing.Point(107, 12);
            this.buttonLoadMidiFile.Name = "buttonLoadMidiFile";
            this.buttonLoadMidiFile.Size = new System.Drawing.Size(84, 40);
            this.buttonLoadMidiFile.TabIndex = 1;
            this.buttonLoadMidiFile.Text = "SONG";
            this.buttonLoadMidiFile.UseVisualStyleBackColor = true;
            this.buttonLoadMidiFile.Click += new System.EventHandler(this.buttonLoadMidiFile_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonStart.Location = new System.Drawing.Point(202, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(84, 40);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "START";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonStop.Location = new System.Drawing.Point(297, 12);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(84, 40);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "STOP";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // listView
            // 
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 74);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(604, 423);
            this.listView.TabIndex = 4;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 508);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonLoadMidiFile);
            this.Controls.Add(this.buttonLoadSoundFont);
            this.Name = "MainForm";
            this.Text = "MIDIPlayer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadSoundFont;
        private System.Windows.Forms.Button buttonLoadMidiFile;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ListView listView;
    }
}

