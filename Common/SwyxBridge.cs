namespace SwyxSharp.Common
{
    public class SwyxBridge
    {
        internal static SwyxClient? SwyxClient;
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
