using IpPbx.CLMgrLib;
using SwyxSharp.Common;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace SwyxSharp.UserInterface
{
    /// <summary>
    /// Interaktionslogik für Softphone.xaml
    /// </summary>
    public partial class Softphone : INotifyPropertyChanged
    {
        private DateTime _ongoingCall;
        private bool _isOnHold = false;

        public string TimePassed
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(TimePassed));
            }
        }

        private readonly DispatcherTimer _timer = new(
            DispatcherPriority.Render,
            Dispatcher.CurrentDispatcher)
        {
            Interval = TimeSpan.FromMilliseconds(200)
        };

        public Softphone()
        {
            InitializeComponent();
            SwyxBridge.SwyxClient?.LineChanged += LineChanged;
            SwyxBridge.SwyxClient?.LineStateChanged += LineStateChanged;

            DataContext = this;
            _timer.Tick += (s, e) => UpdateCallInfo();
        }

        private void UpdateCallInfo()
        {
            if (_ongoingCall == new DateTime(1899, 12, 30, 0, 0, 0))
            {
                TimePassed = "";
            }
            else
            {
                var span = DateTime.Now - _ongoingCall;
                TimePassed = $@"{span:hh\:mm\:ss}";
            }
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

            var allLinesState = SwyxBridge.SwyxClient?.GetAllLineStatus();

            var peerName = line.DispPeerName;
            var peerNumber = line.DispPeerNumber;

            Dispatcher.Invoke(() =>
            {
                var isAllInactive = allLinesState?.All(x => x == LineState.Inactive);
                CallCard.Visibility = isAllInactive == true ? Visibility.Hidden : Visibility.Visible;

                CallerName.Text = peerName;
                CallerNumber.Text = peerNumber;

                switch (newLineState)
                {
                    case LineState.Inactive:
                        lineStateText.Text = "Inaktiv";
                        lineStateCaller.Text = string.Empty;
                        _isOnHold = false;
                        break;
                    case LineState.HookOffInternal:
                        lineStateText.Text = "HookOffInternal";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        break;
                    case LineState.HookOffExternal:
                        lineStateText.Text = "HookOffExternal";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        break;
                    case LineState.Ringing:
                        lineStateText.Text = "Eingehender Anruf";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        break;
                    case LineState.Dialing:
                        lineStateText.Text = "Anrufen ...";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        break;
                    case LineState.Alerting:
                        lineStateText.Text = "Es klingelt";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        break;
                    case LineState.Knocking:
                        lineStateText.Text = "Es wird angeklopft";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        break;
                    case LineState.Busy:
                        lineStateText.Text = "Anruf abgelehnt";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        break;
                    case LineState.Active:
                        lineStateText.Text = "Verbunden";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}"; 
                        _isOnHold = false;
                        _ongoingCall = line.DispConnectionFinishedTime;
                        _timer.Start();
                        break;
                    case LineState.OnHold:
                        lineStateText.Text = "Verbindung gehalten";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = true;
                        break;
                    case LineState.ConferenceActive:
                        lineStateText.Text = "In einer Konferenz";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        break;
                    case LineState.ConferenceOnHold:
                        lineStateText.Text = "Konferenz gehalten";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = true;
                        break;
                    case LineState.Terminated:
                        lineStateText.Text = "Anruf beendet";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        _isOnHold = false;
                        _timer.Stop();
                        break;
                    case LineState.Transferring:
                        lineStateText.Text = "Anruf weitergeleitet";
                        lineStateCaller.Text = $"{peerName}, {peerNumber}";
                        break;
                    case LineState.Disabled:
                        lineStateText.Text = "Deaktiviert";
                        lineStateCaller.Text = "";
                        _isOnHold = false;
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
            HoldCallBtn.Icon = _isOnHold ? new SymbolIcon(SymbolRegular.Pause24) : new SymbolIcon(SymbolRegular.Play24);
            HoldCallBtn.FontSize = 24;

            SwyxBridge.SwyxClient?.HoldCall();
        }

        private void EndCall_Click(object sender, RoutedEventArgs e)
        {
            SwyxBridge.SwyxClient?.EndCall();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
