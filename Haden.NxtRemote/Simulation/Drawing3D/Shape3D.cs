using System.Drawing;

namespace Haden.NXTRemote.Simulation.Drawing3D
{
    /// <summary>
    /// A representation of a three-dimensional shape.
    /// </summary>
    public class Shape3D
    {
        protected Point3D[] Points = new Point3D[8];
        protected Point3D Center = new Point3D(0, 0, 0);
        protected Point Center2D = new Point(0, 0);
        protected Color CurrentLineColor = Color.Black;

        public Point3D CenterPoint
        {
            set
            {
                double dx = value.X - Center.X;
                double dy = value.Y - Center.Y;
                double dz = value.Z - Center.Z;
                Center2D.X = (int)Center.X;
                Center2D.Y = (int)Center.Y;
                Point3D.Offset(Points, dx, dy, dz);
                Center = value;
            }
            get { return Center; }
        }
        public Point ObjectCenter
        {
            get { return Center2D; } 
        }
        public Point3D[] Point3DArray
        {
            get { return Points; }
        }
        public Color LineColor
        {
            set { CurrentLineColor = value; }
            get { return CurrentLineColor; }
        }

        public void RotateAt(Point3D pt, Quaternion q)
        {
            // transform origin to pt
            Point3D[] copy = Point3D.Copy(Points);
            Point3D.Offset(copy, -pt.X, -pt.Y, -pt.Z);

            // rotate
            q.Rotate(copy);
            q.Rotate(Center);

            // transform to original origin
            Point3D.Offset(copy, pt.X, pt.Y, pt.Z);
            Points = copy;
        }

        public virtual void Draw(Graphics g, Camera camera) { }
    }
}
