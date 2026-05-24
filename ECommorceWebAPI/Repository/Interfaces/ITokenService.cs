using ECommorceWebAPI.Models;

namespace ECommorceWebAPI.Repository.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);

        string GenerateRefreshToken();
    }
}
