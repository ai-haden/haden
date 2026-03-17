using System.Windows.Forms;

namespace Haden.NXTRemote.Forms.Child
{
    public partial class YesNo : Form
    {
        private Form _owner;
        private static bool _instance;
        private static string _message;
        public string Decision { get; set; }
        public YesNo(Form mOwner, string message)
        {
            InitializeComponent();
            Decision = "";
            OwnerLocal = mOwner;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ControlBox = false;
            _instance = true;
            _message = message;
            communicateToOperator.Text = message;
        }

        public static bool Instance { get { return _instance; } set { _instance = value; } }
        public Form OwnerLocal { get { return _owner; } set { _owner = value; } }
        public string Message { get { return _message; } set { _message = value; } }

        private void yesButton_Click(object sender, System.EventArgs e)
        {
            Decision = "yes";
        }

        private void noButton_Click(object sender, System.EventArgs e)
        {
            Decision = "no";
        }
    }
}
