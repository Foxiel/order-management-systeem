using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class ProductenModel : PageModel
    {
        public IList<Product> Products { get; set; } = new List<Product>();

        public void OnGet()
        {
            ProductDAL productDAL = new ProductDAL();

            Products = productDAL.GetAllProducts();
        }
    }
}