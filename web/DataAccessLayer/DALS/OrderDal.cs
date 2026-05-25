//Gemaakt door Tobias
using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{

    /// OrderDAL (Data Access Layer) klasse voor Order-gerelateerde databaseoperaties
    /// Bevat CRUD-operaties (Create, Read, Update, Delete) voor bestellingen en orderlijnen
        public class OrderDAL : BaseDAL
    {
        //============================================
        // BESTELLINGEN (Orders) METHODES
        //============================================

      
        /// Haalt ALLE bestellingen uit de database
         public IEnumerable<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT 
                    OrderId,
                    OrderDate,
                    OrderStatusId,
                    PaymentStatusId,
                    ShippingCosts,
                    CustomerId,
                    ShippingAddressId
                FROM [Order]
                ORDER BY OrderDate DESC";

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            {
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        OrderStatusId = Convert.ToInt32(reader["OrderStatusId"]),
                        PaymentStatusId = Convert.ToInt32(reader["PaymentStatusId"]),
                        ShippingCosts = Convert.ToDecimal(reader["ShippingCosts"]),
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        ShippingAddressId = Convert.ToInt32(reader["ShippingAddressId"])
                    });
                }
            }

            return orders;
        }

 
        /// Haalt één specifieke bestelling op via OrderId
                public Order? GetOrderById(int id)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT 
                    OrderId,
                    OrderDate,
                    OrderStatusId,
                    PaymentStatusId,
                    ShippingCosts,
                    CustomerId,
                    ShippingAddressId
                FROM [Order]
                WHERE OrderId = @OrderId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderId", id);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            {
                if (reader.Read())
                {
                    return new Order
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        OrderStatusId = Convert.ToInt32(reader["OrderStatusId"]),
                        PaymentStatusId = Convert.ToInt32(reader["PaymentStatusId"]),
                        ShippingCosts = Convert.ToDecimal(reader["ShippingCosts"]),
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        ShippingAddressId = Convert.ToInt32(reader["ShippingAddressId"])
                    };
                }
            }

            return null; // Bestelling niet gevonden
        }


        /// Voegt een NIEUWE bestelling toe aan de database
                public void AddOrder(Order order)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                INSERT INTO [Order] 
                (OrderDate, OrderStatusId, PaymentStatusId, ShippingCosts, CustomerId, ShippingAddressId)
                VALUES (@OrderDate, @OrderStatusId, @PaymentStatusId, @ShippingCosts, @CustomerId, @ShippingAddressId)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            cmd.Parameters.AddWithValue("@OrderStatusId", order.OrderStatusId);
            cmd.Parameters.AddWithValue("@PaymentStatusId", order.PaymentStatusId);
            cmd.Parameters.AddWithValue("@ShippingCosts", order.ShippingCosts);
            cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
            cmd.Parameters.AddWithValue("@ShippingAddressId", order.ShippingAddressId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        /// Werkt een BESTAANDE bestelling bij in de database
                public void UpdateOrder(Order order)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                UPDATE [Order]
                SET OrderDate = @OrderDate,
                    OrderStatusId = @OrderStatusId,
                    PaymentStatusId = @PaymentStatusId,
                    ShippingCosts = @ShippingCosts,
                    CustomerId = @CustomerId,
                    ShippingAddressId = @ShippingAddressId
                WHERE OrderId = @OrderId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderId", order.OrderId);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            cmd.Parameters.AddWithValue("@OrderStatusId", order.OrderStatusId);
            cmd.Parameters.AddWithValue("@PaymentStatusId", order.PaymentStatusId);
            cmd.Parameters.AddWithValue("@ShippingCosts", order.ShippingCosts);
            cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
            cmd.Parameters.AddWithValue("@ShippingAddressId", order.ShippingAddressId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }


        /// Verwijdert een bestelling uit de database
                public void DeleteOrder(Order order)
        {
            using SqlConnection conn = GetConnection();

            string query = @"DELETE FROM [Order] WHERE OrderId = @OrderId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderId", order.OrderId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        //============================================
        // ORDERLIJNEN (OrderLines) METHODES
        //============================================


        /// Haalt ALLE orderlijnen voor een specifieke bestelling op
        /// Een orderregel bevat: ProductEAN, hoeveelheid, prijs per eenheid
        public IEnumerable<OrderLine> GetOrderLinesByOrderId(int orderId)
        {
            List<OrderLine> orderLines = new List<OrderLine>();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT 
                    OrderId,
                    ProductEAN,
                    Quantity,
                    PricePerUnit
                FROM OrderLine
                WHERE OrderId = @OrderId
                ORDER BY ProductEAN";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            {
                while (reader.Read())
                {
                    orderLines.Add(new OrderLine
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        ProductEAN = reader["ProductEAN"].ToString()!,
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        PricePerUnit = Convert.ToDecimal(reader["PricePerUnit"])
                    });
                }
            }

            return orderLines;
        }

        /// Voegt een NIEUWE orderregel toe aan een bestelling
        public void AddOrderLine(OrderLine orderLine)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                INSERT INTO OrderLine 
                (OrderId, ProductEAN, Quantity, PricePerUnit)
                VALUES (@OrderId, @ProductEAN, @Quantity, @PricePerUnit)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderId", orderLine.OrderId);
            cmd.Parameters.AddWithValue("@ProductEAN", orderLine.ProductEAN);
            cmd.Parameters.AddWithValue("@Quantity", orderLine.Quantity);
            cmd.Parameters.AddWithValue("@PricePerUnit", orderLine.PricePerUnit);

            conn.Open();
            cmd.ExecuteNonQuery();
        }


        /// Verwijdert een orderregel uit een bestelling
        public void DeleteOrderLine(OrderLine orderLine)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                DELETE FROM OrderLine 
                WHERE OrderId = @OrderId AND ProductEAN = @ProductEAN";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderId", orderLine.OrderId);
            cmd.Parameters.AddWithValue("@ProductEAN", orderLine.ProductEAN);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
