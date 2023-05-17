using System.ComponentModel.DataAnnotations.Schema;

namespace NEST.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string ? Image { get;set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public ICollection<Product> ? Products { get; set; }
        public bool IsDeleted { get; set; }
    }
}
