using System;

namespace Haden.NxtSDK
{
    public enum NxtCommand : byte
    {
        SetOutputState = 0x04,
        SetInputMode = 0x05,
        GetOutputState = 0x06,
        GetInputValues = 0x07,
        ResetMotorPosition = 0x0A,
        GetBatteryLevel = 0x0B,
        KeepAlive = 0x0D
    }

    [Flags]
    public enum NxtMotorMode : byte
    {
        None = 0x00,
        MotorOn = 0x01,
        Brake = 0x02,
        Regulated = 0x04
    }

    [Flags]
    public enum NxtMotorRegulationMode : byte
    {
        Idle = 0x00,
        MotorSpeed = 0x01,
        MotorSynchronization = 0x02
    }

    [Flags]
    public enum NxtMotorRunState : byte
    {
        Idle = 0x00,
        RampUp = 0x10,
        Running = 0x20,
        Rampdown = 0x40
    }

    public enum NxtMotorPort : byte
    {
        PortA = 0x00,
        PortB = 0x01,
        PortC = 0x02,
        None = 0xFE,
        All = 0xFF
    }

    public enum NxtSensorPort : byte
    {
        Port1 = 0x00,
        Port2 = 0x01,
        Port3 = 0x02,
        Port4 = 0x03,
        None = 0xFE
    }

    public enum NxtSensorType : byte
    {
        None = 0x00,
        Switch = 0x01,
        Temperature = 0x02,
        Reflection = 0x03,
        Angle = 0x04,
        LightActive = 0x05,
        LightInactive = 0x06,
        SoundDb = 0x07,
        SoundDba = 0x08,
        Custom = 0x09,
        LowSpeed = 0x0A,
        LowSpeed9V = 0x0B
    }

    public enum NxtSensorMode : byte
    {
        Raw = 0x00,
        Boolean = 0x20,
        TransitionCounter = 0x40,
        PeriodCounter = 0x60,
        Percentage = 0x80,
        Celsius = 0xA0,
        Fahrenheit = 0xC0,
        AngleStep = 0xE0
    }

    public enum NxtMessageResult : byte
    {
        Ok = 0x00,
        CommBusError = 0xDD,
        ChannelBusy = 0xE0
    }

    public struct NxtGetInputValues
    {
        public bool Valid;
        public bool Calibrated;
        public NxtSensorType Type;
        public NxtSensorMode Mode;
        public ushort RawAd;
        public ushort NormalizedAd;
        public short ScaledValue;
        public short CalibratedValue;
    }

    public struct NxtGetOutputState
    {
        public sbyte Power;
        public NxtMotorMode Mode;
        public NxtMotorRegulationMode RegulationMode;
        public sbyte TurnRatio;
        public NxtMotorRunState RunState;
        public uint TachoLimit;
        public int TachoCount;
        public int BlockTachoCount;
        public int RotationCount;
    }
}
