using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class ProductenModel : PageModel
    {
        public IList<Product> Products { get; set; } = new List<Product>();

        public IList<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<int> ManufacturerIds { get; set; } = new();

        public void OnGet()
        {
            ProductRepository productRepository = new ProductRepository();
            ManufacturerRepository manufacturerRepository = new ManufacturerRepository();

            Manufacturers = manufacturerRepository.GetAllManufacturers();

            var products = productRepository.GetAllProducts().AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                    (p.ProductDescription != null &&
                     p.ProductDescription.Contains(Search, StringComparison.OrdinalIgnoreCase)) ||
                    p.ProductEAN.Contains(Search, StringComparison.OrdinalIgnoreCase));
            }

            // Filter op geselecteerde fabrikanten
            if (ManufacturerIds.Any())
            {
                products = products.Where(p => ManufacturerIds.Contains(p.ManufacturerId));
            }

            products = SortBy switch
            {
                "price-low" => products.OrderBy(p => p.ProductPrice),
                "price-high" => products.OrderByDescending(p => p.ProductPrice),
                "az" => products.OrderBy(p => p.ProductName),
                "za" => products.OrderByDescending(p => p.ProductName),
                "stock" => products.OrderByDescending(p => p.huidige_voorraad),
                _ => products
            };

            Products = products.ToList();
        }
    }
}