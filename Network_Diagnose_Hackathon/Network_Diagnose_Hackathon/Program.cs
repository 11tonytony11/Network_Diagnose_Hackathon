using System;

namespace Network_Diagnose_Hackathon
{
    class Program
    {
        static void Main(string[] args)
        {
            Network handler = new Network();
            Console.WriteLine(handler.Gateway_Check());

            Database db = new Database("db.sqlite");
            db.OpenDB();

            db.ExecuteQuery(Constants.SQL_TABLE_CREATE_QUERY);

            db.closeDB();

            Console.ReadKey();
        }
    }
}
