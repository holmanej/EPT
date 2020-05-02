using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static EVE_Production_Tool.ESI;
using static EVE_Production_Tool.MarketBrowser;

namespace EVE_Production_Tool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
