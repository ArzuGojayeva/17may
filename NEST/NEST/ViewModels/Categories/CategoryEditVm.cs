using NEST.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEST.ViewModels.Categories
{
    public class CategoryEditVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


    }
}
