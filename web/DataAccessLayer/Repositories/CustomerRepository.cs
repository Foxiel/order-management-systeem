using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class CustomerRepository : dbContext
    {
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    klant_id,
                    naam,
                    telefoon,
                    adres,
                    postcode,
                    woonplaats,
                    land
                FROM Klant";

            SqlCommand cmd = new(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerId = Convert.ToInt32(reader["klant_id"]),
                    CustomerName = reader["naam"].ToString()!,
                    CustomerPhone = reader["telefoon"].ToString(),
                    CustomerAddress = reader["adres"].ToString()!,
                    CustomerPostalCode = reader["postcode"].ToString()!,
                    CustomerCity = reader["woonplaats"].ToString()!,
                    CustomerCountry = reader["land"].ToString()!
                });
            }

            return customers;
        }

        public CustomerProfile? GetCustomerProfileByUsername(string username)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT TOP 1
                    k.klant_id,
                    k.naam,
                    k.telefoon,
                    k.adres,
                    k.postcode,
                    k.woonplaats,
                    k.land,
                    e.email,
                    ba.adres_id,
                    ba.adres AS bezorg_adres,
                    ba.postcode AS bezorg_postcode,
                    ba.woonplaats AS bezorg_woonplaats,
                    ba.land AS bezorg_land
                FROM Account a
                INNER JOIN Klant k
                    ON a.klant_id = k.klant_id
                INNER JOIN Email e
                    ON a.email_id = e.email_id
                LEFT JOIN BezorgAdres ba
                    ON k.klant_id = ba.klant_id
                WHERE a.gebruikersnaam = @Gebruikersnaam";

            SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Gebruikersnaam", username);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new CustomerProfile
                {
                    CustomerId = Convert.ToInt32(reader["klant_id"]),
                    CustomerName = reader["naam"].ToString()!,
                    CustomerEmail = reader["email"].ToString()!,
                    CustomerPhone = reader["telefoon"].ToString(),

                    AddressId = reader["adres_id"] == DBNull.Value ? null : Convert.ToInt32(reader["adres_id"]),
                    Street = reader["bezorg_adres"] == DBNull.Value ? reader["adres"].ToString() : reader["bezorg_adres"].ToString(),
                    PostalCode = reader["bezorg_postcode"] == DBNull.Value ? reader["postcode"].ToString() : reader["bezorg_postcode"].ToString(),
                    City = reader["bezorg_woonplaats"] == DBNull.Value ? reader["woonplaats"].ToString() : reader["bezorg_woonplaats"].ToString(),
                    Country = reader["bezorg_land"] == DBNull.Value ? reader["land"].ToString() : reader["bezorg_land"].ToString()
                };
            }

            return null;
        }

        public bool UpdateCustomerProfile(CustomerProfile profile)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                UPDATE Klant
                SET
                    naam = @Naam,
                    telefoon = @Telefoon,
                    adres = @Adres,
                    postcode = @Postcode,
                    woonplaats = @Woonplaats,
                    land = @Land
                WHERE klant_id = @KlantId;

                UPDATE Email
                SET email = @Email
                WHERE email_id = (
                    SELECT email_id
                    FROM Account
                    WHERE klant_id = @KlantId
                );

                UPDATE BezorgAdres
                SET
                    adres = @Adres,
                    postcode = @Postcode,
                    woonplaats = @Woonplaats,
                    land = @Land
                WHERE adres_id = @AdresId
                AND klant_id = @KlantId;
            ";

            SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@KlantId", profile.CustomerId);
            cmd.Parameters.AddWithValue("@Naam", profile.CustomerName);
            cmd.Parameters.AddWithValue("@Email", profile.CustomerEmail);
            cmd.Parameters.AddWithValue("@Telefoon", (object?)profile.CustomerPhone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AdresId", (object?)profile.AddressId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Adres", (object?)profile.Street ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Postcode", (object?)profile.PostalCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Woonplaats", (object?)profile.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Land", (object?)profile.Country ?? DBNull.Value);

            conn.Open();

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }
        public CustomerProfile? GetCustomerProfileById(int klantId)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
        SELECT TOP 1
            k.klant_id,
            k.naam,
            k.telefoon,
            k.adres,
            k.postcode,
            k.woonplaats,
            k.land,
            e.email,
            ba.adres_id,
            ba.adres AS bezorg_adres,
            ba.postcode AS bezorg_postcode,
            ba.woonplaats AS bezorg_woonplaats,
            ba.land AS bezorg_land
        FROM Klant k
        INNER JOIN Account a
            ON a.klant_id = k.klant_id
        INNER JOIN Email e
            ON a.email_id = e.email_id
        LEFT JOIN BezorgAdres ba
            ON ba.klant_id = k.klant_id
        WHERE k.klant_id = @KlantId";

            SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@KlantId", klantId);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new CustomerProfile
                {
                    CustomerId = Convert.ToInt32(reader["klant_id"]),
                    CustomerName = reader["naam"].ToString()!,
                    CustomerEmail = reader["email"].ToString()!,
                    CustomerPhone = reader["telefoon"].ToString(),

                    AddressId = reader["adres_id"] == DBNull.Value ? null : Convert.ToInt32(reader["adres_id"]),
                    Street = reader["bezorg_adres"] == DBNull.Value ? reader["adres"].ToString() : reader["bezorg_adres"].ToString(),
                    PostalCode = reader["bezorg_postcode"] == DBNull.Value ? reader["postcode"].ToString() : reader["bezorg_postcode"].ToString(),
                    City = reader["bezorg_woonplaats"] == DBNull.Value ? reader["woonplaats"].ToString() : reader["bezorg_woonplaats"].ToString(),
                    Country = reader["bezorg_land"] == DBNull.Value ? reader["land"].ToString() : reader["bezorg_land"].ToString()
                };
            }

            return null;
        }
    }
}