namespace Haden.NXTRemote.Controls
{
    partial class LineWidthDialog
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
            this.CancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.OkButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(98, 70);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(67, 27);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Line Width";
            // 
            // numericUpDown
            // 
            this.numericUpDown.Location = new System.Drawing.Point(117, 24);
            this.numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(58, 22);
            this.numericUpDown.TabIndex = 5;
            this.numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(21, 70);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(67, 27);
            this.OkButton.TabIndex = 4;
            this.OkButton.Text = "&OK";
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // ContextMenuLineWidthDialog
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 130);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown);
            this.Controls.Add(this.OkButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContextMenuLineWidthDialog";
            this.Text = "Line Width";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown;
        private System.Windows.Forms.Button OkButton;
    }
}