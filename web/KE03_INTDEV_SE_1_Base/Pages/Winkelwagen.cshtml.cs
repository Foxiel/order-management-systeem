//gemaakt door Jesse
using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class WinkelwagenModel : PageModel
    {
        private readonly ILogger<WinkelwagenModel> _logger;

        public IList<DataAccessLayer.Models.Product> Products { get; set; }

        public WinkelwagenModel(ILogger<WinkelwagenModel> logger)
        {
            _logger = logger;
            Products = new List<DataAccessLayer.Models.Product>();
        }

        public void OnGet()
        {
            ProductDAL productDAL = new ProductDAL();

            Products = productDAL.GetAllProducts();

            _logger.LogInformation($"Loaded {Products.Count} products for Winkelwagen");
        }
    }
}