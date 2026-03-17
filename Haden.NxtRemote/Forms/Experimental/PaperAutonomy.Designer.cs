namespace Haden.NXTRemote.Forms.Experimental
{
    partial class PaperAutonomy
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
            this.drawButton = new System.Windows.Forms.Button();
            this.linecmenu = new System.Windows.Forms.ContextMenu();
            this.NewLine = new System.Windows.Forms.MenuItem();
            this.LineWidth = new System.Windows.Forms.MenuItem();
            this.Report = new System.Windows.Forms.MenuItem();
            this.Delete = new System.Windows.Forms.MenuItem();
            this.castRayButton = new System.Windows.Forms.Button();
            this.findStationButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.seekStraightIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.seekRightIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.seekLeftIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(485, 519);
            this.closeButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // drawButton
            // 
            this.drawButton.Location = new System.Drawing.Point(501, 12);
            this.drawButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(59, 32);
            this.drawButton.TabIndex = 1;
            this.drawButton.Text = "Draw";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.beginButton_Click);
            // 
            // linecmenu
            // 
            this.linecmenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.NewLine,
            this.LineWidth,
            this.Report,
            this.Delete});
            // 
            // NewLine
            // 
            this.NewLine.Index = 0;
            this.NewLine.Text = "New";
            this.NewLine.Click += new System.EventHandler(this.NewLine_Click);
            // 
            // LineWidth
            // 
            this.LineWidth.Index = 1;
            this.LineWidth.Text = "Width";
            this.LineWidth.Click += new System.EventHandler(this.Width_Click);
            // 
            // Report
            // 
            this.Report.Index = 2;
            this.Report.Text = "Report coordinates";
            this.Report.Click += new System.EventHandler(this.Report_Click);
            // 
            // Delete
            // 
            this.Delete.Index = 3;
            this.Delete.Text = "Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // castRayButton
            // 
            this.castRayButton.Location = new System.Drawing.Point(501, 50);
            this.castRayButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.castRayButton.Name = "castRayButton";
            this.castRayButton.Size = new System.Drawing.Size(59, 32);
            this.castRayButton.TabIndex = 2;
            this.castRayButton.Text = "Cast";
            this.castRayButton.UseVisualStyleBackColor = true;
            this.castRayButton.Click += new System.EventHandler(this.castRayButton_Click);
            // 
            // findStationButton
            // 
            this.findStationButton.Location = new System.Drawing.Point(501, 89);
            this.findStationButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.findStationButton.Name = "findStationButton";
            this.findStationButton.Size = new System.Drawing.Size(59, 32);
            this.findStationButton.TabIndex = 3;
            this.findStationButton.Text = "Find";
            this.findStationButton.UseVisualStyleBackColor = true;
            this.findStationButton.Click += new System.EventHandler(this.findStationButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(501, 431);
            this.stopButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(59, 32);
            this.stopButton.TabIndex = 11;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // seekStraightIndicator
            // 
            this.seekStraightIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.seekStraightIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.seekStraightIndicator.FlashNumber = 0;
            this.seekStraightIndicator.Location = new System.Drawing.Point(174, 514);
            this.seekStraightIndicator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.seekStraightIndicator.Name = "seekStraightIndicator";
            this.seekStraightIndicator.Size = new System.Drawing.Size(113, 33);
            this.seekStraightIndicator.TabIndex = 12;
            this.seekStraightIndicator.Text = "seek straight";
            this.seekStraightIndicator.UseVisualStyleBackColor = true;
            // 
            // seekRightIndicator
            // 
            this.seekRightIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.seekRightIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.seekRightIndicator.FlashNumber = 0;
            this.seekRightIndicator.Location = new System.Drawing.Point(356, 514);
            this.seekRightIndicator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.seekRightIndicator.Name = "seekRightIndicator";
            this.seekRightIndicator.Size = new System.Drawing.Size(85, 33);
            this.seekRightIndicator.TabIndex = 10;
            this.seekRightIndicator.Text = "seek right";
            this.seekRightIndicator.UseVisualStyleBackColor = true;
            // 
            // seekLeftIndicator
            // 
            this.seekLeftIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.seekLeftIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.seekLeftIndicator.FlashNumber = 0;
            this.seekLeftIndicator.Location = new System.Drawing.Point(12, 514);
            this.seekLeftIndicator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.seekLeftIndicator.Name = "seekLeftIndicator";
            this.seekLeftIndicator.Size = new System.Drawing.Size(85, 33);
            this.seekLeftIndicator.TabIndex = 9;
            this.seekLeftIndicator.Text = "seek left";
            this.seekLeftIndicator.UseVisualStyleBackColor = true;
            // 
            // PaperAutonomy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 555);
            this.Controls.Add(this.seekStraightIndicator);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.seekRightIndicator);
            this.Controls.Add(this.seekLeftIndicator);
            this.Controls.Add(this.findStationButton);
            this.Controls.Add(this.castRayButton);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.closeButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PaperAutonomy";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "haden :: Autonomy simulator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.ContextMenu linecmenu;
        private System.Windows.Forms.MenuItem LineWidth;
        private System.Windows.Forms.MenuItem Delete;
        private System.Windows.Forms.MenuItem NewLine;
        private System.Windows.Forms.MenuItem Report;
        private System.Windows.Forms.Button castRayButton;
        private System.Windows.Forms.Button findStationButton;
        private Haden.NXTRemote.Controls.FlashButton seekRightIndicator;
        private Haden.NXTRemote.Controls.FlashButton seekLeftIndicator;
        private System.Windows.Forms.Button stopButton;
        private Controls.FlashButton seekStraightIndicator;
    }
}