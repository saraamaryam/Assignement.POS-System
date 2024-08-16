
namespace POS.API.Models.DTO
{
    public class UpdateProductDTO
    {         
        public string Name { get; set; }
       
        public double Price { get; set; }

        public int Quantity { get; set; }
      
        public string Type { get; set; }
        public string Category { get; set; }
    }
}
