using System;
using System.Globalization;
using System.IO;
using Haden.HardwareSmoke;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class SqliteExperimentStoreTests
    {
        [Test]
        public void StartAppendAndComplete_PersistsSessionStepAndRlPoint()
        {
            string dbPath = Path.Combine(Path.GetTempPath(), "haden-store-test-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".db");

            try
            {
                using (var store = new SqliteExperimentStore(dbPath))
                {
                    DateTime started = DateTime.UtcNow;
                    long sessionId = store.StartSession(started, lightSensorPort: 3, bumpSensorPort: 1, maxIterations: 120);

                    store.AppendStep(
                        sessionId,
                        stepIndex: 0,
                        timestampUtc: started,
                        sensorRaw: 40,
                        sensorSmooth: 38,
                        delta: 2,
                        reward: 1.2,
                        bump: false,
                        scanDirection: 1,
                        scanPower: 18,
                        leftPower: 21,
                        rightPower: 49);

                    ScorecardSnapshot snapshot = store.AppendRlPoint(
                        sessionId,
                        "light:mid;delta:rise;bump:0",
                        "scan:+;steer:right;mag:2",
                        reward: 1.2,
                        success: false,
                        updatedUtc: started);

                    store.CompleteSession(
                        sessionId,
                        endedUtc: started.AddSeconds(2),
                        stopReason: "max-iterations",
                        completedIterations: 1,
                        peakLightValue: 40,
                        totalReward: 1.2,
                        success: false);

                    Assert.That(snapshot.Updates, Is.EqualTo(1));
                    Assert.That(snapshot.Confidence, Is.GreaterThanOrEqualTo(0.0));
                }

                using var connection = new SqliteConnection("Data Source=" + dbPath);
                connection.Open();

                Assert.That(CountRows(connection, "experiment_session"), Is.EqualTo(1));
                Assert.That(CountRows(connection, "experiment_step"), Is.EqualTo(1));
                Assert.That(CountRows(connection, "rl_point"), Is.EqualTo(1));
                Assert.That(CountRows(connection, "rl_scorecard"), Is.EqualTo(1));
            }
            finally
            {
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
            }
        }

        [Test]
        public void AppendRlPoint_RepeatedPositiveUpdates_IncreasesConfidence()
        {
            string dbPath = Path.Combine(Path.GetTempPath(), "haden-store-test-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".db");

            try
            {
                using var store = new SqliteExperimentStore(dbPath);
                long sessionId = store.StartSession(DateTime.UtcNow, lightSensorPort: 3, bumpSensorPort: 1, maxIterations: 120);

                ScorecardSnapshot first = store.AppendRlPoint(
                    sessionId,
                    "light:mid;delta:rise;bump:0",
                    "scan:+;steer:right;mag:2",
                    reward: 0.5,
                    success: false,
                    updatedUtc: DateTime.UtcNow);

                ScorecardSnapshot second = store.AppendRlPoint(
                    sessionId,
                    "light:mid;delta:rise;bump:0",
                    "scan:+;steer:right;mag:2",
                    reward: 2.0,
                    success: true,
                    updatedUtc: DateTime.UtcNow);

                Assert.That(second.Updates, Is.EqualTo(2));
                Assert.That(second.Successes, Is.EqualTo(1));
                Assert.That(second.Confidence, Is.GreaterThan(first.Confidence));
            }
            finally
            {
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
            }
        }

        [Test]
        public void ScorecardQueries_ReturnTopAndLowestConfidenceBuckets()
        {
            string dbPath = Path.Combine(Path.GetTempPath(), "haden-store-test-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".db");

            try
            {
                using var store = new SqliteExperimentStore(dbPath);
                long sessionId = store.StartSession(DateTime.UtcNow, lightSensorPort: 3, bumpSensorPort: 1, maxIterations: 120);

                store.AppendRlPoint(sessionId, "state-a", "action-a", reward: 2.0, success: true, updatedUtc: DateTime.UtcNow);
                store.AppendRlPoint(sessionId, "state-b", "action-b", reward: -0.8, success: false, updatedUtc: DateTime.UtcNow);
                store.AppendRlPoint(sessionId, "state-c", "action-c", reward: 0.2, success: false, updatedUtc: DateTime.UtcNow);

                var top = store.GetTopScorecardEntries(limit: 2);
                var low = store.GetLowConfidenceEntries(limit: 2);

                Assert.That(top.Count, Is.EqualTo(2));
                Assert.That(low.Count, Is.EqualTo(2));
                Assert.That(top[0].Confidence, Is.GreaterThanOrEqualTo(top[1].Confidence));
                Assert.That(low[0].Confidence, Is.LessThanOrEqualTo(low[1].Confidence));
            }
            finally
            {
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
            }
        }

        private static int CountRows(SqliteConnection connection, string tableName)
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM " + tableName + ";";
            object value = command.ExecuteScalar();
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }
    }
}
