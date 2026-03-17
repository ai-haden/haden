using Haden.NxtSharp.Brick;
using Haden.NxtSharp.Controllers;
using Haden.NxtSharp.Motors;
using Haden.NxtSharp.Sensors;
using Haden.NxtSharp.Utilties;

namespace Haden.NXTRemote.Forms
{
    partial class HadenAutonomousControl
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
            this.components = new System.ComponentModel.Container();
            this.closeForm = new System.Windows.Forms.Button();
            this.beginAutonomy = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.differenceLightSensorValue = new System.Windows.Forms.Label();
            this.optimalLightSensorValue = new System.Windows.Forms.Label();
            this.lightSensorValueOut = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPressed = new System.Windows.Forms.Label();
            this.connectBrickButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.resetOptimalValueButton = new System.Windows.Forms.Button();
            this.nxtSensorControl = new NxtTankControl();
            this.nxtSensorDrive = new NxtTankDrive(this.components);
            this.nxtBrick = new NxtBrick(this.components);
            this.nxtMotorA = new NxtMotor(this.components);
            this.nxtMotorB = new NxtMotor(this.components);
            this.nxtMotorC = new NxtMotor(this.components);
            this.nxtPressureSensor = new NxtPressureSensor(this.components);
            this.nxtLightSensor = new NxtLightSensor(this.components);
            this.nxtMotorControl = new NxtMotorControl();
            this.nxtTankControl = new NxtTankControl();
            this.nxtTankDrive = new NxtTankDrive(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeForm
            // 
            this.closeForm.Location = new System.Drawing.Point(454, 6);
            this.closeForm.Name = "closeForm";
            this.closeForm.Size = new System.Drawing.Size(49, 26);
            this.closeForm.TabIndex = 1;
            this.closeForm.Text = "close";
            this.closeForm.UseVisualStyleBackColor = true;
            this.closeForm.Click += new System.EventHandler(this.closeForm_Click);
            // 
            // beginAutonomy
            // 
            this.beginAutonomy.Location = new System.Drawing.Point(459, 37);
            this.beginAutonomy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.beginAutonomy.Name = "beginAutonomy";
            this.beginAutonomy.Size = new System.Drawing.Size(37, 23);
            this.beginAutonomy.TabIndex = 5;
            this.beginAutonomy.Text = "S";
            this.beginAutonomy.UseVisualStyleBackColor = true;
            this.beginAutonomy.Click += new System.EventHandler(this.beginAutonomy_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.differenceLightSensorValue);
            this.groupBox1.Controls.Add(this.optimalLightSensorValue);
            this.groupBox1.Controls.Add(this.lightSensorValueOut);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblPressed);
            this.groupBox1.Location = new System.Drawing.Point(243, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 157);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor data";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 17);
            this.label4.TabIndex = 23;
            this.label4.Text = "Difference:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "Optimal value:";
            // 
            // differenceLightSensorValue
            // 
            this.differenceLightSensorValue.AutoSize = true;
            this.differenceLightSensorValue.Location = new System.Drawing.Point(156, 118);
            this.differenceLightSensorValue.Name = "differenceLightSensorValue";
            this.differenceLightSensorValue.Size = new System.Drawing.Size(23, 17);
            this.differenceLightSensorValue.TabIndex = 21;
            this.differenceLightSensorValue.Text = "s0";
            // 
            // optimalLightSensorValue
            // 
            this.optimalLightSensorValue.AutoSize = true;
            this.optimalLightSensorValue.Location = new System.Drawing.Point(155, 91);
            this.optimalLightSensorValue.Name = "optimalLightSensorValue";
            this.optimalLightSensorValue.Size = new System.Drawing.Size(24, 17);
            this.optimalLightSensorValue.TabIndex = 20;
            this.optimalLightSensorValue.Text = "n0";
            // 
            // lightSensorValueOut
            // 
            this.lightSensorValueOut.AutoSize = true;
            this.lightSensorValueOut.Location = new System.Drawing.Point(156, 61);
            this.lightSensorValueOut.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lightSensorValueOut.Name = "lightSensorValueOut";
            this.lightSensorValueOut.Size = new System.Drawing.Size(44, 17);
            this.lightSensorValueOut.TabIndex = 18;
            this.lightSensorValueOut.Text = "Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Pressure sensor";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 60);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 17);
            this.label3.TabIndex = 17;
            this.label3.Text = "Light sensor";
            // 
            // lblPressed
            // 
            this.lblPressed.AutoSize = true;
            this.lblPressed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPressed.Location = new System.Drawing.Point(156, 32);
            this.lblPressed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPressed.Name = "lblPressed";
            this.lblPressed.Size = new System.Drawing.Size(14, 17);
            this.lblPressed.TabIndex = 19;
            this.lblPressed.Text = "-";
            // 
            // connectBrickButton
            // 
            this.connectBrickButton.Location = new System.Drawing.Point(335, 5);
            this.connectBrickButton.Margin = new System.Windows.Forms.Padding(4);
            this.connectBrickButton.Name = "connectBrickButton";
            this.connectBrickButton.Size = new System.Drawing.Size(108, 28);
            this.connectBrickButton.TabIndex = 9;
            this.connectBrickButton.Text = "Connect brick";
            this.connectBrickButton.UseVisualStyleBackColor = true;
            this.connectBrickButton.Click += new System.EventHandler(this.connectBrickButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nxtMotorControl);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(23, 115);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(207, 123);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Seek";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 70);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 17);
            this.label12.TabIndex = 17;
            this.label12.Text = "Right";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(27, 28);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 17);
            this.label13.TabIndex = 16;
            this.label13.Text = "Left";
            // 
            // resetOptimalValueButton
            // 
            this.resetOptimalValueButton.Location = new System.Drawing.Point(335, 36);
            this.resetOptimalValueButton.Name = "resetOptimalValueButton";
            this.resetOptimalValueButton.Size = new System.Drawing.Size(108, 23);
            this.resetOptimalValueButton.TabIndex = 20;
            this.resetOptimalValueButton.Text = "Reset value";
            this.resetOptimalValueButton.UseVisualStyleBackColor = true;
            this.resetOptimalValueButton.Click += new System.EventHandler(this.resetOptimalValueButton_Click);
            // 
            // nxtSensorControl
            // 
            this.nxtSensorControl.Brake = true;
            this.nxtSensorControl.Enabled = false;
            this.nxtSensorControl.Location = new System.Drawing.Point(167, 13);
            this.nxtSensorControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nxtSensorControl.Name = "nxtSensorControl";
            this.nxtSensorControl.Power = 75;
            this.nxtSensorControl.Size = new System.Drawing.Size(69, 94);
            this.nxtSensorControl.TabIndex = 21;
            this.nxtSensorControl.TankDrive = this.nxtSensorDrive;
            // 
            // nxtSensorDrive
            // 
            this.nxtSensorDrive.Brick = this.nxtBrick;
            this.nxtSensorDrive.Motor1 = this.nxtMotorA;
            this.nxtSensorDrive.Motor2 = null;
            // 
            // nxtBrick
            // 
            this.nxtBrick.AutoPoll = true;
            this.nxtBrick.ComPortName = "COM40";
            this.nxtBrick.MotorA = this.nxtMotorA;
            this.nxtBrick.MotorB = this.nxtMotorB;
            this.nxtBrick.MotorC = this.nxtMotorC;
            this.nxtBrick.Sensor1 = this.nxtPressureSensor;
            this.nxtBrick.Sensor2 = null;
            this.nxtBrick.Sensor3 = this.nxtLightSensor;
            this.nxtBrick.Sensor4 = null;
            // 
            // nxtMotorA
            // 
            this.nxtMotorA.Brick = this.nxtBrick;
            this.nxtMotorA.Flip = false;
            this.nxtMotorA.Port = NxtMotorPort.PortA;
            // 
            // nxtMotorB
            // 
            this.nxtMotorB.Brick = this.nxtBrick;
            this.nxtMotorB.Flip = false;
            this.nxtMotorB.Port = NxtMotorPort.PortB;
            // 
            // nxtMotorC
            // 
            this.nxtMotorC.Brick = this.nxtBrick;
            this.nxtMotorC.Flip = false;
            this.nxtMotorC.Port = NxtMotorPort.PortC;
            // 
            // nxtPressureSensor
            // 
            this.nxtPressureSensor.AutoPoll = true;
            this.nxtPressureSensor.AutoPollDelay = 100;
            this.nxtPressureSensor.Brick = this.nxtBrick;
            this.nxtPressureSensor.Port = NxtSensorPort.Port1;
            this.nxtPressureSensor.ValueChanged += new SensorEvent(this.nxtPressureSensor_Polled);
            // 
            // nxtLightSensor
            // 
            this.nxtLightSensor.Active = false;
            this.nxtLightSensor.AutoPoll = true;
            this.nxtLightSensor.AutoPollDelay = 100;
            this.nxtLightSensor.Brick = this.nxtBrick;
            this.nxtLightSensor.Port = NxtSensorPort.Port3;
            this.nxtLightSensor.ValueChanged += new SensorEvent(this.nxtLightSensor_ValueChanged);
            // 
            // nxtMotorControl
            // 
            this.nxtMotorControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.nxtMotorControl.Brake = true;
            this.nxtMotorControl.ButtonDistance = 4;
            this.nxtMotorControl.Key1 = System.Windows.Forms.Keys.None;
            this.nxtMotorControl.Key2 = System.Windows.Forms.Keys.None;
            this.nxtMotorControl.Location = new System.Drawing.Point(69, 18);
            this.nxtMotorControl.Margin = new System.Windows.Forms.Padding(5);
            this.nxtMotorControl.MaximumSize = new System.Drawing.Size(104, 85);
            this.nxtMotorControl.MinimumSize = new System.Drawing.Size(46, 62);
            this.nxtMotorControl.Motor = this.nxtMotorA;
            this.nxtMotorControl.Name = "nxtMotorControl";
            this.nxtMotorControl.Orientation = NxtMotorControlOrientation.Vertical;
            this.nxtMotorControl.Power = 30;
            this.nxtMotorControl.Size = new System.Drawing.Size(104, 85);
            this.nxtMotorControl.TabIndex = 6;
            // 
            // nxtTankControl
            // 
            this.nxtTankControl.Brake = false;
            this.nxtTankControl.Enabled = false;
            this.nxtTankControl.Location = new System.Drawing.Point(23, 13);
            this.nxtTankControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nxtTankControl.Name = "nxtTankControl";
            this.nxtTankControl.Power = 75;
            this.nxtTankControl.Size = new System.Drawing.Size(126, 93);
            this.nxtTankControl.TabIndex = 7;
            this.nxtTankControl.TankDrive = this.nxtTankDrive;
            // 
            // nxtTankDrive
            // 
            this.nxtTankDrive.Brick = this.nxtBrick;
            this.nxtTankDrive.Motor1 = this.nxtMotorB;
            this.nxtTankDrive.Motor2 = this.nxtMotorC;
            // 
            // HadenAutonomousControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 283);
            this.Controls.Add(this.nxtSensorControl);
            this.Controls.Add(this.resetOptimalValueButton);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.connectBrickButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nxtTankControl);
            this.Controls.Add(this.beginAutonomy);
            this.Controls.Add(this.closeForm);
            this.Name = "HadenAutonomousControl";
            this.Text = "Autonomous Control";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeForm;
        private System.Windows.Forms.Button beginAutonomy;
        private NxtBrick nxtBrick;
        private NxtTankControl nxtTankControl;
        private NxtTankDrive nxtTankDrive;
        private NxtMotor nxtMotorB;
        private NxtMotor nxtMotorC;
        private NxtLightSensor nxtLightSensor;
        private NxtPressureSensor nxtPressureSensor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lightSensorValueOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPressed;
        private System.Windows.Forms.Button connectBrickButton;
        private System.Windows.Forms.Label differenceLightSensorValue;
        private System.Windows.Forms.Label optimalLightSensorValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private NxtMotorControl nxtMotorControl;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button resetOptimalValueButton;
        private NxtTankControl nxtSensorControl;
        private NxtTankDrive nxtSensorDrive;
        public NxtMotor nxtMotorA;
    }
}