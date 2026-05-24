using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace DataAccessLayer.DAL
{
    public class DAL
    {
        private readonly string _connectionString;
        public DAL()
        {
            var webserver = "tt-server-01.database.windows.net";
            var database = "Matrix-inc";
            var username = "server-root";
            var password = "m4^m^dd4ScWsJF*";
            _connectionString =
                $"Server={webserver};Database={database};User Id={username};Password={password};TrustServerCertificate=True;";
        }
    }
}