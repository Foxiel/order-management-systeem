using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class ProductModel : PageModel
    {
        // Gemaakt door Fabian
        public Product? Product { get; set; }

        // id komt uit querystring: /Product?id=ACT-003
        public void OnGet([FromQuery] string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Product = null;
                return;
            }

            ProductRepository productRepository = new ProductRepository();
            Product = productRepository.GetProductByEAN(id);
        }
    }
}