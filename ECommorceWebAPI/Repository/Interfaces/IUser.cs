using ECommorceWebAPI.Models;

namespace ECommorceWebAPI.Repository.Interfaces
{
    public interface IUser
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(int id);

        Task<object> AddAsync(User user);

        Task<User> UpdateAsync(User user);

        Task<object> DeleteAsync(int id);
        Task<User?> LoginAsync(string email, string password
        );
        Task SaveRefreshTokenAsync( int userId, string refreshToken  );
        Task<RefreshToken?>  GetRefreshTokenAsync( string refreshToken);
    }

}
