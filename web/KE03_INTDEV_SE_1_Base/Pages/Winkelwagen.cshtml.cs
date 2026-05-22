using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class WinkelwagenModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<WinkelwagenModel> _logger;

        public IList<Product> Products { get; set; }

        public WinkelwagenModel(IProductRepository productRepository, ILogger<WinkelwagenModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
            Products = new List<Product>();
        }

        public void OnGet()
        {
            Products = _productRepository.GetAllProducts().ToList();
            _logger.LogInformation($"Loaded {Products.Count} products for Winkelwagen");
        }
    }
}
