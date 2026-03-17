using System.ComponentModel;

namespace Haden.NxtSharp.Sensors
{
    /// <summary>
    /// The class encapsulating the functionality of the sound sensor.
    /// </summary>
	public partial class NxtSonar : NxtSensor
    {
        int _rawValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NxtSonar"/> class.
        /// </summary>
		public NxtSonar()
        {
			InitializeComponent();
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="NxtSonar"/> class using IContainer.
        /// </summary>
        /// <param name="container">The container.</param>
		public NxtSonar(IContainer container)
        {
			container.Add(this);
			InitializeComponent();
		}
		/// <summary>
		/// Use the boolean mode.
		/// </summary>
		protected override NxtSensorMode Mode { get { return NxtSensorMode.Raw; } }
		/// <summary>
		/// The type of this sensor.
		/// </summary>
		protected override NxtSensorType Type { get { return NxtSensorType.LowSpeed_9V; } }
		/// <summary>
		/// The raw value of the current sensor reading.
		/// </summary>
		public override int RawValue { get { return _rawValue; } }
		/// <summary>
		/// Initializes the sonar sensor.
		/// </summary>
		public override void InitializeSensor()
        {
			base.InitializeSensor();

			//int status = Brick.Communicator.LsGetStatus(Port); // Commented until tested.
			// Read any returning message.
            if (Brick.Communicator.LsGetStatus(Port) > 0)
            {
                Brick.Communicator.LsRead(Port);
			}
			// Initialize continuous mode.
            Brick.Communicator.I2CSetByte(Port, (byte)NxtSonarRegister.Mode, 0x02);

		}
		/// <summary>
		/// Polls from the sonar sensor.
		/// </summary>
		public override void Poll()
        {
			if(Brick != null)
            {
				int previous = RawValue;
				_rawValue = ReadSonar();
				OnPolled();
				if(previous != RawValue)
                {
					OnValueChanged();
				}
			}
		}
		/// <summary>
		/// Reads the value from the sonar sensor.
		/// </summary>
		private int ReadSonar()
        {
            return Brick.Communicator.I2CGetByte(Port, (byte)NxtSonarRegister.MeasurementByte0);
		}
	}
}
