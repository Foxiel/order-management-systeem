//Gemaakt door Tristan
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class CategoryDAL : BaseDAL
    {
        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    CategoryId,
                    CategoryName
                FROM Category";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Category
                {
                    CategoryId = reader["CategoryId"].ToString()!,
                    CategoryName = reader["CategoryName"].ToString()!
                });
            }

            return categories;
        }
    }
}