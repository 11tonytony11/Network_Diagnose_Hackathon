using System;

namespace Network_Diagnose_Hackathon
{
    class Program
    {
        static void Main(string[] args)
        {
            Network handler = new Network();
            Console.WriteLine(handler.Gateway_Check());
            Console.ReadKey();
        }
    }
}
