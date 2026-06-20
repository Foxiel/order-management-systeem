using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class CategoryDAL : BaseDAL
    {
        public List<Category> GetAllCategories()
        {
            List<Category> categories = new();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    categorie_id,
                    naam
                FROM Categorie";

            SqlCommand cmd = new(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Category
                {
                    CategoryId = reader["categorie_id"].ToString()!,
                    CategoryName = reader["naam"].ToString()!
                });
            }

            return categories;
        }
    }
}