using ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace ECommorceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _repository.GetAllAsync();

            return Ok(products);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromForm] ProductDto model)
        {
            
            var fileName =
            Guid.NewGuid().ToString()+ Path.GetExtension(model.Image.FileName);
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            var imageUrl =
          $"{Request.Scheme}://{Request.Host}/images/{fileName}";
            Product product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Quantity = model.Quantity,
                Image = imageUrl
            };
            var result = await _repository.AddAsync(product);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id,[FromForm] ProductDto model)
        {
            var fileName =
           Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            var imageUrl =
          $"{Request.Scheme}://{Request.Host}/images/{fileName}";
            Product product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Quantity = model.Quantity,
                Image = imageUrl

            };
            var result = await _repository.UpdateAsync(id, product);

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

    

