namespace Haden.NXTRemote.Controls
{
    partial class CoordinatePositionEntryDialog
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
            this.Cancel = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FirstCoordinateX = new System.Windows.Forms.NumericUpDown();
            this.FirstCoordinateY = new System.Windows.Forms.NumericUpDown();
            this.SecondCoordinateX = new System.Windows.Forms.NumericUpDown();
            this.SecondCoordinateY = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FirstCoordinateX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirstCoordinateY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SecondCoordinateX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SecondCoordinateY)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(99, 93);
            this.Cancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(50, 22);
            this.Cancel.TabIndex = 9;
            this.Cancel.Text = "&Cancel";
            this.Cancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(30, 93);
            this.OkButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(50, 22);
            this.OkButton.TabIndex = 8;
            this.OkButton.Text = "&OK";
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Y";
            // 
            // FirstCoordinateX
            // 
            this.FirstCoordinateX.Location = new System.Drawing.Point(30, 36);
            this.FirstCoordinateX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FirstCoordinateX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FirstCoordinateX.Name = "FirstCoordinateX";
            this.FirstCoordinateX.Size = new System.Drawing.Size(44, 20);
            this.FirstCoordinateX.TabIndex = 14;
            this.FirstCoordinateX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FirstCoordinateY
            // 
            this.FirstCoordinateY.Location = new System.Drawing.Point(99, 36);
            this.FirstCoordinateY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FirstCoordinateY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FirstCoordinateY.Name = "FirstCoordinateY";
            this.FirstCoordinateY.Size = new System.Drawing.Size(44, 20);
            this.FirstCoordinateY.TabIndex = 15;
            this.FirstCoordinateY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SecondCoordinateX
            // 
            this.SecondCoordinateX.Location = new System.Drawing.Point(30, 71);
            this.SecondCoordinateX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SecondCoordinateX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SecondCoordinateX.Name = "SecondCoordinateX";
            this.SecondCoordinateX.Size = new System.Drawing.Size(44, 20);
            this.SecondCoordinateX.TabIndex = 16;
            this.SecondCoordinateX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SecondCoordinateY
            // 
            this.SecondCoordinateY.Location = new System.Drawing.Point(99, 71);
            this.SecondCoordinateY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SecondCoordinateY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SecondCoordinateY.Name = "SecondCoordinateY";
            this.SecondCoordinateY.Size = new System.Drawing.Size(44, 20);
            this.SecondCoordinateY.TabIndex = 17;
            this.SecondCoordinateY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "1st";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 72);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "2nd";
            // 
            // CoordinatePositionEntryDialog
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(157, 136);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SecondCoordinateY);
            this.Controls.Add(this.SecondCoordinateX);
            this.Controls.Add(this.FirstCoordinateY);
            this.Controls.Add(this.FirstCoordinateX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OkButton);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoordinatePositionEntryDialog";
            this.ShowIcon = false;
            this.Text = "Coordinates";
            ((System.ComponentModel.ISupportInitialize)(this.FirstCoordinateX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirstCoordinateY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SecondCoordinateX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SecondCoordinateY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown FirstCoordinateX;
        public System.Windows.Forms.NumericUpDown FirstCoordinateY;
        public System.Windows.Forms.NumericUpDown SecondCoordinateX;
        public System.Windows.Forms.NumericUpDown SecondCoordinateY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}