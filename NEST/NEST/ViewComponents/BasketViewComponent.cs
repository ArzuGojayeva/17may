using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;
using NEST.ViewModels;
using Newtonsoft.Json;

namespace NEST.ViewComponents
{
    public class BasketViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public BasketViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketVm>? basketvm = GetBasket();

            List<BasketItems> basketItems = new List<BasketItems>();
            foreach (var item in basketvm)
            {
                var products =await _context.products.Include(x=>x.productImages).FirstOrDefaultAsync(x=>x.Id==item.Id);
                basketItems.Add(new BasketItems
                {
                    Id = products.Id,
                    Count = item.Count,
                    Name =products.Name,
                    Image = products.productImages.FirstOrDefault(x => x.IsFront == true).Image,
                    SellPrice = products.SellPrice


                });
            }

            return View(basketItems);
        }

        private List<BasketVm> GetBasket()
        {
            List<BasketVm> basketvm = new List<BasketVm>();
            if (Request.Cookies["Product"] != null)
            {
                basketvm = JsonConvert.DeserializeObject<List<BasketVm>>(Request.Cookies["Product"]);
            }
            return basketvm;
        }
    }
    
}
