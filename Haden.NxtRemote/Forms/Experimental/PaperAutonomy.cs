using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Haden.NXTRemote.Controls;
using Haden.NXTRemote.Simulation.Drawing3D;
using Haden.NXTRemote.Simulation.Plotting3D;
using Haden.Library;
using Timer = System.Windows.Forms.Timer;
using SpeechLib;

namespace Haden.NXTRemote.Forms.Experimental
{
    public partial class PaperAutonomy : Form
    {
        /// <summary>
        /// An instance of haden's voice.
        /// </summary>
        private readonly SpVoice _voice = new SpVoice();
        private bool _isSelected;
        private int _x, _y;
        protected Timer PollingTimer;
        // Step 0: Primitives
        private readonly ContextMenuStrip _linecmenu;
        private Cuboid _station;// = new Cuboid(75, 75, 75);
        private Cuboid _robot;// = new Cuboid(50, 60, 45);
        private readonly Camera _cam = new Camera();
        private Pen _ray;
        //private Graphics _robotRay;
        //private Graphics _stationRay;
        private HadenCore AutonomousAgent { get; set; }
        public DesiredAttribute DesiredAttribute { get; set; }
        private Domino Domino = new Domino(2.0f, 4.0f, 1.0f);
        private Graphics _canvas;

        public int Iterator;
        public ArrayList Lines;
        public static bool Instance { get; set; }
        public static int CurrentValue { get; set; }
        public static int OptimalValue { get; set; }
        //public Graphics Canvas { get => canvas; set => canvas = value; }

        public SettingsDictionary GlobalSettings;

        public PaperAutonomy()
        {
            InitializeComponent();

            //_voice.Speak("Beginning the initial phase.");

            #region WinForm
            _linecmenu = linecmenu;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = true;
            ControlBox = true;
            Instance = true;
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            #endregion

            #region Setup simulation objects.
            _cam.Location = new NXTRemote.Simulation.Drawing3D.Point3D(400, 240, -500);
            Lines = new ArrayList();
            // Initialize animals framework.
            GlobalSettings = new SettingsDictionary();
            LoadSettings();
            // Create a new A.I. core.
            AutonomousAgent = new HadenCore { DesiredAttribute = DesiredAttribute.Chained };
            // Create the environment for the simulation.
            DrawEnvironment();
            #endregion

            Dominos();

        }

        public void Dominos()
        {
            var simDominos = new Dominoes(7, 2.0f, 4.0f, 1.0f, 6.0f);
            //var objDominos = new Dominoes(Domino);
            _canvas = CreateGraphics();
            var plotter = new Plotter3D(_canvas);
            simDominos.Render(plotter);

        }

        // Step 1: Draw the environment (shapes) - "robot" and "station"
        public void DrawEnvironment()
        {
            _ray = new Pen(Color.Blue);
            _station = new Cuboid(175, 75, 60);
            _robot = new Cuboid(75, 60, 75);
            _robot.CenterPoint = new NXTRemote.Simulation.Drawing3D.Point3D(300, 350, 100);
            //_robotRay = CreateGraphics();
            //_robotRay.DrawLine(_ray, 0, 0, 400, 240);
            Invalidate();
            //_station.CenterPoint = new NXTRemote.Simulation.Drawing3D.Point3D(90, 90, 180);
            //_stationRay = CreateGraphics();
            //_stationRay.DrawLine(_ray, 0, 100, 60, 40);
            Invalidate();
            drawButton.Enabled = false;
        }
        // Step 2: Cast a ray to locate the object robot --> station
        public void CastSeekingRay(Point robot, Point station)
        {
            // Adds marks that are the beginning/end of the line.
            var mark1 = new MarkBox();
            mark1.Location = new Point(robot.X, robot.Y);
            mark1.ContextMenuStrip = _linecmenu;
            if (ActiveForm != null)
            {
                ActiveForm.Controls.Add(mark1);

                var mark2 = new MarkBox();
                mark2.Location = new Point(station.X, station.Y);
                mark2.ContextMenuStrip = _linecmenu;
                ActiveForm.Controls.Add(mark2);

                // Line Struct contains the information for a single line.
                var line = new Line();
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
            var seekLeftRight = new Random();
            var leftRight = seekLeftRight.Next(1, 3);
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
            PollingTimer = new Timer {Interval = 5000};
            PollingTimer.Start();
            PollingTimer.Tick += PollingTimer_OnTick;
        }
        public void AutonomousTurn(string direction)
        {
            try
            {
                switch (direction)
                {
                    case "left":
                        seekLeftIndicator.FlasherButtonStart(FlashIntervalSpeed.BlipFast);
                        seekLeftIndicator.FlashNumber = 3;
                        BoxedLastTurn = "left";
                        // Move items in the environment.
                        var qx = new Quaternion();
                        qx.FromAxisAngle(new NXTRemote.Simulation.Drawing3D.Vector3D(0, 0, 1), -10*Math.PI/180.0);
                        // Here is where we program the movement for positive phototaxic behaviour of the robot, relative to the position of the station.
                        _robot.RotateAt(_robot.CenterPoint, qx);
                        Invalidate();
                        break;
                    case "right":
                        seekRightIndicator.FlasherButtonStart(FlashIntervalSpeed.BlipFast);
                        seekRightIndicator.FlashNumber = 3;
                        BoxedLastTurn = "right";
                        // Move items in the environment
                        var qy = new Quaternion();
                        qy.FromAxisAngle(new NXTRemote.Simulation.Drawing3D.Vector3D(0, 0, 1), 10*Math.PI/180.0);
                        _robot.RotateAt(_robot.CenterPoint, qy);
                        Invalidate();
                        break;
                    case "straight":
                        seekStraightIndicator.FlasherButtonStart(FlashIntervalSpeed.FlashFinite);
                        seekStraightIndicator.FlashNumber = 3;
                        BoxedLastTurn = "straight";
                        // Move items in the environment
                        var qz = new Quaternion();
                        qz.FromAxisAngle(new NXTRemote.Simulation.Drawing3D.Vector3D(1, 1, 0), 10*Math.PI/180.0);
                        _robot.RotateAt(_robot.CenterPoint, qz);
                        Invalidate();
                        break;
                }
                Thread.Sleep(2000);
            }
            catch (Exception)
            {
                // Cannot get the flashing to stop once flashing for the number of flashes. Strange, huh?
            }
            finally
            {
                seekLeftIndicator.FlasherButtonStop();
                seekRightIndicator.FlasherButtonStop();
                seekStraightIndicator.FlasherButtonStop();
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
            seekLeftIndicator.FlasherButtonStop();
            seekRightIndicator.FlasherButtonStop();
            seekStraightIndicator.FlasherButtonStop();
            var dummySensorValue = new Random();
            var leftRight = dummySensorValue.Next(12, 60);
            BoxedCurrentValue = leftRight;
            CalculateSensorValueDifference();
        }
        // Step 'X': Mathematical operations (application support) Best to continue here.
        public void CalculateSensorValueDifference()
        {
            var differingValue = (int)BoxedCurrentValue - (int)BoxedOptimalValue;
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
            var l = GetLineByMark(((MarkBox)linecmenu.SourceControl));
            Lines.Remove(l);
            l.FirstMark.Dispose();
            l.SecondMark.Dispose();
            linecmenu.SourceControl.Dispose();
            Redraw();
        }
        private void NewLine_Click(object sender, EventArgs e)
        {
            var x1 = 0;
            var x2 = 0;
            var y1 = 0;
            var y2 = 0;
            var cpd = new CoordinatePositionEntryDialog();
            var res = cpd.ShowDialog();
            if (res == DialogResult.OK)
            {
                var xf = cpd.FirstCoordinateX.Text;
                var xs = cpd.SecondCoordinateX.Text;
                var yf = cpd.FirstCoordinateY.Text;
                var ys = cpd.SecondCoordinateY.Text;
                x1 = Convert.ToInt32(xf);
                x2 = Convert.ToInt32(xs);
                y1 = Convert.ToInt32(yf);
                y2 = Convert.ToInt32(ys);
            }

            // Adds marks that are the beginning/end of the line.
            var mark1 = new MarkBox();
            mark1.Location = new Point(x1, y1);
            mark1.ContextMenuStrip = _linecmenu;
            if (ActiveForm != null)
            {
                ActiveForm.Controls.Add(mark1);

                var mark2 = new MarkBox();
                mark2.Location = new Point(x2, y2);
                mark2.ContextMenuStrip = _linecmenu;
                ActiveForm.Controls.Add(mark2);

                // Line Struct contains the information for a single line.
                var line = new Line();
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
            var l = GetLineByMark(((MarkBox)linecmenu.SourceControl));
            var firstCoordinate = l.FirstMark.Location;
            var secondCoordinate = l.SecondMark.Location;
            // Create PopUp dialog to report coordinates of marks
            MessageBox.Show("Coordinates of this line are:  " + firstCoordinate.X + ", " + firstCoordinate.Y + " and " + secondCoordinate.X + ", " + secondCoordinate.Y + ".", "Line information");
        }
        private void Width_Click(object sender, EventArgs e)
        {
            Line l;
            var wd = new LineWidthDialog(GetLineByMark((MarkBox)linecmenu.SourceControl).Width);
            var res = wd.ShowDialog();
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
            //DrawEnvironment();
        }
        private void castRayButton_Click(object sender, EventArgs e)
        {
            CastSeekingRay(new Point((int)_robot.CenterPoint.X, (int)_robot.CenterPoint.Y), new Point((int)_station.CenterPoint.X, (int)_station.CenterPoint.Y));
        }
        private void findStationButton_Click(object sender, EventArgs e)
        {
            FindStation();
        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            PollingTimer.Stop();
            PollingTimer.Tick -= PollingTimer_OnTick;
            seekLeftIndicator.FlasherButtonStop();
            seekRightIndicator.FlasherButtonStop();
            seekStraightIndicator.FlasherButtonStop();
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
                var mb1 = (MarkBox)sender;
                var l = GetLineByMark(mb1);
                var p = new Point(e.X - _x + mb1.Left, e.Y - _y + mb1.Top);

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
            var g = CreateGraphics();
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

            var r = GetRegionByLine(line, p);
            Invalidate(r);
            Update();
        }
        private static Region GetRegionByLine(Line l, Point p)
        {
            var gp = new GraphicsPath();
            gp.AddPolygon(new Point[] { l.FirstMark.Center, l.SecondMark.Center, p, l.FirstMark.Center });

            var rf = gp.GetBounds();
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

        #region Utilities
        public void LoadSettings()
        {
            var path = Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "Settings.xml"));
            GlobalSettings.LoadSettings(path);
        }
        #endregion
    }
}
