using ECommorceWebAPI.Data;
using ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommorceWebAPI.Repository.Services
{
        public class ProductRepository : IProductRepository
        {
            private readonly ApplicationDbContext _context;

            public ProductRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Product>> GetAllAsync()
            {
                return await _context.Products.ToListAsync();
            }

            public async Task<Product> GetByIdAsync(int id)
            {
                return await _context.Products.FindAsync(id);
            }

            public async Task<object> AddAsync(Product product)
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return new
                {
                    success = true,
                    message = "Product Added Successfully"
                };
        }

        public async Task<object> UpdateAsync(int id, Product product)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
            {
                return new
                {
                    success = false,
                    message = "Product not found"
                };
            }
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Quantity= product.Quantity;
                existing.Description = product.Description;
               await _context.SaveChangesAsync();
            return new
            {
                success = true,
                message = "Product Updated Successfully"
            };
        }
            
            

            public async Task<object> DeleteAsync(int id)
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                    return false;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

            return new
            {
                success = true,
                message = "Product Deleted Successfully"
            };
        }
        }
    }


