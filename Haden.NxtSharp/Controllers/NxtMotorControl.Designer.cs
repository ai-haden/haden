namespace Haden.NxtSharp.Controllers
{
    /// <summary>
    /// The designer attributes for the motor control.
    /// </summary>
	partial class NxtMotorControl
    {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.buttonTurnCounterClockwise = new System.Windows.Forms.Button();
            this.buttonTurnClockwise = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTurnCounterClockwise
            // 
            this.buttonTurnCounterClockwise.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonTurnCounterClockwise.Location = new System.Drawing.Point(4, 34);
            this.buttonTurnCounterClockwise.Name = "buttonTurnCounterClockwise";
            this.buttonTurnCounterClockwise.Size = new System.Drawing.Size(71, 28);
            this.buttonTurnCounterClockwise.TabIndex = 5;
            this.buttonTurnCounterClockwise.Text = "6";
            this.buttonTurnCounterClockwise.UseVisualStyleBackColor = true;
            this.buttonTurnCounterClockwise.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTurnCounterClockwise_MouseDown);
            this.buttonTurnCounterClockwise.MouseHover += new System.EventHandler(this.buttonTurnCounterClockwise_MouseHover);
            this.buttonTurnCounterClockwise.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTurnCounterClockwise_MouseUp);
            // 
            // buttonTurnClockwise
            // 
            this.buttonTurnClockwise.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonTurnClockwise.Location = new System.Drawing.Point(4, 4);
            this.buttonTurnClockwise.Name = "buttonTurnClockwise";
            this.buttonTurnClockwise.Size = new System.Drawing.Size(71, 24);
            this.buttonTurnClockwise.TabIndex = 4;
            this.buttonTurnClockwise.Text = "5";
            this.buttonTurnClockwise.UseVisualStyleBackColor = true;
            this.buttonTurnClockwise.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTurnClockwise_MouseDown);
            this.buttonTurnClockwise.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTurnClockwise_MouseUp);
            // 
            // NxtMotorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.buttonTurnCounterClockwise);
            this.Controls.Add(this.buttonTurnClockwise);
            this.Name = "NxtMotorControl";
            this.Size = new System.Drawing.Size(78, 69);
            this.ResumeLayout(false);

		}

		#endregion

        /// <summary>
        /// The button to turn a motor counter-clockwise.
        /// </summary>
        public System.Windows.Forms.Button buttonTurnCounterClockwise;
        /// <summary>
        /// The button to turn a motor clockwise.
        /// </summary>
        public System.Windows.Forms.Button buttonTurnClockwise;
	}
}
