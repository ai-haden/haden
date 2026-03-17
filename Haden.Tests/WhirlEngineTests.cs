using System.Collections.Generic;
using Haden.Library;
using NUnit.Framework;

namespace Haden.Tests
{
    [TestFixture]
    public class WhirlEngineTests
    {
        [Test]
        public void InitialState_NotConnected_StartsInZeroWithBootMessage()
        {
            WhirlEngine engine = new WhirlEngine
            {
                ConnectedHaden = false
            };

            WhirlTickResult tick = engine.GetCurrentTick();

            Assert.That(tick.State, Is.EqualTo(WhirlState.Zero));
            Assert.That(tick.Action, Is.EqualTo("Beginning my whirl. Not connected to haden."));
            Assert.That(tick.ShouldSeekLight, Is.False);
        }

        [Test]
        public void TickCycle_Connected_TransitionsDeterministically()
        {
            WhirlEngine engine = new WhirlEngine
            {
                ConnectedHaden = true
            };

            List<WhirlTickResult> ticks = new List<WhirlTickResult>
            {
                engine.GetCurrentTick(),
                engine.AdvanceTick(),
                engine.AdvanceTick(),
                engine.AdvanceTick(),
                engine.AdvanceTick(),
                engine.AdvanceTick()
            };

            Assert.That(ticks[0].State, Is.EqualTo(WhirlState.Zero));
            Assert.That(ticks[1].State, Is.EqualTo(WhirlState.One));
            Assert.That(ticks[2].State, Is.EqualTo(WhirlState.Two));
            Assert.That(ticks[3].State, Is.EqualTo(WhirlState.Three));
            Assert.That(ticks[4].State, Is.EqualTo(WhirlState.Four));
            Assert.That(ticks[5].State, Is.EqualTo(WhirlState.Zero));

            Assert.That(ticks[1].ShouldSeekLight, Is.True);
            Assert.That(ticks[0].ShouldSeekLight, Is.False);
            Assert.That(ticks[2].ShouldSeekLight, Is.False);
            Assert.That(ticks[3].ShouldSeekLight, Is.False);
            Assert.That(ticks[4].ShouldSeekLight, Is.False);
            Assert.That(ticks[5].ShouldSeekLight, Is.False);
        }

        [Test]
        public void Mission_SeekWindows_CanDriveBrightestLightAcquisition()
        {
            // Simulated sensor intensity values seen during autonomous seek windows.
            int[] lightReadings = { 12, 25, 39, 51, 68, 80, 73, 60 };
            WhirlEngine engine = new WhirlEngine
            {
                ConnectedHaden = true
            };

            int readingIndex = 0;
            int peakSeen = int.MinValue;
            WhirlTickResult tick = engine.GetCurrentTick();

            for (int i = 0; i < 60 && readingIndex < lightReadings.Length; i++)
            {
                if (tick.ShouldSeekLight)
                {
                    int current = lightReadings[readingIndex++];
                    if (current > peakSeen)
                    {
                        peakSeen = current;
                    }
                }

                tick = engine.AdvanceTick();
            }

            Assert.That(readingIndex, Is.EqualTo(lightReadings.Length), "Whirl should schedule enough seek windows.");
            Assert.That(peakSeen, Is.EqualTo(80), "Autonomous seek windows should allow finding the brightest observed source.");
        }
    }
}
