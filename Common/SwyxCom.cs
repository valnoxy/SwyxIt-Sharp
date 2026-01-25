using System.Collections;
using IpPbx.CLMgrLib;
using SwyxSharp.Common.Debug;

namespace SwyxSharp.Common
{
    internal class SwyxClient
    {
        private readonly ClientLineMgrClass? _clientLineMgrClass;

        public SwyxClient()
        {
            try
            {
                Logging.Log("Initializing Swyx Client API ...");
                _clientLineMgrClass = new ClientLineMgrClass();
                Logging.Log("Swyx Client API initialized!");
                Test();
            }
            catch (Exception ex)
            {
                Logging.Log($"Failed to initialize ClientLineMgrClass: {ex.Message}", Logging.LogLevel.ERROR);
                _clientLineMgrClass = null;
            }
        }

        public void Test()
        {
            Logging.Log("Performing some tests ...");
            //_clientLineMgrClass.OpenClientUiDialog(0x00040000);
            UserInterface.ListAllPeers(_clientLineMgrClass);
        }

        #region User Interface
        public List<UserInterface.SpeedDial> GetSpeedDials()
        {
            if (_clientLineMgrClass == null)
            {
                Logging.Log("ClientLineMgrClass is not initialized.", Logging.LogLevel.ERROR);
                return [];
            }
            var speedDials = UserInterface.GetSpeedDials(_clientLineMgrClass);
            return speedDials;
        }

        /// <summary>
        /// Opens the specified dialog in the user interface.
        /// </summary>
        /// <param name="dialog">The identifier of the dialog to open.</param>
        public void OpenDialog(UserInterface.DialogId dialog)
        {
            if (_clientLineMgrClass == null)
            {
                Logging.Log("ClientLineMgrClass is not initialized.", Logging.LogLevel.ERROR);
                return;
            }
            UserInterface.OpenDialog(dialog, _clientLineMgrClass);
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

            public class SpeedDial
            {
                public string Name { get; set; }
                public string Number { get; set; }
                public string Picture { get; set; }
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
            /// <returns>A list of speed dial entries associated with the client. The list will be empty if no speed dials are
            /// configured.</returns>
            internal static List<SpeedDial> GetSpeedDials(ClientLineMgrClass client)
            {
                var speedDials = new List<SpeedDial>();
                client.PubGetNumberOfSpeedDials(out var a);
                Logging.Log($"Available SpeedDials: {a}");
                for (var i = 0; i <= a - 1; i++)
                {
                    client.PubGetSpeedDialName(i, out var name);
                    client.PubGetSpeedDialNumber(i, out var number);
                    if (string.IsNullOrEmpty(name)) continue;
                    
                    Logging.Log($"SpeedDial {i}: {name} - {number}");
                    speedDials.Add(new SpeedDial
                    {
                        Name = name,
                        Number = number,
                        Picture = string.Empty // TODO: Not implemented yet
                    });
                }

                return speedDials;
            }

            internal static void ListAllPeers(ClientLineMgrClass client)
            {
                // TODO: Implement it lol
            }
        }
    }
}
