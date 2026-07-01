//Gemaakt door Tristan
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class ProductRepository : dbContext
    {
        public List<Product> GetAllProducts()
        {
            List<Product> products = new();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    p.product_id,
                    p.ean,
                    l.leverancier_id AS leverancier_id,
                    l.naam AS leverancier,
                    p.locatie_id,
                    p.naam,
                    p.beschrijving,
                    p.prijs,
                    p.gewicht,
                    p.garantie,
                    p.huidige_voorraad,
                    p.minimum_voorraad,
                    p.status
                FROM Product p
                JOIN Leverancier l
                    ON p.leverancier_id = l.leverancier_id";

            SqlCommand cmd = new(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    ProductId = Convert.ToInt32(reader["product_id"]),
                    ProductEAN = reader["ean"].ToString()!,
                    ProductName = reader["naam"].ToString()!,
                    ProductDescription = reader["beschrijving"].ToString(),
                    ProductPrice = Convert.ToDecimal(reader["prijs"]),
                    ProductWeightKg = Convert.ToDecimal(reader["gewicht"]),
                    ProductWarranty = reader["garantie"].ToString()!,
                    ManufacturerId = Convert.ToInt32(reader["leverancier_id"]),
                    Manufacturer = reader["leverancier"].ToString()!,
                    huidige_voorraad = Convert.ToInt32(reader["huidige_voorraad"]),
                    minimum_voorraad = Convert.ToInt32(reader["minimum_voorraad"]),
                    status = reader["status"].ToString()
                });
            }

            return products;
        }

        public Product? GetProductByEAN(string ean)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    p.product_id,
                    p.ean,
                    l.leverancier_id  AS leverancier_id,
                    l.naam AS leverancier,
                    p.locatie_id,
                    p.naam,
                    p.beschrijving,
                    p.prijs,
                    p.gewicht,
                    p.garantie,
                    p.huidige_voorraad,
                    p.minimum_voorraad,
                    p.status,
                    pa.pad_locatie,
                    s.naam AS specificatie
                FROM Product p
                JOIN ProductAfbeelding pa
                    ON p.product_id = pa.product_id
                    AND pa.volgorde = 1
                JOIN Leverancier l
                    ON p.leverancier_id = l.leverancier_id
                JOIN ProductSpecificatie ps
                    ON p.product_id = ps.product_id
                JOIN Specificatie s
                    ON ps.specificatie_id = s.specificatie_id
                WHERE p.ean = @EAN";

            SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@EAN", ean);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Product
                {
                    ProductId = Convert.ToInt32(reader["product_id"]),
                    ProductEAN = reader["ean"].ToString()!,
                    ProductName = reader["naam"].ToString()!,
                    ProductDescription = reader["beschrijving"].ToString(),
                    ProductPrice = Convert.ToDecimal(reader["prijs"]),
                    ProductWeightKg = Convert.ToDecimal(reader["gewicht"]),
                    ProductWarranty = reader["garantie"].ToString()!,
                    ManufacturerId = Convert.ToInt32(reader["leverancier_id"]),
                    Manufacturer = reader["leverancier"].ToString()!,
                    huidige_voorraad = Convert.ToInt32(reader["huidige_voorraad"]),
                    minimum_voorraad = Convert.ToInt32(reader["minimum_voorraad"]),
                    status = reader["status"].ToString(),
                    ImageUrl = reader["pad_locatie"] == DBNull.Value ? null : reader["pad_locatie"].ToString(),
                    Specification = reader["specificatie"].ToString()!
                };
            }

            return null;
        }
    }
}