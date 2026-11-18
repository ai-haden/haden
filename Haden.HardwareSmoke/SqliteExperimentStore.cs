using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Haden.HardwareSmoke
{
    public sealed class ScorecardSnapshot
    {
        public ScorecardSnapshot(int updates, int successes, double qValue, double confidence)
        {
            Updates = updates;
            Successes = successes;
            QValue = qValue;
            Confidence = confidence;
        }

        public int Updates { get; }
        public int Successes { get; }
        public double QValue { get; }
        public double Confidence { get; }
    }

    public sealed class ScorecardSummary
    {
        public ScorecardSummary(int entries, double averageConfidence)
        {
            Entries = entries;
            AverageConfidence = averageConfidence;
        }

        public int Entries { get; }
        public double AverageConfidence { get; }
    }

    public sealed class ScorecardEntry
    {
        public ScorecardEntry(
            string stateKey,
            string actionKey,
            int updates,
            int successes,
            double qValue,
            double confidence,
            DateTime lastUpdatedUtc)
        {
            StateKey = stateKey;
            ActionKey = actionKey;
            Updates = updates;
            Successes = successes;
            QValue = qValue;
            Confidence = confidence;
            LastUpdatedUtc = lastUpdatedUtc;
        }

        public string StateKey { get; }
        public string ActionKey { get; }
        public int Updates { get; }
        public int Successes { get; }
        public double QValue { get; }
        public double Confidence { get; }
        public DateTime LastUpdatedUtc { get; }
    }

    public sealed class SqliteExperimentStore : IDisposable
    {
        private readonly SqliteConnection _connection;

        public SqliteExperimentStore(string databasePath)
        {
            if (string.IsNullOrWhiteSpace(databasePath))
            {
                throw new ArgumentException("Database path is required.", nameof(databasePath));
            }

            string fullPath = Path.GetFullPath(databasePath);
            string directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _connection = new SqliteConnection("Data Source=" + fullPath);
            _connection.Open();
            EnsureSchema();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public ScorecardSummary GetScorecardSummary()
        {
            using var command = _connection.CreateCommand();
            command.CommandText =
                "SELECT COUNT(*), COALESCE(AVG(confidence), 0.0) FROM rl_scorecard;";

            using SqliteDataReader reader = command.ExecuteReader();
            reader.Read();
            return new ScorecardSummary(
                reader.GetInt32(0),
                reader.GetDouble(1));
        }

        public IReadOnlyList<ScorecardEntry> GetTopScorecardEntries(int limit)
        {
            return QueryScorecardEntries(limit, ascendingConfidence: false);
        }

        public IReadOnlyList<ScorecardEntry> GetLowConfidenceEntries(int limit)
        {
            return QueryScorecardEntries(limit, ascendingConfidence: true);
        }

        public long StartSession(
            DateTime startedUtc,
            int lightSensorPort,
            int bumpSensorPort,
            int maxIterations)
        {
            using var command = _connection.CreateCommand();
            command.CommandText =
                "INSERT INTO experiment_session " +
                "(started_utc, stop_reason, bump_sensor_port, light_sensor_port, max_iterations, completed_iterations, peak_light_value, total_reward, success) " +
                "VALUES ($startedUtc, 'running', $bumpSensorPort, $lightSensorPort, $maxIterations, 0, 0, 0.0, 0); " +
                "SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$startedUtc", FormatUtc(startedUtc));
            command.Parameters.AddWithValue("$bumpSensorPort", bumpSensorPort);
            command.Parameters.AddWithValue("$lightSensorPort", lightSensorPort);
            command.Parameters.AddWithValue("$maxIterations", maxIterations);
            object value = command.ExecuteScalar();
            return Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        public void AppendStep(
            long sessionId,
            int stepIndex,
            DateTime timestampUtc,
            int sensorRaw,
            int sensorSmooth,
            int delta,
            double reward,
            bool bump,
            int scanDirection,
            int scanPower,
            int leftPower,
            int rightPower)
        {
            using var command = _connection.CreateCommand();
            command.CommandText =
                "INSERT INTO experiment_step " +
                "(session_id, step_index, ts_utc, sensor_raw, sensor_smooth, delta, reward, bump, scan_dir, scan_power, left_power, right_power) " +
                "VALUES ($sessionId, $stepIndex, $tsUtc, $sensorRaw, $sensorSmooth, $delta, $reward, $bump, $scanDir, $scanPower, $leftPower, $rightPower);";
            command.Parameters.AddWithValue("$sessionId", sessionId);
            command.Parameters.AddWithValue("$stepIndex", stepIndex);
            command.Parameters.AddWithValue("$tsUtc", FormatUtc(timestampUtc));
            command.Parameters.AddWithValue("$sensorRaw", sensorRaw);
            command.Parameters.AddWithValue("$sensorSmooth", sensorSmooth);
            command.Parameters.AddWithValue("$delta", delta);
            command.Parameters.AddWithValue("$reward", reward);
            command.Parameters.AddWithValue("$bump", bump ? 1 : 0);
            command.Parameters.AddWithValue("$scanDir", scanDirection);
            command.Parameters.AddWithValue("$scanPower", scanPower);
            command.Parameters.AddWithValue("$leftPower", leftPower);
            command.Parameters.AddWithValue("$rightPower", rightPower);
            command.ExecuteNonQuery();
        }

        public ScorecardSnapshot AppendRlPoint(
            long sessionId,
            string stateKey,
            string actionKey,
            double reward,
            bool success,
            DateTime updatedUtc)
        {
            if (string.IsNullOrWhiteSpace(stateKey))
            {
                throw new ArgumentException("State key is required.", nameof(stateKey));
            }

            if (string.IsNullOrWhiteSpace(actionKey))
            {
                throw new ArgumentException("Action key is required.", nameof(actionKey));
            }

            using SqliteTransaction transaction = _connection.BeginTransaction();

            int currentUpdates = 0;
            int currentSuccesses = 0;
            double currentQValue = 0.0;

            using (var select = _connection.CreateCommand())
            {
                select.Transaction = transaction;
                select.CommandText =
                    "SELECT updates, successes, q_value " +
                    "FROM rl_scorecard " +
                    "WHERE state_key = $stateKey AND action_key = $actionKey;";
                select.Parameters.AddWithValue("$stateKey", stateKey);
                select.Parameters.AddWithValue("$actionKey", actionKey);

                using SqliteDataReader reader = select.ExecuteReader();
                if (reader.Read())
                {
                    currentUpdates = reader.GetInt32(0);
                    currentSuccesses = reader.GetInt32(1);
                    currentQValue = reader.GetDouble(2);
                }
            }

            int updatedCount = currentUpdates + 1;
            int updatedSuccesses = currentSuccesses + (success ? 1 : 0);
            double updatedQ = currentQValue + ((reward - currentQValue) / updatedCount);
            double confidence = ComputeConfidence(updatedCount, updatedSuccesses, updatedQ);

            using (var upsert = _connection.CreateCommand())
            {
                upsert.Transaction = transaction;
                upsert.CommandText =
                    "INSERT INTO rl_scorecard " +
                    "(state_key, action_key, updates, successes, q_value, confidence, last_updated_utc) " +
                    "VALUES ($stateKey, $actionKey, $updates, $successes, $qValue, $confidence, $updatedUtc) " +
                    "ON CONFLICT(state_key, action_key) DO UPDATE SET " +
                    "updates = excluded.updates, " +
                    "successes = excluded.successes, " +
                    "q_value = excluded.q_value, " +
                    "confidence = excluded.confidence, " +
                    "last_updated_utc = excluded.last_updated_utc;";
                upsert.Parameters.AddWithValue("$stateKey", stateKey);
                upsert.Parameters.AddWithValue("$actionKey", actionKey);
                upsert.Parameters.AddWithValue("$updates", updatedCount);
                upsert.Parameters.AddWithValue("$successes", updatedSuccesses);
                upsert.Parameters.AddWithValue("$qValue", updatedQ);
                upsert.Parameters.AddWithValue("$confidence", confidence);
                upsert.Parameters.AddWithValue("$updatedUtc", FormatUtc(updatedUtc));
                upsert.ExecuteNonQuery();
            }

            using (var insertPoint = _connection.CreateCommand())
            {
                insertPoint.Transaction = transaction;
                insertPoint.CommandText =
                    "INSERT INTO rl_point " +
                    "(session_id, state_key, action_key, reward, confidence, updated_utc) " +
                    "VALUES ($sessionId, $stateKey, $actionKey, $reward, $confidence, $updatedUtc);";
                insertPoint.Parameters.AddWithValue("$sessionId", sessionId);
                insertPoint.Parameters.AddWithValue("$stateKey", stateKey);
                insertPoint.Parameters.AddWithValue("$actionKey", actionKey);
                insertPoint.Parameters.AddWithValue("$reward", reward);
                insertPoint.Parameters.AddWithValue("$confidence", confidence);
                insertPoint.Parameters.AddWithValue("$updatedUtc", FormatUtc(updatedUtc));
                insertPoint.ExecuteNonQuery();
            }

            transaction.Commit();
            return new ScorecardSnapshot(updatedCount, updatedSuccesses, updatedQ, confidence);
        }

        public void CompleteSession(
            long sessionId,
            DateTime endedUtc,
            string stopReason,
            int completedIterations,
            int peakLightValue,
            double totalReward,
            bool success)
        {
            using var command = _connection.CreateCommand();
            command.CommandText =
                "UPDATE experiment_session SET " +
                "ended_utc = $endedUtc, " +
                "stop_reason = $stopReason, " +
                "completed_iterations = $completedIterations, " +
                "peak_light_value = $peakLightValue, " +
                "total_reward = $totalReward, " +
                "success = $success " +
                "WHERE id = $sessionId;";
            command.Parameters.AddWithValue("$endedUtc", FormatUtc(endedUtc));
            command.Parameters.AddWithValue("$stopReason", stopReason ?? "unknown");
            command.Parameters.AddWithValue("$completedIterations", completedIterations);
            command.Parameters.AddWithValue("$peakLightValue", peakLightValue);
            command.Parameters.AddWithValue("$totalReward", totalReward);
            command.Parameters.AddWithValue("$success", success ? 1 : 0);
            command.Parameters.AddWithValue("$sessionId", sessionId);
            command.ExecuteNonQuery();
        }

        private static double ComputeConfidence(int updates, int successes, double qValue)
        {
            if (updates <= 0)
            {
                return 0.0;
            }

            double successRate = (double)successes / updates;
            double normalizedReward = (Math.Tanh(qValue / 3.0) + 1.0) * 0.5;
            double experienceFactor = Math.Min(1.0, updates / 50.0);
            double confidence = (successRate * 0.6 + normalizedReward * 0.4) * experienceFactor;
            return Math.Clamp(confidence, 0.0, 1.0);
        }

        private void EnsureSchema()
        {
            using var command = _connection.CreateCommand();
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS experiment_session (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "started_utc TEXT NOT NULL, " +
                "ended_utc TEXT, " +
                "stop_reason TEXT NOT NULL, " +
                "bump_sensor_port INTEGER NOT NULL, " +
                "light_sensor_port INTEGER NOT NULL, " +
                "max_iterations INTEGER NOT NULL, " +
                "completed_iterations INTEGER NOT NULL, " +
                "peak_light_value INTEGER NOT NULL, " +
                "total_reward REAL NOT NULL, " +
                "success INTEGER NOT NULL); " +
                "CREATE TABLE IF NOT EXISTS experiment_step (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "session_id INTEGER NOT NULL, " +
                "step_index INTEGER NOT NULL, " +
                "ts_utc TEXT NOT NULL, " +
                "sensor_raw INTEGER NOT NULL, " +
                "sensor_smooth INTEGER NOT NULL, " +
                "delta INTEGER NOT NULL, " +
                "reward REAL NOT NULL, " +
                "bump INTEGER NOT NULL, " +
                "scan_dir INTEGER NOT NULL, " +
                "scan_power INTEGER NOT NULL, " +
                "left_power INTEGER NOT NULL, " +
                "right_power INTEGER NOT NULL, " +
                "FOREIGN KEY(session_id) REFERENCES experiment_session(id)); " +
                "CREATE TABLE IF NOT EXISTS rl_point (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "session_id INTEGER NOT NULL, " +
                "state_key TEXT NOT NULL, " +
                "action_key TEXT NOT NULL, " +
                "reward REAL NOT NULL, " +
                "confidence REAL NOT NULL, " +
                "updated_utc TEXT NOT NULL, " +
                "FOREIGN KEY(session_id) REFERENCES experiment_session(id)); " +
                "CREATE TABLE IF NOT EXISTS rl_scorecard (" +
                "state_key TEXT NOT NULL, " +
                "action_key TEXT NOT NULL, " +
                "updates INTEGER NOT NULL, " +
                "successes INTEGER NOT NULL, " +
                "q_value REAL NOT NULL, " +
                "confidence REAL NOT NULL, " +
                "last_updated_utc TEXT NOT NULL, " +
                "PRIMARY KEY(state_key, action_key)); " +
                "CREATE INDEX IF NOT EXISTS idx_step_session ON experiment_step(session_id); " +
                "CREATE INDEX IF NOT EXISTS idx_rl_session ON rl_point(session_id); " +
                "CREATE INDEX IF NOT EXISTS idx_rl_state_action ON rl_point(state_key, action_key);";
            command.ExecuteNonQuery();
        }

        private static string FormatUtc(DateTime utc)
        {
            return utc.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
        }

        private IReadOnlyList<ScorecardEntry> QueryScorecardEntries(int limit, bool ascendingConfidence)
        {
            int safeLimit = Math.Clamp(limit, 1, 100);
            string direction = ascendingConfidence ? "ASC" : "DESC";

            using var command = _connection.CreateCommand();
            command.CommandText =
                "SELECT state_key, action_key, updates, successes, q_value, confidence, last_updated_utc " +
                "FROM rl_scorecard " +
                "ORDER BY confidence " + direction + ", updates DESC " +
                "LIMIT $limit;";
            command.Parameters.AddWithValue("$limit", safeLimit);

            var rows = new List<ScorecardEntry>();
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                rows.Add(new ScorecardEntry(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDouble(4),
                    reader.GetDouble(5),
                    DateTime.Parse(reader.GetString(6), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)));
            }

            return rows;
        }
    }
}
