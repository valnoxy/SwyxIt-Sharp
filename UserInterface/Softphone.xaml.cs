using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SwyxSharp.Common;

namespace SwyxSharp.UserInterface
{
    /// <summary>
    /// Interaktionslogik für Softphone.xaml
    /// </summary>
    public partial class Softphone : UserControl
    {
        public Softphone()
        {
            InitializeComponent();
        }

        private void CallForward_Click(object sender, RoutedEventArgs e)
        {
            SwyxBridge.SwyxClient?.OpenDialog(SwyxClient.UserInterface.DialogId.CallForward + 1);
        }

        private void CallForward_RightClick(object sender, MouseButtonEventArgs e)
        {
            SwyxBridge.SwyxClient?.OpenDialog(SwyxClient.UserInterface.DialogId.CallForward);
        }
    }
}
