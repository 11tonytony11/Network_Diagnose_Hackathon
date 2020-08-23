#pragma warning disable CS0618 // Type or member is obsolete
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Net_Diagnose
{
    public class TracertEntry
    {
        // Get and Set functions
        public int HopID { get; set; }
        public string Address { get; set; }
        public string Hostname { get; set; }
        public long ReplyTime { get; set; }
        public IPStatus ReplyStatus { get; set; }

        //---------------------------------------------------------------------------------
        /*
         This function performs trace
         Input:  None
         Output: Error report
        */
        public static IEnumerable<TracertEntry> Tracert(string ipAddress, int maxHops, int timeout)
        {
            IPAddress address = Dns.GetHostAddresses(ipAddress)[0];
            PingOptions pingOptions = new PingOptions(1, true);
            Stopwatch pingReplyTime = new Stopwatch();
            string hostname = string.Empty;
            Ping ping = new Ping();
            PingReply reply;

            do
            {
                pingReplyTime.Start();
                reply = ping.Send(address, timeout, new byte[] { 0 }, pingOptions);
                pingReplyTime.Stop();

                if (reply.Address != null)
                {
                    try
                    {
                        hostname = Dns.GetHostByAddress(reply.Address).HostName;
                    }
                    catch (SocketException) { }
                }

                yield return new TracertEntry()
                {
                    HopID = pingOptions.Ttl,
                    Address = reply.Address == null ? "N/A" : reply.Address.ToString(),
                    Hostname = hostname,
                    ReplyTime = pingReplyTime.ElapsedMilliseconds,
                    ReplyStatus = reply.Status
                };

                pingOptions.Ttl++;
                pingReplyTime.Reset();
            }
            while (reply.Status != IPStatus.Success && pingOptions.Ttl <= maxHops);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete