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
    }
}