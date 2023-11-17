using System.Runtime.InteropServices;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class OpenLinks
    {
        [DllImport("__Internal")]
        private static extern void OpenTab(string url);
        [DllImport("__Internal")]
        private static extern void GoBack();

        public static void OpenURL(string url)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            OpenTab(url); 
#endif
        }

        public static void GoBackPage()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            GoBack();
#endif
        }
    }
}