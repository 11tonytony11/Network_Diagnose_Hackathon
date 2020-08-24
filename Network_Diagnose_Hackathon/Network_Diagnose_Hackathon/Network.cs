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
        public string[] Nslookup()
        {
            IPHostEntry DnsIP;
            int counter = 0;

            string[] ans = new string[Constants.LENGTH] { "Everything is ok", "No Action requierd" };

            for (int i = 0; i < targerts.Length; i++)
            {
                try
                {
                    DnsIP = Dns.GetHostByName(targerts[i]);
                }
                catch 
                {
                    counter++;
                }
            }

            if(counter == 1)
            {
                ans[0] = "There seems to be an issue in only one website";
                ans[1] = "Try using different website";
            }

            if(counter > 1)
            {
                ans[0] = "The seems to be DNS issue";
                ans[1] = "Change your DNS server";
            }
            return ans;
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
            string[] ans = new string[Constants.LENGTH] { "Everything is ok", "No Action requierd" };
            int counter = 0;
            int ptr = 0;

            for (int i = 0; i < targerts.Length; i++)
            {
                foreach (var entry in TracertEntry.Tracert(targerts[i], Constants.MAX_TTL, Constants.TIMEOUT))
                {
                    if (entry.ReplyTime > Constants.MAX_LATENCY || (entry.ReplyStatus != IPStatus.Success && entry.ReplyStatus != IPStatus.TtlExpired))
                    {
                        errors[ptr] = true;
                        counter++;
                    }
                    ptr++;
                }
            }

            if (errors[0] == errors[1] == errors[2] == errors[3] == true)
            {
                ans[0] = "There seems to be a problem connecting to all websites";
                ans[1] = "Make sure router is connected via dsl/coax/fiber. If issue persist contact your ISP";
            }

            if (errors[0] == false && errors[1] == errors[2] == errors[3] == true)
            {
                ans[0] = "There seems to be a regional issue with the ISP so you cant connect to websites from abroad";
                ans[1] = "Contact your ISP to get details regarding fix ETA";
            }
            if(counter == 1)
            {
                ans[0] = "There seems to be an issue only on specific website";
                ans[1] = "Try surfing on different websites";
            }
            return errors;
        }
        //---------------------------------------------------------------------------------
        /*
         This function checks connection to gateway
         Input:  None
         Output: Error report
        */
        public string[] Gateway_Check()
        {
            NetworkInterface[] allNICs = NetworkInterface.GetAllNetworkInterfaces();
            string[] ans = new string[2] { "Connection is Good", "Nothing to fix" };

            foreach (var nic in allNICs)
            {
                var ipProp = nic.GetIPProperties();
                var gwAddresses = ipProp.GatewayAddresses;
                if (gwAddresses.Count > 0 &&
                    gwAddresses.FirstOrDefault(g => g.Address.AddressFamily == AddressFamily.InterNetwork) != null)
                {
                    if (ipProp.UnicastAddresses.First(d => d.Address.AddressFamily == AddressFamily.InterNetwork).Address.ToString().Contains("169.254") == false)
                    {
                        ans[0] = "There is a connection error between this computer and the router";
                        ans[1] = "Make sure your computer is connected via cable or Wi-Fi. If the issue is not resolved try restarting the router.";
                    }
                }
            }
            return ans;
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
