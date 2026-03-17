namespace Haden.NXTRemote.Forms
{
    partial class SimulatorMockup
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
            this.closeForm = new System.Windows.Forms.Button();
            this.castRayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // closeForm
            // 
            this.closeForm.Location = new System.Drawing.Point(577, 12);
            this.closeForm.Name = "closeForm";
            this.closeForm.Size = new System.Drawing.Size(49, 23);
            this.closeForm.TabIndex = 0;
            this.closeForm.Text = "close";
            this.closeForm.UseVisualStyleBackColor = true;
            this.closeForm.Click += new System.EventHandler(this.closeForm_Click);
            // 
            // castRayButton
            // 
            this.castRayButton.Location = new System.Drawing.Point(514, 11);
            this.castRayButton.Name = "castRayButton";
            this.castRayButton.Size = new System.Drawing.Size(57, 23);
            this.castRayButton.TabIndex = 1;
            this.castRayButton.Text = "cast";
            this.castRayButton.UseVisualStyleBackColor = true;
            this.castRayButton.Click += new System.EventHandler(this.castRayButton_Click);
            // 
            // SimulatorMockup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.castRayButton);
            this.Controls.Add(this.closeForm);
            this.Name = "SimulatorMockup";
            this.Text = "Mockup";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeForm;
        private System.Windows.Forms.Button castRayButton;
    }
}