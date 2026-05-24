//Gemaakt door Tristan
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class ProductDAL : BaseDAL
    {
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT 
                    p.ProductEAN,
                    p.ProductName,
                    p.ProductDescription,
                    p.ProductSpecification,
                    p.ProductPrice,
                    p.ProductStock,
                    p.ProductWeightKg,
                    p.ProductWarrantyMonths,
                    p.ProductReleaseDate,
                    p.ManufacturerId,
                    p.SubcategoryId,
                    pi.ImageUrl
                FROM Product p
                LEFT JOIN ProductImage pi 
                    ON p.ProductEAN = pi.ProductEAN 
                    AND pi.IsMainImage = 1";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    ProductEAN = reader["ProductEAN"].ToString()!,
                    ProductName = reader["ProductName"].ToString()!,
                    ProductDescription = reader["ProductDescription"].ToString(),
                    ProductSpecification = reader["ProductSpecification"].ToString(),
                    ProductPrice = Convert.ToDecimal(reader["ProductPrice"]),
                    ProductStock = Convert.ToInt32(reader["ProductStock"]),
                    ProductWeightKg = Convert.ToDecimal(reader["ProductWeightKg"]),
                    ProductWarrantyMonths = Convert.ToInt32(reader["ProductWarrantyMonths"]),
                    ProductReleaseDate = Convert.ToDateTime(reader["ProductReleaseDate"]),
                    ManufacturerId = reader["ManufacturerId"].ToString()!,
                    SubcategoryId = reader["SubcategoryId"].ToString()!,
                    ImageUrl = reader["ImageUrl"] == DBNull.Value ? null : reader["ImageUrl"].ToString()
                });
            }

            return products;
        }
    }
}