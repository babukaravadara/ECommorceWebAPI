using ECommorceWebAPI.Data;
using ECommorceWebAPI.Models;
using ECommorceWebAPI.Models.ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using ECommorceWebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ECommorceWebAPI.Repository.Services
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> AddAsync(Cart cart)
        {
            var existingCart = await _context.Cart
                .FirstOrDefaultAsync(x => x.ProductId == cart.ProductId && x.UserId == cart.UserId);

            // start with the requested quantity (Cart.Quantity is nullable)
            int totalQuantity = cart.Quantity ?? 0;

            if (existingCart != null)
            {
                totalQuantity += existingCart.Quantity ?? 0;
            }
            // load product for stock check
            var product = await _context.Products.FindAsync(cart.ProductId);
            if (product == null)
            {
                return new
                {
                    success = false,
                    message = "Product not found"
                };
            }
            // STOCK CHECK
            if (totalQuantity > product.Quantity)
            {
                return new
                {
                    success = false,
                    message = "Product quantity not available"
                };
            }

            // UPDATE CART
            if (existingCart != null)
            {
                existingCart.Quantity = (existingCart.Quantity ?? 0) + (cart.Quantity ?? 0);
                _context.Cart.Update(existingCart);
            }
            else
            {
                await _context.Cart.AddAsync(cart);
            }

            await _context.SaveChangesAsync();

            return new
            {
                success = true,
                message = "Items Added To Cart, Go to Cart"
            };
        }

        public async Task<List<CartItemViewModel>> GetAllAsync()
        {
            var cartData = await (from cart in _context.Cart
                                  join product in _context.Products
                                  on cart.ProductId equals product.Id
                                  select new CartItemViewModel
                                  {
                                      Id = cart.Id,
                                      ProductId = product.Id,
                                      Quantity = cart.Quantity,
                                      AvailableQuantity = product.Quantity,
                                      ImageUrl = cart.ImageUrl,
                                      UserId= cart.UserId,
                                      Price = cart.Price,
                                      ProductName=cart.ProductName
                                  }).ToListAsync();
            return cartData;
        }
        public async Task<object> UpdateQuantityAsync( int id,int quantity)
        {
            var item =await _context.Cart.FindAsync(id);
            if (item == null)
            {
                return null;
            }
            item.Quantity = quantity;
            Product product = await _context.Products.FindAsync(item.ProductId);
            // STOCK CHECK'
            if (quantity > product.Quantity)
            {
                return new
                {
                    success = false,
                    message = "Product quantity not available"
                };
            }
            await _context.SaveChangesAsync();
            return new
            {
                success = true,
                message = "Sucess"
            };
        }

        public async Task<bool>
        DeleteAsync(int id)
        {
            var item = await _context.Cart.FindAsync(id);
            if (item == null)
            {
                return false;
            }
            _context.Cart.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}