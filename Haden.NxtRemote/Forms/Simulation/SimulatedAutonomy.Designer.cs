namespace Haden.NXTRemote.Forms.Simulation
{
    partial class SimulatedAutonomy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.closeButton = new System.Windows.Forms.Button();
            this.beginButton = new System.Windows.Forms.Button();
            this.linecmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NewLine = new System.Windows.Forms.ToolStripMenuItem();
            this.LineWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.Report = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.castRayButton = new System.Windows.Forms.Button();
            this.findStationButton = new System.Windows.Forms.Button();
            this.seekRightIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.seekLeftIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.stopButton = new System.Windows.Forms.Button();
            this.otherSimButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(364, 422);
            this.closeButton.Margin = new System.Windows.Forms.Padding(2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(56, 19);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // beginButton
            // 
            this.beginButton.Location = new System.Drawing.Point(376, 10);
            this.beginButton.Margin = new System.Windows.Forms.Padding(2);
            this.beginButton.Name = "beginButton";
            this.beginButton.Size = new System.Drawing.Size(44, 26);
            this.beginButton.TabIndex = 1;
            this.beginButton.Text = "Draw";
            this.beginButton.UseVisualStyleBackColor = true;
            this.beginButton.Click += new System.EventHandler(this.beginButton_Click);
            // 
            // linecmenu
            // 
            this.linecmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewLine,
            this.LineWidth,
            this.Report,
            this.Delete});
            // 
            // NewLine
            // 
            this.NewLine.Text = "New";
            this.NewLine.Click += new System.EventHandler(this.NewLine_Click);
            // 
            // LineWidth
            // 
            this.LineWidth.Text = "Width";
            this.LineWidth.Click += new System.EventHandler(this.Width_Click);
            // 
            // Report
            // 
            this.Report.Text = "Report coordinates";
            this.Report.Click += new System.EventHandler(this.Report_Click);
            // 
            // Delete
            // 
            this.Delete.Text = "Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // castRayButton
            // 
            this.castRayButton.Location = new System.Drawing.Point(376, 41);
            this.castRayButton.Margin = new System.Windows.Forms.Padding(2);
            this.castRayButton.Name = "castRayButton";
            this.castRayButton.Size = new System.Drawing.Size(44, 26);
            this.castRayButton.TabIndex = 2;
            this.castRayButton.Text = "Cast";
            this.castRayButton.UseVisualStyleBackColor = true;
            this.castRayButton.Click += new System.EventHandler(this.castRayButton_Click);
            // 
            // findStationButton
            // 
            this.findStationButton.Location = new System.Drawing.Point(376, 72);
            this.findStationButton.Margin = new System.Windows.Forms.Padding(2);
            this.findStationButton.Name = "findStationButton";
            this.findStationButton.Size = new System.Drawing.Size(44, 26);
            this.findStationButton.TabIndex = 3;
            this.findStationButton.Text = "Find";
            this.findStationButton.UseVisualStyleBackColor = true;
            this.findStationButton.Click += new System.EventHandler(this.findStationButton_Click);
            // 
            // seekRightIndicator
            // 
            this.seekRightIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.seekRightIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.seekRightIndicator.FlashNumber = 0;
            this.seekRightIndicator.Location = new System.Drawing.Point(267, 418);
            this.seekRightIndicator.Margin = new System.Windows.Forms.Padding(2);
            this.seekRightIndicator.Name = "seekRightIndicator";
            this.seekRightIndicator.Size = new System.Drawing.Size(64, 27);
            this.seekRightIndicator.TabIndex = 10;
            this.seekRightIndicator.Text = "seek right";
            this.seekRightIndicator.UseVisualStyleBackColor = true;
            // 
            // seekLeftIndicator
            // 
            this.seekLeftIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.seekLeftIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.seekLeftIndicator.FlashNumber = 0;
            this.seekLeftIndicator.Location = new System.Drawing.Point(9, 418);
            this.seekLeftIndicator.Margin = new System.Windows.Forms.Padding(2);
            this.seekLeftIndicator.Name = "seekLeftIndicator";
            this.seekLeftIndicator.Size = new System.Drawing.Size(64, 27);
            this.seekLeftIndicator.TabIndex = 9;
            this.seekLeftIndicator.Text = "seek left";
            this.seekLeftIndicator.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(376, 350);
            this.stopButton.Margin = new System.Windows.Forms.Padding(2);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(44, 26);
            this.stopButton.TabIndex = 11;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // otherSimButton
            // 
            this.otherSimButton.Location = new System.Drawing.Point(139, 421);
            this.otherSimButton.Name = "otherSimButton";
            this.otherSimButton.Size = new System.Drawing.Size(75, 23);
            this.otherSimButton.TabIndex = 12;
            this.otherSimButton.Text = "other sim";
            this.otherSimButton.UseVisualStyleBackColor = true;
            this.otherSimButton.Click += new System.EventHandler(this.otherSimButton_Click);
            // 
            // SimulatedAutonomy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 451);
            this.Controls.Add(this.otherSimButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.seekRightIndicator);
            this.Controls.Add(this.seekLeftIndicator);
            this.Controls.Add(this.findStationButton);
            this.Controls.Add(this.castRayButton);
            this.Controls.Add(this.beginButton);
            this.Controls.Add(this.closeButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SimulatedAutonomy";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "haden :: Autonomy simulator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button beginButton;
        private System.Windows.Forms.ContextMenuStrip linecmenu;
        private System.Windows.Forms.ToolStripMenuItem LineWidth;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ToolStripMenuItem NewLine;
        private System.Windows.Forms.ToolStripMenuItem Report;
        private System.Windows.Forms.Button castRayButton;
        private System.Windows.Forms.Button findStationButton;
        private Haden.NXTRemote.Controls.FlashButton seekRightIndicator;
        private Haden.NXTRemote.Controls.FlashButton seekLeftIndicator;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button otherSimButton;
    }
}
