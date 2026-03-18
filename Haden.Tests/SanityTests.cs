using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Haden.NxtSharp;
using Haden.NxtSharp.Brick;
using Haden.NxtSharp.Controllers;
using Haden.NxtSharp.Motors;
using Haden.NxtSharp.Sensors;
using Haden.NxtSharp.Utilties;
using NUnit.Framework;
using SpeechLib;
using Logging = Haden.NxtSharp.Utilties.Logging;

namespace Haden.Tests
{
    [TestFixture]
    [Category("Hardware")]
    [Explicit("Manual/hardware integration tests. Use targeted runs when NXT hardware and speech setup are available.")]
    public class SanityTests
    {
        // Aesthetics
        private SpVoice _voice;
        private bool VoiceSpoken { get; set; }

        public const int IterationsUntilFoundSource = 3; // Of solution-verified criteria.

        private NxtBrick _nxtBrick;
        private NxtMotor _nxtMotorA;
        private NxtMotor _nxtMotorB;
        private NxtMotor _nxtMotorC;
        private NxtLightSensor _nxtLightSensor;
        private NxtPressureSensor _nxtPressureSensor;
        private NxtMotorControl _nxtMotorControl;
        private NxtTankControl _nxtTankControl;
        private string _testComPort;

        public object BoxedLastTurn;
        public object BoxedLastSeek;
        public object BoxedCurrentValue; // value
        public object BoxedPeakValue; // n0
        public object BoxedDifferenceValue; // s0
        // Variables the program can set for learning/training tests.
        public int MotorPower { get; private set; } // v0
        public int MotorDegrees { get; private set; } // theta
        public int CountFoundSame { get; private set; }
        public int TotalDegreesMoved { get; private set; } // Sum(theta)
        public int TotalDegreesInOneDirectionMoved { get; private set; } // Sum(theta(T(x)))
        public List<string> TurnDirectionSequence { get; private set; }

        public string SignIndicator { get; private set; }
        public int CurrentIteration { get; private set; }
        public int IteratorLimit { get; private set; }
        public int BatteryLevel { get; private set; }

        private void CoinFlip()
        {
            var leftRight = StaticRandom.Next(1, 3);
            BoxedLastSeek = leftRight.Equals(1) ? "left" : "right";
            BoxedLastTurn = leftRight.Equals(1) ? "left" : "right";
        }
        private void StorePeakValue()
        {
            BoxedPeakValue = BoxedCurrentValue;
        }
        private int Difference()
        {
            return (int) BoxedCurrentValue - (int) BoxedPeakValue;
        }
        private void Iterate()
        {
            for (CurrentIteration = 1; CurrentIteration <= IteratorLimit; CurrentIteration++)
            {
                Logging.WriteLog("Iteration number " + CurrentIteration + " computing light intensity as " + BoxedCurrentValue + " seen at the sensor, while the largest (peak) value already seen is " + BoxedPeakValue + ".", Logging.LogType.Information);
                ContemplateAction();
            }
            Logging.WriteLog("Total degrees moved is: " + TotalDegreesMoved + ".", Logging.LogType.Information);
        }

        [SetUp]
        public void InitHaden()
        {
            try
            {
                _nxtBrick = new NxtBrick();
                _nxtMotorA = new NxtMotor();
                _nxtMotorB = new NxtMotor();
                _nxtMotorC = new NxtMotor();
                _nxtPressureSensor = new NxtPressureSensor();
                _nxtLightSensor = new NxtLightSensor();
                _nxtMotorControl = new NxtMotorControl();
                _nxtTankControl = new NxtTankControl();
                // nxtBrick
                _nxtBrick.AutoPoll = true;
                var availablePorts = SerialPort.GetPortNames();
                _testComPort = availablePorts.Length > 0 ? availablePorts[0] : null;
                if (!string.IsNullOrEmpty(_testComPort))
                {
                    _nxtBrick.ComPortName = _testComPort;
                }
                _nxtBrick.MotorA = _nxtMotorA;
                _nxtBrick.MotorB = _nxtMotorB;
                _nxtBrick.MotorC = _nxtMotorC;
                _nxtBrick.Sensor1 = _nxtPressureSensor;
                _nxtBrick.Sensor2 = null;
                _nxtBrick.Sensor3 = _nxtLightSensor;
                _nxtBrick.Sensor4 = null;
                // nxtMotorA
                _nxtMotorA.Brick = _nxtBrick;
                _nxtMotorA.Flip = false;
                _nxtMotorA.Port = NxtMotorPort.PortA;
                // nxtMotorB
                _nxtMotorB.Brick = _nxtBrick;
                _nxtMotorB.Flip = false;
                _nxtMotorB.Port = NxtMotorPort.PortB;
                // nxtMotorC
                _nxtMotorC.Brick = _nxtBrick;
                _nxtMotorC.Flip = false;
                _nxtMotorC.Port = NxtMotorPort.PortC;
                // nxtPressureSensor
                _nxtPressureSensor.AutoPoll = true;
                _nxtPressureSensor.AutoPollDelay = 100;
                _nxtPressureSensor.Brick = _nxtBrick;
                _nxtPressureSensor.Port = NxtSensorPort.Port1;
                _nxtPressureSensor.ValueChanged += NxtPressureSensorPolled;
                // nxtLightSensor
                _nxtLightSensor.Active = false;
                _nxtLightSensor.AutoPoll = true;
                _nxtLightSensor.AutoPollDelay = 100;
                _nxtLightSensor.Brick = _nxtBrick;
                _nxtLightSensor.Port = NxtSensorPort.Port3;
                _nxtLightSensor.ValueChanged += NxtLightSensorValueChanged;
                // nxtMotorControl
                _nxtMotorControl.Brake = true;
                _nxtMotorControl.Motor = _nxtMotorA;
                // Variables
                BoxedCurrentValue = 0;
                BoxedPeakValue = 0;
                BoxedDifferenceValue = 0;
                BoxedLastSeek = "";
                BoxedLastTurn = "";
                MotorPower = 30;
                MotorDegrees = 30;
                CurrentIteration = 0;
                TurnDirectionSequence = new List<string>();
                CoinFlip();

            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.TestFramework);
                Console.WriteLine(ex.Message);
            }
            
        }
        [TearDown]
        public void ExitHaden()
        {
            if (_nxtBrick == null) return;
            // Reset the motor position.
            if (TurnDirectionSequence.Count != 0)
            {
                var lastDirection = TurnDirectionSequence[TurnDirectionSequence.Count - 1];
                if (lastDirection == "left")
                {
                    _nxtMotorControl.TurnClockwise(MotorPower, TotalDegreesInOneDirectionMoved);
                    Logging.WriteLog("Returned to original position through " + TotalDegreesInOneDirectionMoved + " degrees.", Logging.LogType.Information);
                }
                if (lastDirection == "right")
                {
                    _nxtMotorControl.TurnCounterClockwise(MotorPower, TotalDegreesInOneDirectionMoved);
                    Logging.WriteLog("Returned to original position through " + TotalDegreesInOneDirectionMoved + " degrees.", Logging.LogType.Information);
                }
            }
            try
            {
                _nxtBrick.Disconnect();
                _nxtBrick = null;
                Logging.WriteLog("Haden robot is disconnected.", Logging.LogType.Information);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.TestFramework);
            }
        }

        #region The algorithm
        // Criteria satisfaction (goal met):
        // Number of iterations to achieve the solution the measure of efficiency.
        // To measure fitness, the algorithm can set the value of MotorDegrees from default.
        // STEPS:
        // Query the current sensor value.
        // Send command to the motor to turn the sensor in order to explore if the next position yields a higher value.
        //   -- If not, (you are moving away from the source intensity) turn in the opposite direction to see if the value is greater than the previous in this position.
        //   -- When the current and peak value are equal for x iterations (IterationsUntilFoundSource), you have found the source.
        // Report the number of iterations it took to reach the solution.
        // Reset the positon to the starting point, e.g., match the position to the first seen sensor value. Use variable TotalDegreesMoved to walk back.
        // Q: Have a limit in the form of the maximum turning radius in one direction (in degrees) such that the cable will become twisted around.
        protected void ContemplateAction()
        {
            // Perform a comparison to see how close you are to the light source (by intensity).
            if (Difference() > 0)
            {
                SignIndicator = "+";
                Logging.WriteLog("Difference is " + SignIndicator, Logging.LogType.Information);
                StorePeakValue();
                Logging.WriteLog("The (max) value of " + BoxedCurrentValue + " has been seen by the sensor is a positive quantity.", Logging.LogType.Information);
                BoxedDifferenceValue = Difference();
                Logging.WriteLog("The difference value of " + BoxedDifferenceValue + " exists between the max and current sensor values.", Logging.LogType.Information);
                Logging.WriteLog("Turn in the same direction.", Logging.LogType.Information);

                if (BoxedLastSeek.ToString() == "left")
                    SeekAction("left");
                if (BoxedLastSeek.ToString() == "right")
                    SeekAction("right");
            }
            else if (Difference() < 0)
            {
                SignIndicator = "-";
                Logging.WriteLog("Difference is " + SignIndicator, Logging.LogType.Information);
                Logging.WriteLog("The (max) value of " + BoxedCurrentValue + " has been seen by the sensor is a negative quantity.", Logging.LogType.Information);
                BoxedDifferenceValue = Difference();
                Logging.WriteLog("The difference value of " + BoxedDifferenceValue + " exists between the max and current sensor values.", Logging.LogType.Information);
                Logging.WriteLog("Turn in the opposite direction.", Logging.LogType.Information);

                if (BoxedLastSeek.ToString() == "left")
                    SeekAction("right");
                if (BoxedLastSeek.ToString() == "right")
                    SeekAction("left");
            }
            else if (Difference() == 0)
            {
                SignIndicator = "0";
                Logging.WriteLog("Difference is " + SignIndicator, Logging.LogType.Information);
                BoxedDifferenceValue = Difference();
                Logging.WriteLog("Values for current and maximum seen are equal. Have I found the light source?", Logging.LogType.Information);
                if (CurrentIteration != 1)
                {
                    CountFoundSame++;
                }
                if (CountFoundSame == IterationsUntilFoundSource)// Process should exit when this is satisfied.
                {
                    Logging.WriteLog("Criteria of sequential iterations is " + IterationsUntilFoundSource + " and I have seen it " + CountFoundSame + " times. That means the source has been found and my exploration task is finished. I did so in " + CurrentIteration + " iterations, moving " + TotalDegreesInOneDirectionMoved + " degrees in one direction and a total of " + TotalDegreesMoved + ".", Logging.LogType.Information);
                }
                if (BoxedLastSeek.ToString() == "")
                    CoinFlip();
            }
        }
        // The Action.
        protected void SeekAction(string direction)
        {
            if (!_nxtBrick.IsConnected)
                return;
            switch (direction)
            {
                case "left":
                    _nxtMotorControl.Power = MotorPower;
                    _nxtMotorControl.TurnCounterClockwise(MotorPower, MotorDegrees);
                    Thread.Sleep(500);
                    _nxtMotorControl.Stop();
                    BoxedLastTurn = "left";
                    TurnDirectionSequence.Add("left");
                    // Sum all the angles (theta) involved in the task.
                    TotalDegreesMoved = TotalDegreesMoved + MotorDegrees;
                    // If you are starting in this direction, the value is always positive.
                    if (TotalDegreesInOneDirectionMoved == 0)
                    {
                        TotalDegreesInOneDirectionMoved = TotalDegreesInOneDirectionMoved + MotorDegrees;
                    }
                    // Last movement was in the same direction.
                    if (TurnDirectionSequence[CurrentIteration - 1] == "left")
                    {
                        TotalDegreesInOneDirectionMoved = TotalDegreesInOneDirectionMoved + MotorDegrees;
                    }
                    // Last movement was not in the same direction.
                    if (TurnDirectionSequence[CurrentIteration - 1] == "right")
                    {
                        TotalDegreesInOneDirectionMoved = TotalDegreesInOneDirectionMoved - MotorDegrees;
                    }
                    break;
                case "right":
                    _nxtMotorControl.Power = MotorPower;
                    _nxtMotorControl.TurnClockwise(MotorPower, MotorDegrees);
                    Thread.Sleep(500);
                    _nxtMotorControl.Stop();
                    BoxedLastTurn = "right";
                    TurnDirectionSequence.Add("right");
                    // Sum all the angles (theta) involved in the task.
                    TotalDegreesMoved = TotalDegreesMoved + MotorDegrees;
                    // If you are starting in this direction, the value is always positive.
                    if (TotalDegreesInOneDirectionMoved == 0)
                    {
                        TotalDegreesInOneDirectionMoved = TotalDegreesInOneDirectionMoved + MotorDegrees;
                    }
                    // Last movement was in the same direction.
                    if (TurnDirectionSequence[CurrentIteration - 1] == "right")
                    {
                        TotalDegreesInOneDirectionMoved = TotalDegreesInOneDirectionMoved + MotorDegrees;
                    }
                    // Last movement was not in the same direction.
                    if (TurnDirectionSequence[CurrentIteration - 1] == "left")
                    {
                        TotalDegreesInOneDirectionMoved = TotalDegreesInOneDirectionMoved - MotorDegrees;
                    }
                    break;
            }
        }

        #endregion

        #region Tests

        [SetUp]
        public void InitVoice()
        {
            _voice = new SpVoice();
        }

        [TestCase]
        public void SpeakSomething()
        {
          _voice.Speak("I am speaking now. Can you hear me?");
        }

        [TestCase(15)]
        public void StartAutonomy(int iterations)
        {
            if (string.IsNullOrEmpty(_testComPort))
            {
                Assert.Ignore("No Bluetooth serial COM port detected. Pair the NXT brick and expose a COM port to run hardware integration tests.");
            }

            IteratorLimit = iterations;
            if (!_nxtBrick.IsConnected)
            {
                _nxtBrick.Connect();
                Thread.Sleep(1000);
                BoxedCurrentValue = (int)Functions.Clamp(_nxtLightSensor.Value, 0, 100);
                StorePeakValue();
                BoxedCurrentValue = 0;
                Logging.WriteLog(Environment.NewLine, Logging.LogType.Information);
                Logging.WriteLog("Haden robot is connected.", Logging.LogType.Information);
                Logging.WriteLog("The first light intensity seen at the sensor is: " + BoxedCurrentValue, Logging.LogType.Information);
            }
            Iterate();
            Thread.Sleep(3000); // Let messages already sent but yet to be processed time to complete.
            // Report details on the results to begin gathering what is important for training runs.
            var outlie = new StringBuilder();
            foreach (var list in TurnDirectionSequence)
            {
                outlie.Append(list + ", ");
            }
            Logging.WriteLog("The movement action sequence is " + outlie, Logging.LogType.Information);
            if (CountFoundSame != IterationsUntilFoundSource)// Report if the equality was not satisfied during the run.
            {
                Logging.WriteLog("Criteria of sequential iterations is " + IterationsUntilFoundSource + " and I have seen it " + CountFoundSame + " time(s). The run completed in " + CurrentIteration + " iterations, moving " + TotalDegreesInOneDirectionMoved + " degrees in one direction, for a total of " + TotalDegreesMoved + " degrees.", Logging.LogType.Information);
            }
        }

        [Test]
        public void GetPowerLevel()
        {
            if (string.IsNullOrEmpty(_testComPort))
            {
                Assert.Ignore("No Bluetooth serial COM port detected. Pair the NXT brick and expose a COM port to run hardware integration tests.");
            }

            if (!_nxtBrick.IsConnected)
            {
                _nxtBrick.Connect();
                Thread.Sleep(500);
                BatteryLevel = _nxtBrick.Communicator.GetBatteryLevel();
                Logging.WriteLog("Battery level being reported is: " + BatteryLevel + " mV.", Logging.LogType.Information);
                Console.WriteLine(BatteryLevel + " mV");
            }
        }

        #endregion

        #region Events

        private void NxtLightSensorValueChanged(NxtSensor sensor)
        {
            BoxedCurrentValue = (int)Functions.Clamp(_nxtLightSensor.Value, 0, 100);
        }
        private void NxtPressureSensorPolled(NxtSensor sensor)
        {
            if (_nxtPressureSensor.IsPressed)
            {
                _nxtTankControl.TankDrive.Brake();
                _nxtMotorControl.Stop();
                if (VoiceSpoken == false)
                    _voice.Speak("The robot has reached its goal.");
                VoiceSpoken = true;
            }
        }
        #endregion
    }
}
