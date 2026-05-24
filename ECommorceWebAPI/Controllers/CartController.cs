using ECommorceWebAPI.Models;
using ECommorceWebAPI.Models.ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;

        public CartController(
            ICartRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]

        public async Task<IActionResult>AddToCart(Cart cart)
        {
            var userId =User.FindFirst( ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }
            cart.UserId = (int)Convert.ToInt64(userId);
            var result =
                await _repository.AddAsync(cart);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult>
        GetCart()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateQuantity(int id, UpdateCartDto cart)
        {
            var result =
                await _repository
                .UpdateQuantityAsync(
                    id,
                    cart.Quantity
                );

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>
        Delete(int id)
        {
            var result =
                await _repository.DeleteAsync(id);
            return Ok(result);
        }

    }
}