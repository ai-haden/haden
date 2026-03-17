using System;
using System.Drawing;

namespace Haden.NXTRemote.Simulation.Plotting3D
{
    /// <summary>
    /// Plots points and draws lines in 3D space.
    /// </summary>
    public class Plotter3D : IDisposable
    {
        /// <summary>
        /// Represents the orientation of the cursor in 3D space.
        /// </summary>
        Orientation3D _orientation;
        /// <summary>
        /// The pen we use to draw on the canvas.
        /// </summary>
        private Pen _pen;
        /// <summary>
        /// The location of the "camera" that we use to determine perspective. All points are projected onto a "screen" that is the XY plane at the Z origin, and moving the camera around messes with the perspective.
        /// </summary>
        private readonly Point3D _cameraLocation = new Point3D(60, 0, -600);
        /// <summary>
        /// If the pen is down, we draw lines when we move forward.  If not, we just change our location without drawing any lines.
        /// </summary>
        private bool _isPenDown = true;
        /// <summary>
        /// The graphics object that we're drawing on.  This can be any graphics object, be it from a windows Form, a Bitmap, or a Metafile.
        /// </summary>
        readonly Graphics _canvas;
        /// <summary>
        /// A rectangle that represents the boundary of the object that have been drawn.
        /// </summary>
        Rectangle _boundingBox;
        /// <summary>
        /// Instantiates a new Plotter.
        /// </summary>
        /// <param name="canvas">The Graphics object that we want to draw on.</param>
        public Plotter3D(Graphics canvas) : this(canvas, new Pen(Color.Black)) {}
        /// <summary>
        /// Instantiates a new Plotter.
        /// </summary>
        /// <param name="canvas">The Graphics object that we want to draw on.</param>
        /// <param name="pen">The pen we want to use to draw on the canvas.</param>
        public Plotter3D(Graphics canvas, Pen pen) : this(canvas, pen, new Point3D(-30, 0, -600)) {}
        /// <summary>
        /// Instantiates a new Plotter.
        /// </summary>
        /// <param name="canvas">The Graphics object that we want to draw on.</param>
        /// <param name="cameraLocation">The location of the camera that we use to calculate perspective.</param>
        public Plotter3D(Graphics canvas, Point3D cameraLocation) : this(canvas, new Pen(Color.Black), cameraLocation) {}
        /// <summary>
        /// Instantiates a new Plotter.
        /// </summary>
        /// <param name="canvas">The Graphics object that we want to draw on.</param>
        /// <param name="pen">The pen we want to use to draw on the canvas.</param>
        /// <param name="cameraLocation">The location of the camera that we use to calculate perspective.</param>
        public Plotter3D(Graphics canvas, Pen pen, Point3D cameraLocation)
        {
            _canvas = canvas;
            _pen = pen;
            _cameraLocation = cameraLocation;

            _orientation = new Orientation3D();
        }
        /// <summary>
        /// Gets or sets the orientation of the cursor.
        /// </summary>
        public Orientation3D Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Orientation cannot be null.");

                _orientation = value;
            }
        }
        /// <summary>
        /// Gets or sets the pen used to draw on the canvas.
        /// </summary>
        public Pen Pen
        {
            get
            {
                return _pen;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Pen cannot be null.");

                try
                {
                    _pen.Dispose();
                }
                catch (ArgumentException)
                {
                    // If the pen is immutable, like one from the System.Drawing.Pens collection,
                    // it'll throw an exception when we try to dispose it.  I don't think there's
                    // any reasonable way to find out if a pen is immutable other than to try and 
                    // dispose it, though.  Which just seems silly.  Anyway, this exception may 
                    // happen in the normal course of things, in which case we just silently ignore it.
                }

                _pen = value;
            }
        }
        /// <summary>
        /// Gets or sets the color of the pen used to draw on the canvas.
        /// </summary>
        public Color PenColor
        {
            get
            {
                return _pen.Color;
            }
            set
            {
                Pen newPen = (Pen)_pen.Clone();
                newPen.Color = value;

                try
                {
                    _pen.Dispose();
                }
                catch (ArgumentException)
                {
                    // If the pen is immutable, like one from the System.Drawing.Pens collection,
                    // it'll throw an exception when we try to dispose it.  I don't think there's
                    // any reasonable way to find out if a pen is immutable other than to try and 
                    // dispose it, though.  Which just seems silly.  Anyway, this exception may 
                    // happen in the normal course of things, in which case we just silently ignore it.
                }

                _pen = newPen;
            }
        }
        /// <summary>
        /// Gets or sets the width of the pen used to draw on the canvas.
        /// </summary>
        public float PenWidth
        {
            get
            {
                return _pen.Width;
            }
            set
            {
                Pen newPen = (Pen)_pen.Clone();
                newPen.Width = value;

                try
                {
                    _pen.Dispose();
                }
                catch (ArgumentException)
                {
                    // If the pen is immutable, like one from the System.Drawing.Pens collection,
                    // it'll throw an exception when we try to dispose it.  I don't think there's
                    // any reasonable way to find out if a pen is immutable other than to try and 
                    // dispose it, though.  Which just seems silly.  Anyway, this exception may 
                    // happen in the normal course of things, in which case we just silently ignore it.
                }

                _pen = newPen;
            }
        }
        /// <summary>
        /// Gets or sets the drawing mode of the plotter.  If IsPenDown == true, moving the pen
        /// around will draw on the canvas.  If IsPenDown == false, moving the pen around will 
        /// change the position of the pen, but won't draw anything.
        /// </summary>
        public bool IsPenDown
        {
            get
            {
                return _isPenDown;
            }
            set
            {
                _isPenDown = value;
            }
        }
        /// <summary>
        /// Gets or sets the cursor's location in 3D space.
        /// </summary>
        public Point3D Location { get; set; } = new Point3D(0, 0, 0);
        /// <summary>
        /// Gets the location of the camera used to determine perspective
        /// </summary>
        public Point3D CameraLocation
        {
            get
            {
                return _cameraLocation;
            }
        }
        /// <summary>
        /// Gets the graphics object to draw on.
        /// </summary>
        public Graphics Canvas
        {
            get
            {
                return _canvas;
            }
        }
        /// <summary>
        /// Gets or sets whether angles are measured in degrees or radians.
        /// </summary>
        public AngleMeasurement AngleMeasurement
        {
            get
            {
                return _orientation.AngleMeasurement;
            }
            set
            {
                _orientation.AngleMeasurement = value;
            }
        }
        /// <summary>
        /// Gets a rectangle that contains everything that's been drawn so far.
        /// </summary>
        /// <remarks>
        /// The math here is a little imprecise (especially when you're dealing with
        /// a PenWidth > 1) but the box should contain AT LEAST the bounds of the drawing,
        /// possibly with a couple of pixels of padding on each side.
        /// </remarks>
        public Rectangle BoundingBox
        {
            get
            {
                return _boundingBox;
            }
        }
        /// <summary>
        /// Moves the cursor forward, and draws a line from the start point to the end point
        /// if IsPenDown == true.
        /// </summary>
        /// <param name="distance">The distance to move forward.</param>
        public void Forward(double distance)
        {
            Point3D oldLocation = Location;

            Location += (Orientation.ForwardVector * distance);

            if (IsPenDown)
            {
                _canvas.DrawLine(_pen, oldLocation.GetScreenPosition(_cameraLocation), Location.GetScreenPosition(_cameraLocation));

                ExpandBoundingBox(oldLocation.GetScreenPosition(CameraLocation));
                ExpandBoundingBox(Location.GetScreenPosition(CameraLocation));
            }
        }
        /// <summary>
        /// Moves the cursor to the specified location, and draws a line from teh start point 
        /// to the end point if IsPenDown == true.
        /// </summary>
        /// <param name="newLocation">The location to move the cursor to.</param>
        /// <remarks>
        /// This method allows you to draw a line to an absolute point, which runs counter
        /// to the spirit of this library.  This library uses relative positioning, which 
        /// allows you to define an object relatively, then move or rotate it however you like,
        /// and that all falls apart as soon as you start using absolute positioning.  Nevertheless,
        /// there are some times when it's useful to be able to draw a line to an absolute position,
        /// so there are times when this method is handy.  But use it sparingly.
        /// </remarks>
        public void MoveTo(Point3D newLocation)
        {
            Point3D oldLocation = Location;

            Location = newLocation;

            if (IsPenDown)
            {
                _canvas.DrawLine(_pen, oldLocation.GetScreenPosition(_cameraLocation), Location.GetScreenPosition(_cameraLocation));

                ExpandBoundingBox(oldLocation.GetScreenPosition(CameraLocation));
                ExpandBoundingBox(Location.GetScreenPosition(CameraLocation));
            }
        }
        /// <summary>
        /// Sets the IsPenDown property to true.
        /// </summary>
        /// <remarks>
        /// This method has been included because I think that 
        ///     p.PenDown();
        /// is more readable than
        ///     p.IsPenDown = true;
        /// </remarks>
        public void PenDown()
        {
            IsPenDown = true;
        }
        /// <summary>
        /// Sets the IsPenDown property to false.
        /// </summary>
        /// <remarks>
        /// This method has been included because I think that
        ///     p.PenUp();
        /// is more readable than
        ///     p.IsPenDown = false;
        /// </remarks>
        public void PenUp()
        {
            IsPenDown = false;
        }
        /// <summary>
        /// Rotates the cursor right, relative to its current orientation.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void TurnRight(double angle)
        {
            Orientation.YawRight(angle);
        }
        /// <summary>
        /// Rotates the cursor left, relative to its current orientation.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void TurnLeft(double angle)
        {
            TurnRight(-angle);
        }
        /// <summary>
        /// Rotates the cursor up, relative to its current orientation.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void TurnUp(double angle)
        {
            Orientation.PitchUp(angle);
        }
        /// <summary>
        /// Rotates the cursor down, relative to its current orientation.
        /// </summary>
        /// <param name="angle">
        /// The rotation angle in degrees or radians depending on the value
        /// of the AngleMeasurement property.
        /// </param>
        public void TurnDown(double angle)
        {
            TurnUp(-angle);
        }
        /// <summary>
        /// Expands the bounding box as you draw so that the box always contains the
        /// drawing entirely.
        /// </summary>
        /// <param name="point">A new point being drawn.</param>
        private void ExpandBoundingBox(PointF point)
        {
            int currentLeft;
            int currentRight;
            int currentTop;
            int currentBottom;

            if (_boundingBox == Rectangle.Empty)
            {
                currentLeft = int.MaxValue;
                currentRight = int.MinValue;
                currentTop = int.MaxValue;
                currentBottom = int.MinValue;
            }
            else
            {
                currentLeft = _boundingBox.Left;
                currentRight = _boundingBox.Right;
                currentTop = _boundingBox.Top;
                currentBottom = _boundingBox.Bottom;
            }

            int halfPenSize = (int)(Pen.Width / 2);

            int newLeft = (int)Math.Floor(Math.Min(point.X - halfPenSize - 2, currentLeft));
            int newRight = (int)Math.Ceiling(Math.Max(point.X + halfPenSize + 1, currentRight));
            int newTop = (int)Math.Floor(Math.Min(point.Y - halfPenSize - 2, currentTop));
            int newBottom = (int)Math.Ceiling(Math.Max(point.Y + halfPenSize + 1, currentBottom));

            _boundingBox = Rectangle.FromLTRB(newLeft, newTop, newRight, newBottom);
        }
        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Disposes of the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    _pen.Dispose();
                }
                catch (ArgumentException)
                {
                    // If the pen is immutable, like one from the System.Drawing.Pens collection,
                    // it'll throw an exception when we try to dispose it.  I don't think there's
                    // any reasonable way to find out if a pen is immutable other than to try and 
                    // dispose it, though.  Which just seems silly.  Anyway, this exception may 
                    // happen in the normal course of things, in which case we just silently ignore it.
                }
            }
        }
    }  
}
