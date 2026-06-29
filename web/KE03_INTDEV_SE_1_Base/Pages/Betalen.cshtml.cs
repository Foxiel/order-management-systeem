using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class BetalenModel : PageModel
    {
        [BindProperty]
        public string Naam { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Telefoon { get; set; } = string.Empty;

        [BindProperty]
        public string CartJson { get; set; } = string.Empty;

        public void OnGet()
        {
            CustomerDAL customerDAL = new CustomerDAL();

            int klantId = 500; // later uit session/login halen

            CustomerProfile? klant = customerDAL.GetCustomerProfileById(klantId);

            if (klant != null)
            {
                Naam = klant.CustomerName;
                Email = klant.CustomerEmail;
                Telefoon = klant.CustomerPhone ?? "";
            }
        }

        public IActionResult OnPost()
        {
            List<CartItemDto> cartItems = JsonSerializer.Deserialize<List<CartItemDto>>(CartJson) ?? new();

            if (cartItems.Count == 0)
            {
                return Page();
            }

            OrderDAL orderDAL = new OrderDAL();

            Order order = new Order
            {
                CustomerId = 500,
                OrderDate = DateTime.Now,
                OrderStatusId = 0,
                ShippingCosts = 4.99m
            };

            int newOrderId = orderDAL.AddOrderAndReturnId(order);

            foreach (var item in cartItems)
            {
                orderDAL.AddOrderLineByEan(new OrderLine
                {
                    OrderId = newOrderId,
                    ProductEAN = item.Id,
                    Quantity = item.Amount,
                    PricePerUnit = item.Price
                });
            }

            return RedirectToPage("/NaBetalen");
        }
    }
}