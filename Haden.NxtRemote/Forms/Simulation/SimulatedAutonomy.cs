using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Haden.NXTRemote.Controls;
using Haden.NXTRemote.Forms.Child;
using Haden.NXTRemote.Simulation.Drawing3D;
using Timer = System.Windows.Forms.Timer;

namespace Haden.NXTRemote.Forms.Simulation
{
    public partial class SimulatedAutonomy : Form
    {
        private bool _isSelected;
        private int _x, _y;
        protected Timer PollingTimer;
        public int Iterator;
        private Form _owner;
        // Step 0: Primitives
        private readonly ContextMenuStrip _linecmenu;
        private readonly Cuboid _station = new Cuboid(75, 75, 75);
        private readonly Cuboid _robot = new Cuboid(50, 60, 45);
        private readonly Camera _cam = new Camera();
        private Pen Ray;
        private Graphics RobotRay;
        private Graphics StationRay;
        public ArrayList Lines;
        public static bool Instance { get; set; }
        public static int CurrentValue { get; set; }
        public static int OptimalValue { get; set; }

        public SimulatedAutonomy()
        {
            InitializeComponent();
            _linecmenu = linecmenu;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = true;
            ControlBox = true;
            Instance = true;

            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            _cam.Location = new Point3D(400, 240, -500);
            Lines = new ArrayList();
        }
        /// <summary>
        /// I think it wise to explore leverage of RL and forward-forward neural networks for this project.
        /// </summary>
        /// <param name="mOwner"></param>
        /// <remarks>This is the oldest robotics project in my repetoire.</remarks>
        public SimulatedAutonomy(Form mOwner)
        {
            InitializeComponent();
            _owner = mOwner;
            _linecmenu = linecmenu;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = true;
            ControlBox = true;
            Instance = true;

            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            _cam.Location = new Point3D(400, 240, -500);
            Lines = new ArrayList();
        }

        // Step 1: Draw the environment (shapes) - "robot" and "station"
        public void DrawEnvironment()
        {
            Ray = new Pen(Color.Blue);
            _robot.CenterPoint = new Point3D(400, 200, 50);
            RobotRay = CreateGraphics();
            //RobotRay.DrawLine(Ray, 0, 0, 400, 240);
            _station.CenterPoint = new Point3D(100, 60, 40);
            StationRay = CreateGraphics();
            //StationRay.DrawLine(Ray, 0, 100, 60, 40);
            Invalidate();
        }
        // Step 2: Cast a ray to locate the object robot --> station
        public void CastSeekingRay()
        {
            // Adds marks that are the beginning/end of the line.
            MarkBox mark1 = new MarkBox();
            mark1.Location = new Point(400, 200);
            mark1.ContextMenuStrip = _linecmenu;
            if (ActiveForm != null)
            {
                ActiveForm.Controls.Add(mark1);

                MarkBox mark2 = new MarkBox();
                mark2.Location = new Point(100, 60);
                mark2.ContextMenuStrip = _linecmenu;
                ActiveForm.Controls.Add(mark2);

                // Line Struct contains the information for a single line.
                Line line = new Line();
                line.FirstMark = mark1;
                line.SecondMark = mark2;
                line.Width = 1;

                // Events for moving marks
                mark1.MouseUp += Mark_MouseUp;
                mark1.MouseDown += Mark_MouseDown;
                mark1.MouseMove += Mark_MouseMove;

                mark2.MouseUp += Mark_MouseUp;
                mark2.MouseDown += Mark_MouseDown;
                mark2.MouseMove += Mark_MouseMove;

                // Adds Line object to an arraylist.
                Lines.Add(line);
            }
            Redraw();
        }
        // Step 3: Navigate toward the station using the ray as guide.
        public void FindStation()
        {
            SeekLeftOrRight();
        }
        // Step 4: Autonomy routines (Find)
        public void SeekLeftOrRight()
        {
            Random seekLeftRight = new Random();
            int leftRight = seekLeftRight.Next(1, 3);
            if (leftRight.Equals(1))
            {
                AutonomousTurn("left");
                BoxedLastTurn = "left";
            }
            else
            {
                AutonomousTurn("right");
                BoxedLastTurn = "right";
            }
            PollingTimer = new Timer();
            PollingTimer.Interval = 1000;
            PollingTimer.Start();
            PollingTimer.Tick += PollingTimer_OnTick;
        }
        public void AutonomousTurn(string direction)
        {
            switch (direction)
            {
                case "left":
                    seekLeftIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
                    seekLeftIndicator.FlashNumber = 2;
                    BoxedLastTurn = "left";
                    // Move items in the environment.
                    Quaternion qx = new Quaternion();
                    qx.FromAxisAngle(new Vector3D(0, 0, 1), 5 * Math.PI / 180.0);
                    _robot.RotateAt(_robot.CenterPoint, qx);
                    Invalidate();
                    Thread.Sleep(250);
                    break;
                case "right":
                    seekRightIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
                    seekRightIndicator.FlashNumber = 2;
                    BoxedLastTurn = "right";
                    // Move items in the environment
                    Quaternion qy = new Quaternion();
                    qy.FromAxisAngle(new Vector3D(0, 0, 1), -5 * Math.PI / 180.0);
                    _robot.RotateAt(_robot.CenterPoint, qy);
                    Invalidate();
                    Thread.Sleep(250);
                    break;
            }

            //Ray = new Pen(Color.Red);
            //Graphics formGraphics = CreateGraphics();
            //formGraphics.DrawLine(Ray, 0, 0, 200, 200);
            //Invalidate();
            //Ray.Dispose();
            //formGraphics.Dispose();
        }
        public void PollingTimer_OnTick(object sender, EventArgs e)
        {
            Random dummySensorValue = new Random();
            int leftRight = dummySensorValue.Next(12, 60);
            BoxedCurrentValue = leftRight;
            CalculateSensorValueDifference();
        }
        // Step 'X': Mathematical operations (application support)
        public void CalculateSensorValueDifference()
        {
            int differingValue = (int)BoxedCurrentValue - (int)BoxedOptimalValue;
            if (differingValue > 0)
            {
                BoxedOptimalValue = BoxedCurrentValue;
                BoxedDifferenceValue = differingValue;
                // Turn in the same direction.
                if (BoxedLastTurn.ToString() == "left")
                    AutonomousTurn("left");
                // Move the simulated item--how to represent tracking?
                if (BoxedLastTurn.ToString() == "right")
                    AutonomousTurn("right");
            }
            else if (differingValue < 0)
            {
                // Turn in the opposite direction.
                if (BoxedLastTurn.ToString() == "left")
                    AutonomousTurn("right");
                if (BoxedLastTurn.ToString() == "right")
                    AutonomousTurn("left");
            }
            else if (differingValue == 0)
            {
                // Switch between one to the left and one to the right.
                if (BoxedLastTurn.ToString() == "left")
                    AutonomousTurn("left");
                if (BoxedLastTurn.ToString() == "right")
                    AutonomousTurn("right");
                Iterator++;
            }

        }

        #region Boxed values
        public object BoxedCurrentValue;
        public object BoxedOptimalValue = OptimalValue;
        public object BoxedLastTurn;
        public object BoxedDifferenceValue;
        #endregion

        #region Line context menu
        private void Delete_Click(object sender, EventArgs e)
        {
            Line l = GetLineByMark(((MarkBox)linecmenu.SourceControl));
            Lines.Remove(l);
            l.FirstMark.Dispose();
            l.SecondMark.Dispose();
            linecmenu.SourceControl.Dispose();
            Redraw();
        }
        private void NewLine_Click(object sender, EventArgs e)
        {
            int x1 = 0;
            int x2 = 0;
            int y1 = 0;
            int y2 = 0;
            CoordinatePositionEntryDialog cpd = new CoordinatePositionEntryDialog();
            DialogResult res = cpd.ShowDialog();
            if (res == DialogResult.OK)
            {
                string xf = cpd.FirstCoordinateX.Text;
                string xs = cpd.SecondCoordinateX.Text;
                string yf = cpd.FirstCoordinateY.Text;
                string ys = cpd.SecondCoordinateY.Text;
                x1 = Convert.ToInt32(xf);
                x2 = Convert.ToInt32(xs);
                y1 = Convert.ToInt32(yf);
                y2 = Convert.ToInt32(ys);
            }

            // Adds marks that are the beginning/end of the line.
            MarkBox mark1 = new MarkBox();
            mark1.Location = new Point(x1, y1);
            mark1.ContextMenuStrip = _linecmenu;
            if (ActiveForm != null)
            {
                ActiveForm.Controls.Add(mark1);

                MarkBox mark2 = new MarkBox();
                mark2.Location = new Point(x2, y2);
                mark2.ContextMenuStrip = _linecmenu;
                ActiveForm.Controls.Add(mark2);

                // Line Struct contains the information for a single line.
                Line line = new Line();
                line.FirstMark = mark1;
                line.SecondMark = mark2;
                line.Width = 1;

                // Events for moving marks
                mark1.MouseUp += Mark_MouseUp;
                mark1.MouseDown += Mark_MouseDown;
                mark1.MouseMove += Mark_MouseMove;

                mark2.MouseUp += Mark_MouseUp;
                mark2.MouseDown += Mark_MouseDown;
                mark2.MouseMove += Mark_MouseMove;

                // Adds Line object to an arraylist.
                Lines.Add(line);
            }
            Redraw();
        }
        private void Report_Click(object sender, EventArgs e)
        {
            // Get the coordinate where the mouse is placed.
            Line l = GetLineByMark(((MarkBox)linecmenu.SourceControl));
            Point firstCoordinate = l.FirstMark.Location;
            Point secondCoordinate = l.SecondMark.Location;
            // Create PopUp dialog to report coordinates of marks
            MessageBox.Show("Coordinates of this line are:  " + firstCoordinate.X + ", " + firstCoordinate.Y + " and " + secondCoordinate.X + ", " + secondCoordinate.Y + ".", "Line information");
        }
        private void Width_Click(object sender, EventArgs e)
        {
            Line l;
            LineWidthDialog wd = new LineWidthDialog(GetLineByMark((MarkBox)linecmenu.SourceControl).Width);
            DialogResult res = wd.ShowDialog();
            if (res == DialogResult.OK)
            {
                l = GetLineByMark((MarkBox)linecmenu.SourceControl);
                Lines.Remove(l);
                l.Width = wd.LineWidth;
                Lines.Add(l);
                Redraw();
            }
        }
        #endregion

        #region Events
        private void closeButton_Click(object sender, EventArgs e)
        {
            Instance = false;
            Close();
        }
        private void beginButton_Click(object sender, EventArgs e)
        {
            DrawEnvironment();
        }
        private void castRayButton_Click(object sender, EventArgs e)
        {
            CastSeekingRay();
        }
        private void findStationButton_Click(object sender, EventArgs e)
        {
            FindStation();
        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            PollingTimer.Stop();
            PollingTimer.Tick -= PollingTimer_OnTick;
        }
        private void otherSimButton_Click(object sender, EventArgs e)
        {
            if (HadenAutonomySimulator.Instance == false)
            {
                HadenAutonomySimulator form = new HadenAutonomySimulator(this);
                form.Show(this);
                HadenAutonomySimulator.Instance = true;
            }
            else if (HadenAutonomySimulator.Instance)
            {
                // Do nothing.
            }
        }
        private void Mark_MouseDown(object sender, MouseEventArgs e)
        {
            SuspendLayout();
            _isSelected = true;
            _x = e.X;
            _y = e.Y;
        }
        private void Mark_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isSelected)
            {
                MarkBox mb1 = (MarkBox)sender;
                Line l = GetLineByMark(mb1);
                Point p = new Point(e.X - _x + mb1.Left, e.Y - _y + mb1.Top);

                mb1.Location = p;
                Redraw(l, p);
            }
        }
        private void Mark_MouseUp(object sender, MouseEventArgs e)
        {
            _isSelected = false;
            ResumeLayout();
            Redraw();
        }
        private void DrawLine(Line line)
        {
            Graphics g = CreateGraphics();
            g.DrawLine(new Pen(Color.Black, line.Width), line.FirstMark.Center.X, line.FirstMark.Center.Y, line.SecondMark.Center.X, line.SecondMark.Center.Y);
            g.Dispose();
        }
        private void Redraw()
        {
            foreach (Line line in Lines)
            {
                DrawLine(line);
            }
        }
        private void Redraw(Line line, Point p)
        {
            foreach (Line l in Lines)
            {
                DrawLine(l);
            }

            Region r = GetRegionByLine(line, p);
            Invalidate(r);
            Update();
        }
        private static Region GetRegionByLine(Line l, Point p)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddPolygon(new Point[] { l.FirstMark.Center, l.SecondMark.Center, p, l.FirstMark.Center });

            RectangleF rf = gp.GetBounds();
            gp.Dispose();

            rf.Inflate(100f, 100f);

            return new Region(rf);
        }
        private Line GetLineByMark(MarkBox m)
        {
            foreach (Line l in Lines)
            {
                if (l.FirstMark == m || l.SecondMark == m)
                    return l;
            }
            throw new Exception("No line found");
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            _robot.Draw(e.Graphics, _cam);
            _station.Draw(e.Graphics, _cam);
            base.OnPaint(e);
        }
        #endregion

        
    }
}
