using System.ComponentModel.DataAnnotations.Schema;

namespace NEST.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int StockCount { get; set; }
        public decimal SellPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DisCountPrice { get; set; }
        public double Rating { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<ProductImage>?productImages { get; set; }
        [NotMapped]
        public IFormFile ImageFront { get; set; } = null!;
        [NotMapped]
        public IFormFile ImageBack { get;set; }=null!;
        [NotMapped]
        public List< IFormFile>? Files { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Tag> Tags { get; set; }
       
    }
}
