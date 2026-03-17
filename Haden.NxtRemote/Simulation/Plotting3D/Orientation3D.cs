using System;

namespace Haden.NXTRemote.Simulation.Plotting3D
{
    /// <summary>
    /// Represents whether an angle is specified in degrees or radians.
    /// </summary>
    public enum AngleMeasurement
    {
        /// <summary>
        /// Angles are represented in degrees
        /// </summary>
        Degrees,
        /// <summary>
        /// Angles are represented in radians
        /// </summary>
        Radians
    }

    /// <summary>
    /// Represents an orientation in 3D space.
    /// </summary>
    public class Orientation3D : ICloneable
    {
        const double PiDividedBy180 = Math.PI / 180;

        // These two unit vectors establish our orientation. These will move around as our direction changes.
        Vector3D _forwardVector;
        Vector3D _downVector;

        // This enumeration specifies whether we interpret angles as degrees or radians
        AngleMeasurement _angleMeasurement;

        /// <summary>
        /// Instantiates a new Orientation object.
        /// </summary>
        public Orientation3D()
        {
            _forwardVector = new Vector3D(1, 0, 0);
            _downVector = new Vector3D(0, 0, 1);

            _angleMeasurement = AngleMeasurement.Degrees;
        }
        /// <summary>
        /// Creates a deep copy of an existing orientation object.
        /// </summary>
        /// <param name="source">The source orientation for the copy.</param>
        private Orientation3D(Orientation3D source)
        {
            _forwardVector = source.ForwardVector;
            _downVector = source._downVector;

            _angleMeasurement = source._angleMeasurement;
        }
        /// <summary>
        /// Gets or sets whether angles are measured in degrees or radians.
        /// </summary>
        public AngleMeasurement AngleMeasurement
        {
            get
            {
                return _angleMeasurement;
            }
            set
            {
                if (value == AngleMeasurement.Degrees || value == AngleMeasurement.Radians)
                    _angleMeasurement = value;
                else
                    throw new ArgumentException("AngleMeasurement must be either Degrees or Radians.");
            }
        }
        /// <summary>
        /// Gets a unit vector representing the forward direction from the object's perspective.
        /// </summary>
        public Vector3D ForwardVector
        {
            get
            {
                return _forwardVector;
            }
        }
        /// <summary>
        /// Gets a unit vector representing the backward direction from the object's perspective.
        /// </summary>
        public Vector3D BackwardVector
        {
            get
            {
                return -_forwardVector;
            }
        }
        /// <summary>
        /// Gets a unit vector representing the left direction from the object's perspective.
        /// </summary>
        public Vector3D LeftVector
        {
            get
            {
                return -RightVector;
            }
        }
        /// <summary>
        /// Gets a unit vector representing the right direction from the object's perspective.
        /// </summary>
        public Vector3D RightVector
        {
            get
            {
                return DownVector.CrossProduct(ForwardVector);
            }
        }
        /// <summary>
        /// Gets a unit vector representing the up direction from the object's perspective.
        /// </summary>
        public Vector3D UpVector
        {
            get
            {
                return -_downVector;
            }
        }
        /// <summary>
        /// Gets a unit vector representing the down direction from the object's perspective.
        /// </summary>
        public Vector3D DownVector
        {
            get
            {
                return _downVector;
            }
        }
        /// <summary>
        /// Rotates right around the up/down axis.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void YawRight(double angle)
        {
            if (_angleMeasurement == AngleMeasurement.Degrees)
                angle = DegreesToRadians(angle);

            _forwardVector = _forwardVector.Rotate(_downVector, angle);
        }
        /// <summary>
        /// Rotates left around the up/down axis.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void YawLeft(double angle)
        {
            YawRight(-angle);
        }
        /// <summary>
        /// Rotates up around the left/right axis.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void PitchUp(double angle)
        {
            if (_angleMeasurement == AngleMeasurement.Degrees)
                angle = DegreesToRadians(angle);

            Vector3D rightVectorPreRotation = RightVector;

            _forwardVector = _forwardVector.Rotate(rightVectorPreRotation, angle);
            _downVector = _downVector.Rotate(rightVectorPreRotation, angle);
        }
        /// <summary>
        /// Rotates down around the left/right axis.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void PitchDown(double angle)
        {
            PitchUp(-angle);
        }
        /// <summary>
        /// Rotates right around the forward/backward axis.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void RollRight(double angle)
        {
            if (_angleMeasurement == AngleMeasurement.Degrees)
                angle = DegreesToRadians(angle);

            _downVector = _downVector.Rotate(_forwardVector, angle);
        }
        /// <summary>
        /// Rotates left around the forward/backward axis.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void RollLeft(double angle)
        {
            RollRight(-angle);
        }
        /// <summary>
        /// Converts from degrees to radians.
        /// </summary>
        /// <param name="degrees">An angle specified in degrees.</param>
        /// <returns>An angle specified in radians.</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * PiDividedBy180;
        }
        /// <summary>
        /// Converts from radians to degrees.
        /// </summary>
        /// <param name="radians">An angle specified in radians.</param>
        /// <returns>An angle specified in degrees.</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians / PiDividedBy180;
        }
        /// <summary>
        /// Performs a deep copy of the Orientation object.
        /// </summary>
        /// <returns>A deep copy of the Orientation object.</returns>
        public Orientation3D Clone()
        {
            return new Orientation3D(this);
        }
        /// <summary>
        /// Performs a deep copy of the Orientation object.
        /// </summary>
        /// <returns>A deep copy of the Orientation object.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
