using System.Drawing;
using System.Windows.Forms;

namespace Haden.NXTRemote.Controls
{
    public sealed partial class MarkBox : UserControl
    {
        public MarkBox()
        {
            InitializeComponent();
            BackColor = Color.Blue;
        }

        public Point Center
        {
            get { return new Point(Location.X + 4, Location.Y + 4); }
        }

        protected override void OnPaint(PaintEventArgs e) { }
    }
}
