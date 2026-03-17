namespace Haden.NxtSharp.Sensors
{
    /// <summary>
    /// The registry for the compass sensor.
    /// </summary>
    public enum NxtCompassRegister
    {
        /// <summary>
        /// The version of the sensor.
        /// </summary>
        SensorVersion = 0x00,
        /// <summary>
        /// The manufacturer of the sensor.
        /// </summary>
        Manufacturer = 0x08,
        /// <summary>
        /// The type of the sensor.
        /// </summary>
        SensorType = 0x10,
        /// <summary>
        /// The control mode of the sensor.
        /// </summary>
        ModeControl = 0x41,
        /// <summary>
        /// The two-degree heading of the sensor.
        /// </summary>
        HeadingTwoDegrees = 0x42,
        /// <summary>
        /// Adds the heading to the sensor.
        /// </summary>
        HeadingAdder = 0x43,
        /// <summary>
        /// Word format of the heading to the sensor.
        /// </summary>
        HeadingWord = 0x44,
    }
}