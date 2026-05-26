using System.ComponentModel.DataAnnotations;

namespace ECommorceWebAPI.Models
{
    public class User
    {
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; } = string.Empty;
       
    }
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class TokenApiDto
    {
        public string RefreshToken
        { get; set; }
    }
}
