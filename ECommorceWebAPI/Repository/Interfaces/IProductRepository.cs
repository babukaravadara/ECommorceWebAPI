using ECommorceWebAPI.Models;

namespace ECommorceWebAPI.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> GetByIdAsync(int id);

        Task<object> AddAsync(Product product);

        Task<object> UpdateAsync(int id, Product product);

        Task<object> DeleteAsync(int id);
    }
}
