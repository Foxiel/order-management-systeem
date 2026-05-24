//Gemaakt door Tristan
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    /// <summary>
    /// Deze DAL is gemaakt zodat we de connectie niet elke keer opnieuw hoeven te schrijven in een nieuwe DAL.
    /// Hierdoor kunnen we makkelijk nieuwe DALS maken die we nodig zouden hebben zonder merge conflics te veroorzaken.
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
                $"Server={webserver};Database={database};User Id={username};Password={password};TrustServerCertificate=True;";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}