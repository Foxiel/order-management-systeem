using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IList<DataAccessLayer.Models.Customer> Customers { get; set; }
        public IList<DataAccessLayer.Models.Product> Products { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Customers = new List<DataAccessLayer.Models.Customer>();
            Products = new List<DataAccessLayer.Models.Product>();
        }

        public void OnGet()
        {
            CustomerDAL customerDAL = new CustomerDAL();
            ProductDAL productDAL = new ProductDAL();

            Customers = customerDAL.GetAllCustomers();
            Products = productDAL.GetAllProducts();
        }
    }
}