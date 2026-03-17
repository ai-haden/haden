using System;
using System.Globalization;
using System.Windows.Forms;
using Haden.NXTRemote.Properties;
using Haden.NxtSharp;
using Haden.NxtSharp.Sensors;
using Haden.NxtSharp.Utilties;
using SpeechLib;

namespace Haden.NXTRemote.Forms
{
    public partial class HadenAutonomousControl : Form
    {
        private Form _owner;
        private static bool _instance;
        public object BoxedLastTurn;
        public object BoxedLastSeek;
        public object BoxedCurrentValue;
        public object BoxedOptimalValue;
        public object BoxedDifferenceValue;
        public int Iterator;
        public int IteratorLimit;
        private readonly SpVoice _voice = new SpVoice();

        public HadenAutonomousControl(Form mOwner)
        {
            InitializeComponent();
            OwnerLocal = mOwner;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ControlBox = false;
            _instance = true;
            BoxedOptimalValue = 0;
            BoxedDifferenceValue = 0;
            BoxedCurrentValue = 0;
            BoxedLastTurn = "";
            BoxedLastSeek = "";
            Iterator = 0;
            IteratorLimit = 10;
        }

        public static bool Instance { get { return _instance; } set { _instance = value; } }
        public Form OwnerLocal { get { return _owner; } set { _owner = value; } }

        #region Nxt activities
        /// <summary>
        /// Captures the NXTs light sensor changed value.
        /// </summary>
        /// <param name="sensor">The sensor.</param>
        private void nxtLightSensor_ValueChanged(NxtSensor sensor)
        {
            Invoke((MethodInvoker)delegate
            {
                BoxedCurrentValue = (int)Functions.Clamp(nxtLightSensor.Value, 0, 100);
                lightSensorValueOut.Text = BoxedCurrentValue.ToString();
                for (int iterator = 0; iterator < IteratorLimit; iterator++)
                {
                    //CalculateSensorValueDifference();
                    CalculateLightSeen();
                }

            });
        }
        /// <summary>
        /// Captures the NXTs pressure sensor changed value.
        /// </summary>
        /// <param name="sensor">The sensor.</param>
        private void nxtPressureSensor_Polled(NxtSensor sensor)
        {
            Invoke((MethodInvoker)delegate
            {
                lblPressed.Text = nxtPressureSensor.IsPressed.ToString();
            });
        }
        #endregion

        // Autonomy routine list
        // 1. First choose whether to seek to the left or to the right,
        // 2. In turn, move in that direction.
        public void SeekLeftOrRight()
        {
            Random seekLeftRight = new Random();
            int leftRight = seekLeftRight.Next(1, 3);
            if (leftRight.Equals(1))
            {
                AutonomousSeek("left");
                BoxedLastSeek = "left";
            }
            else
            {
                AutonomousSeek("right");
                BoxedLastSeek = "right";
            }
        }
        public void TurnLeftOrRight()
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
        }
        protected void AutonomousTurn(string direction)
        {
            switch (direction)
            {
                case "left":
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        nxtTankDrive.TurnLeft(30, 70); }));
                    BoxedLastTurn = "left";
                    break;
                case "right":
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        nxtTankDrive.TurnRight(30, 70); }));
                    BoxedLastTurn = "right";
                    break;
            }
        }
        protected void AutonomousSeek(string direction)
        {
            switch (direction)
            {
                case "left":
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        nxtSensorDrive.TurnLeft(30, 10, true);
                    }));
                    BoxedLastTurn = "left";
                    break;
                case "right":
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        nxtSensorDrive.TurnRight(30, 10, true);
                    }));
                    BoxedLastTurn = "right";
                    break;
            }
        }
        protected void CalculateSensorValueDifference()
        {
            int differingValue = (int)BoxedCurrentValue - (int)BoxedOptimalValue;
            PostDifference();
            if (differingValue > 0)
            {
                BoxedOptimalValue = BoxedCurrentValue;
                optimalLightSensorValue.Text = BoxedOptimalValue.ToString();
                BoxedDifferenceValue = differingValue;
                // Turn in the same direction.
                if (BoxedLastTurn.ToString() == "left")
                    AutonomousTurn("left");
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
            }

        }
        protected void PostDifference()
        {
            int difference = Math.Abs((int)BoxedOptimalValue - (int)BoxedCurrentValue);
            differenceLightSensorValue.Text = difference.ToString(CultureInfo.InvariantCulture);
        }

        #region Autonomous optimal light seek routine

        // Query the current sensor value

        // Port the AutonomousTurn methods for the sensor motor

        // Compute when the optimal value is seen
        protected void CalculateLightSeen()
        {
            int differingValue = (int)BoxedCurrentValue - (int)BoxedOptimalValue;
            PostDifference();
            if (differingValue > 0)
            {
                BoxedOptimalValue = BoxedCurrentValue;
                optimalLightSensorValue.Text = BoxedOptimalValue.ToString();
                BoxedDifferenceValue = differingValue;
                // Turn in the same direction.
                if (BoxedLastSeek.ToString() == "left")
                    AutonomousSeek("left");
                if (BoxedLastSeek.ToString() == "right")
                    AutonomousSeek("right");
            }
            else if (differingValue < 0)
            {
                // Turn in the opposite direction.
                if (BoxedLastSeek.ToString() == "left")
                    AutonomousSeek("right");
                if (BoxedLastSeek.ToString() == "right")
                    AutonomousSeek("left");
            }
            else if (differingValue == 0)
            {
                // Switch between one to the left and one to the right.
                if (BoxedLastSeek.ToString() == "left")
                    AutonomousSeek("left");
                if (BoxedLastSeek.ToString() == "right")
                    AutonomousSeek("right");
                Iterator++;
            }
        }

        #endregion

        #region Autonomous motor drive to push optimal light seek routine



        #endregion

        #region Click Events
        private void connectBrickButton_Click(object sender, EventArgs e)
        {
            try
            {
                nxtBrick.Connect();
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.Generic_NXT_Brick_not_available, Resources.Generic_Click_Connection_error, MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                connectBrickButton.Enabled = false;
            }
        }
        private void closeForm_Click(object sender, EventArgs e)
        {
            nxtBrick.Disconnect();
            _voice.Speak("Disconnecting from haden.", SpeechVoiceSpeakFlags.SVSFDefault);
            Instance = false;
            Close();
        }
        private void resetOptimalValueButton_Click(object sender, EventArgs e)
        {
            BoxedOptimalValue = 0;
            optimalLightSensorValue.Text = Resources.Common__NumberZero;
        }
        private void beginAutonomy_Click(object sender, EventArgs e)
        {
            //_voice.Speak("Beginning autonomous runtime.", SpeechVoiceSpeakFlags.SVSFDefault);
            SeekLeftOrRight();
        }
        #endregion

    }
}
