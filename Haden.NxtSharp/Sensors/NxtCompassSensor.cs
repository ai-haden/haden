using System.ComponentModel;

namespace Haden.NxtSharp.Sensors
{
	/// <summary>
	/// This class enables using the HiTechnic Compass sensor. Is used in tandem with an LBS addon.
	/// </summary>
	public partial class NxtCompassSensor : NxtSensor
    {
        bool _useDoublePrecision = true;
        int _rawValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NxtCompassSensor"/> class.
        /// </summary>
		public NxtCompassSensor()
        {
			InitializeComponent();
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="NxtCompassSensor"/> class using the IContainer.
        /// </summary>
        /// <param name="container">The container.</param>
		public NxtCompassSensor(IContainer container)
        {
			container.Add(this);
			InitializeComponent();
		}
		/// <summary>
		/// Should the sensor use double precision? This is more precies but also slower.
		/// </summary>
		[Category("Compass sensor settings"), Description("Should the compass sensor use double precision? This is more precies but also slower.")]
		public bool UseDoublePrecision {
			get {
				return _useDoublePrecision;
			}
			set {
				_useDoublePrecision = value;
			}
		}
		/// <summary>
		/// Use the boolean mode.
		/// </summary>
		[Browsable(false)]
		protected override NxtSensorMode Mode { get { return NxtSensorMode.Raw; } }
		/// <summary>
		/// The type of this sensor. Uses I2C communication.
		/// </summary>
		[Browsable(false)]
		protected override NxtSensorType Type { get { return NxtSensorType.LowSpeed_9V; } }
		/// <summary>
		/// The raw value of this sensor reading.
		/// </summary>
		[Browsable(false)]
		public override int RawValue { get { return _rawValue; } }
		/// <summary>
		/// Initializes the sonar sensor.
		/// </summary>
		public override void InitializeSensor()
        {
			base.InitializeSensor();
			// Read any returning messages.
            Brick.Communicator.LsRead(Port);
		}
		/// <summary>
		/// Polls the sonar sensor.
		/// </summary>
		public override void Poll()
        {
			if(Brick != null) {
				int previous = RawValue;
				_rawValue = ReadCompass();
				OnPolled();
				if(previous != RawValue) {
					OnValueChanged();
				}
			}
		}
		/// <summary>
		/// Reads a value from the compass sensor.
		/// </summary>
		private int ReadCompass()
        {
            int result = 2 * Brick.Communicator.I2CGetByte(Port, (byte)NxtCompassRegister.HeadingTwoDegrees);
			if(UseDoublePrecision)
            {
                result += Brick.Communicator.I2CGetByte(Port, (byte)NxtCompassRegister.HeadingAdder);
			}
			return result;
		}
	}
}
