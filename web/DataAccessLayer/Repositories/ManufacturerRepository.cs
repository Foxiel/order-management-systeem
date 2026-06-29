//Gemaakt door Tristan
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class ManufacturerRepository : dbContext
    {
        public List<Manufacturer> GetAllManufacturers()
        {
            List<Manufacturer> manufacturers = new();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    leverancier_id,
                    naam
                FROM Leverancier
                ORDER BY naam";

            SqlCommand cmd = new(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                manufacturers.Add(new Manufacturer
                {
                    ManufacturerId = Convert.ToInt32(reader["leverancier_id"]),
                    ManufacturerName = reader["naam"].ToString()!
                });
            }

            return manufacturers;
        }
    }
}