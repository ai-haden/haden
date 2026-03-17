using System.Windows.Forms;

namespace Haden.NXTRemote.Controls
{
    public partial class LineWidthDialog : Form
    {
        public LineWidthDialog(int width)
        {
            InitializeComponent();
            numericUpDown.Value = width;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ControlBox = false;
        }

        private int _linewidth = 1;
        public int LineWidth { get { return _linewidth; } }

        private void OkButton_Click(object sender, System.EventArgs e)
        {
            _linewidth = (int)numericUpDown.Value;
        }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}