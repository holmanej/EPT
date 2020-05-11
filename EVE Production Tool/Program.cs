using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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

            Console.WriteLine(FindAssets("assets"));

            Form1 EPT_Form = new Form1();
            EPT_Form.Controls.Add(new MarketBrowser());

            Application.Run(EPT_Form);
        }

        static bool FindAssets(string dirName)
        {
            while (!File.Exists("EVE Production Tool.sln"))
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                Directory.SetCurrentDirectory("..");
            }
            if (Directory.Exists(dirName))
            {
                Directory.SetCurrentDirectory(dirName);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
