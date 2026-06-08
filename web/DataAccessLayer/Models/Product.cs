//Gemaakt door Tristan

namespace DataAccessLayer.Models;

public class Product
{
    public string ProductEAN { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? ProductDescription { get; set; }
    public string? ProductSpecification { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStock { get; set; }
    public decimal ProductWeightKg { get; set; }
    public string ProductWarranty { get; set; } = string.Empty;
    public int ManufacturerId { get; set; }
    public int SubcategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public int huidige_voorraad { get; set; }
    public int minimum_voorraad { get; set; }
    public string? status { get; set; }


}