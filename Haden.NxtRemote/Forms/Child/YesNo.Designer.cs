namespace Haden.NXTRemote.Forms.Child
{
    partial class YesNo
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
            this.yesButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.communicateToOperator = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(16, 119);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(46, 23);
            this.yesButton.TabIndex = 1;
            this.yesButton.Text = "Yes";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
            // 
            // noButton
            // 
            this.noButton.Location = new System.Drawing.Point(68, 119);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(39, 23);
            this.noButton.TabIndex = 2;
            this.noButton.Text = "No";
            this.noButton.UseVisualStyleBackColor = true;
            this.noButton.Click += new System.EventHandler(this.noButton_Click);
            // 
            // communicateToOperator
            // 
            this.communicateToOperator.Location = new System.Drawing.Point(16, 12);
            this.communicateToOperator.Name = "communicateToOperator";
            this.communicateToOperator.Size = new System.Drawing.Size(129, 96);
            this.communicateToOperator.TabIndex = 3;
            this.communicateToOperator.Text = "";
            // 
            // YesNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 238);
            this.Controls.Add(this.communicateToOperator);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.yesButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(240, 277);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(240, 277);
            this.Name = "YesNo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Yes or No?";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.RichTextBox communicateToOperator;
    }
}