using System;
using System.Drawing;

namespace Haden.NXTRemote.Simulation.Drawing3D
{ 
    //orintation vector (0,0,-1)
    public class Camera
    {
        Point3D _location = new Point3D(0, 0, 0);
        double _d = 500.0;
        Quaternion _quantity = new Quaternion(1, 0, 0, 0);

        public Point3D Location
        {
            set { _location = value; }
            get { return _location; }
        }

        public double FocalDistance
        {
            set { _d = value; }
            get { return _d; }
        }

        public Quaternion Quaternion
        {
            set { _quantity = value; }
            get { return _quantity; }
        }

        public void MoveRight(double d)
        {
            _location.X += d;
        }

        public void MoveLeft(double d)
        {
            _location.X -= d;
        }

        public void MoveUp(double d)
        {
            _location.Y -= d;
        }

        public void MoveDown(double d)
        {
            _location.Y += d;
        }

        public void MoveIn(double d)
        {
            _location.Z += d;
        }

        public void MoveOut(double d)
        {
            _location.Z -= d;
        }

        public void Roll(int degree) // rotate around Z axis
        {
            Quaternion q = new Quaternion();
            q.FromAxisAngle(new Vector3D(0, 0, 1), degree * Math.PI / 180.0);
            _quantity = q * _quantity;
        }

        public void Yaw(int degree)  // rotate around Y axis
        {
            Quaternion q = new Quaternion();
            q.FromAxisAngle(new Vector3D(0, 1, 0), degree * Math.PI / 180.0);
            _quantity = q * _quantity;
        }

        public void Pitch(int degree) // rotate around X axis
        {
            Quaternion q = new Quaternion();
            q.FromAxisAngle(new Vector3D(1, 0, 0), degree * Math.PI / 180.0);
            _quantity = q * _quantity;
        }

        public void TurnUp(int degree)
        {
            Pitch(-degree);
        }

        public void TurnDown(int degree)
        {
            Pitch(degree);
        }

        public void TurnLeft(int degree)
        {
            Yaw(degree);
        }

        public void TurnRight(int degree)
        {
            Yaw(-degree);
        }

        public PointF[] GetProjection(Point3D[] pts)
        {
            PointF[] pt2Ds = new PointF[pts.Length];

            // transform to new coordinates system which origin is camera location
            Point3D[] pts1 = Point3D.Copy(pts);
            Point3D.Offset(pts1, -_location.X, -_location.Y, -_location.Z);

            // rotate
            _quantity.Rotate(pts1);

            //project
            for (int i = 0; i < pts.Length; i++)
            {
                if (pts1[i].Z > 0.1)
                {
                    pt2Ds[i] = new PointF((float)(_location.X + pts1[i].X * _d / pts1[i].Z),
                        (float)(_location.Y + pts1[i].Y * _d / pts1[i].Z));
                }
                else
                {
                    pt2Ds[i] = new PointF(float.MaxValue, float.MaxValue);
                }
            }
            return pt2Ds;
        }
    }
}
