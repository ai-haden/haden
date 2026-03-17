using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Haden.NXTRemote.Forms.Child;
using Haden.NXTRemote.Forms.Simulation;
using Haden.NXTRemote.Properties;
using Haden.NxtSharp;
using Haden.NxtSharp.Sensors;
using Haden.NxtSharp.Utilties;
using Haden.Library;
using SpeechLib;
using Controller = Haden.NXTRemote.Data.Controller;
using System.IO;

namespace Haden.NXTRemote.Forms
{
    /// <summary>
    /// The instance of the Haden manual control form.
    /// </summary>
	public partial class HadenManualControl : Form
    {
        private System.Timers.Timer _whirlTimer;
        private Controller _controller;

        public Session Session { get; }
        public SettingsDictionary GlobalSettings { get; set; }
        private HadenCore Aeon { get; set; }
        public DesiredAttribute DesiredAttribute { get; set; }

        /// <summary>
        /// An instance of haden's voice.
        /// </summary>
        private readonly SpVoice _voice = new SpVoice();
        private bool WhirlActive {  get; set; } 
        private bool VoiceSpoken { get; set; }
        /// <summary>
        /// The peak light sensor value, called np. The highest value ever seen.
        /// </summary>
        public object BoxedPeakValue;
        /// <summary>
        /// The current light sensor value, called nc.
        /// </summary>
        public object BoxedCurrentValue;
        /// <summary>
        /// The stored value for difference between current and optimal light sensor values, called d.
        /// </summary>
        public object BoxedDifferenceValue;
        /// <summary>
        /// The stored value for the last turn. The depth of primitive memory is now n-1.
        /// </summary>
        public string LastTurn;
        /// <summary>
        /// The number of degrees to turn a motor.
        /// </summary>
        /// <value>
        /// The last turn granularity.
        /// </value>
        public int MotorTurnGranularity { get; set; }
        /// <summary>
        /// The stored value for an interation to define the number of autonomous iterations of a function before pausing.
        /// </summary>
        public int Iteration;
        /// <summary>
        /// The iteration limit.
        /// </summary>
        public int InterationLimit;
        /// <summary>
        /// The n - 1 values.
        /// </summary>
        public List<double> Then { get; set; }
        /// <summary>
        /// The n + 1, n + 2, ..., nmax value.
        /// </summary>
        public double Now { get; set; }
        /// <summary>
        /// The n, np value.
        /// </summary>
        public double CurrentValue { get; set; }
        public bool GreaterNow { get; set; }
        public bool LessNow { get; set; }
        public double PeakLightValue { get; set; }
        //public List<double> LightValuesSeen { get; set; }
        public List<string> TurnsMade{ get; set; }
        public int Reward { get; private set; }

        // Velocity control variables.
        public const int PowerDrive = 30;
        public const int PowerSeek = 20;
        public const int Degrees = 30;
        // Know if connected to a brick and if autonomously or dummy.
        public bool IsConnectedBrickAutonomous = false;
        public bool IsConnectedBrickDummy = false;
        /// <summary>Initializes a new instance of the <see cref="HadenManualControl" /> class.</summary>
		public HadenManualControl()
        {
            InitializeComponent();
            //speechPhraseTraceOutputBox.Text = "stuff appearing here  *& ^% ";
            GlobalSettings = new SettingsDictionary(); // < -- NEXT SESSION HERE
            LoadSettings();
            // Create a new named A.I. core.
            Aeon = new HadenCore { DesiredAttribute = DesiredAttribute.None };
            BoxedPeakValue = 0;
            BoxedDifferenceValue = 0;
            BoxedCurrentValue = 0;
            LastTurn = "";
            Iteration = 0;
            InterationLimit = 10;
            MotorTurnGranularity = 20;
            disconnectBrickButton.Enabled = false;
            KeyPreview = true;
            comPortSelectionBox.Text = "COM7";
            //LightValuesSeen = new List<double>();
            TurnsMade = new List<string>();
            Then = new List<double>();
            _controller = new Controller();
            Session = new Session() { };
            sessionLabel.Text = "Session: " + Session.SessionID;
            //BeginToRueTheWhirl(Aeon.WhirlDuration);
        }

        #region Methods
        /// <summary>
        /// Loads settings based upon the default location of the Settings.xml file
        /// </summary>
        public void LoadSettings()
        {
            // Try a default setup using the Settings.xml file.
            string path = Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "Settings.xml"));
            LoadSettings(path);
        }
        /// <summary>
        /// Loads settings and configuration info from various xml files referenced in the settings file passed in the args. Also generates some default values if such values have not been set by the settings file.
        /// </summary>
        /// <param name="pathToSettings">Path to the settings xml file</param>
        public void LoadSettings(string pathToSettings)
        {
            GlobalSettings.LoadSettings(pathToSettings);

            // Checks for some important default settings.
            if (!GlobalSettings.ContainsSettingCalled("trainingData"))
            {
                GlobalSettings.AddSetting("trainingData", "exponent");
            }
            if (!GlobalSettings.ContainsSettingCalled("iterations"))
            {
                GlobalSettings.AddSetting("iterations", "1000");
            }
            if (!GlobalSettings.ContainsSettingCalled("momentum"))
            {
                GlobalSettings.AddSetting("momentum", "0");
            }
            if (!GlobalSettings.ContainsSettingCalled("learningRate"))
            {
                GlobalSettings.AddSetting("learningRate", "0.1");
            }
            if (!GlobalSettings.ContainsSettingCalled("sigmoidAlpha"))
            {
                GlobalSettings.AddSetting("sigmoidAlpha", "2.0");
            }
            if (!GlobalSettings.ContainsSettingCalled("windowSize"))
            {
                GlobalSettings.AddSetting("windowSize", "5");
            }
            if (!GlobalSettings.ContainsSettingCalled("predictionSize"))
            {
                GlobalSettings.AddSetting("predictionSize", "1");
            }
            if (!GlobalSettings.ContainsSettingCalled("logfile"))
            {
                GlobalSettings.AddSetting("logfile", "logs");
            }

        }
        /// <summary>
        /// Loads the data from a file.
        /// </summary>
        /// <summary>
        /// Parses the Xml document.
        /// </summary>
        protected void ParseXmlDocument(XmlDocument input)
        {
            XmlNodeList nodeList = input.SelectNodes("GRAMMAR/RULE/L/P");

            if (nodeList != null)
                foreach (XmlNode speechTexts in nodeList)
                {
                    //speechListBox.Items.Add(speechTexts.LastChild.InnerText);
                }
        }
        #endregion


        #region The Novel Whirl
        protected void BeginToRueTheWhirl(double duration)
        {
            _whirlTimer = new System.Timers.Timer
            {
                Interval = duration // Duration of time in each state (in ms)
            };
            _whirlTimer.Elapsed += WhirlTimerElapsed;
            _whirlTimer.Start(); // Start timer.
            WhirlActive = true;
            RueTheWhirl.CurrentState = "Zero"; // Start the whirl.
            reportLabel.Text = RueTheWhirl.ActionController(); // Set the action for the whirl.
        }
        protected void WhirlTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            switch (RueTheWhirl.CurrentState)
            {
                case "Zero":
                    RueTheWhirl.CurrentState = "One";
                    reportLabel.Text = RueTheWhirl.ActionController();
                    break;
                case "One":
                    RueTheWhirl.CurrentState = "Two";
                    reportLabel.Text = RueTheWhirl.ActionController();
                    break;
                case "Two":
                    RueTheWhirl.CurrentState = "Three";
                    reportLabel.Text = RueTheWhirl.ActionController();
                    break;
                case "Three":
                    RueTheWhirl.CurrentState = "Four";
                    reportLabel.Text = RueTheWhirl.ActionController();
                    break;
                case "Four":
                    RueTheWhirl.CurrentState = "Zero";
                    reportLabel.Text = RueTheWhirl.ActionController();
                    break;
                default:
                    break;
            }

            if (RueTheWhirl.ActionController().Contains("Discover") | RueTheWhirl.ActionController() == "Discover the peak value detected at a light sensor.")
            {
                // At those points in the whirl-cycle, we need a routine that brings life to this old toy robot.
                // Based on the form of two wheel motors, one motor turning a light sensor through a range of 150-degrees, and a bump sensor.
                //
                // Tune to the changing value of light intensity.
                // Better-yet: Follow a lamp left or right, as it is the brightest source.
                //SensorTemporalValueDifference();
                // Process taxonomy.
                //ProcessTaxonomy();
                // Start as a find of a peek.
                PeekValue();
                // Disturb equilibrium.
                //IncrementLeft();
                // Check the connection to the robot.
                //QueryConnection();
            }
        }

        #endregion

        #region Sanity Checks

        protected void CheckMotorMovements()
        {
            //motorDelegate = BeginInvoke(new MethodInvoker(() => lightSeekingMotor.TurnCounterClockwise(25, 100)));
            //motorDelegate = BeginInvoke(new MethodInvoker(() => nxtMotorControl1.TurnClockwise(PowerSeek, 400)));
            nxtBrick.MotorA.Turn(25, 100);
            Thread.Sleep(1000);
            nxtBrick.MotorA.Flip = true;
            nxtBrick.MotorA.Turn(25, 100);
            lightSeekingMotor.TurnCounterClockwise(25, 100);
            Thread.Sleep(1000);
            //nxtBrick.MotorB.Turn(75, 1800);
            //nxtBrick.MotorC.Turn(75, 1800);
        }

        protected void CheckDataSystem()
        {
            Controller.ReadData();
            _controller.StoreData("left", 3, Iteration);
            Controller.ReadData();
        }

        #endregion

        #region Actions
        protected void QueryConnection()
        {
            if (IsConnectedBrickAutonomous == false)
                notifyLabel.Text = "No hardware detected in autonomous mode.";
            if (IsConnectedBrickAutonomous == true)
                notifyLabel.Text = "Interaction mode -- no notifications.";
            //if (IsConnectedBrickDummy == false)
            // notifyLabel.Text = "No hardware detected in dummy mode.";
        }
        /// <summary>
        /// Peek the value of the sensor when iteration is equal to zero.
        /// </summary>
        /// <returns></returns>
        protected bool PeekValue()
        {
            // Store the trace of where this begins.
            StoreTrace("Current", BoxedCurrentValue.ToString());
            StoreTrace("Highest-seen", BoxedPeakValue.ToString());
            // Set iteration to coincide with the starting temporal value of n.
            if (Iteration == 0)
            {
                StoreTrace("Iteration", Iteration.ToString()); 
                // Poll the n value and assign ast the current value.
                Now = Convert.ToDouble(BoxedCurrentValue);
                StoreTrace("INFO", "Set Current to Now");
                CurrentValue = Now;
                StoreTrace("Now", Now.ToString());
                StoreTrace("Current", CurrentValue.ToString());
                StoreTrace("INFO", "Append to Then list");
                Then.Add(Now);
                // The disturb routine.
                if (peakLightSensorValue.Text == "np")
                {
                    // Purposefully distrub this first equilibrium to set the system into motion.
                    CurrentValue = 10;
                    // Update the form.
                    BeginInvoke(new MethodInvoker(() => peakLightSensorValue.Text = Now.ToString())); // np
                    Compare();
                }
                // Increment the iteration value (for data-parity with count).
                Iteration++;
                
                return true;
            }
            if (Iteration == 1)
            {
                StoreTrace("Iteration", Iteration.ToString());
                // Poll the n value.
                Now = Convert.ToDouble(BoxedCurrentValue);
                if (Now != CurrentValue)
                {
                    DanceRoutine();
                    Iteration++;
                }
                return true;
            }
            if (Iteration != 0 | Iteration != 1)
            {
                // Increment the iteration value (for data-parity with count).
                StoreTrace("Iteration", Iteration.ToString());
                // Poll the n + 1 value.
                Now = Convert.ToDouble(BoxedCurrentValue);
                Compare();
                // Report to trace.
                StoreTrace("Now", Now.ToString());
                // Print the n value.
                StoreTrace("Current", CurrentValue.ToString());
                // Print the np value.
                StoreTrace("Peak", PeakLightValue.ToString());
                // Add it to the list.
                Then.Add(Now);
                StoreTrace("Now", Now.ToString());
                for (int i = 0; i < Then.Count; i++)
                    StoreTrace("Then" + i, Then[i].ToString());
                StoreTrace("Iteration", Iteration.ToString());
                Iteration++;
                return true;
            }
            return false;
        }
        void Buzz()
        {
            if (LastTurn == "left")
            {
                IncrementRight(MotorTurnGranularity);
                StoreTrace("last turn", LastTurn);
            }
            if (LastTurn == "right")
            {
                IncrementLeft(MotorTurnGranularity);
                StoreTrace("last turn", LastTurn);
            }
        }
        public void DanceRoutine()
        {
            // Check if a value of n is greater that current and if is the brightest seen.
            // Which way to turn to see if we can find a brighter light?
            if (Now > CurrentValue)
            {
                if (Now > PeakLightValue)
                {
                    PeakLightValue = Now;
                    // You know you are at the peak. Take your reward.
                    StoreTrace("INFO", "Taking my reward and a rest on my next whirl.");
                    Reward++;
                }
            }
            if (Now < CurrentValue)
            {
                Buzz();
                // Check the state of Now.
                Now = (int)BoxedCurrentValue;
                if (Now < CurrentValue)
                {
                    Buzz();
                }
                if (Now > CurrentValue)
                {
                    Buzz();
                }
            }
            if (PeakLightValue == Now)
            {
                for (int i = 0; i < Then.Count; i++)
                    StoreTrace("Then", Then[i].ToString());
            }
            // Run a comparison and decide.
            Compare();
        }
        /// <summary>
        /// Compares temporal quantities and sets the properties for booleans GreaterNow and LessNow.
        /// </summary>
        void Compare()
        {
            if (CurrentValue == 0 && Now == 0)
                return;
            StoreTrace("PROCESS", "Compare if 'n + 1' is greater or less than 'n'");
            if (CurrentValue.IsGreaterThan(Now))// Means you have in the past seen a higher value.
            {
                // If LastTurn is "", then complain and seed a value.
                if (LastTurn == "")
                {
                    StoreTrace("STUCK", "Last turn is empty, so I am stuck. Triggering a turn.");
                    IncrementLeft(30);
                    Compare();
                }
                IncrementLeft(30);
                GreaterNow = false;
                LessNow = true;
                StoreTrace("Greater State?", GreaterNow.ToString());
                StoreTrace("Lesser State?", LessNow.ToString());
                Compare();
            }
            if (CurrentValue.IsLessThan(Now)) // Means you are now seeing a higher value.
            {
                IncrementRight(30);
                GreaterNow = true;
                LessNow = false;
                StoreTrace("Greater State?", GreaterNow.ToString());
                StoreTrace("Lesser State?", LessNow.ToString());
            }
            if (Now > PeakLightValue)// If now is bigly, pass it to the peak seen value.
            {
                PeakLightValue = Now;
                StoreTrace("Peak value seen", PeakLightValue.ToString());
            }
            if (Now == PeakLightValue)
            {
                StoreTrace("Peak value seen", PeakLightValue.ToString());
                StoreTrace("INFO", "Taking my reward and a rest on my next whirl.");
                Reward++;
            }
            // Compute and display the difference.
            var diff = Math.Abs(CurrentValue - Now);
            BeginInvoke(new MethodInvoker(() => differenceLightSensorValue.Text = diff.ToString())); // d
            BeginInvoke(new MethodInvoker(() => Refresh()));

        }
        void Act(string action)
        {
            StoreTrace("Action", "Set for subsequent following Compare");
            if (action == "apriori")
            {
                StoreTrace("Action", "An apriori statement discovered.");
                // U can code it here...
                if (LastTurn == "left")
                {
                    IncrementLeft(30);
                    // What was the algorithm again? You're looking at it, buddy.
                }
            }
            if (action == "aposteriori")
            {
                StoreTrace("Action", "An aposteriori statement discovered.");
                // U can code it here...
            }
            else
                StoreTrace("Action", "Meaningless statement applied.");

        }

        void Sample()
        {
            // The n + x value. Add it to the list.
            Now = Convert.ToDouble(BoxedCurrentValue);
            Then.Add(Now);
            Iteration++;
            Compare();
        }

        /// <summary>
        /// On retrospection: How effective was the increment-turn to finding the peak light source direction?
        /// </summary>
        /// <returns></returns>
        protected bool DeterminePeakValue()
        {
            // Increment to seek of n.
            if (Iteration == 0)
            {
                PeekValue();
            }
            // Compute from the values reported on the form.

            // Increment to seek of n + 1.
            if (Iteration != 1)
            {
                IncrementLeft(MotorTurnGranularity);
            }
            // Is the n + 1 value greater to less than the than the n - 1 value?
            GreaterNow = Now.IsGreaterThan(CurrentValue);
            LessNow = Now.IsLessThan(CurrentValue);

            if (GreaterNow)
            {
                // Check in the same direction, since it was a positive result.
                Iteration++;
                IncrementLeft(MotorTurnGranularity);
                // Check now.
                // The n + 2 value. Add it to the list.
                Now = Convert.ToDouble(BoxedCurrentValue);
                Then.Add(Now);
                GreaterNow = Now.IsGreaterThan(CurrentValue);
                LessNow = Now.IsLessThan(CurrentValue);
                // Is it still increasing? Increment again.
                if (GreaterNow)
                {
                    Iteration++;
                    IncrementLeft(MotorTurnGranularity);
                    Recompute(true);
                }
                if (LessNow)
                {
                    Iteration++;
                    IncrementRight(MotorTurnGranularity);
                    Recompute(true);
                }
            }
            if (LessNow)
            {
                Iteration++;
                IncrementRight(MotorTurnGranularity);
                Recompute(true);
            }
            if (GreaterNow == false & LessNow == false)
            {
                Recompute(true);
            }
            return true;
        }
        protected bool Recompute(bool finalCheck)
        {
            if (finalCheck)
            {
                // The n + 3...np value. Add it to the list.
                Now = Convert.ToDouble(BoxedCurrentValue);
                Then.Add(Now);
                // Check now.
                GreaterNow = Now.IsGreaterThan(CurrentValue);
                LessNow = Now.IsLessThan(CurrentValue);
                if (GreaterNow == false && LessNow == false)
                {
                    if (Then.Equals(Now))
                    {
                        // You are finished.
                        notifyLabel.Text = "Peak has been determined and sensor points toward the lightsource.";
                        // But maybe this is not true. Let's disturb the equilibirum point just discovered.
                        IncrementLeft(MotorTurnGranularity);
                    }
                }
                else
                {
                    // Re-enter the loop.
                    DeterminePeakValue();
                }
                return true;
            }
            return false;
        }

        protected void ProcessTaxonomy()
        {
            if (SensorTemporalValueDifference() > 0)
            {
                BoxedPeakValue = BoxedCurrentValue; // nc
                BeginInvoke(new MethodInvoker(() => peakLightSensorValue.Text = BoxedPeakValue.ToString())); // np
                BoxedDifferenceValue = SensorTemporalValueDifference().ToString(CultureInfo.InvariantCulture);
                BeginInvoke(new MethodInvoker(() => differenceLightSensorValue.Text = BoxedDifferenceValue.ToString())); // d
                // Turn in the same direction.
                if (LastTurn.ToString() == "left")
                {
                    IncrementLeft(MotorTurnGranularity);
                }
                if (LastTurn.ToString() == "right")
                {
                    IncrementRight(MotorTurnGranularity);
                }
            }
            else if (SensorTemporalValueDifference() < 0)
            {
                BeginInvoke(new MethodInvoker(() => differenceLightSensorValue.Text = SensorTemporalValueDifference().ToString(CultureInfo.InvariantCulture)));
                // Turn in the opposite direction.
                if (LastTurn.ToString() == "left")
                {
                    IncrementRight(MotorTurnGranularity);
                }
                if (LastTurn.ToString() == "right")
                {
                    IncrementLeft(MotorTurnGranularity);
                }
            }
            else if (SensorTemporalValueDifference() == 0)
            {
                BeginInvoke(new MethodInvoker(() => differenceLightSensorValue.Text = SensorTemporalValueDifference().ToString(CultureInfo.InvariantCulture)));
                
                // Display a successful end to the discovery routine. Q: What is the response gesture?
            }
        }

        #endregion

        #region Autonomy test routines

        protected int SensorTemporalValueDifference()
        {
            BoxedDifferenceValue = (int)BoxedCurrentValue - (int)BoxedPeakValue;
            if ((int)BoxedCurrentValue > (int)BoxedPeakValue)
                PeakLightValue = (int)BoxedCurrentValue;
            return (int)BoxedDifferenceValue;
        }
        /// <summary>
        /// Increments the motor to the left.
        /// </summary>
        protected void IncrementLeft(int degrees)
        {
            try
            {
                nxtBrick.MotorA.Flip = false;
                nxtBrick.MotorA.Turn(25, degrees);
                LastTurn = "left";
                TurnsMade.Add(LastTurn);
                _controller.StoreData(LastTurn, TurnsMade.Count, Iteration);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.Motor);
            }
        }
        /// <summary>
        /// Increments the motor to the right.
        /// </summary>
        protected void IncrementRight(int degrees)
        {
            try
            {
                //nxtBrick.MotorA.Flip = true;
                nxtBrick.MotorA.Turn(-25, degrees);
                LastTurn = "right";
                TurnsMade.Add(LastTurn);
                _controller.StoreData(LastTurn, TurnsMade.Count, Iteration);
                Thread.Sleep(1000);
                nxtBrick.MotorA.Flip = false;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.Motor);
            }
        }
        /// <summary>
        /// Turns a motor one-tick autonomously.
        /// </summary>
        /// <param name="direction">The direction to turn.</param>
        /// <remarks>Can be extended to include other motor controls.</remarks>
        protected void TurnOneClick(string direction)
        {
            switch (direction)
            {
                case "left":
                    BeginInvoke(new MethodInvoker(() => lightSeekingMotor.buttonTurnCounterClockwise.PerformClick()));
                    LastTurn = "left";
                    break;
                case "right":
                    BeginInvoke(new MethodInvoker(() => lightSeekingMotor.buttonTurnClockwise.PerformClick()));
                    LastTurn = "right";
                    break;
            }
        }

        private void NxtMotorPositionChanged(NxtSensor sensor)
        {
            Invoke((MethodInvoker)delegate
            {
                BoxedCurrentValue = (int)Functions.Clamp(nxtLightSensor.Value, 0, 100);
                valLight.Value = (int)BoxedCurrentValue;
                lightSensorValueOut.Text = nxtLightSensor.Value.ToString(CultureInfo.InvariantCulture);
                SensorTemporalValueDifference();
            });
            if (WhirlActive)
            {
                // Add logic when haden responds to a light event and is intelligent.

                var hold = 0;
            }
        }

        private void NxtLightSensorValueChanged(NxtSensor sensor)
        {
            Invoke((MethodInvoker)delegate
            {
                BoxedCurrentValue = (int)Functions.Clamp(nxtLightSensor.Value, 0, 100);
                valLight.Value = (int)BoxedCurrentValue;
                lightSensorValueOut.Text = nxtLightSensor.Value.ToString(CultureInfo.InvariantCulture);
                SensorTemporalValueDifference();
            });
            if (WhirlActive)
            {
                // Add logic when haden responds to a light event and is intelligent.

                var hold = 0;
            }
        }
        private void NxtPressureSensorPolled(NxtSensor sensor)
        {
            if (nxtPressureSensor.IsPressed)
            {
                nxtTankControl1.TankDrive.Brake();
                lightSeekingMotor.Stop();
                if (VoiceSpoken == false)
                    _voice.Speak("The robot has reached its goal.");
                VoiceSpoken = true;
            }
        }

        #endregion

        #region Coin flip, decider, and session generator

        /// <summary>
        /// Decides to seek to the left or to the right using a (static) random number generator.
        /// </summary>
        public void DecideLeftOrRight()
        {
            var leftRight = StaticRandom.Next(1, 3);
            lastRememberedTextBox.Text = leftRight.ToString();
            Thread.Sleep(1000);
            if (leftRight.Equals(1))
            {
                LastTurn = "none";
                lastRememberedTextBox.Text = LastTurn.ToString();
                statusStrip.Text = "I remember not turning before. A random decision is that I turn to the left. Is this okay?";
                CallDecisionForm(statusStrip.Text);
                //Thread.Sleep(10000);
                //AutonomousTurn("left");
            }
            if (leftRight.Equals(2))
            {
                LastTurn = "none";
                lastRememberedTextBox.Text = LastTurn.ToString();
                statusStrip.Text = "I remember not turning before. A random decision is that I turn to the right. Is this okay?";
                CallDecisionForm(statusStrip.Text);
                //Thread.Sleep(10000);
                //AutonomousTurn("right");
            }
        }
        public void CallDecisionForm(string message)
        {
            if (YesNo.Instance == false)
            {
                YesNo form = new YesNo(this, message);
                form.Show(this);
                YesNo.Instance = true;
                statusStrip.Text = "Called the decision form.";
            }
            else if (YesNo.Instance)
            {
                // Do nothing.
            }
        }

        #endregion

        #region Events        
        /// <summary>
        /// Begins an autonmous seek illustration of artificial life research - in the spirit of Walter and Grand.
        /// </summary>
        private void AutonomousClick(object sender, EventArgs e)
        {
            statusLabel.Text = "";
            try
            {
                nxtBrick.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + @" Autonomous instance of brick not available. Check that the brick is powered-on.", @"Autonomous connection error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

            }
            if (sanityCheckBox.Checked)
            {
                CheckDataSystem();
                CheckMotorMovements();
            }
            if (!nxtBrick.IsConnected) return;
            statusLabel.Text = Resources.HadenControlForm_Robot_connected;

            disconnectBrickButton.Enabled = true;
            connectBrickButton.Enabled = false;
            RueTheWhirl.ConnectedHaden = true;
            IsConnectedBrickAutonomous = true;
            dummyModeButton.Enabled = false;
            IsConnectedBrickDummy = false;
            
            if (!sanityCheckBox.Checked)
                BeginToRueTheWhirl(Aeon.WhirlDuration);
        }
        private void DisconnectBrickButtonClick(object sender, EventArgs e)
        {
            valLight.Value = 0;
            statusLabel.Text = Resources.HadenControlForm_Robot_disconnected;

            connectBrickButton.Enabled = true;
            IsConnectedBrickAutonomous = false;
            RueTheWhirl.ConnectedHaden = false;
            dummyModeButton.Enabled = true;
            IsConnectedBrickDummy = false;
            disconnectBrickButton.Enabled = false;

            try
            {
                nxtBrick.Disconnect();
                speechPhraseTraceOutputBox.Text += statusLabel.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + @" Disconnection socket error.", @"Disconnection error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

            }
        }
        /// <summary>
        /// Begins a dummy mode for mechanical calibration and settings.
        /// </summary>
        private void DummyModeClick(object sender, EventArgs e)
        {
            if (sanityCheckBox.Checked)
            {
                CheckDataSystem();
                CheckMotorMovements();
            }
            statusLabel.Text = "";
            try
            {
                nxtBrick.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + @" Dummy of brick not available. Check that it is powered-on.", @"Dummy-mode connection error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

            }
            if (!nxtBrick.IsConnected) return;
            statusLabel.Text = Resources.HadenControlForm_Robot_connected;

            disconnectBrickButton.Enabled = true;
            connectBrickButton.Enabled = false;
            IsConnectedBrickAutonomous = false;
            RueTheWhirl.ConnectedHaden = false;
            dummyModeButton.Enabled = false;
            IsConnectedBrickDummy = true;
        }
        //
        // Others.
        private void loadPhraseFileButton_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();

            if (openSpeechFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openSpeechFileDialog.FileName);
                document.LoadXml(sr.ReadToEnd());
                ParseXmlDocument(document);
                sr.Close();
            }
        }
        private void speakPhraseButton_Click(object sender, EventArgs e)
        {
            try 
            {
                _voice.Speak(speechPhraseTraceOutputBox.Text, SpeechVoiceSpeakFlags.SVSFDefault);
            }
            catch
            {
                MessageBox.Show(Resources.HadenControlForm_Please_select_a_phrase_to_voice, Resources.HadenControlForm_Voice_Error, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _voice.Speak("Exiting haden remote, goodbye.", SpeechVoiceSpeakFlags.SVSFDefault);
            if (disconnectBrickButton.Enabled)
                nxtBrick.Disconnect();
            Close();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _voice.Speak("About this program", SpeechVoiceSpeakFlags.SVSFDefault);
            MessageBox.Show(Resources.HadenControlForm_aboutToolStripMenuItem_About, Resources.HadenControlForm_aboutToolStripMenuItem_Click_AboutText, MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _voice.Speak("Haden status", SpeechVoiceSpeakFlags.SVSFDefault);
            MessageBox.Show(Resources.HadenManualControl_statusToolStripMenuItem_Click_Status_implemented_, Resources.HadenManualControl_statusToolStripMenuItem_Click_Status_of_device, MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        }
        private void autonomyFormButton_Click(object sender, EventArgs e)
        {
            _voice.Speak("Opening autonomy simulator.", SpeechVoiceSpeakFlags.SVSFDefault);
            if (HadenAutonomySimulator.Instance == false)
            {
                HadenAutonomySimulator form = new HadenAutonomySimulator(this);
                form.Show(this);
                HadenAutonomySimulator.Instance = true;
            }
            else if (HadenAutonomySimulator.Instance)
            {
                // Do nothing.
            }
        }
        private void comPortSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            nxtBrick.ComPortName = comPortSelectionBox.Text.ToString();
        }
        private void hadenAutonomousFormButton_Click(object sender, EventArgs e)
        {
            _voice.Speak("Opening autonomous control.", SpeechVoiceSpeakFlags.SVSFDefault);
            if (HadenAutonomousControl.Instance == false)
            {
                HadenAutonomousControl form = new HadenAutonomousControl(this);
                form.Show(this);
                HadenAutonomousControl.Instance = true;
            }
            else if (HadenAutonomousControl.Instance)
            {
                // Do nothing.
            }
        }
        private void simulatedAutonomyButton_Click(object sender, EventArgs e)
        {
            if (SimulatedAutonomy.Instance == false)
            {
                SimulatedAutonomy form = new SimulatedAutonomy(this);
                form.Show(this);
                SimulatedAutonomy.Instance = true;
            }
            else if (SimulatedAutonomy.Instance)
            {
                // Do nothing.
            }
        }
        private void skratchFormButton_Click(object sender, EventArgs e)
        {
            _voice.Speak("Opening mockup form.", SpeechVoiceSpeakFlags.SVSFDefault);
            if (SimulatorMockup.Instance == false)
            {
                SimulatorMockup form = new SimulatorMockup(this, new ContextMenu());
                form.Show(this);
                SimulatorMockup.Instance = true;
            }
            else if (SimulatorMockup.Instance)
            {
                // Do nothing.
            }
        }
        private void freshAutonomyButton_Click(object sender, EventArgs e)
        {
            //if (SimulatedAutonomy.Instance == false)
            //{
            //    SimulatedAutonomy form = new SimulatedAutonomy(this);
            //    form.Show(this);
            //    SimulatedAutonomy.Instance = true;
            //}
            //else if (SimulatedAutonomy.Instance)
            //{
            //    // Do nothing.
            //}
        }
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Print statistics in lieu of saving them in a database for later analysis.
            //MessageBox.Show(TurnsMade.ToString(), "Statistics of the run", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int powerDrive = 30;
            const int powerSeek = 10;
            const int degrees = 3;

            if (keyData == Keys.Down)
            {
                VoiceSpoken = false;
                nxtTankControl1.TankDrive.Brake();
                nxtTankControl1.TankDrive.MoveForward(powerDrive, 0);
                return true;
            }
            if (keyData == Keys.Up)
            {
                VoiceSpoken = false;
                nxtTankControl1.TankDrive.Brake();
                nxtTankControl1.TankDrive.MoveBack(powerDrive, 0);
                return true;
            }
            if (keyData == Keys.Left)
            {
                VoiceSpoken = false;
                nxtTankControl1.TankDrive.Brake();
                nxtTankControl1.TankDrive.TurnLeft(powerDrive, 0);
                return true;
            }
            if (keyData == Keys.Right)
            {
                VoiceSpoken = false;
                nxtTankControl1.TankDrive.Brake();
                nxtTankControl1.TankDrive.TurnRight(powerDrive, 0);
                return true;
            }
            if (keyData == Keys.V)
            {
                VoiceSpoken = false;
                nxtTankControl1.TankDrive.Brake();
                lightSeekingMotor.Stop();
                return true;
            }
            if (keyData == Keys.X)
            {
                VoiceSpoken = false;
                lightSeekingMotor.TurnCounterClockwise(powerSeek, degrees);
                return true;
            }
            if (keyData == Keys.Z)
            {
                VoiceSpoken = false;
                lightSeekingMotor.TurnClockwise(powerSeek, degrees);
                return true;
            }
            if (keyData == Keys.Space)
            {
                VoiceSpoken = false;
                lightSeekingMotor.Brake = true;
                lightSeekingMotor.IsRunning = false;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void StoreTrace(string objectType, string text)
        {
            Logging.WriteLog(objectType + ": " + text, Logging.LogType.Information);
        }
        #endregion
    }
}
