namespace Haden.NxtSharp.Brick
{
    partial class NxtBrick
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.keepAliveTimer = new System.Windows.Forms.Timer(this.components);
            // 
            // keepAliveTimer
            // 
            this.keepAliveTimer.Interval = 90000;
            this.keepAliveTimer.Tick += new System.EventHandler(this.KeepAliveTimerTick);

        }

        #endregion

        /// <summary>
        /// The timer which sets the interval to send the KeepAlive() signal to the Nxt brick.
        /// </summary>
        private System.Windows.Forms.Timer keepAliveTimer;

    }
}
