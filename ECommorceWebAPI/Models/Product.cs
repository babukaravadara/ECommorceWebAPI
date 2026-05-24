using System.ComponentModel.DataAnnotations;

namespace ECommorceWebAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }


    }
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }

        public int Quantity { get; set; }
    }
}
