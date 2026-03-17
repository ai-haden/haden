using System.Drawing;

namespace Haden.NXTRemote.Simulation.Drawing3D
{
    /// <summary>
    /// A structure for a set of points forming a three-dimensional projection.
    /// </summary>
    public struct Point3D
    {
        /// <summary>
        /// A system of coordinates following the right-hand rule.
        /// </summary>
        public double X, Y, Z;
        /// <summary>
        /// Initializes a new instance of the <see cref="Point3D"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Point3D(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        public Point3D(Vector3D v)
        {
            X = v.X; Y = v.Y; Z = v.Z;
        }

        public Point3D Copy()
        {
            return new Point3D(X, Y, Z);
        }

        public Vector3D ToVector3D()
        {
            return new Vector3D(X, Y, Z);
        }

        public void Offset(double x, double y, double z)
        {
            X += x;
            Y += y;
            Z += z;
        }

        public static Point3D[] Copy(Point3D[] pts)
        {
            Point3D[] copy = new Point3D[pts.Length];
            for (int i = 0; i < pts.Length; i++)
            {
                copy[i] = pts[i].Copy();
            }
            return copy;
        }

        public static void Offset(Point3D[] pts, double offsetX, double offsetY, double offsetZ)
        {
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].Offset(offsetX, offsetY, offsetZ);
            }
        }
        /// <summary>
        /// Gets the projected point.
        /// </summary>
        /// <param name="distance">The projection distance, from eye to the screen.</param>
        /// <returns></returns>
        public PointF GetProjectedPoint(double distance)
        {
            return new PointF((float)(X * distance / (distance + Z)), (float)(Y * distance / (distance + Z)));
        }
        /// <summary>
        /// Projects the specified points.
        /// </summary>
        /// <param name="points">The array of points.</param>
        /// <param name="distance">The projection distance, from eye to the screen.</param>
        /// <returns></returns>
        public static PointF[] Project(Point3D[] points, double distance)
        {
            PointF[] pt2Ds = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pt2Ds[i] = points[i].GetProjectedPoint(distance);
            }
            return pt2Ds;
        }
    }
}
