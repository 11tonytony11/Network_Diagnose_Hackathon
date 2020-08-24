using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Network_Diagnose_Hackathon
{
    class Database
    {
        SQLiteConnection m_dbConnection;
        string file;

        public Database (string path)
        {
            this.file = path;

            if (!File.Exists(this.file)) this.createDB(this.file);
            this.OpenDB();
        }

        private void createDB(string path) 
        {
            SQLiteConnection.CreateFile(path); // file type is .sqlite
            this.file = path;
        }

        private void OpenDB()
        {
            this.m_dbConnection = new SQLiteConnection("Data Source=" + this.file + ";Version=3;");
            m_dbConnection.Open();
        }

        private SQLiteDataReader ExecuteQuery(string query) 
            // CREATE TABLE IF NOT EXISTS Diag ( name TEXT PRIMARY KEY, router_counter INTEGER, dns_counter INTEGER, trace_counter INTEGER )
        {
            SQLiteCommand cmd = new SQLiteCommand(query, m_dbConnection);
            cmd.ExecuteNonQuery();
            SQLiteDataReader reader = cmd.ExecuteReader();
            return reader;
        }

    }
}
