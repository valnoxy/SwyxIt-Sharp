using System.Windows;

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
            Common.Debug.Logging.Initialize();
            Common.SwyxBridge.Initialize();

            var w = new MainWindow();
            w.ShowDialog();
        }
    }

}
