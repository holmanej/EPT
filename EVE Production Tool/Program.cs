using System;
using System.Collections.Generic;
using System.Windows.Forms;
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

            Form1 EPT_Form = new Form1();
            EPT_Form.Controls.Add(new MarketBrowser());

            Application.Run(EPT_Form);
        }
    }
}
