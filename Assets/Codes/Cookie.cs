using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AD.JS
{
    public class Cookie
    {

        [DllImport("__Internal")]
        private static extern string getToken();

        public static string Token() => getToken();
    }
}