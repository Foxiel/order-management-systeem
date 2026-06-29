using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class AccountRepository : dbContext
    {
        public Account? GetByUsername(string username)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    account_id,
                    klant_id,
                    email_id,
                    gebruikersnaam,
                    wachtwoord,
                    account_type
                FROM Account
                WHERE gebruikersnaam = @Gebruikersnaam";

            SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Gebruikersnaam", username);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Account
                {
                    AccountId = Convert.ToInt32(reader["account_id"]),
                    Username = reader["gebruikersnaam"].ToString()!,
                    PasswordHash = reader["wachtwoord"].ToString()!,
                    CustomerId = reader["klant_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["klant_id"]),
                    AccountType = reader["account_type"].ToString()!
                };
            }

            return null;
        }

        public bool LoginByUsernameAndPassword(string username, string password)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT COUNT(*)
                FROM Account
                WHERE gebruikersnaam = @Gebruikersnaam
                AND wachtwoord = @Wachtwoord";

            SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@Gebruikersnaam", username);
            cmd.Parameters.AddWithValue("@Wachtwoord", password);

            conn.Open();

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }
    }
}