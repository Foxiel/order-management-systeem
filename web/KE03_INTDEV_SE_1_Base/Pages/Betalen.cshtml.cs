using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query;
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
            CustomerRepository customerRepository = new CustomerRepository();

            int klantId = 500; // later uit session/login halen

            CustomerProfile? klant = customerRepository.GetCustomerProfileById(klantId);

            if (klant != null)
            {
                Naam = klant.CustomerName;
                Email = klant.CustomerEmail;
                Telefoon = klant.CustomerPhone ?? "";
            }
        }

        public IActionResult OnPost()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<CartItemDto> cartItems =
                JsonSerializer.Deserialize<List<CartItemDto>>(CartJson, options) ?? new();

            if (cartItems.Count == 0)
            {
                return Page();
            }

            OrderRepository orderRepository = new OrderRepository();

            Order order = new Order
            {
                CustomerId = 500,
                OrderDate = DateTime.Now,
                OrderStatus = "Bestelling ontvangen",
                ShippingCosts = 4.99m
            };

            int newOrderId = orderRepository.AddOrderAndReturnId(order);

            foreach (var item in cartItems)
            {


                OrderLine orderLine = new OrderLine
                {
                    OrderId = newOrderId,
                    ProductEAN = item.Id,
                    Quantity = item.Amount,
                    PricePerUnit = item.Price
                };

                orderRepository.AddOrderLineByEan(orderLine);
                
                // empty localstorage
            }

            return RedirectToPage("/NaBetalen");
        }
    }
}