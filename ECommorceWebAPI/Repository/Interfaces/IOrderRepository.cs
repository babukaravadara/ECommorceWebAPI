using ECommorceWebAPI.Models;
using ECommorceWebAPI.ViewModels;

namespace ECommorceWebAPI.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<int>CheckoutAsync( int userId,CheckoutDto dto);
        Task<List<OrderViewModel>>GetOrdersByUserIdAsync(int userId
);
    }
}
