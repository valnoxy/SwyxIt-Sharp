namespace SwyxSharp.Common
{
    public class SwyxEnums
    {
        public enum DialogId : uint
        {
            CtiSettings = 0x00000000,
            CallForward = 0x00010000,    // Add 1 to toggle the Default Forwarding (and call up the settings dialog if the number is not set)
            SpeedDials = 0x00020000,     // Add the speed dial ID (0 - 65535) to get a dialog for a specific button
            UserSettings = 0x00030000,
            LocalSettings = 0x00040000
        }

        public enum SpeedDialState
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
            public SpeedDialState SpeedDialState { get; set; }
            public string State { get; set; }
            public int UserId { get; set; }
            public int SiteId { get; set; }
        }
    }
}
