using System;

namespace Net_Diagnose
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
