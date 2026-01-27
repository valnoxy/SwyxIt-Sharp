using System.Windows;
using SwyxSharp.Common.Debugging;

namespace SwyxSharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Prepare Swyx API
            var useDebug = false;
#if DEBUG
            useDebug = true;
#endif
            Logging.Initialize(useDebug);
            Common.SwyxBridge.Initialize();

            // Show main window
            var w = new MainWindow();
            w.ShowDialog();

            // Shutdown Swyx API
            Common.SwyxBridge.Shutdown();
        }
    }

}
