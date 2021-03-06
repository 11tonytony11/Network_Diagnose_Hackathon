﻿using System;
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

        public void OpenDB()
        {
            this.m_dbConnection = new SQLiteConnection("Data Source=" + this.file + ";Version=3;");
            m_dbConnection.Open();
        }

        public SQLiteDataReader ExecuteQuery(string query) 
            // CREATE TABLE IF NOT EXISTS Diag ( name TEXT PRIMARY KEY, router_counter INTEGER, dns_counter INTEGER, trace_counter INTEGER, highest TEXT )
        {
            SQLiteCommand cmd = new SQLiteCommand(query, m_dbConnection);
            cmd.ExecuteNonQuery();
            SQLiteDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        
        public void closeDB()
        {
            this.m_dbConnection.Close();
        }

        public void InsertUser(string name)
        {
            this.ExecuteQuery(String.Format("INSERT INTO Diag (name, router_counter, dns_counter, trace_counter, highest) VALUES({0}, {1}, {2}, {3}); ", name, 0, 0, 0, "router_counter"));
        }

        public void IncrementCounter(string name, string field)
        {
            int router = 0, dns = 0, trace = 0;
            string highest = "";

            this.ExecuteQuery(string.Format("UPDATE Diag SET {0} = {0} + 1 WHERE name = {1}; ", field, name));
            SQLiteDataReader reader = this.ExecuteQuery("Select * From Diag");

            while (reader.Read())
            {
                router = Int16.Parse(reader["router_counter"].ToString());
                dns = Int16.Parse(reader["dns_counter"].ToString());
                trace = Int16.Parse(reader["trace_counter"].ToString());
            }

            if ((router > dns) && (router > trace))
            {
                highest = "router_counter";
            }
            else if ((dns > router) && (dns > trace))
            {
                highest = "dns_counter";
            }
            else if ((trace > router) && (trace > dns))
            {
                highest = "trace_counter";
            }

            this.ExecuteQuery(string.Format("UPDATE Diag SET highest = {0} WHERE name = {1}; ", highest, name));

        }
    }
}
