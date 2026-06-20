//Gemaakt door Tristan
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccessLayer.DAL
{
    /// <summary>
    /// Deze DAL is gemaakt zodat we de connectie niet elke keer opnieuw hoeven te schrijven in een nieuwe DAL.
    /// Hierdoor kunnen we makkelijk nieuwe DALs maken die we nodig hebben zonder merge conflicts te veroorzaken.
    /// </summary>
    public class BaseDAL
    {
        private readonly string _connectionString = @"Server=tcp:sql.bsite.net\MSSQL2016;Database=coldfire0412_MatrixInc;User ID=coldfire0412_MatrixInc;Password=4LZC#jz5wCk^3kY;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}