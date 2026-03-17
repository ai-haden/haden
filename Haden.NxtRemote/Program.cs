using System;
using System.Windows.Forms;
using Haden.NXTRemote.Forms;
using Haden.NXTRemote.Forms.Experimental;
using Haden.NXTRemote.Forms.Simulation;

namespace Haden.NXTRemote
{
    internal static class Program
    {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
        {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new SimulatedAutonomy());
            Application.Run(new HadenManualControl());
            //Application.Run(new PaperAutonomy());// <-- a temporary form so as to not disturb the original code. Changes migrated later.
		}
	}
}
