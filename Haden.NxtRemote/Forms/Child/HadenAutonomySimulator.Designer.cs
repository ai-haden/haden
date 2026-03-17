using Haden.NxtSharp.Brick;
using Haden.NxtSharp.Motors;
using Haden.NxtSharp.Sensors;

namespace Haden.NXTRemote.Forms.Child
{
    partial class HadenAutonomySimulator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HadenAutonomySimulator));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bumpIndicator = new System.Windows.Forms.Button();
            this.differenceLightSensorValue = new System.Windows.Forms.Label();
            this.optimalValueIndicator = new System.Windows.Forms.Label();
            this.currentValueIndicator = new System.Windows.Forms.Label();
            this.seekPowerBlock = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.commandOutputTextBox = new System.Windows.Forms.TextBox();
            this.closeFormButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.backCounterClockwiseLeftIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.backClockwiseRightIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.aheadCounterClockwiseLeftIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.aheadClockwiseRightIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.moveReverseIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.seekRightIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.seekLeftIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.moveForwardIndicator = new Haden.NXTRemote.Controls.FlashButton();
            this.nxtLightSensor = new NxtLightSensor(this.components);
            this.nxtBrick = new NxtBrick(this.components);
            this.nxtMotorA = new NxtMotor(this.components);
            this.nxtMotorB = new NxtMotor(this.components);
            this.nxtMotorC = new NxtMotor(this.components);
            this.nxtPressureSensor = new NxtPressureSensor(this.components);
            this.nxtSoundSensor = new NxtSoundSensor(this.components);
            this.nxtSonar = new NxtSonar(this.components);
            this.iteratorValue = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.iteratorValue);
            this.groupBox1.Controls.Add(this.bumpIndicator);
            this.groupBox1.Controls.Add(this.differenceLightSensorValue);
            this.groupBox1.Controls.Add(this.optimalValueIndicator);
            this.groupBox1.Controls.Add(this.currentValueIndicator);
            this.groupBox1.Controls.Add(this.seekPowerBlock);
            this.groupBox1.Location = new System.Drawing.Point(22, 397);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(368, 61);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Code Blocks";
            // 
            // bumpIndicator
            // 
            this.bumpIndicator.Location = new System.Drawing.Point(87, 25);
            this.bumpIndicator.Margin = new System.Windows.Forms.Padding(2);
            this.bumpIndicator.Name = "bumpIndicator";
            this.bumpIndicator.Size = new System.Drawing.Size(43, 22);
            this.bumpIndicator.TabIndex = 20;
            this.bumpIndicator.Text = "Bump";
            this.bumpIndicator.UseVisualStyleBackColor = true;
            this.bumpIndicator.Click += new System.EventHandler(this.bumpIndicator_Click);
            // 
            // differenceLightSensorValue
            // 
            this.differenceLightSensorValue.AutoSize = true;
            this.differenceLightSensorValue.Location = new System.Drawing.Point(311, 31);
            this.differenceLightSensorValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.differenceLightSensorValue.Name = "differenceLightSensorValue";
            this.differenceLightSensorValue.Size = new System.Drawing.Size(18, 13);
            this.differenceLightSensorValue.TabIndex = 19;
            this.differenceLightSensorValue.Text = "s0";
            // 
            // optimalValueIndicator
            // 
            this.optimalValueIndicator.AutoSize = true;
            this.optimalValueIndicator.Location = new System.Drawing.Point(287, 31);
            this.optimalValueIndicator.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.optimalValueIndicator.Name = "optimalValueIndicator";
            this.optimalValueIndicator.Size = new System.Drawing.Size(19, 13);
            this.optimalValueIndicator.TabIndex = 14;
            this.optimalValueIndicator.Text = "n0";
            // 
            // currentValueIndicator
            // 
            this.currentValueIndicator.AutoSize = true;
            this.currentValueIndicator.Location = new System.Drawing.Point(264, 31);
            this.currentValueIndicator.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.currentValueIndicator.Name = "currentValueIndicator";
            this.currentValueIndicator.Size = new System.Drawing.Size(19, 13);
            this.currentValueIndicator.TabIndex = 15;
            this.currentValueIndicator.Text = "n1";
            // 
            // seekPowerBlock
            // 
            this.seekPowerBlock.Location = new System.Drawing.Point(39, 25);
            this.seekPowerBlock.Margin = new System.Windows.Forms.Padding(2);
            this.seekPowerBlock.Name = "seekPowerBlock";
            this.seekPowerBlock.Size = new System.Drawing.Size(41, 22);
            this.seekPowerBlock.TabIndex = 0;
            this.seekPowerBlock.Text = "Seek";
            this.seekPowerBlock.UseVisualStyleBackColor = true;
            this.seekPowerBlock.Click += new System.EventHandler(this.seekPowerBlock_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.commandOutputTextBox);
            this.groupBox2.Location = new System.Drawing.Point(22, 462);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(368, 159);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Command window";
            // 
            // commandOutputTextBox
            // 
            this.commandOutputTextBox.Location = new System.Drawing.Point(4, 25);
            this.commandOutputTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.commandOutputTextBox.Multiline = true;
            this.commandOutputTextBox.Name = "commandOutputTextBox";
            this.commandOutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.commandOutputTextBox.Size = new System.Drawing.Size(360, 132);
            this.commandOutputTextBox.TabIndex = 0;
            // 
            // closeFormButton
            // 
            this.closeFormButton.Location = new System.Drawing.Point(363, 2);
            this.closeFormButton.Margin = new System.Windows.Forms.Padding(2);
            this.closeFormButton.Name = "closeFormButton";
            this.closeFormButton.Size = new System.Drawing.Size(56, 19);
            this.closeFormButton.TabIndex = 2;
            this.closeFormButton.Text = "Close";
            this.closeFormButton.UseVisualStyleBackColor = true;
            this.closeFormButton.Click += new System.EventHandler(this.closeFormButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.backCounterClockwiseLeftIndicator);
            this.groupBox3.Controls.Add(this.backClockwiseRightIndicator);
            this.groupBox3.Controls.Add(this.aheadCounterClockwiseLeftIndicator);
            this.groupBox3.Controls.Add(this.aheadClockwiseRightIndicator);
            this.groupBox3.Controls.Add(this.moveReverseIndicator);
            this.groupBox3.Controls.Add(this.seekRightIndicator);
            this.groupBox3.Controls.Add(this.seekLeftIndicator);
            this.groupBox3.Controls.Add(this.moveForwardIndicator);
            this.groupBox3.Location = new System.Drawing.Point(22, 302);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(368, 91);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Indicators";
            // 
            // backCounterClockwiseLeftIndicator
            // 
            this.backCounterClockwiseLeftIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.backCounterClockwiseLeftIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.backCounterClockwiseLeftIndicator.FlashNumber = 0;
            this.backCounterClockwiseLeftIndicator.Location = new System.Drawing.Point(252, 50);
            this.backCounterClockwiseLeftIndicator.Name = "backCounterClockwiseLeftIndicator";
            this.backCounterClockwiseLeftIndicator.Size = new System.Drawing.Size(76, 26);
            this.backCounterClockwiseLeftIndicator.TabIndex = 13;
            this.backCounterClockwiseLeftIndicator.Text = "back CCW-l";
            this.backCounterClockwiseLeftIndicator.UseVisualStyleBackColor = true;
            // 
            // backClockwiseRightIndicator
            // 
            this.backClockwiseRightIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.backClockwiseRightIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.backClockwiseRightIndicator.FlashNumber = 0;
            this.backClockwiseRightIndicator.Location = new System.Drawing.Point(170, 50);
            this.backClockwiseRightIndicator.Name = "backClockwiseRightIndicator";
            this.backClockwiseRightIndicator.Size = new System.Drawing.Size(76, 26);
            this.backClockwiseRightIndicator.TabIndex = 12;
            this.backClockwiseRightIndicator.Text = "back CW-r";
            this.backClockwiseRightIndicator.UseVisualStyleBackColor = true;
            // 
            // aheadCounterClockwiseLeftIndicator
            // 
            this.aheadCounterClockwiseLeftIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.aheadCounterClockwiseLeftIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.aheadCounterClockwiseLeftIndicator.FlashNumber = 0;
            this.aheadCounterClockwiseLeftIndicator.Location = new System.Drawing.Point(87, 50);
            this.aheadCounterClockwiseLeftIndicator.Name = "aheadCounterClockwiseLeftIndicator";
            this.aheadCounterClockwiseLeftIndicator.Size = new System.Drawing.Size(76, 26);
            this.aheadCounterClockwiseLeftIndicator.TabIndex = 11;
            this.aheadCounterClockwiseLeftIndicator.Text = "ahead CCW-l";
            this.aheadCounterClockwiseLeftIndicator.UseVisualStyleBackColor = true;
            // 
            // aheadClockwiseRightIndicator
            // 
            this.aheadClockwiseRightIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.aheadClockwiseRightIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.aheadClockwiseRightIndicator.FlashNumber = 0;
            this.aheadClockwiseRightIndicator.Location = new System.Drawing.Point(4, 50);
            this.aheadClockwiseRightIndicator.Name = "aheadClockwiseRightIndicator";
            this.aheadClockwiseRightIndicator.Size = new System.Drawing.Size(76, 26);
            this.aheadClockwiseRightIndicator.TabIndex = 10;
            this.aheadClockwiseRightIndicator.Text = "ahead CW-r";
            this.aheadClockwiseRightIndicator.UseVisualStyleBackColor = true;
            // 
            // moveReverseIndicator
            // 
            this.moveReverseIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.moveReverseIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.moveReverseIndicator.FlashNumber = 0;
            this.moveReverseIndicator.Location = new System.Drawing.Point(209, 19);
            this.moveReverseIndicator.Name = "moveReverseIndicator";
            this.moveReverseIndicator.Size = new System.Drawing.Size(62, 26);
            this.moveReverseIndicator.TabIndex = 9;
            this.moveReverseIndicator.Text = "reverse";
            this.moveReverseIndicator.UseVisualStyleBackColor = true;
            // 
            // seekRightIndicator
            // 
            this.seekRightIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.seekRightIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.seekRightIndicator.FlashNumber = 0;
            this.seekRightIndicator.Location = new System.Drawing.Point(72, 17);
            this.seekRightIndicator.Margin = new System.Windows.Forms.Padding(2);
            this.seekRightIndicator.Name = "seekRightIndicator";
            this.seekRightIndicator.Size = new System.Drawing.Size(64, 27);
            this.seekRightIndicator.TabIndex = 8;
            this.seekRightIndicator.Text = "seek right";
            this.seekRightIndicator.UseVisualStyleBackColor = true;
            // 
            // seekLeftIndicator
            // 
            this.seekLeftIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.seekLeftIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.seekLeftIndicator.FlashNumber = 0;
            this.seekLeftIndicator.Location = new System.Drawing.Point(4, 18);
            this.seekLeftIndicator.Margin = new System.Windows.Forms.Padding(2);
            this.seekLeftIndicator.Name = "seekLeftIndicator";
            this.seekLeftIndicator.Size = new System.Drawing.Size(64, 27);
            this.seekLeftIndicator.TabIndex = 7;
            this.seekLeftIndicator.Text = "seek left";
            this.seekLeftIndicator.UseVisualStyleBackColor = true;
            // 
            // moveForwardIndicator
            // 
            this.moveForwardIndicator.FlasherButtonColorOff = System.Drawing.SystemColors.Control;
            this.moveForwardIndicator.FlasherButtonColorOn = System.Drawing.Color.LightGreen;
            this.moveForwardIndicator.FlashNumber = 0;
            this.moveForwardIndicator.Location = new System.Drawing.Point(141, 18);
            this.moveForwardIndicator.Name = "moveForwardIndicator";
            this.moveForwardIndicator.Size = new System.Drawing.Size(62, 26);
            this.moveForwardIndicator.TabIndex = 5;
            this.moveForwardIndicator.Text = "forward";
            this.moveForwardIndicator.UseVisualStyleBackColor = true;
            // 
            // nxtLightSensor
            // 
            this.nxtLightSensor.Active = true;
            this.nxtLightSensor.AutoPoll = false;
            this.nxtLightSensor.AutoPollDelay = 100;
            this.nxtLightSensor.Brick = this.nxtBrick;
            this.nxtLightSensor.Port = NxtSensorPort.Port3;
            this.nxtLightSensor.ValueChanged += new SensorEvent(this.nxtLightSensor_ValueChanged);
            // 
            // nxtBrick
            // 
            this.nxtBrick.AutoPoll = false;
            this.nxtBrick.ComPortName = "COM40";
            this.nxtBrick.MotorA = this.nxtMotorA;
            this.nxtBrick.MotorB = this.nxtMotorB;
            this.nxtBrick.MotorC = this.nxtMotorC;
            this.nxtBrick.Sensor1 = this.nxtPressureSensor;
            this.nxtBrick.Sensor2 = this.nxtSoundSensor;
            this.nxtBrick.Sensor3 = this.nxtLightSensor;
            this.nxtBrick.Sensor4 = this.nxtSonar;
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
            this.nxtPressureSensor.AutoPoll = false;
            this.nxtPressureSensor.AutoPollDelay = 100;
            this.nxtPressureSensor.Brick = this.nxtBrick;
            this.nxtPressureSensor.Port = NxtSensorPort.Port1;
            this.nxtPressureSensor.ValueChanged += new SensorEvent(this.nxtPressureSensor_Polled);
            // 
            // nxtSoundSensor
            // 
            this.nxtSoundSensor.AdjustForHumanEar = true;
            this.nxtSoundSensor.AutoPoll = false;
            this.nxtSoundSensor.AutoPollDelay = 100;
            this.nxtSoundSensor.Brick = this.nxtBrick;
            this.nxtSoundSensor.Port = NxtSensorPort.Port2;
            // 
            // nxtSonar
            // 
            this.nxtSonar.AutoPoll = false;
            this.nxtSonar.AutoPollDelay = 100;
            this.nxtSonar.Brick = this.nxtBrick;
            this.nxtSonar.Port = NxtSensorPort.Port4;
            // 
            // iteratorValue
            // 
            this.iteratorValue.AutoSize = true;
            this.iteratorValue.Location = new System.Drawing.Point(210, 31);
            this.iteratorValue.Name = "iteratorValue";
            this.iteratorValue.Size = new System.Drawing.Size(9, 13);
            this.iteratorValue.TabIndex = 21;
            this.iteratorValue.Text = "i";
            // 
            // HadenAutonomySimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 630);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.closeFormButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "HadenAutonomySimulator";
            this.ShowInTaskbar = false;
            this.Text = "Autonomy Simulator";
            this.Load += new System.EventHandler(this.HadenAutonomySimulator_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button seekPowerBlock;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox commandOutputTextBox;
        private System.Windows.Forms.Button closeFormButton;
        private Controls.FlashButton moveForwardIndicator;
        private Haden.NXTRemote.Controls.FlashButton seekLeftIndicator;
        private System.Windows.Forms.GroupBox groupBox3;
        private Haden.NXTRemote.Controls.FlashButton seekRightIndicator;
        private Haden.NXTRemote.Controls.FlashButton moveReverseIndicator;
        private Haden.NXTRemote.Controls.FlashButton aheadClockwiseRightIndicator;
        private Haden.NXTRemote.Controls.FlashButton aheadCounterClockwiseLeftIndicator;
        private Haden.NXTRemote.Controls.FlashButton backCounterClockwiseLeftIndicator;
        private Haden.NXTRemote.Controls.FlashButton backClockwiseRightIndicator;
        private NxtLightSensor nxtLightSensor;
        private NxtBrick nxtBrick;
        private NxtMotor nxtMotorA;
        private NxtMotor nxtMotorB;
        private NxtMotor nxtMotorC;
        private NxtPressureSensor nxtPressureSensor;
        private NxtSoundSensor nxtSoundSensor;
        private NxtSonar nxtSonar;
        private System.Windows.Forms.Label currentValueIndicator;
        private System.Windows.Forms.Label optimalValueIndicator;
        private System.Windows.Forms.Label differenceLightSensorValue;
        private System.Windows.Forms.Button bumpIndicator;
        private System.Windows.Forms.Label iteratorValue;
    }
}