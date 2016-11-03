using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Birth_First
{
    class Network
    {
        public static bool Network_Connected()
        {
            if (!(System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()))
                return false;

            string testhost = "http://www.google.com";

            HttpWebRequest webreq = null;
            HttpWebResponse webres = null;
            try
            {
                webreq = (HttpWebRequest)WebRequest.Create(testhost);
                webreq.Method = "HEAD";
                webres = (HttpWebResponse)webreq.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (webres != null)
                    webres.Close();
            }
        }
    }
}
