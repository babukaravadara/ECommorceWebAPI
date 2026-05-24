using ECommorceWebAPI.Models.ECommorceWebAPI.Models;
using ECommorceWebAPI.ViewModels;

namespace ECommorceWebAPI.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<object> AddAsync(Cart cart);
        Task<List<CartItemViewModel>> GetAllAsync();
        Task<object> UpdateQuantityAsync(int id,int quantity);
        Task<bool> DeleteAsync(int id);
    }
}
