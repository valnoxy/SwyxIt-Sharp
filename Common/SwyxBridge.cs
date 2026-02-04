namespace SwyxSharp.Common
{
    public class SwyxBridge
    {
        internal static SwyxClient? SwyxClient;

        internal static class Myself
        {
            internal static string Username { get; set; }
            internal static string CallerId { get; set; }
            internal static string AwayText { get; set; }
        }
        
        /// <summary>
        ///  Initialize Swyx API
        /// </summary>
        public static void Initialize()
        {
            SwyxClient = new SwyxClient();
        }

        public static void Shutdown()
        {
            SwyxClient.Shutdown();
        }
    }
}
