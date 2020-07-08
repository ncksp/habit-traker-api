
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Constants
{
    public class Connection
    {
        private static readonly String Host = "localhost";
        private static readonly String Username = "postgres";
        private static readonly String Password = "root";
        private static readonly String Database = "abc";
        private static readonly String Port = "5432";
        
        private static String property = "Host="+Host+";Username="+Username+";Password="+Password+";Database="+Database+";Port="+Port+"";
        private static NpgsqlConnection _connection = new NpgsqlConnection(property);

        public static NpgsqlConnection GetConnection()
        {
            _connection.Close();
            _connection.Open();
            return _connection;
        } 

    }
}
