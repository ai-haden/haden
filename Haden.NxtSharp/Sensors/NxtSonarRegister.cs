namespace Haden.NxtSharp.Sensors
{
    /// <summary>
    /// The register available to the sonar sensor.
    /// </summary>
    public enum NxtSonarRegister
    {
        /// <summary>
        /// The measurement units.
        /// </summary>
        MeasurementUnits = 0x14,
        /// <summary>
        /// The polling interval.
        /// </summary>
        PollInterval = 0x40,
        /// <summary>
        /// The sonar sensor mode.
        /// </summary>
        Mode = 0x41,
        /// <summary>
        /// The zeroth measurment byte.
        /// </summary>
        MeasurementByte0 = 0x42,
        /// <summary>
        /// The first measurment byte.
        /// </summary>
        MeasurementByte1 = 0x43,
        /// <summary>
        /// The second measurment byte.
        /// </summary>
        MeasurementByte2 = 0x44,
        /// <summary>
        /// The third measurment byte.
        /// </summary>
        MeasurementByte3 = 0x45,
        /// <summary>
        /// The fourth measurment byte.
        /// </summary>
        MeasurementByte4 = 0x46,
        /// <summary>
        /// The fifth measurment byte.
        /// </summary>
        MeasurementByte5 = 0x47,
        /// <summary>
        /// The sixth measurment byte.
        /// </summary>
        MeasurementByte6 = 0x48
    }
}
