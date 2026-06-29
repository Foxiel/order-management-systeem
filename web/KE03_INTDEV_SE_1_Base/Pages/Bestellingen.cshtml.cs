//Gemaakt door Tobias
using DataAccessLayer.DAL;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{

    /// PageModel voor de Bestellingen pagina
    /// Haalt bestellingen en bijbehorende orderlijnen uit de database
    public class BestellingenModel : PageModel
    {
        // ===========================================
        // PROPERTIES - Gegevens voor de View
        // ===========================================


        /// Lijst met ALLE bestellingen van de gebruiker
        /// Wordt ingevuld in de OnGet() methode
        public IList<Order> Orders { get; set; } = new List<Order>();


        /// Dictionary die de relatie legt tussen:
        /// KEY: OrderId (uniek bestellingsnummer)
        /// VALUE: Lijst van OrderLine objecten (de producten in die bestelling)
        /// 
        /// Voorbeeld:
        /// OrderLines[1] = List van producten in bestelling #1
        /// OrderLines[2] = List van producten in bestelling #2

        public Dictionary<int, List<OrderLine>> OrderLines { get; set; } = new();

        // ===========================================
        // METHODES
        // ===========================================


        /// OnGet() wordt aangeroepen wanneer de pagina wordt geladen
        /// Deze methode haalt alle bestellingen en orderlijnen uit de database
        public void OnGet()
        {
            // Maak een OrderDAL object aan voor databaseoperaties
            OrderRepository orderRepository = new OrderRepository();

            // Haal ALLE bestellingen op uit de database
            // GetAllOrders() geeft een IEnumerable terug, dus we zetten het om naar een List
            Orders = orderRepository.GetAllOrders().ToList();

            // Voor ELKE bestelling, haal alle bijbehorende orderlijnen op
            // Bijvoorbeeld: als bestelling 1 3 producten bevat, haal die 3 producten op
            foreach (var order in Orders)
            {
                // GetOrderLinesByOrderId() haalt alle producten voor deze bestelling op
                // We slaan het resultaat op in de Dictionary met de OrderId als key
                OrderLines[order.OrderId] = orderRepository.GetOrderLinesByOrderId(order.OrderId).ToList();
            }
        }
    }
}
