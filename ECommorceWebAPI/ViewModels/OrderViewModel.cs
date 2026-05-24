namespace ECommorceWebAPI.ViewModels
{
        public class OrderViewModel
        {
            public int OrderId { get; set; }
            public string ProductName{ get; set; }
            public int Quantity{ get; set; }
            public decimal Price{ get; set; }
            public string Status{ get; set; }
            public string ImageUrl { get; set; }
    }
    
}
