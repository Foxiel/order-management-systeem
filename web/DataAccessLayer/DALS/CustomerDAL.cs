//Gemaakt door Tristan
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class CustomerDAL : BaseDAL
    {
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT 
                    CustomerId,
                    CustomerName,
                    CustomerEmail,
                    CustomerPhone
                FROM Customer";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerId = Convert.ToInt32(reader["CustomerId"]),
                    CustomerName = reader["CustomerName"].ToString()!,
                    CustomerEmail = reader["CustomerEmail"].ToString()!,
                    CustomerPhone = reader["CustomerPhone"] == DBNull.Value ? null : reader["CustomerPhone"].ToString()
                });
            }

            return customers;
        }

        public CustomerProfile? GetCustomerProfileByUsername(string username)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
        SELECT TOP 1
            c.CustomerId,
            c.CustomerName,
            c.CustomerEmail,
            c.CustomerPhone,

            ad.AddressId,
            ad.Street,
            ad.HouseNumber,
            ad.PostalCode,
            ad.City,
            ad.Country,
            ad.AddressType

        FROM Account a

        INNER JOIN Customer c
            ON a.CustomerId = c.CustomerId

        LEFT JOIN Address ad
            ON c.CustomerId = ad.CustomerId

        WHERE a.Username = @Username";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new CustomerProfile
                {
                    CustomerId =
                        Convert.ToInt32(reader["CustomerId"]),

                    CustomerName =
                        reader["CustomerName"].ToString()!,

                    CustomerEmail =
                        reader["CustomerEmail"].ToString()!,

                    CustomerPhone =
                        reader["CustomerPhone"] == DBNull.Value
                            ? null
                            : reader["CustomerPhone"].ToString(),

                    AddressId =
                        reader["AddressId"] == DBNull.Value
                            ? null
                            : Convert.ToInt32(reader["AddressId"]),

                    Street =
                        reader["Street"] == DBNull.Value
                            ? null
                            : reader["Street"].ToString(),

                    HouseNumber =
                        reader["HouseNumber"] == DBNull.Value
                            ? null
                            : reader["HouseNumber"].ToString(),

                    PostalCode =
                        reader["PostalCode"] == DBNull.Value
                            ? null
                            : reader["PostalCode"].ToString(),

                    City =
                        reader["City"] == DBNull.Value
                            ? null
                            : reader["City"].ToString(),

                    Country =
                        reader["Country"] == DBNull.Value
                            ? null
                            : reader["Country"].ToString(),

                    AddressType =
                        reader["AddressType"] == DBNull.Value
                            ? null
                            : reader["AddressType"].ToString()
                };
            }

            return null;
        }

        public bool UpdateCustomerProfile(CustomerProfile profile)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
        UPDATE Customer
        SET
            CustomerName = @CustomerName,
            CustomerEmail = @CustomerEmail,
            CustomerPhone = @CustomerPhone
        WHERE CustomerId = @CustomerId;

        UPDATE Address
        SET
            Street = @Street,
            HouseNumber = @HouseNumber,
            PostalCode = @PostalCode,
            City = @City,
            Country = @Country,
            AddressType = @AddressType
        WHERE AddressId = @AddressId
        AND CustomerId = @CustomerId;
    ";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@CustomerId", profile.CustomerId);
            cmd.Parameters.AddWithValue("@CustomerName", profile.CustomerName);
            cmd.Parameters.AddWithValue("@CustomerEmail", profile.CustomerEmail);
            cmd.Parameters.AddWithValue("@CustomerPhone", (object?)profile.CustomerPhone ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@AddressId", (object?)profile.AddressId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Street", (object?)profile.Street ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@HouseNumber", (object?)profile.HouseNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PostalCode", (object?)profile.PostalCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@City", (object?)profile.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Country", (object?)profile.Country ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AddressType", (object?)profile.AddressType ?? DBNull.Value);

            conn.Open();

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}