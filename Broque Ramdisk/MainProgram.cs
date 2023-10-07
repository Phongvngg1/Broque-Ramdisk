using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

class MainClass
{
    static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());

        Main(null);
    }
}