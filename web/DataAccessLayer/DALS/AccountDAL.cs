//Gemaakt door Tristan
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{

    public class AccountDAL : BaseDAL
    {
        public Account? GetByUsername(string username)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT *
                FROM Account
                WHERE Username = @Username";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Account
                {
                    AccountId = Convert.ToInt32(reader["AccountId"]),
                    Username = reader["Username"].ToString()!,
                    PasswordHash = reader["PasswordHash"].ToString()!,
                    CustomerId = Convert.ToInt32(reader["CustomerId"])
                };
            }

            return null;
        }
    }
}