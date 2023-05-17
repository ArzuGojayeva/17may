using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;

namespace NEST.ViewComponents
{
    public class RecentProductViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public RecentProductViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.products.Include(p => p.productImages).Include(x => x.Category).Where(x => x.IsDeleted == false).OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
