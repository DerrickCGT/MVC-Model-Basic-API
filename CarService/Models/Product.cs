using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CarService.Models
{
    public class Product
    {
        public int Id { get; set; }

        //public string Sku { get; set; }
    
        //public string UnitName { get; set; } 
        [Required]

        public string Name { get; set; }
        [Required]
        public string Description { get; set; } 
        
        public Decimal Price { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category? Category { get; set; }

    }
}
