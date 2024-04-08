using System;
using System.Windows.Forms;
using GingaGame.UI;

namespace GingaGame;

internal static class Program
{
    /// <summary>
    ///     Entry point of the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new GingaGameForm());
    }
}