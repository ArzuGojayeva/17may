using NEST.Models;

namespace NEST.ViewModels
{
    public class BasketItems
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Count { get; set; }
        public decimal SellPrice { get; set; }
        public double? Rating { get; set; }
        public string Image { get; set; }
    }
}
