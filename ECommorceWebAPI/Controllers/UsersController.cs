using ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ECommorceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUser _repository;
        public UsersController(IUser repository)
        {
            _repository = repository;
            
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _repository.GetAllAsync();

            return Ok(users);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var result = await _repository.AddAsync(user);

            return Ok(result);
        }
        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            var result = await _repository.UpdateAsync(user);

            return Ok(result);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return Ok(result);
        }

    
    }
}
