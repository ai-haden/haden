using System;
using System.Collections.Generic;
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

namespace Haden.Tests
{
    [TestFixture]
    // Bring-over the esssence algorithm from TD Policy project to prepare it for use with haden.
    public class RewardTests
    {
        #region The algorithm
        // Criteria satisfaction (goal met --> reward received):
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
        [TestCase]
        public void CheckTemporalDifference()
        {
            
        }
        // The Action.
        [TestCase]
        public void SeekReward()
        {
            
        }

        #endregion
    }
}
