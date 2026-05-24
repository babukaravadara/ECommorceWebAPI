using ECommorceWebAPI.Models;
using ECommorceWebAPI.Models.ECommorceWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommorceWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User>  Users { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<RefreshToken>RefreshTokens{ get; set; }
        public DbSet<Order>Orders{ get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
