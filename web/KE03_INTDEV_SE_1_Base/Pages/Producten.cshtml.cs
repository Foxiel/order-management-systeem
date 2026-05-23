using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class ProductenModel(IProductRepository productRepository) : PageModel
    {
        public IList<Product> Products { get; set; } = new List<Product>();

        public void OnGet()
        {
            Products = productRepository.GetAllProducts().ToList();
        }
    }
}