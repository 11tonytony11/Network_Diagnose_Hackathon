using System;

namespace Network_Diagnose_Hackathon
{
    class Program
    {
        static void Main(string[] args)
        {

            Database db = new Database("db.sqlite");
            db.OpenDB();

            db.ExecuteQuery(Constants.SQL_TABLE_CREATE_QUERY);

            db.closeDB();

            Console.ReadKey();
        }
    }
}
