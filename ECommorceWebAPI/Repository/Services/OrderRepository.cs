using ECommorceWebAPI.Data;
using ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using ECommorceWebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace ECommorceWebAPI.Repository.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly
        ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> CheckoutAsync(int userId, CheckoutDto dto)
        {
            var cartItems = await _context.Cart.Where(x => x.UserId == userId).ToListAsync();
            decimal? total = cartItems.Sum(x => x.Price * x.Quantity);
            var order = new Order
            {
                UserId = userId,
                Address = dto.Address,
                Phone = dto.Phone,
                TotalAmount = (decimal)total,
                OrderDate = DateTime.UtcNow,
                Status= "Processing",
                OrderItems =
                    cartItems.Select(x =>
                        new OrderItem
                        {
                            ProductId = x.ProductId,
                            Quantity = (int)x.Quantity,
                            Price = x.Price
                        }).AsQueryable()
                    .ToList()
            };

            await _context.Orders.AddAsync(order);
            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return order.Id;
        }
        public async Task<List<OrderViewModel>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Product)
                .Where(x => x.Order.UserId == userId)
                .Select(x =>
                    new OrderViewModel
                    {
                        OrderId = x.OrderId,
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        Status = x.Order.Status,
                        ImageUrl=x.Product.Image

                    }

                )

                .ToListAsync();
        }
    }
}