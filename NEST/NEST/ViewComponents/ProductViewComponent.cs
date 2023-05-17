using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;

namespace NEST.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int take,int skip)
        {
            return View(await _context.products.
                Include(x=>x.Category).
                Include(x=>x.productImages).Skip(skip).Take(take).
                ToListAsync());
        }
    }
}
