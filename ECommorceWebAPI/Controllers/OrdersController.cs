using System.Security.Claims;
using ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommorceWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        public OrdersController(IOrderRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "User not found"
                });
            }

            var result = await _repository.CheckoutAsync(int.Parse(userId), dto);
            return Ok(new
            {
                success = true,
                message = "Order placed successfully",

            });
        }

        [HttpGet]
        public async Task<IActionResult>GetOrders()
        {
            var userId =
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var orders = await _repository.GetOrdersByUserIdAsync(int.Parse(userId));
            return Ok(orders);
        }
    }
}