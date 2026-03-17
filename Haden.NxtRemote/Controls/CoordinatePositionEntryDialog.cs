using System.Windows.Forms;

namespace Haden.NXTRemote.Controls
{
    public partial class CoordinatePositionEntryDialog : Form
    {
        public CoordinatePositionEntryDialog()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ControlBox = false;
        }
        private void OkButton_Click(object sender, System.EventArgs e)  {  }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}