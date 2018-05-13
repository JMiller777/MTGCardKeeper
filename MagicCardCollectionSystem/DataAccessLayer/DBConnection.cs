using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    internal static class DBConnection
    {
        public static SqlConnection GetDBConnection()
        {
            // Only place in the application where connection string appears.
            // Only method any class uses to create a DB connection object
            var connString = @"Data Source=localhost;Initial Catalog=MagicDB;Integrated Security=True";
            var conn = new SqlConnection(connString);
            return conn;
        } 
    }
}
