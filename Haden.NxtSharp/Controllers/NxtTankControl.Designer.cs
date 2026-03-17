namespace Haden.NxtSharp.Controllers {
	partial class NxtTankControl {
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTurnRight = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonTurnLeft = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.buttonTurnRight, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonForward, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonTurnLeft, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonBack, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(130, 149);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonTurnRight
            // 
            this.buttonTurnRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTurnRight.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonTurnRight.Location = new System.Drawing.Point(68, 52);
            this.buttonTurnRight.Name = "buttonTurnRight";
            this.buttonTurnRight.Size = new System.Drawing.Size(59, 43);
            this.buttonTurnRight.TabIndex = 2;
            this.buttonTurnRight.Text = "4";
            this.buttonTurnRight.UseVisualStyleBackColor = true;
            this.buttonTurnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTurnRight_MouseDown);
            this.buttonTurnRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTurnRight_MouseUp);
            // 
            // buttonForward
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonForward, 2);
            this.buttonForward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonForward.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonForward.Location = new System.Drawing.Point(3, 3);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(124, 43);
            this.buttonForward.TabIndex = 0;
            this.buttonForward.Text = "5";
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonForward_MouseDown);
            this.buttonForward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonForward_MouseUp);
            // 
            // buttonTurnLeft
            // 
            this.buttonTurnLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTurnLeft.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonTurnLeft.Location = new System.Drawing.Point(3, 52);
            this.buttonTurnLeft.Name = "buttonTurnLeft";
            this.buttonTurnLeft.Size = new System.Drawing.Size(59, 43);
            this.buttonTurnLeft.TabIndex = 1;
            this.buttonTurnLeft.Text = "3";
            this.buttonTurnLeft.UseVisualStyleBackColor = true;
            this.buttonTurnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTurnLeft_MouseDown);
            this.buttonTurnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTurnLeft_MouseUp);
            // 
            // buttonBack
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonBack, 2);
            this.buttonBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBack.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonBack.Location = new System.Drawing.Point(3, 101);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(124, 45);
            this.buttonBack.TabIndex = 3;
            this.buttonBack.Text = "6";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonBack_MouseDown);
            this.buttonBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonBack_MouseUp);
            // 
            // NxtTankControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "NxtTankControl";
            this.Size = new System.Drawing.Size(130, 149);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button buttonForward;
		private System.Windows.Forms.Button buttonTurnLeft;
		private System.Windows.Forms.Button buttonBack;
		private System.Windows.Forms.Button buttonTurnRight;

	}
}
