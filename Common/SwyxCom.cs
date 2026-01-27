using System.IO;
using IpPbx.CLMgrLib;
using SwyxSharp.Common.Debugging;

namespace SwyxSharp.Common
{
    internal class SwyxClient
    {
        private readonly ClientLineMgrClass? _lineManager;
        private readonly IClientConfig? _clientConfig;
        private SwyxEventsSink? _eventSink;

        public SwyxClient()
        {
            try
            {
                Logging.Log("Initializing Swyx Client API ...");
                _lineManager = new ClientLineMgrClass();
                _eventSink = new SwyxEventsSink();
                _clientConfig = (IClientConfig)_lineManager.ClientConfig;

                // Get some infos
                var serverId = _clientConfig.GetUniqueServerId();
                var licence = _clientConfig.ServerLicenceType;
                Logging.Log($"Connected to Swyx Server ID: {serverId} | License Type: {licence}");

                // Subscribe to events
                _eventSink.Connect(_lineManager, OnLineManagerMessage);
                
                Logging.Log("Swyx Client API initialized!");
            }
            catch (Exception ex)
            {
                Logging.Log($"Failed to initialize ClientLineMgrClass: {ex.Message}", Logging.LogLevel.ERROR);
                _lineManager = null;
            }

            try
            {
                Test();
            }
            catch (Exception ex)
            {
                Logging.Log($"Test failed !!! -> {ex.Message}");
            }
        }

        public void Shutdown()
        {
            _eventSink?.Disconnect();
            Logging.Log("Swyx Client API shut down.");
        }

        #region Events

        // Events for UI updates
        public event Action<string> UserStateChanged;

        // Event handler for line manager messages
        private void OnLineManagerMessage(SwyxEventsArgs e)
        {
            var enumName = Enum.GetName(typeof(CLMgrMessage), e.Msg);
            Logging.Log($"Event {enumName} -> [Msg: {e.Msg} - Param: {e.Param}]", Logging.LogLevel.DEBUG);
            switch (e.Msg)
            {
                case CLMgrMessage.CLMgrLineStateChangedMessage:
                    break;
                case CLMgrMessage.CLMgrLineSelectionChangedMessage:
                    break;
                case CLMgrMessage.CLMgrLineDetailsChangedMessage:
                    break;
                case CLMgrMessage.CLMgrUserDataChangedMessage:
                    break;
                case CLMgrMessage.CLMgrCallDetailsMessage:
                    break;
                case CLMgrMessage.CLMgrServerDownMessage:
                    break;
                case CLMgrMessage.CLMgrServerUpMessage:
                    break;
                case CLMgrMessage.CLMgrWaveDeviceChanged:
                    break;
                case CLMgrMessage.CLMgrGroupCallNotificationMessage:
                    break;
                case CLMgrMessage.CLMgrNameKeyStateChangedMessage:
                    UserStateChanged?.Invoke(e.Param.ToString());
                    break;
                case CLMgrMessage.CLMgrNumberOfLinesChangedMessage:
                    break;
                case CLMgrMessage.CLMgrClientShutDownRequest:
                    break;
                case CLMgrMessage.CLMgrPowerSuspendMessage:
                    break;
                case CLMgrMessage.CLMgrPowerResumeMessage:
                    break;
                case CLMgrMessage.CLMgrHandsetStateChangedMessage:
                    break;
                case CLMgrMessage.CLMgrSkinPhoneCommandMessage:
                    break;
                case CLMgrMessage.CLMgrSkinActionAreaStateChangedMessage:
                    break;
                case CLMgrMessage.CLMgrSkinInfoDetailChangedMessage:
                    break;
                case CLMgrMessage.CLMgrCallbackOnBusyNotification:
                    break;
                case CLMgrMessage.CLMgrAsyncMessage:
                    break;
                case CLMgrMessage.CLMgrCtiPairingStateChanged:
                    break;
                case CLMgrMessage.CLMgrChatMessage:
                    break;
                case CLMgrMessage.CLMgrChatMessageAck:
                    break;
                case CLMgrMessage.CLMgrRecordingError:
                    break;
                case CLMgrMessage.CLMgrVolumeChanged:
                    break;
                case CLMgrMessage.CLMgrAudioModeChanged:
                    break;
                case CLMgrMessage.CLMgrMicAdjustLevelMeter:
                    break;
                case CLMgrMessage.CLMgrMicAdjustProceedMeter:
                    break;
                case CLMgrMessage.CLMgrLineStateChangedMessageEx:
                    break;
                case CLMgrMessage.CLMgrPlaySoundFileDxProceedMeter:
                    break;
                case CLMgrMessage.CLMgrSIPRegistrationStateChanged:
                    break;
                case CLMgrMessage.CLMgrWaveFilePlayed:
                    break;
                case CLMgrMessage.CLMgrFirstDataReceived:
                    break;
                case CLMgrMessage.CLMgrRegisteredSipDeviceListChanged:
                    break;
                case CLMgrMessage.CLMgrDialerStartCallResult:
                    break;
                case CLMgrMessage.CLMgrLineStateChangedMessageEx2:
                    break;
                case CLMgrMessage.CLMgrMediaEncryptionStatusChanged:
                    break;
                case CLMgrMessage.CLMgrLateDisconnect:
                    break;
                case CLMgrMessage.CLMgrDeviceStateChanged:
                    break;
                case CLMgrMessage.CLMgrBlockDialStringChanged:
                    break;
                case CLMgrMessage.CLMgrPlaySoundFileState:
                    break;
                case CLMgrMessage.CLMgrVoicemailPlayerAudioMode:
                    break;
                case CLMgrMessage.CLMgrPluginNotLicensed:
                    break;
                case CLMgrMessage.CLMgrCloudConnectorStatus:
                    break;
                case CLMgrMessage.CLMgrClientRegisterRequest:
                    break;
                case CLMgrMessage.CLMgrCtiDeviceListChanged:
                    break;
                case CLMgrMessage.CLMgrCtiDeviceListUnavailable:
                    break;
                case CLMgrMessage.CLMgrNewVersionAvailable:
                    break;
                case CLMgrMessage.CLMgrUnreadInstantMessageCount:
                    break;
                case CLMgrMessage.CLMgrOpenUiDialog:
                    break;
                case CLMgrMessage.CLMgrDoneWithModalUiDialog:
                    break;
                case CLMgrMessage.CLMgrInvokeVoicemailAction:
                    break;
                case CLMgrMessage.CLMgrPhoneBookReloaded:
                    break;
                case CLMgrMessage.CLMgrFederatedLoginSessionExpired:
                    break;
                case CLMgrMessage.CLMgrCloseSwyxIt:
                    break;
                case CLMgrMessage.CLMgrLoadModernSkin:
                    break;
                case CLMgrMessage.CLMgrSwitchToClassic:
                    break;
                case CLMgrMessage.CLMgrOpenCallRouting:
                    break;
                case CLMgrMessage.CLMgrShowCallControl:
                    break;
                case CLMgrMessage.CLMgrNotificationCallsChanged:
                    break;
                case CLMgrMessage.CLMgrOwnPresenceStatusChanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        #endregion

        public void Test()
        {
            Logging.Log("Performing some tests ...", Logging.LogLevel.DEBUG);

            // Analyze COM Object
            var unknownObject = _clientConfig?.PbxPhoneBookEnumerator;
            //var unknownObject = _lineManager.GetUserAppearances();
            var comTypeName = Microsoft.VisualBasic.Information.TypeName(unknownObject);
            Logging.Log($"COM Type name: {comTypeName}", Logging.LogLevel.DEBUG);

            var data = (IDispCollection)unknownObject!;
            var enumerator = data.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        #region User Interface
        public List<UserInterface.SpeedDial> GetSpeedDials()
        {
            if (_lineManager == null)
            {
                Logging.Log("ClientLineMgrClass is not initialized.", Logging.LogLevel.ERROR);
                return [];
            }

            if (_clientConfig == null)
            {
                Logging.Log("IClientConfig is not initialized.", Logging.LogLevel.ERROR);
                return [];
            }

            var speedDials = UserInterface.GetSpeedDials(_lineManager, _clientConfig);
            return speedDials;
        }

        /// <summary>
        /// Opens the specified dialog in the user interface.
        /// </summary>
        /// <param name="dialog">The identifier of the dialog to open.</param>
        public void OpenDialog(UserInterface.DialogId dialog)
        {
            if (_lineManager == null)
            {
                Logging.Log("ClientLineMgrClass is not initialized.", Logging.LogLevel.ERROR);
                return;
            }
            UserInterface.OpenDialog(dialog, _lineManager);
        }

        #endregion

        internal class UserInterface
        {
            internal enum DialogId : uint
            {
                CtiSettings = 0x00000000,
                CallForward = 0x00010000,    // Add 1 to toggle the Default Forwarding (and call up the settings dialog if the number is not set)
                SpeedDials = 0x00020000,     // Add the speed dial ID (0 - 65535) to get a dialog for a specific button
                UserSettings = 0x00030000,
                LocalSettings = 0x00040000
            }

            internal enum SpeedDialState
            {
                Unknown = 0,                 // unknown user or external number
                LoggedOut = 1,               // known user, not logged on to PBX
                Online = 2,                  // known user, logged on to PBX
                Calling = 3,                 // known user, client is in call
                GroupCallNotification = 4,   // known user, that user currently receives a call; notification call
                Away = 5,                    // known user, client is away
                DoNotDisturb = 6             // known user, client is in DND
            }

            public class SpeedDial
            {
                public string Name { get; set; }
                public string Number { get; set; }
                public string Picture { get; set; }
                public string State { get; set; }
                public int UserId { get; set; }
                public int SiteId { get; set; }
            }

            /// <summary>
            /// Opens the specified client UI dialog for the given client instance.
            /// </summary>
            /// <param name="dialog">The identifier of the dialog to open. Specifies which UI dialog will be displayed to the client.</param>
            /// <param name="client">The client instance for which the dialog will be opened. Must not be null.</param>
            internal static void OpenDialog(DialogId dialog, ClientLineMgrClass client)
            {
                client.OpenClientUiDialog((uint)dialog);
            }

            /// <summary>
            /// Retrieves the list of configured speed dials for the specified client line manager.
            /// </summary>
            /// <param name="client">The client line manager instance from which to retrieve speed dial entries. Cannot be null.</param>
            /// <param name="clientConfig">The IClientConfig interface from client line manager instance. Cannot be null.</param>
            /// <exception cref="ArgumentOutOfRangeException">The client has an unknown state.</exception>
            /// <returns>A list of speed dial entries associated with the client. The list will be empty if no speed dials are
            /// configured.</returns>
            internal static List<SpeedDial> GetSpeedDials(ClientLineMgrClass client, IClientConfig clientConfig)
            {
                var speedDials = new List<SpeedDial>();
                client.PubGetNumberOfSpeedDials(out var a);
                Logging.Log($"Available SpeedDials: {a}", Logging.LogLevel.DEBUG);
                for (var i = 0; i <= a - 1; i++)
                {
                    client.PubGetSpeedDialName(i, out var name);
                    client.PubGetSpeedDialNumber(i, out var number);
                    client.PubGetSpeedDialState(i, out var stateId);
                    client.GetUserIdByPhoneNumber(number, out var siteId, out var userId);
                    clientConfig.GetAvatarBitmap(siteId, userId, 0, out var pbModified, out var fileName);

                    var fileCache = clientConfig.FileCacheFolder;
                    var userAvatar = Path.Combine(fileCache, fileName);
                    if (!File.Exists(userAvatar)) userAvatar = string.Empty;

                    if (string.IsNullOrEmpty(name)) continue;

                    var stateObj = (SpeedDialState)stateId;
                    var state = stateObj switch
                    {
                        SpeedDialState.Unknown => "Unbekannt",
                        SpeedDialState.LoggedOut => "Abgemeldet",
                        SpeedDialState.Online => "Online",
                        SpeedDialState.Calling => "Spricht gerade",
                        SpeedDialState.GroupCallNotification => "In einer Konferenz",
                        SpeedDialState.Away => "Abwesend",
                        SpeedDialState.DoNotDisturb => "Nicht stören",
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    Logging.Log($"SpeedDial {i}: {name} - {number} - {state} | UserID: {userId} - SiteID: {siteId}", Logging.LogLevel.DEBUG);
                    speedDials.Add(new SpeedDial
                    {
                        Name = name,
                        Number = number,
                        State = state,
                        Picture = userAvatar,
                        UserId = userId,
                        SiteId = siteId
                    });
                }

                return speedDials;
            }
        }
    }
}
