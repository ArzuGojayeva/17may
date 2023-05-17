using NEST.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEST.ViewModels.Products
{
    public class ProductImgVm
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public bool IsFront { get; set; }
        public bool IsBack { get; set; }
   

    }
}
