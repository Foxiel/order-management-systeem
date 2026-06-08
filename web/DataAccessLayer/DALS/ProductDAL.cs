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
                    p.product_id,
                    p.EAN,
                    p.leverancier_id,
                    p.locatie_id,
                    p.naam,
                    p.beschrijving,
                    p.prijs,
                    p.gewicht,
                    p.garantie,
                    p.huidige_voorraad,
                    p.minimum_voorraad,
                    p.status
                FROM Product p";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    ProductEAN = reader["EAN"].ToString()!,
                    ProductName = reader["naam"].ToString()!,
                    ProductDescription = reader["beschrijving"] == DBNull.Value
                    ? null
                    : reader["beschrijving"].ToString(),

                    ProductPrice = Convert.ToDecimal(reader["prijs"]),
                    ProductWeightKg = Convert.ToDecimal(reader["gewicht"]),
                    ProductWarranty = reader["garantie"].ToString()!,

                    ManufacturerId = Convert.ToInt32(reader["leverancier_id"]),

                    huidige_voorraad = Convert.ToInt32(reader["huidige_voorraad"]),
                    minimum_voorraad = Convert.ToInt32(reader["minimum_voorraad"]),
                    status = reader["status"].ToString()
                });
            }

            return products;
        }

        // Gemaakt door Fabian
        public Product GetProductByEAN(string ean)
        {
            Product product = new Product();

            using SqlConnection conn = GetConnection();

            string query = query = @"
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
            AND pi.IsMainImage = 1
        WHERE p.ProductEAN = @ProductEAN";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductEAN", ean);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                product = new Product
                {
                    ProductEAN = reader["ProductEAN"].ToString()!,
                    ProductName = reader["ProductName"].ToString()!,
                    ProductDescription = reader["ProductDescription"].ToString(),
                    ProductSpecification = reader["ProductSpecification"].ToString(),
                    ProductPrice = Convert.ToDecimal(reader["ProductPrice"]),
                    ProductStock = Convert.ToInt32(reader["ProductStock"]),
                    ProductWeightKg = Convert.ToDecimal(reader["ProductWeightKg"]),
                    ProductWarranty = reader["ProductWarranty"].ToString()!,
                    ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]),
                    SubcategoryId = Convert.ToInt32(reader["SubcategoryId"]),
                    ImageUrl = reader["ImageUrl"] == DBNull.Value ? null : reader["ImageUrl"].ToString()
                };
            }

            return product;
        }
    }
}