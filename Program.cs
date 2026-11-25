using PixelLab14.UI;

namespace PixelLab14;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // WinForms должен запускаться в главном STA-потоке.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
