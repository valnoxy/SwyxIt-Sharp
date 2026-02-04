using IpPbx.CLMgrLib;
using SwyxSharp.Common;
using System.Windows;
using System.Windows.Input;

namespace SwyxSharp.UserInterface
{
    /// <summary>
    /// Interaktionslogik für Softphone.xaml
    /// </summary>
    public partial class Softphone
    {
        public Softphone()
        {
            InitializeComponent();
            SwyxBridge.SwyxClient?.LineChanged += LineChanged;
            SwyxBridge.SwyxClient?.LineStateChanged += LineStateChanged;
        }

        private void LineStateChanged(int index, int state, ClientLine line)
        {
            var newLineState = (LineState)state;
            Wpf.Ui.Controls.TextBlock lineStateText;
            Wpf.Ui.Controls.TextBlock lineStateCaller;
            if (index == 0)
            {
                lineStateText = Line1State;
                lineStateCaller = Line1Caller;
            }
            else
            {
                lineStateText = Line2State;
                lineStateCaller = Line2Caller;
            }

            var peerName = line.DispPeerName;
            var peerNumber = line.DispPeerNumber;

            Dispatcher.Invoke(() =>
            {
                CallerName.Text = peerName;
                CallerNumber.Text = peerNumber;

                switch (newLineState)
                {
                    case LineState.Inactive:
                        lineStateText.Text = "Inaktiv";
                        lineStateCaller.Text = string.Empty;
                        break;
                    case LineState.HookOffInternal:
                        lineStateText.Text = "HookOffInternal";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.HookOffExternal:
                        lineStateText.Text = "HookOffExternal";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Ringing:
                        lineStateText.Text = "Eingehender Anruf";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Dialing:
                        lineStateText.Text = "Anrufen ...";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Alerting:
                        lineStateText.Text = "Es klingelt";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Knocking:
                        lineStateText.Text = "Es wird angeklopft";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Busy:
                        lineStateText.Text = "Anruf abgelehnt";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Active:
                        lineStateText.Text = "Verbunden";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.OnHold:
                        lineStateText.Text = "Verbindung gehalten";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.ConferenceActive:
                        lineStateText.Text = "In einer Konferenz";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.ConferenceOnHold:
                        lineStateText.Text = "Konferenz gehalten";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Terminated:
                        lineStateText.Text = "Anruf beendet";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Transferring:
                        lineStateText.Text = "Anruf weitergeleitet";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Disabled:
                        lineStateText.Text = "Deaktiviert";
                        lineStateCaller.Text = "";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        private void LineChanged(int newLine)
        {
            Dispatcher.Invoke(() => {
                switch (newLine)
                {
                    case 0:
                        ActiveBorderLine1.Visibility = Visibility.Visible;
                        ActiveBorderLine2.Visibility = Visibility.Hidden;
                        break;
                    case 1:
                        ActiveBorderLine1.Visibility = Visibility.Hidden;
                        ActiveBorderLine2.Visibility = Visibility.Visible;
                        break;
                }
            });
        }

        private void CallForward_Click(object sender, RoutedEventArgs e)
        {
            SwyxBridge.SwyxClient?.OpenDialog(SwyxEnums.DialogId.CallForward + 1);
        }

        private void CallForward_RightClick(object sender, MouseButtonEventArgs e)
        {
            SwyxBridge.SwyxClient?.OpenDialog(SwyxEnums.DialogId.CallForward);
        }

        private void DialNumber_Click(object sender, RoutedEventArgs e)
        {
            SwyxBridge.SwyxClient?.InitiateCall(CallBlock.Text);
            CallBlock.Text = "";
        }

        private void CallBlock_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            SwyxBridge.SwyxClient?.InitiateCall(CallBlock.Text);
            CallBlock.Text = "";
        }

        private void HoldCall_Click(object sender, RoutedEventArgs e)
        {
            SwyxBridge.SwyxClient?.HoldCall();
        }

        private void EndCall_Click(object sender, RoutedEventArgs e)
        {
            SwyxBridge.SwyxClient?.EndCall();
        }

    }
}
