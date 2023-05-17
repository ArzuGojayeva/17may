using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;

namespace NEST.ViewComponents
{
    public class TopRatedViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public TopRatedViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.products.Include(p => p.productImages).Include(x => x.Category).Where(x => x.IsDeleted == false).OrderByDescending(p => p.Rating).Take(10).ToListAsync());
        }
    }
}
