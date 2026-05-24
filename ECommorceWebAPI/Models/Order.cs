namespace ECommorceWebAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
    public class CheckoutDto
    {
        public string Address{ get; set; }
        public string Phone { get; set; }
    }

}