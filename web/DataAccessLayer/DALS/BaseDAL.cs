//Gemaakt door Tristan
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    /// <summary>
    /// Deze DAL is gemaakt zodat we de connectie niet elke keer opnieuw hoeven te schrijven in een nieuwe DAL.
    /// Hierdoor kunnen we makkelijk nieuwe DALs maken die we nodig hebben zonder merge conflicts te veroorzaken.
    /// </summary>
    public class BaseDAL
    {
        protected readonly string _connectionString;

        public BaseDAL()
        {
            var webserver = "tt-server-01.database.windows.net";
            var database = "Matrix-inc";
            var username = "server-root";
            var password = "m4^m^dd4ScWsJF*";

            _connectionString =
                $"Server=tcp:{webserver},1433;" +
                $"Initial Catalog={database};" +
                $"User ID={username};" +
                $"Password={password};" +
                $"Encrypt=True;" +
                $"TrustServerCertificate=False;" +
                $"Connection Timeout=60;" +
                $"MultipleActiveResultSets=False;";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}