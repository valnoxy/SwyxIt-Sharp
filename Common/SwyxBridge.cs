namespace SwyxSharp.Common
{
    public class SwyxBridge
    {
        internal static SwyxClient _swyxClient;
        /// <summary>
        ///  Initialize Swyx API
        /// </summary>
        public static void Initialize()
        {
            _swyxClient = new SwyxClient();
        }
    }
}
