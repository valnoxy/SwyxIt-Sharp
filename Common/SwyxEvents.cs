using System.Diagnostics.CodeAnalysis;
using IpPbx.CLMgrLib;

namespace SwyxSharp.Common
{
    public class SwyxEventsArgs : EventArgs
    {
        public CLMgrMessage Msg;
        public int Param;

        public SwyxEventsArgs(CLMgrMessage msg, int param)
        {
            Msg = msg;
            Param = param;
        }
    }

    public delegate void LineManagerMessageHandler(SwyxEventsArgs e);

    public class SwyxEventsSink
    {
        private ClientLineMgrClass _connectedLineManager;
        private readonly IClientLineMgrEventsPub_PubOnLineMgrNotificationEventHandler _eventHandler;
        private LineManagerMessageHandler _lineManagerMessageDelegateOfForm;

        public SwyxEventsSink()
        {
            _eventHandler = clmgr_EventSink;
        }

        public void Connect(ClientLineMgrClass lineManager, LineManagerMessageHandler lineManagerMessageDelegateOfForm)
        {
            _connectedLineManager = lineManager;
            _lineManagerMessageDelegateOfForm = lineManagerMessageDelegateOfForm;

            // Add event handler for the PubOnlineMgrNotification Events
            _connectedLineManager.PubOnLineMgrNotification += _eventHandler;
        }

        public void Disconnect()
        {
            // Remove event handler for the PubOnlineMgrNotification Events
            _connectedLineManager.PubOnLineMgrNotification -= _eventHandler;
            _connectedLineManager = null;
            _lineManagerMessageDelegateOfForm = null;
        }

        private void clmgr_EventSink(int msg, int param)
        {
            // This method receives the COM events from the client line manager
            _lineManagerMessageDelegateOfForm?.Invoke(new SwyxEventsArgs((CLMgrMessage)msg, param));
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum CLMgrMessage
    {
        CLMgrLineStateChangedMessage            = 0,   // state of at least one line has changed
        CLMgrLineSelectionChangedMessage        = 1,   // line in focus has changed
        CLMgrLineDetailsChangedMessage          = 2,   // details of at least one line have changed
        CLMgrUserDataChangedMessage             = 3,   // 
        CLMgrCallDetailsMessage                 = 4,   // details of last call are available, post-mortem for logging purpose
        CLMgrServerDownMessage                  = 5,   // server goes down, keep line manager, wait for ServerUp message
        CLMgrServerUpMessage                    = 6,   // server is up again, keep interfaces to line manager
        CLMgrWaveDeviceChanged                  = 7,   // speaker / micro has been switched on / off
        CLMgrGroupCallNotificationMessage       = 8,   // notification about group call
        CLMgrNameKeyStateChangedMessage         = 9,   // 
        CLMgrNumberOfLinesChangedMessage        = 10,  // the number of lines has changed
        CLMgrClientShutDownRequest              = 11,  // Client Line Manager requests client to shut down and release all interfaces
        CLMgrPowerSuspendMessage                = 12,  // 
        CLMgrPowerResumeMessage                 = 13,  // 
        CLMgrHandsetStateChangedMessage         = 14,  // 
        CLMgrSkinPhoneCommandMessage            = 15,  // 
        CLMgrSkinActionAreaStateChangedMessage  = 16,  // 
        CLMgrSkinInfoDetailChangedMessage       = 17,  // 
        CLMgrCallbackOnBusyNotification         = 18,  // 
        CLMgrAsyncMessage                       = 19,  // 
        CLMgrCtiPairingStateChanged             = 20,  // 
        CLMgrChatMessage                        = 21,  // 
        CLMgrChatMessageAck                     = 22,  // 
        CLMgrRecordingError                     = 23,  // 
        CLMgrVolumeChanged                      = 24,  // 
        CLMgrAudioModeChanged                   = 25,  // 
        CLMgrMicAdjustLevelMeter                = 26,  // 
        CLMgrMicAdjustProceedMeter              = 27,  // 
        CLMgrLineStateChangedMessageEx          = 28,  // state of certain line has changed, lParam: LOWORD: line index of line that changed its state (starting with 0) HIWORD: new state of this line
        CLMgrPlaySoundFileDxProceedMeter        = 29,  // 
        CLMgrSIPRegistrationStateChanged        = 30,  // registration state of SIP account has changed
                                                       //   lParam: LOBYTE: Account index
                                                       //           HIBYTE: new state
        CLMgrWaveFilePlayed                     = 31,  // wave file playback finished
                                                       //   lParam: line index;
                                                       //   if -1, the message is related to a LineMgr function PlaySoundFile or PlayToRtp
                                                       //   if >=0 the message is related to a line function PlaySoundFile of line with this index
        CLMgrFirstDataReceived                  = 32,  // first RTP data received on-line, might be silence
                                                       // lParam: line index;
        CLMgrRegisteredSipDeviceListChanged     = 33,  // 
        CLMgrDialerStartCallResult              = 34,  // 
        CLMgrLineStateChangedMessageEx2         = 35,  // 
        CLMgrMediaEncryptionStatusChanged       = 36,  // 
        CLMgrLateDisconnect                     = 37,  // 
        CLMgrDeviceStateChanged                 = 38,  // 
        CLMgrBlockDialStringChanged             = 39,  // 
        CLMgrPlaySoundFileState                 = 40,  // 
        CLMgrVoicemailPlayerAudioMode           = 41,  // 
        CLMgrPluginNotLicensed                  = 42,  // 
        CLMgrCloudConnectorStatus               = 43,  // 
        CLMgrClientRegisterRequest              = 44,  // 
        CLMgrCtiDeviceListChanged               = 45,  // 
        CLMgrCtiDeviceListUnavailable           = 46,  // 
        CLMgrNewVersionAvailable                = 47,  // 
        CLMgrUnreadInstantMessageCount          = 48,  // 
        CLMgrOpenUiDialog                       = 49,  // 
        CLMgrDoneWithModalUiDialog              = 50,  // 
        CLMgrInvokeVoicemailAction              = 51,  // 
        CLMgrPhoneBookReloaded                  = 52,  // 
        CLMgrFederatedLoginSessionExpired       = 53,  // 
        CLMgrCloseSwyxIt                        = 54,  // 
        CLMgrLoadModernSkin                     = 55,  // 
        CLMgrSwitchToClassic                    = 56,  // 
        CLMgrOpenCallRouting                    = 57,  // 
        CLMgrShowCallControl                    = 58,  // 
        CLMgrNotificationCallsChanged           = 59,  // 
        CLMgrOwnPresenceStatusChanged           = 60,  // 
    }

    public enum LineState
    {
        Inactive = 0,                        // line is inactive
        HookOffInternal = 1,                 // off hook, internal dial tone
        HookOffExternal = 2,                 // off hook, external dial tone
        Ringing = 3,                         // incoming call, ringing
        Dialing = 4,                         // outgoing call, we are dialing, no sound
        Alerting = 5,                        // outgoing call, alerting = ringing on destination
        Knocking = 6,                        // outgoing call, knocking = second call ringing on destination
        Busy = 7,                            // outgoing call, destination is busy
        Active = 8,                          // incoming / outgoing call, logical and physical connection is established
        OnHold = 9,                          // incoming / outgoing call, logical connection is established, destination gets music on hold
        ConferenceActive = 10,               // incoming / outgoing conference, logical and physical connection is established
        ConferenceOnHold = 11,               // incoming / outgoing conference, logical connection is established, not physically connected
        Terminated = 12,                     // incoming / outgoing connection / call has been disconnected
        Transferring = 13,                   // special LSOnHold, call is awaiting to be transferred, peer gets special music on hold
        Disabled = 14                        // special LSInactive: wrap up time
    }

    public enum DisconnectReason
    {
        Normal = 0,
        Busy = 1,
        Rejected = 2,
        Cancelled = 3,
        Transferred = 4,
        JoinedConference = 5,
        NoAnswer = 6,
        TooLate = 7,
        DirectCallImpossible = 8,
        WrongNumber = 9,
        Unreachable = 10,
        CallDiverted = 11,
        CallRoutingFailed = 12,
        PermissionDenied = 13,
        NetworkCongestion = 14,
        NoChannelAvailable = 15,
        NumberChanged = 16,
        IncompatibleDestination = 17
    }
}
