#pragma warning disable CS0618 // Type or member is obsolete
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Network_Diagnose_Hackathon
{
    class Network
    {
        string[] targerts;
        readonly string  _fb_url;
        readonly string _ggl_url;
        readonly string _bng_url;
        readonly string _iix_url;

        //---------------------------------------------------------------------------------
        /*
         This function is the c'tor
         Input:  None
         Output: object init
        */
        public Network()
        {
            _fb_url  = "facebook.com";
            _iix_url = "isoc.org.il";
            _ggl_url = "google.com";
            _bng_url = "bing.com";

            targerts = new string[Constants.NUM_OF_TARGETS] { _iix_url, _fb_url, _ggl_url, _bng_url };
        }
        //---------------------------------------------------------------------------------
        /*
         This function is checking the dns server
         Input:  None
         Output: Error report
        */
        public bool[] Nslookup()
        {
            IPHostEntry DnsIP;
            bool[] errors = new bool[Constants.NUM_OF_TARGETS] {false, false, false, false};

            for (int i = 0; i < targerts.Length; i++)
            {
                try
                {
                    DnsIP = Dns.GetHostByName(targerts[i]);
                }
                catch 
                {
                    errors[i] = true;
                }
            }
            return errors;
        }
        //---------------------------------------------------------------------------------
        /*
         This function checks routing to Israel and abroad
         Input:  None
         Output: Error report
        */
        public bool[] Trace()
        {
            bool[] errors = new bool[Constants.NUM_OF_TARGETS] { false, false, false, false };
            int ptr = 0;

            for (int i = 0; i < targerts.Length; i++)
            {
                foreach (var entry in TracertEntry.Tracert(targerts[i], Constants.MAX_TTL, Constants.TIMEOUT))
                {
                    if (entry.ReplyTime > Constants.MAX_LATENCY || (entry.ReplyStatus != IPStatus.Success && entry.ReplyStatus != IPStatus.TtlExpired))
                    {
                        errors[ptr] = true;
                    }
                    ptr++;
                }
            }
            return errors;
        }
        //---------------------------------------------------------------------------------
        /*
         This function checks connection to gateway
         Input:  None
         Output: Error report
        */
        public bool Gateway_Check()
        {
            NetworkInterface[] allNICs = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nic in allNICs)
            {
                var ipProp = nic.GetIPProperties();
                var gwAddresses = ipProp.GatewayAddresses;
                if (gwAddresses.Count > 0 &&
                    gwAddresses.FirstOrDefault(g => g.Address.AddressFamily == AddressFamily.InterNetwork) != null)
                {
                    if (ipProp.UnicastAddresses.First(d => d.Address.AddressFamily == AddressFamily.InterNetwork).Address.ToString().Contains("169.254") == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete