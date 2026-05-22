using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class WinkelwagenModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public List<Product> AllProducts { get; set; } = new List<Product>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }

        public WinkelwagenModel(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public void OnGet()
        {
            ViewData["Title"] = "Winkelwagen";
            AllProducts = _productRepository.GetAllProducts().ToList();
            LoadCart();
            CalculateTotal();
        }

        public IActionResult OnPostAddProduct(int productId)
        {
            LoadCart();
            
            var cartItem = CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                var product = _productRepository.GetProductById(productId);
                if (product != null)
                {
                    CartItems.Add(new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = 1
                    });
                }
            }

            SaveCart();
            CalculateTotal();
            AllProducts = _productRepository.GetAllProducts().ToList();
            
            return Page();
        }

        public IActionResult OnPostRemoveProduct(int productId)
        {
            LoadCart();
            var cartItem = CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                CartItems.Remove(cartItem);
            }
            SaveCart();
            CalculateTotal();
            AllProducts = _productRepository.GetAllProducts().ToList();
            
            return Page();
        }

        public IActionResult OnPostUpdateQuantity(int productId, int quantity)
        {
            LoadCart();
            var cartItem = CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
            }
            SaveCart();
            CalculateTotal();
            AllProducts = _productRepository.GetAllProducts().ToList();
            
            return Page();
        }

        private void LoadCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cartJson))
            {
                CartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
            }
        }

        private void SaveCart()
        {
            var cartJson = System.Text.Json.JsonSerializer.Serialize(CartItems);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        private void CalculateTotal()
        {
            TotalPrice = CartItems.Sum(item => item.Price * item.Quantity);
        }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
