using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Haden.NXTRemote.Controls;
using Haden.NXTRemote.Simulation.Drawing3D;
using Haden.NxtSharp.Sensors;
using Timer = System.Windows.Forms.Timer;

namespace Haden.NXTRemote.Forms.Child
{
    public partial class HadenAutonomySimulator : Form
    {
        private Form _owner;
        private static bool _instance;
        // Orientation
        //Bitmap[] _bmp = new Bitmap[6];
        //int i = 0;
        int cameraX = 0, cameraY = 0, cameraZ = 0;
        private int _cubeX;
        private int _cubeY;
        int cubeZ = 0;
        readonly Cuboid _station = new Cuboid(75, 75, 75);
        readonly Cuboid _robot = new Cuboid(50, 60, 45);
        readonly Camera _cam = new Camera();
        public EventHandler Seek;
        public EventHandler Nudge;
        //private const int Granularity = 1;
        //private bool IsPressed;
        private static int _currentValue;
        private static int _optimalValue;
        public object BoxedCurrentValue;
        public object BoxedOptimalValue = OptimalValue;
        public object BoxedLastTurn;
        public object BoxedDifferenceValue;
        protected int Iterator;
        protected int IteratorLimit;
        protected Timer PollingTimer;
        protected Pen Ray;
        protected Graphics RobotRay;
        protected Graphics StationRay;
        /// <summary>
        /// A public static rectangle determing the size of the game area.
        /// </summary>
        public static Rectangle GameArea;

        public HadenAutonomySimulator(Form mOwner)
        {
            InitializeComponent();
            _owner = mOwner;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ControlBox = false;
            _instance = true;
            BoxedDifferenceValue = 0;
            BoxedLastTurn = "";
            Iterator = 0;
            IteratorLimit = 100;

            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);	
        }

        private void nxtPressureSensor_Polled(NxtSensor sensor)
        {
            // The stop event.
        }

        protected void PollingTimer_OnTick(object sender, EventArgs e)
        {
            Random dummySensorValue = new Random();
            int leftRight = dummySensorValue.Next(35, 60);
            BoxedCurrentValue = leftRight;
            currentValueIndicator.Text = BoxedCurrentValue.ToString();
            // Commented since traps the value to zero.
            //while (iterator < iteratorLimit)
            //{
                CalculateSensorValueDifference();
            //}
        }

        private void nxtLightSensor_ValueChanged(NxtSensor sensor)
        {
            // The continuous event.
        }

        private void HadenAutonomySimulator_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 10000;
            toolTip.InitialDelay = 400;
            toolTip.ReshowDelay = 250;
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(iteratorValue, "This is the value of the iterator, when the difference between current and optimal sensor values is zero. Which means the light is seen.");
            toolTip.SetToolTip(currentValueIndicator, "This is returning the current light sensor value.");
            toolTip.SetToolTip(optimalValueIndicator, "This is returning the highest seen light sensor value.");
            toolTip.SetToolTip(differenceLightSensorValue, "This is the differenece between the two sensor values.");
            toolTip.SetToolTip(seekPowerBlock, "Start the seek autonomy event.");
            toolTip.SetToolTip(bumpIndicator, "Simulate the robot engaging the bump sensor--e.g., finding the station.");

            _cam.Location = new Point3D(400, 240, -500);
            CurrentValue = 0;
            BoxedCurrentValue = CurrentValue;
            currentValueIndicator.Text = BoxedCurrentValue.ToString();
            OptimalValue = 0;
            BoxedOptimalValue = OptimalValue;
            optimalValueIndicator.Text = BoxedOptimalValue.ToString();
            //DrawEnvironment();
        }

        #region Activities copied from the other form

        #region Autonomy test routines

        /// <summary>
        /// Calculates the sensor value difference.
        /// </summary>
        protected void CalculateSensorValueDifference()
        {
            int differingValue = (int)BoxedCurrentValue - (int)BoxedOptimalValue;
            if (differingValue > 0)
            {
                BoxedOptimalValue = BoxedCurrentValue;
                optimalValueIndicator.Text = BoxedOptimalValue.ToString();
                BoxedDifferenceValue = differingValue;
                differenceLightSensorValue.Text = BoxedDifferenceValue.ToString(); // s0
                // Turn in the same direction.
                if (BoxedLastTurn.ToString() == "left")
                    AutonomousTurn("left");
                // Move the simulated item--how to represent tracking?
                if (BoxedLastTurn.ToString() == "right")
                    AutonomousTurn("right");
            }
            else if (differingValue < 0)
            {
                // Turn in the opposite direction.
                if (BoxedLastTurn.ToString() == "left")
                    AutonomousTurn("right");
                if (BoxedLastTurn.ToString() == "right")
                    AutonomousTurn("left");
            }
            else if (differingValue == 0)
            {
                // Switch between one to the left and one to the right.
                if (BoxedLastTurn.ToString() == "left")
                    AutonomousTurn("left");
                if (BoxedLastTurn.ToString() == "right")
                    AutonomousTurn("right");
                Iterator++;
                iteratorValue.Text = Iterator.ToString(CultureInfo.InvariantCulture);
            }

        }
        /// <summary>
        /// Turns the motor autonomously.
        /// </summary>
        /// <param name="direction">The direction to turn.</param>
        /// <remarks>Can be extended to include other motor controls.</remarks>
        protected void AutonomousTurn(string direction)
        {
            switch (direction)
            {
                case "left":
                    seekLeftIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
                    seekLeftIndicator.FlashNumber = 2;
                    BoxedLastTurn = "left";
                    // Move items in the environment.
                    CubeX += 5;
                    Quaternion qx = new Quaternion();
                    qx.FromAxisAngle(new Vector3D(0, 0, 1), 5 * Math.PI / 180.0);
                    _robot.RotateAt(_robot.CenterPoint, qx);
                    Invalidate();
                    Thread.Sleep(250);
                    break;
                case "right":
                    seekRightIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
                    seekRightIndicator.FlashNumber = 2;
                    BoxedLastTurn = "right";
                    // Move items in the environment
                    CubeY += 5;
                    Quaternion qy = new Quaternion();
                    qy.FromAxisAngle(new Vector3D(0, 0, 1), -5 * Math.PI / 180.0);
                    _robot.RotateAt(_robot.CenterPoint, qy);
                    Invalidate();
                    Thread.Sleep(250);
                    break;
            }
            
            //Ray = new Pen(Color.Red);
            //Graphics formGraphics = CreateGraphics();
            //formGraphics.DrawLine(Ray, 0, 0, 200, 200);
            //Invalidate();
            //Ray.Dispose();
            //formGraphics.Dispose();
        }

        #endregion

        #region Coin flip (FOR THE FIRST TIME RUNNING--AN INITIAL CONDITION ONLY!)

        /// <summary>
        /// Seeks to the left or right using a random number generator.
        /// </summary>
        public void SeekLeftOrRight()
        {
            Random seekLeftRight = new Random();
            int leftRight = seekLeftRight.Next(1, 3);
            if (leftRight.Equals(1))
            {
                AutonomousTurn("left");
                BoxedLastTurn = "left";
            }
            else
            {
                AutonomousTurn("right");
                BoxedLastTurn = "right";
            }
            PollingTimer = new Timer();
            PollingTimer.Interval = 1000;
            PollingTimer.Start();
            PollingTimer.Tick += PollingTimer_OnTick;
        }

        #endregion

        #endregion

        #region Seek activities

        public void SeekLeft(int granularity)
        {
            seekLeftIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
            seekLeftIndicator.FlashNumber = 3;
            commandOutputTextBox.Text += "Seeking to the left.";
            commandOutputTextBox.Text += "\r\n";
            // Set sensor value
            SetCurrentLightSensorValue();

            // Move objects in the simulator.
            CubeX += 5;
            Quaternion q = new Quaternion();
            q.FromAxisAngle(new Vector3D(1, 0, 0), 5 * Math.PI / 180.0);
            _robot.RotateAt(_robot.CenterPoint, q);
        }

        public void SeekRight(int granularity)
        {
            seekRightIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
            seekRightIndicator.FlashNumber = 5;
            commandOutputTextBox.Text += "Seeking to the right.";
            commandOutputTextBox.Text += "\r\n";
            // Set sensor value
            SetCurrentLightSensorValue();

            // Move objects in the simulator.
            CubeY += 7;
            Quaternion q = new Quaternion();
            q.FromAxisAngle(new Vector3D(1, 1, 0), 5 * Math.PI / 180.0);
            _robot.RotateAt(_robot.CenterPoint, q);
        }

        #endregion

        #region Negotiate activities

        public void MoveAheadCwRight()
        {
            aheadClockwiseRightIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
            aheadClockwiseRightIndicator.FlashNumber = 5;
        }

        public void MoveAheadCcwLeft()
        {
            aheadCounterClockwiseLeftIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
            aheadCounterClockwiseLeftIndicator.FlashNumber = 5;
        }

        public void MoveBackCcwLeft()
        {
            backCounterClockwiseLeftIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
            backCounterClockwiseLeftIndicator.FlashNumber = 5;
        }

        public void MoveBackCwRight()
        {
            backClockwiseRightIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
            backClockwiseRightIndicator.FlashNumber = 5;
        }

        public void Pause(int time)
        {
            //Thread.Sleep(time);
        }

        #endregion

        #region Move activities

        public void MoveReverse()
        {
            moveReverseIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
            moveReverseIndicator.FlashNumber = 5;
        }

        public void MoveForward()
        {
            moveForwardIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
            moveForwardIndicator.FlashNumber = 5;
        }

        #endregion

        #region SIMULATOR STARTS HERE

        private void seekPowerBlock_Click(object sender, EventArgs e)
        {
            DrawEnvironment();
            commandOutputTextBox.Text = "Seek power algorithm started.";
            commandOutputTextBox.Text += "\r\n";
            SeekLeftOrRight();
            //HandleTargeting();
        }
        // needs to be an event if used alone
        protected void DrawEnvironment()
        {
            Ray = new Pen(Color.Blue);
            _robot.CenterPoint = new Point3D(400, 200, 50);
            RobotRay = CreateGraphics();
            RobotRay.DrawLine(Ray, 0, 0, 400, 240);
            _station.CenterPoint = new Point3D(100, 60, 40);
            StationRay = CreateGraphics();
            StationRay.DrawLine(Ray, 0, 100, 60, 40);
            Invalidate();
        }

        #endregion

        #region Coin flip (FOR THE FIRST TIME RUNNING--AN INITIAL CONDITION ONLY!)

        //public void SeekLeftOrRight(int granularity)
        //{
        //    Random seekLeftRight = new Random();
        //    int leftRight = seekLeftRight.Next(1, 3);
        //    if (leftRight.Equals(1))
        //    {
        //        SeekLeft(granularity);
        //    }
        //    else
        //    {
        //        SeekRight(granularity);
        //    }
        //}

        #endregion

        #region Computational operations

        protected void SetCurrentLightSensorValue()
        {
            CurrentValue = 0;
            BoxedCurrentValue = CurrentValue;
            currentValueIndicator.Text = BoxedCurrentValue.ToString();
            CalculateSensorValueDifference();
        }

        //protected void CalculateSensorValueDifference()
        //{
        //    int differingValue = (int)BoxedCurrentValue - (int)BoxedPeakValue;
        //    if (differingValue > 0)
        //    {
        //        BoxedPeakValue = CurrentValue;
        //        optimalValueIndicator.Text = BoxedPeakValue.ToString();
        //    }
        //}

        #endregion

        #region Windows forms and public properties

        /// <summary>
        /// Checks the instance of the form.
        /// </summary>
        public static bool Instance { get { return _instance; } set { _instance = value; } }
        public static int CurrentValue { get { return _currentValue; } set { _currentValue = value; } }
        public static int OptimalValue { get { return _optimalValue; } set { _optimalValue = value; } }

        public int CubeY
        {
            get { return _cubeY; }
            set { _cubeY = value; }
        }

        public int CubeX
        {
            get { return _cubeX; }
            set { _cubeX = value; }
        }

        private void closeFormButton_Click(object sender, EventArgs e)
        {
            Instance = false;
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _robot.Draw(e.Graphics, _cam);
            _station.Draw(e.Graphics, _cam);
            base.OnPaint(e);
        }

        #endregion

        #region Seek Events (NOT USED)

        public virtual void OnSeek(EventArgs e, string direction)
        {
            if (Seek != null)
                Seek(this, e);
            switch (direction)
            {
                case "left":
                    
                    break;
                case "right":
                    
                    break;
            }

        }

        #endregion

        private void bumpIndicator_Click(object sender, EventArgs e)
        {
            // Simulates true at the bump sensor--finding the station.
            commandOutputTextBox.Text = "Bump indicator triggered.";
            commandOutputTextBox.Text += "\r\n";
            PollingTimer.Stop();
            PollingTimer.Tick -= PollingTimer_OnTick;
        }
    }
}
