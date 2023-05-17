using NEST.Models;

namespace NEST.ViewModels
{
    public class HomeVm
    {
        public List<Slider>sliders { get; set; }
        public List<Category>Popularcategories { get; set; }
        public List<Product> products { get; set; }
        public List<Category>Randomcategories { get; set; }
        public List<Product>TopRatedProducts { get; set; }
        public List<Product>RecentProducts { get; set; }
    }
}
