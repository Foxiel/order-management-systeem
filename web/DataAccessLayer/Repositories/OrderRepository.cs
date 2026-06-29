using DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer.DAL
{
    public class OrderRepository : dbContext
    {
        public IEnumerable<Order> GetAllOrders()
        {
            List<Order> orders = new();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    bestelling_id,
                    klant_id,
                    order_datum,
                    order_status 
                FROM Bestelling
                ORDER BY order_datum DESC";

            SqlCommand cmd = new(query, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    OrderId = Convert.ToInt32(reader["bestelling_id"]),
                    CustomerId = Convert.ToInt32(reader["klant_id"]),
                    OrderDate = Convert.ToDateTime(reader["order_datum"]),
                    OrderStatus = reader["order_status"].ToString()!
                });
            }

            return orders;
        }

        public Order? GetOrderById(int id)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    bestelling_id,
                    klant_id,
                    order_datum,
                    order_status
                FROM Bestelling
                WHERE bestelling_id = @BestellingId";

            SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@BestellingId", id);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Order
                {
                    OrderId = Convert.ToInt32(reader["bestelling_id"]),
                    CustomerId = Convert.ToInt32(reader["klant_id"]),
                    OrderDate = Convert.ToDateTime(reader["order_datum"]),
                    OrderStatus = reader["order_status"].ToString()!
                };
            }

            return null;
        }

        public void AddOrder(Order order)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                INSERT INTO Bestelling
                (
                    klant_id,
                    order_datum,
                    order_status
                )
                VALUES
                (
                    @KlantId,
                    @OrderDatum,
                    @OrderStatus
                )";

            SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@KlantId", order.CustomerId);
            cmd.Parameters.AddWithValue("@OrderDatum", order.OrderDate);
            cmd.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateOrder(Order order)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                UPDATE Bestelling
                SET
                    klant_id = @KlantId,
                    order_datum = @OrderDatum,
                    order_status = @OrderStatus
                WHERE bestelling_id = @BestellingId";

            SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@BestellingId", order.OrderId);
            cmd.Parameters.AddWithValue("@KlantId", order.CustomerId);
            cmd.Parameters.AddWithValue("@OrderDatum", order.OrderDate);
            cmd.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteOrder(Order order)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                DELETE FROM Bestelling
                WHERE bestelling_id = @BestellingId";

            SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@BestellingId", order.OrderId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<OrderLine> GetOrderLinesByOrderId(int orderId)
        {
            List<OrderLine> orderLines = new();

            using SqlConnection conn = GetConnection();

            string query = @"
                SELECT
                    br.bestelling_id,
                    br.product_id,
                    br.aantal,
                    p.ean,
                    p.naam,
                    p.prijs
                FROM Bestelregel br
                INNER JOIN Product p
                    ON br.product_id = p.product_id
                WHERE br.bestelling_id = @BestellingId
                ORDER BY p.naam";

            SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@BestellingId", orderId);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orderLines.Add(new OrderLine
                {
                    OrderId = Convert.ToInt32(reader["bestelling_id"]),
                    ProductId = Convert.ToInt32(reader["product_id"]),
                    ProductEAN = reader["ean"].ToString()!,
                    ProductNaam = reader["naam"].ToString()!,
                    Quantity = Convert.ToInt32(reader["aantal"]),
                    PricePerUnit = Convert.ToDecimal(reader["prijs"])
                });
            }

            return orderLines;
        }

        public void AddOrderLine(OrderLine orderLine)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                INSERT INTO Bestelregel
                (
                    bestelling_id,
                    product_id,
                    aantal
                )
                VALUES
                (
                    @BestellingId,
                    @ProductId,
                    @Aantal
                )";

            SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@BestellingId", orderLine.OrderId);
            cmd.Parameters.AddWithValue("@ProductId", orderLine.ProductId);
            cmd.Parameters.AddWithValue("@Aantal", orderLine.Quantity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteOrderLine(OrderLine orderLine)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
                DELETE FROM Bestelregel
                WHERE bestelling_id = @BestellingId
                AND product_id = @ProductId";

            SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@BestellingId", orderLine.OrderId);
            cmd.Parameters.AddWithValue("@ProductId", orderLine.ProductId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public int AddOrderAndReturnId(Order order)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
        INSERT INTO Bestelling
        (
            klant_id,
            order_datum,
            order_status
        )
        VALUES
        (
            @KlantId,
            @OrderDatum,
            @order_status
        );

        SELECT CAST(SCOPE_IDENTITY() AS int);";

            SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@KlantId", order.CustomerId);
            cmd.Parameters.AddWithValue("@OrderDatum", order.OrderDate);
            cmd.Parameters.AddWithValue("@order_status", order.OrderStatus);

            conn.Open();

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void AddOrderLineByEan(OrderLine orderLine)
        {
            using SqlConnection conn = GetConnection();

            string query = @"
        INSERT INTO Bestelregel
        (
            bestelling_id,
            product_id,
            aantal
        )
        SELECT
            @BestellingId,
            product_id,
            @Aantal
        FROM Product
        WHERE ean = @Ean";

            SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@BestellingId", orderLine.OrderId);
            cmd.Parameters.AddWithValue("@Ean", orderLine.ProductEAN);
            cmd.Parameters.AddWithValue("@Aantal", orderLine.Quantity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

}