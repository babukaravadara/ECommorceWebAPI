using ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUser _repository;
        private readonly ITokenService _tokenService;
        public AuthController(IUser repository, ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _repository.LoginAsync(dto.Email, dto.Password
                );

            if (user == null)
            {
                return Unauthorized();
            }

            var accessToken =
                _tokenService
                .CreateToken(user);

            var refreshToken =
                _tokenService
                .GenerateRefreshToken();

            await _repository
                .SaveRefreshTokenAsync(
                    user.Id,
                    refreshToken
                );

            Response.Cookies.Append(
                "accessToken",
                accessToken,

                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite =
                        SameSiteMode.None,
                    Expires =
                        DateTimeOffset.Now
                        .AddMinutes(60)
                });

            Response.Cookies.Append(
                "refreshToken",
                refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite =
                        SameSiteMode.None,
                    Expires =
                        DateTimeOffset.Now
                        .AddDays(7)
                });
            Response.Cookies.Append(
             "role",
             user.Role,

             new CookieOptions
             {
                 HttpOnly = true,
                 Secure = true,
                 SameSite =
                     SameSiteMode.None,
                 Expires =
                     DateTimeOffset.Now
                     .AddMinutes(60)
             });

            return Ok(new
            {
                message =
                    "Login Successful",
                    role=user.Role
            });
        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                email = User.FindFirst(ClaimTypes.Email)?.Value,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }
       
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(
                refreshToken))
            {
                return Unauthorized();
            }
            var storedToken =
                await _repository.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null)
            {
                return Unauthorized();
            }
            if (storedToken.ExpiryDate < DateTime.Now)
            {
                return Unauthorized();
            }

            var user = storedToken.User;

            var newAccessToken =
                _tokenService
                .CreateToken(user);

            var newRefreshToken =
                _tokenService
                .GenerateRefreshToken();

            await _repository
                .SaveRefreshTokenAsync(
                    user.Id,
                    newRefreshToken
                );

            // NEW ACCESS COOKIE

            Response.Cookies.Append(
                "accessToken",
                newAccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.Now.AddMinutes(60)
                });

            Response.Cookies.Append(
                "refreshToken",
                newRefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.Now.AddDays(7)
                });

            return Ok();
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(
                "accessToken"
            );

            Response.Cookies.Delete(
                "refreshToken"
            );

            return Ok();
        }
    }
}