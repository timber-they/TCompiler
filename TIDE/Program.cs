#region

using System;
using System.Windows.Forms;

using TIDE.Forms;

#endregion


namespace TIDE
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main ()
        {
            Application.SetCompatibleTextRenderingDefault (false);
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new TIDE_MainWindow ());
        }
    }
}