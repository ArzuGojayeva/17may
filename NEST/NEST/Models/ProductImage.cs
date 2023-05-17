using System.ComponentModel.DataAnnotations.Schema;

namespace NEST.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public bool IsFront { get;set; }
        public bool IsBack { get;set; }
        public  int ProductId{ get; set; }
        public Product product { get; set; }

    }
}
