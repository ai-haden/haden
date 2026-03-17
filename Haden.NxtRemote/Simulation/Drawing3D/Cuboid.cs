using System;
using System.Drawing;
using Haden.NXTRemote.Simulation.Imaging.Filters;

namespace Haden.NXTRemote.Simulation.Drawing3D
{
    /// <summary>
    /// The three-dimensional cuboid shape.
    /// </summary>
    public class Cuboid : Shape3D
    {
        bool _drawingLine, _fillingFace = true, _drawingImage;
        /// <summary>
        /// Gets or sets a value indicating whether [drawing line].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [drawing line]; otherwise, <c>false</c>.
        /// </value>
        public bool DrawingLine { set { _drawingLine = value; } get { return _drawingLine; } }
        /// <summary>
        /// Gets or sets a value indicating whether [filling face].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [filling face]; otherwise, <c>false</c>.
        /// </value>
        public bool FillingFace { set { _fillingFace = value; } get { return _fillingFace; } }
        /// <summary>
        /// Gets or sets a value indicating whether [drawing image].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [drawing image]; otherwise, <c>false</c>.
        /// </value>
        public bool DrawingImage { set { _drawingImage = value; } get { return _drawingImage; } }
        /// <summary>
        /// The face color.
        /// </summary>
        readonly Color[] _faceColor = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple };
        /// <summary>
        /// Initializes a new instance of the <see cref="Cuboid"/> class. A three-dimensional cubic shape with arugments of its position in x, y, and z.
        /// </summary>
        /// <param name="a">The x-parameter.</param>
        /// <param name="b">The y-parameter.</param>
        /// <param name="c">The z-parameter.</param>
        public Cuboid(double a, double b, double c)
        {
            Center = new Point3D(a / 2, b / 2, c / 2);
            Points[0] = new Point3D(0, 0, 0);
            Points[1] = new Point3D(a, 0, 0);
            Points[2] = new Point3D(a, b, 0);
            Points[3] = new Point3D(0, b, 0);
            Points[4] = new Point3D(0, 0, c);
            Points[5] = new Point3D(a, 0, c);
            Points[6] = new Point3D(a, b, c);
            Points[7] = new Point3D(0, b, c);
        }
        /// <summary>
        /// Gets or sets the face color array.
        /// </summary>
        public Color[] FaceColorArray
        {
            set
            {
                int n = Math.Min(value.Length, _faceColor.Length);
                for (int i = 0; i < n; i++)
                    _faceColor[i] = value[i];
            }
            get { return _faceColor; }
        }
        /// <summary>
        /// The a bitmap.
        /// </summary>
        readonly Bitmap[] _bmp = new Bitmap[6];
        /// <summary>
        /// Gets or sets the face image array.
        /// </summary>
        public Bitmap[] FaceImageArray
        {
            set
            {
                int n = Math.Min(value.Length, 6);
                for (int i = 0; i < n; i++)
                    _bmp[i] = value[i];
                SetupFilter();
            }
            get { return _bmp; }
        }
        /// <summary>
        /// The filters transform
        /// </summary>
        readonly FreeTransform[] _filters = new FreeTransform[6];
        /// <summary>
        /// Set-up the filter.
        /// </summary>
        private void SetupFilter()
        {
            for (int i = 0; i < 6; i++)
            {
                _filters[i] = new FreeTransform();
                _filters[i].Bitmap = _bmp[i];
            }
        }
        /// <summary>
        /// Draws the specified cuboid.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="camera">The camera.</param>
        public override void Draw(Graphics g, Camera camera)
        {
            PointF[] pts2D = camera.GetProjection(Points);

            PointF[][] face = new PointF[6][];
            face[0] = new PointF[] { pts2D[0], pts2D[1], pts2D[2], pts2D[3] };
            face[1] = new PointF[] { pts2D[5], pts2D[1], pts2D[0], pts2D[4] };
            face[2] = new PointF[] { pts2D[1], pts2D[5], pts2D[6], pts2D[2] };
            face[3] = new PointF[] { pts2D[2], pts2D[6], pts2D[7], pts2D[3] };
            face[4] = new PointF[] { pts2D[3], pts2D[7], pts2D[4], pts2D[0] };
            face[5] = new PointF[] { pts2D[4], pts2D[7], pts2D[6], pts2D[5] };

            for (int i = 0; i < 6; i++)
            {
                bool isout = false;
                for (int j = 0; j < 4; j++)
                {
                    if (face[i][j] == new PointF(float.MaxValue, float.MaxValue))
                    {
                        isout = true;
                    }
                }
                if (!isout)
                {
                    if (_drawingLine) g.DrawPolygon(new Pen(CurrentLineColor), face[i]);
                    if (Vector.IsClockwise(face[i][0], face[i][1], face[i][2])) // the face can be seen by camera
                    {
                        if (_fillingFace) g.FillPolygon(new SolidBrush(_faceColor[i]), face[i]);
                        if (_drawingImage && _bmp[i] != null)
                        {
                            _filters[i].FourCorners = face[i];
                            g.DrawImage(_filters[i].Bitmap, _filters[i].ImageLocation);
                        }
                    }
                }
            }
        }
    }
}
