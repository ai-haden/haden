using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Haden.NXTRemote.Controls;
using Haden.NXTRemote.Simulation.Drawing3D;

namespace Haden.NXTRemote.Forms
{
    public partial class SimulatorMockup : Form
    {
        Form _owner;
        static bool _instance;

        public readonly Cuboid Zero = new Cuboid(75, 75, 75);
        public readonly Cuboid One = new Cuboid(50, 60, 45);
        public readonly Cuboid Two = new Cuboid(30, 45, 25);
        readonly Camera _cam = new Camera();

        public Pen Ray;
        public Graphics ZeroRayGph;
        public Graphics OneRay;
        public Graphics TwoRay;

        public ArrayList Lines;

        #region Context menu (not presently used)
        private readonly ContextMenu _linecmenu;
        //private MenuItem LWidth;
        //private MenuItem Delete;
        #endregion

        public SimulatorMockup(Form mOwner, ContextMenu linecmenu)
        {
            InitializeComponent();
            _owner = mOwner;
            _linecmenu = linecmenu;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ControlBox = false;
            _instance = true;
            _cam.Location = new Point3D(400, 240, -400);
            _cam.Location = new Point3D(400, 400, -500);
            Zero.CenterPoint = new Point3D(400, 200, 0);
            One.CenterPoint = new Point3D(200, 500, 150);
            Two.CenterPoint = new Point3D(100, 50, 200);
            Lines = new ArrayList();
        }

        public void ZeroRay(PaintEventArgs e)
        {
            Ray = new Pen(Color.Blue);
            ZeroRayGph = CreateGraphics();
            ZeroRayGph.DrawLine(Ray, 20, 10, 500, 400);
            Invalidate();
        }

        public static bool Instance { get { return _instance; } set { _instance = value; } }
        public Form MOwner { get { return _owner; } set { _owner = value; } }

        private void closeForm_Click(object sender, System.EventArgs e)
        {
            Instance = false;
            Close();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Zero.Draw(e.Graphics, _cam);
            One.Draw(e.Graphics, _cam);
            Two.Draw(e.Graphics, _cam);

            base.OnPaint(e);
        }

        private void castRayButton_Click(object sender, System.EventArgs e)
        {
            // Adds red marks that are the beginning/end of the line.
            MarkBox mark1 = new MarkBox();
            mark1.Location = new Point(50, 50);
            mark1.ContextMenu = _linecmenu;
            if (ActiveForm != null)
            {
                ActiveForm.Controls.Add(mark1);

                MarkBox mark2 = new MarkBox();
                mark2.Location = new Point(100, 100);
                mark2.ContextMenu = _linecmenu;
                ActiveForm.Controls.Add(mark2);

                // Line Struct contains the information for a single line.
                Line line = new Line();
                line.FirstMark = mark1;
                line.SecondMark = mark2;
                line.Width = 1;

                // Events for moving marks
                //mark1.MouseUp += Mark_MouseUp;
                //mark1.MouseDown += Mark_MouseDown;
                //mark1.MouseMove += Mark_MouseMove;

                //mark2.MouseUp += Mark_MouseUp;
                //mark2.MouseDown += Mark_MouseDown;
                //mark2.MouseMove += Mark_MouseMove;

                // Adds Line object to an arraylist.
                Lines.Add(line);
            }
            Redraw();
        }

        /// <summary>
        /// Redraws the lines.
        /// </summary>
        private void Redraw()
        {
            foreach (Line line in Lines)
            {
                DrawLine(line);
            }
        }
        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="line">The line element.</param>
        private void DrawLine(Line line)
        {
            Graphics g = CreateGraphics();
            g.DrawLine(new Pen(Color.Black, line.Width), line.FirstMark.Center.X, line.FirstMark.Center.Y, line.SecondMark.Center.X, line.SecondMark.Center.Y);
            g.Dispose();
        }
    }
}
