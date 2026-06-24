using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class ProductenModel : PageModel
    {
        public IList<DataAccessLayer.Models.Product> Products { get; set; } = new List<DataAccessLayer.Models.Product>();

        public void OnGet()
        {
            ProductRepository productRepository = new ProductRepository();

            Products = productRepository.GetAllProducts();
        }
    }
}