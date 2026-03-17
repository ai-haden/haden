using Haden.NxtSharp.Brick;
using Haden.NxtSharp.Controllers;
using Haden.NxtSharp.Motors;
using Haden.NxtSharp.Sensors;
using Haden.NxtSharp.Utilties;

namespace Haden.NXTRemote.Forms
{
	partial class HadenManualControl
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HadenManualControl));
            this.valLight = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lastRememberedTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comPortSelectionBox = new System.Windows.Forms.ComboBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.simulatedAutonomyButton = new System.Windows.Forms.Button();
            this.hadenAutonomousFormButton = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.nxtTankControl1 = new Haden.NxtSharp.Controllers.NxtTankControl();
            this.nxtTankDrive = new Haden.NxtSharp.Controllers.NxtTankDrive(this.components);
            this.nxtBrick = new Haden.NxtSharp.Brick.NxtBrick(this.components);
            this.nxtMotorA = new Haden.NxtSharp.Motors.NxtMotor(this.components);
            this.nxtMotorB = new Haden.NxtSharp.Motors.NxtMotor(this.components);
            this.nxtMotorC = new Haden.NxtSharp.Motors.NxtMotor(this.components);
            this.nxtPressureSensor = new Haden.NxtSharp.Sensors.NxtPressureSensor(this.components);
            this.nxtLightSensor = new Haden.NxtSharp.Sensors.NxtLightSensor(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.nxtMotorControl3 = new Haden.NxtSharp.Controllers.NxtMotorControl();
            this.liftLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.rotateLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nxtMotorControl2 = new Haden.NxtSharp.Controllers.NxtMotorControl();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lightSeekingMotor = new Haden.NxtSharp.Controllers.NxtMotorControl();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.differenceLightSensorValue = new System.Windows.Forms.Label();
            this.peakLightSensorValue = new System.Windows.Forms.Label();
            this.lightSensorValueOut = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.sanityCheckBox = new System.Windows.Forms.CheckBox();
            this.dummyModeButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.disconnectBrickButton = new System.Windows.Forms.Button();
            this.connectBrickButton = new System.Windows.Forms.Button();
            this.featuresGroupBox = new System.Windows.Forms.GroupBox();
            this.speechPhraseTraceOutputBox = new System.Windows.Forms.TextBox();
            this.speakPhraseButton = new System.Windows.Forms.Button();
            this.loadPhraseFileButton = new System.Windows.Forms.Button();
            this.openSpeechFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autonomyFormButton = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.freshAutonomyButton = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.skratchFormButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.reportLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.sessionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.featuresGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // valLight
            // 
            this.valLight.Location = new System.Drawing.Point(104, 23);
            this.valLight.Name = "valLight";
            this.valLight.Size = new System.Drawing.Size(91, 13);
            this.valLight.Step = 5;
            this.valLight.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lastRememberedTextBox);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comPortSelectionBox);
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 408);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Environment exploration";
            // 
            // lastRememberedTextBox
            // 
            this.lastRememberedTextBox.Location = new System.Drawing.Point(206, 79);
            this.lastRememberedTextBox.Name = "lastRememberedTextBox";
            this.lastRememberedTextBox.Size = new System.Drawing.Size(68, 20);
            this.lastRememberedTextBox.TabIndex = 25;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(196, 56);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(88, 13);
            this.label15.TabIndex = 24;
            this.label15.Text = "Last remembered";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(206, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Port";
            // 
            // comPortSelectionBox
            // 
            this.comPortSelectionBox.FormattingEnabled = true;
            this.comPortSelectionBox.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM23",
            "COM40"});
            this.comPortSelectionBox.Location = new System.Drawing.Point(206, 32);
            this.comPortSelectionBox.Name = "comPortSelectionBox";
            this.comPortSelectionBox.Size = new System.Drawing.Size(68, 21);
            this.comPortSelectionBox.TabIndex = 22;
            this.comPortSelectionBox.SelectedIndexChanged += new System.EventHandler(this.comPortSelectionBox_SelectedIndexChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.simulatedAutonomyButton);
            this.groupBox9.Controls.Add(this.hadenAutonomousFormButton);
            this.groupBox9.Location = new System.Drawing.Point(185, 283);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox9.Size = new System.Drawing.Size(104, 111);
            this.groupBox9.TabIndex = 21;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Autonomous";
            // 
            // simulatedAutonomyButton
            // 
            this.simulatedAutonomyButton.Location = new System.Drawing.Point(14, 67);
            this.simulatedAutonomyButton.Name = "simulatedAutonomyButton";
            this.simulatedAutonomyButton.Size = new System.Drawing.Size(75, 23);
            this.simulatedAutonomyButton.TabIndex = 19;
            this.simulatedAutonomyButton.Text = "Simulator";
            this.simulatedAutonomyButton.UseVisualStyleBackColor = true;
            this.simulatedAutonomyButton.Click += new System.EventHandler(this.simulatedAutonomyButton_Click);
            // 
            // hadenAutonomousFormButton
            // 
            this.hadenAutonomousFormButton.Location = new System.Drawing.Point(21, 31);
            this.hadenAutonomousFormButton.Margin = new System.Windows.Forms.Padding(2);
            this.hadenAutonomousFormButton.Name = "hadenAutonomousFormButton";
            this.hadenAutonomousFormButton.Size = new System.Drawing.Size(68, 20);
            this.hadenAutonomousFormButton.TabIndex = 18;
            this.hadenAutonomousFormButton.Text = "Open form";
            this.hadenAutonomousFormButton.UseVisualStyleBackColor = true;
            this.hadenAutonomousFormButton.Click += new System.EventHandler(this.hadenAutonomousFormButton_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.nxtTankControl1);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Location = new System.Drawing.Point(0, 283);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(174, 110);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Move";
            // 
            // nxtTankControl1
            // 
            this.nxtTankControl1.Brake = true;
            this.nxtTankControl1.Location = new System.Drawing.Point(84, 16);
            this.nxtTankControl1.Name = "nxtTankControl1";
            this.nxtTankControl1.Power = 60;
            this.nxtTankControl1.Size = new System.Drawing.Size(73, 79);
            this.nxtTankControl1.TabIndex = 12;
            this.nxtTankControl1.TankDrive = this.nxtTankDrive;
            // 
            // nxtTankDrive
            // 
            this.nxtTankDrive.Brick = this.nxtBrick;
            this.nxtTankDrive.Motor1 = this.nxtMotorB;
            this.nxtTankDrive.Motor2 = this.nxtMotorC;
            // 
            // nxtBrick
            // 
            this.nxtBrick.AutoPoll = true;
            this.nxtBrick.ComPortName = "";
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
            this.nxtMotorA.Port = Haden.NxtSharp.Motors.NxtMotorPort.PortA;
            // 
            // nxtMotorB
            // 
            this.nxtMotorB.Brick = this.nxtBrick;
            this.nxtMotorB.Flip = true;
            this.nxtMotorB.Port = Haden.NxtSharp.Motors.NxtMotorPort.PortB;
            // 
            // nxtMotorC
            // 
            this.nxtMotorC.Brick = this.nxtBrick;
            this.nxtMotorC.Flip = true;
            this.nxtMotorC.Port = Haden.NxtSharp.Motors.NxtMotorPort.PortC;
            // 
            // nxtPressureSensor
            // 
            this.nxtPressureSensor.AutoPoll = true;
            this.nxtPressureSensor.AutoPollDelay = 100;
            this.nxtPressureSensor.Brick = this.nxtBrick;
            this.nxtPressureSensor.Port = Haden.NxtSharp.Sensors.NxtSensorPort.Port1;
            this.nxtPressureSensor.ValueChanged += new Haden.NxtSharp.Sensors.SensorEvent(this.NxtPressureSensorPolled);
            // 
            // nxtLightSensor
            // 
            this.nxtLightSensor.Active = false;
            this.nxtLightSensor.AutoPoll = true;
            this.nxtLightSensor.AutoPollDelay = 500;
            this.nxtLightSensor.Brick = this.nxtBrick;
            this.nxtLightSensor.Port = Haden.NxtSharp.Sensors.NxtSensorPort.Port3;
            this.nxtLightSensor.ValueChanged += new Haden.NxtSharp.Sensors.SensorEvent(this.NxtLightSensorValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Forward";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Reverse";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.nxtMotorControl3);
            this.groupBox5.Controls.Add(this.liftLabel);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.rotateLabel);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.nxtMotorControl2);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(27, 132);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(247, 144);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Negotiating tank";
            // 
            // nxtMotorControl3
            // 
            this.nxtMotorControl3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.nxtMotorControl3.Brake = false;
            this.nxtMotorControl3.ButtonDistance = 4;
            this.nxtMotorControl3.IsConnected = false;
            this.nxtMotorControl3.IsRunning = false;
            this.nxtMotorControl3.Key1 = System.Windows.Forms.Keys.None;
            this.nxtMotorControl3.Key2 = System.Windows.Forms.Keys.None;
            this.nxtMotorControl3.Location = new System.Drawing.Point(141, 39);
            this.nxtMotorControl3.Margin = new System.Windows.Forms.Padding(4);
            this.nxtMotorControl3.MinimumSize = new System.Drawing.Size(44, 60);
            this.nxtMotorControl3.Motor = this.nxtMotorC;
            this.nxtMotorControl3.Name = "nxtMotorControl3";
            this.nxtMotorControl3.Orientation = Haden.NxtSharp.Controllers.NxtMotorControlOrientation.Vertical;
            this.nxtMotorControl3.Power = 75;
            this.nxtMotorControl3.Size = new System.Drawing.Size(78, 69);
            this.nxtMotorControl3.TabIndex = 8;
            // 
            // liftLabel
            // 
            this.liftLabel.AutoSize = true;
            this.liftLabel.Location = new System.Drawing.Point(156, 22);
            this.liftLabel.Name = "liftLabel";
            this.liftLabel.Size = new System.Drawing.Size(49, 13);
            this.liftLabel.TabIndex = 4;
            this.liftLabel.Text = "CCW left";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(156, 112);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "CW right";
            // 
            // rotateLabel
            // 
            this.rotateLabel.AutoSize = true;
            this.rotateLabel.Location = new System.Drawing.Point(69, 22);
            this.rotateLabel.Name = "rotateLabel";
            this.rotateLabel.Size = new System.Drawing.Size(48, 13);
            this.rotateLabel.TabIndex = 5;
            this.rotateLabel.Text = "CW right";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Back";
            // 
            // nxtMotorControl2
            // 
            this.nxtMotorControl2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.nxtMotorControl2.Brake = false;
            this.nxtMotorControl2.ButtonDistance = 4;
            this.nxtMotorControl2.IsConnected = false;
            this.nxtMotorControl2.IsRunning = false;
            this.nxtMotorControl2.Key1 = System.Windows.Forms.Keys.None;
            this.nxtMotorControl2.Key2 = System.Windows.Forms.Keys.None;
            this.nxtMotorControl2.Location = new System.Drawing.Point(57, 39);
            this.nxtMotorControl2.Margin = new System.Windows.Forms.Padding(4);
            this.nxtMotorControl2.MinimumSize = new System.Drawing.Size(44, 60);
            this.nxtMotorControl2.Motor = this.nxtMotorB;
            this.nxtMotorControl2.Name = "nxtMotorControl2";
            this.nxtMotorControl2.Orientation = Haden.NxtSharp.Controllers.NxtMotorControlOrientation.Vertical;
            this.nxtMotorControl2.Power = 75;
            this.nxtMotorControl2.Size = new System.Drawing.Size(78, 69);
            this.nxtMotorControl2.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Ahead";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(69, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "CCW left";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lightSeekingMotor);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(27, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(155, 100);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Seek";
            // 
            // lightSeekingMotor
            // 
            this.lightSeekingMotor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.lightSeekingMotor.Brake = true;
            this.lightSeekingMotor.ButtonDistance = 4;
            this.lightSeekingMotor.IsConnected = false;
            this.lightSeekingMotor.IsRunning = false;
            this.lightSeekingMotor.Key1 = System.Windows.Forms.Keys.None;
            this.lightSeekingMotor.Key2 = System.Windows.Forms.Keys.None;
            this.lightSeekingMotor.Location = new System.Drawing.Point(52, 15);
            this.lightSeekingMotor.Margin = new System.Windows.Forms.Padding(4);
            this.lightSeekingMotor.MaximumSize = new System.Drawing.Size(78, 69);
            this.lightSeekingMotor.MinimumSize = new System.Drawing.Size(44, 60);
            this.lightSeekingMotor.Motor = this.nxtMotorA;
            this.lightSeekingMotor.Name = "lightSeekingMotor";
            this.lightSeekingMotor.Orientation = Haden.NxtSharp.Controllers.NxtMotorControlOrientation.Vertical;
            this.lightSeekingMotor.Power = 30;
            this.lightSeekingMotor.Size = new System.Drawing.Size(78, 69);
            this.lightSeekingMotor.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Right";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Left";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.differenceLightSensorValue);
            this.groupBox2.Controls.Add(this.peakLightSensorValue);
            this.groupBox2.Controls.Add(this.lightSensorValueOut);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.valLight);
            this.groupBox2.Location = new System.Drawing.Point(312, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 126);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scanning behavior";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 96);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(124, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "and current sensor value";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Difference between peak";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Peak seen light sensor value";
            // 
            // differenceLightSensorValue
            // 
            this.differenceLightSensorValue.AutoSize = true;
            this.differenceLightSensorValue.Location = new System.Drawing.Point(208, 86);
            this.differenceLightSensorValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.differenceLightSensorValue.Name = "differenceLightSensorValue";
            this.differenceLightSensorValue.Size = new System.Drawing.Size(13, 13);
            this.differenceLightSensorValue.TabIndex = 18;
            this.differenceLightSensorValue.Text = "d";
            // 
            // peakLightSensorValue
            // 
            this.peakLightSensorValue.AutoSize = true;
            this.peakLightSensorValue.Location = new System.Drawing.Point(208, 53);
            this.peakLightSensorValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.peakLightSensorValue.Name = "peakLightSensorValue";
            this.peakLightSensorValue.Size = new System.Drawing.Size(19, 13);
            this.peakLightSensorValue.TabIndex = 17;
            this.peakLightSensorValue.Text = "np";
            // 
            // lightSensorValueOut
            // 
            this.lightSensorValueOut.AutoSize = true;
            this.lightSensorValueOut.Location = new System.Drawing.Point(221, 23);
            this.lightSensorValueOut.Name = "lightSensorValueOut";
            this.lightSensorValueOut.Size = new System.Drawing.Size(34, 13);
            this.lightSensorValueOut.TabIndex = 15;
            this.lightSensorValueOut.Text = "Value";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Light sensor";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.sanityCheckBox);
            this.groupBox3.Controls.Add(this.dummyModeButton);
            this.groupBox3.Controls.Add(this.statusLabel);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.disconnectBrickButton);
            this.groupBox3.Controls.Add(this.connectBrickButton);
            this.groupBox3.Location = new System.Drawing.Point(312, 158);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(294, 99);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Admin";
            // 
            // sanityCheckBox
            // 
            this.sanityCheckBox.AutoSize = true;
            this.sanityCheckBox.Location = new System.Drawing.Point(211, 67);
            this.sanityCheckBox.Name = "sanityCheckBox";
            this.sanityCheckBox.Size = new System.Drawing.Size(61, 17);
            this.sanityCheckBox.TabIndex = 5;
            this.sanityCheckBox.Text = "Sanity?";
            this.sanityCheckBox.UseVisualStyleBackColor = true;
            // 
            // dummyModeButton
            // 
            this.dummyModeButton.Location = new System.Drawing.Point(194, 32);
            this.dummyModeButton.Margin = new System.Windows.Forms.Padding(2);
            this.dummyModeButton.Name = "dummyModeButton";
            this.dummyModeButton.Size = new System.Drawing.Size(76, 23);
            this.dummyModeButton.TabIndex = 4;
            this.dummyModeButton.Text = "Dummy";
            this.dummyModeButton.UseVisualStyleBackColor = true;
            this.dummyModeButton.Click += new System.EventHandler(this.DummyModeClick);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(56, 67);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 13);
            this.statusLabel.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Status:";
            // 
            // disconnectBrickButton
            // 
            this.disconnectBrickButton.Location = new System.Drawing.Point(89, 32);
            this.disconnectBrickButton.Name = "disconnectBrickButton";
            this.disconnectBrickButton.Size = new System.Drawing.Size(80, 23);
            this.disconnectBrickButton.TabIndex = 1;
            this.disconnectBrickButton.Text = "Disconnect";
            this.disconnectBrickButton.UseVisualStyleBackColor = true;
            this.disconnectBrickButton.Click += new System.EventHandler(this.DisconnectBrickButtonClick);
            // 
            // connectBrickButton
            // 
            this.connectBrickButton.Location = new System.Drawing.Point(12, 32);
            this.connectBrickButton.Name = "connectBrickButton";
            this.connectBrickButton.Size = new System.Drawing.Size(71, 23);
            this.connectBrickButton.TabIndex = 0;
            this.connectBrickButton.Text = "Connect";
            this.connectBrickButton.UseVisualStyleBackColor = true;
            this.connectBrickButton.Click += new System.EventHandler(this.AutonomousClick);
            // 
            // featuresGroupBox
            // 
            this.featuresGroupBox.Controls.Add(this.speechPhraseTraceOutputBox);
            this.featuresGroupBox.Controls.Add(this.speakPhraseButton);
            this.featuresGroupBox.Controls.Add(this.loadPhraseFileButton);
            this.featuresGroupBox.Location = new System.Drawing.Point(312, 263);
            this.featuresGroupBox.Name = "featuresGroupBox";
            this.featuresGroupBox.Size = new System.Drawing.Size(294, 115);
            this.featuresGroupBox.TabIndex = 14;
            this.featuresGroupBox.TabStop = false;
            this.featuresGroupBox.Text = "Voice";
            // 
            // speechPhraseTraceOutputBox
            // 
            this.speechPhraseTraceOutputBox.Location = new System.Drawing.Point(104, 19);
            this.speechPhraseTraceOutputBox.Multiline = true;
            this.speechPhraseTraceOutputBox.Name = "speechPhraseTraceOutputBox";
            this.speechPhraseTraceOutputBox.Size = new System.Drawing.Size(174, 90);
            this.speechPhraseTraceOutputBox.TabIndex = 4;
            // 
            // speakPhraseButton
            // 
            this.speakPhraseButton.Enabled = true;
            this.speakPhraseButton.Location = new System.Drawing.Point(6, 63);
            this.speakPhraseButton.Name = "speakPhraseButton";
            this.speakPhraseButton.Size = new System.Drawing.Size(80, 23);
            this.speakPhraseButton.TabIndex = 3;
            this.speakPhraseButton.Text = "Speak phrase";
            this.speakPhraseButton.UseVisualStyleBackColor = true;
            this.speakPhraseButton.Click += new System.EventHandler(this.speakPhraseButton_Click);
            // 
            // loadPhraseFileButton
            // 
            this.loadPhraseFileButton.Location = new System.Drawing.Point(6, 34);
            this.loadPhraseFileButton.Name = "loadPhraseFileButton";
            this.loadPhraseFileButton.Size = new System.Drawing.Size(80, 23);
            this.loadPhraseFileButton.TabIndex = 0;
            this.loadPhraseFileButton.Text = "Load phrases";
            this.loadPhraseFileButton.UseVisualStyleBackColor = true;
            this.loadPhraseFileButton.Click += new System.EventHandler(this.loadPhraseFileButton_Click);
            // 
            // openSpeechFileDialog
            // 
            this.openSpeechFileDialog.Filter = " Xml files (*.xml)|*.xml";
            this.openSpeechFileDialog.InitialDirectory = "c:\\aeonWorkingDirectory\\PROJECTS\\aeonFluxProjectDirectory\\haden\\speech\\";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(628, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.statusToolStripMenuItem.Text = "Status...";
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // autonomyFormButton
            // 
            this.autonomyFormButton.Location = new System.Drawing.Point(74, 16);
            this.autonomyFormButton.Margin = new System.Windows.Forms.Padding(2);
            this.autonomyFormButton.Name = "autonomyFormButton";
            this.autonomyFormButton.Size = new System.Drawing.Size(70, 20);
            this.autonomyFormButton.TabIndex = 16;
            this.autonomyFormButton.Text = "Open form";
            this.autonomyFormButton.UseVisualStyleBackColor = true;
            this.autonomyFormButton.Click += new System.EventHandler(this.autonomyFormButton_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.freshAutonomyButton);
            this.groupBox7.Controls.Add(this.autonomyFormButton);
            this.groupBox7.Location = new System.Drawing.Point(312, 384);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox7.Size = new System.Drawing.Size(148, 51);
            this.groupBox7.TabIndex = 17;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Autonomy Simulator";
            // 
            // freshAutonomyButton
            // 
            this.freshAutonomyButton.Location = new System.Drawing.Point(6, 17);
            this.freshAutonomyButton.Margin = new System.Windows.Forms.Padding(2);
            this.freshAutonomyButton.Name = "freshAutonomyButton";
            this.freshAutonomyButton.Size = new System.Drawing.Size(44, 19);
            this.freshAutonomyButton.TabIndex = 17;
            this.freshAutonomyButton.Text = "Fresh";
            this.freshAutonomyButton.UseVisualStyleBackColor = true;
            this.freshAutonomyButton.Click += new System.EventHandler(this.freshAutonomyButton_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.skratchFormButton);
            this.groupBox8.Location = new System.Drawing.Point(471, 384);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox8.Size = new System.Drawing.Size(135, 51);
            this.groupBox8.TabIndex = 18;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Mockup";
            // 
            // skratchFormButton
            // 
            this.skratchFormButton.Location = new System.Drawing.Point(51, 17);
            this.skratchFormButton.Margin = new System.Windows.Forms.Padding(2);
            this.skratchFormButton.Name = "skratchFormButton";
            this.skratchFormButton.Size = new System.Drawing.Size(68, 20);
            this.skratchFormButton.TabIndex = 17;
            this.skratchFormButton.Text = "Open form";
            this.skratchFormButton.UseVisualStyleBackColor = true;
            this.skratchFormButton.Click += new System.EventHandler(this.skratchFormButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportLabel,
            this.notifyLabel,
            this.sessionLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 436);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(628, 22);
            this.statusStrip.TabIndex = 19;
            this.statusStrip.Text = "statusStrip1";
            // 
            // reportLabel
            // 
            this.reportLabel.Name = "reportLabel";
            this.reportLabel.Size = new System.Drawing.Size(88, 17);
            this.reportLabel.Text = "Not started yet.";
            // 
            // notifyLabel
            // 
            this.notifyLabel.Name = "notifyLabel";
            this.notifyLabel.Size = new System.Drawing.Size(95, 17);
            this.notifyLabel.Text = "No notifications.";
            // 
            // sessionLabel
            // 
            this.sessionLabel.Name = "sessionLabel";
            this.sessionLabel.Size = new System.Drawing.Size(46, 17);
            this.sessionLabel.Text = "Session";
            // 
            // HadenManualControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 458);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.featuresGroupBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(644, 497);
            this.MinimumSize = new System.Drawing.Size(644, 497);
            this.Name = "HadenManualControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "haden remote";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.featuresGroupBox.ResumeLayout(false);
            this.featuresGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private NxtBrick nxtBrick;
        private NxtMotor nxtMotorB;
		private NxtMotor nxtMotorA;
        private NxtMotor nxtMotorC;
		private NxtLightSensor nxtLightSensor;
		private System.Windows.Forms.ProgressBar valLight;
		private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button disconnectBrickButton;
        private System.Windows.Forms.Button connectBrickButton;
        private System.Windows.Forms.Label rotateLabel;
        private System.Windows.Forms.Label liftLabel;
        private System.Windows.Forms.GroupBox featuresGroupBox;
        private System.Windows.Forms.Button loadPhraseFileButton;
        private System.Windows.Forms.OpenFileDialog openSpeechFileDialog;
        private System.Windows.Forms.Button speakPhraseButton;
        private NxtMotorControl nxtMotorControl3;
        private NxtMotorControl nxtMotorControl2;
        private NxtMotorControl lightSeekingMotor;
        private System.Windows.Forms.Label lightSensorValueOut;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button autonomyFormButton;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button dummyModeButton;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button skratchFormButton;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button hadenAutonomousFormButton;
        private System.Windows.Forms.Button freshAutonomyButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label differenceLightSensorValue;
        private System.Windows.Forms.Label peakLightSensorValue;
        private System.Windows.Forms.Label label14;
        private NxtTankDrive nxtTankDrive;
        private NxtTankControl nxtTankControl1;
        private System.Windows.Forms.Button simulatedAutonomyButton;
        private NxtPressureSensor nxtPressureSensor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comPortSelectionBox;
        private System.Windows.Forms.TextBox lastRememberedTextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel reportLabel;
        private System.Windows.Forms.ToolStripStatusLabel notifyLabel;
        private System.Windows.Forms.CheckBox sanityCheckBox;
        private System.Windows.Forms.TextBox speechPhraseTraceOutputBox;
        private System.Windows.Forms.ToolStripStatusLabel sessionLabel;
    }
}

