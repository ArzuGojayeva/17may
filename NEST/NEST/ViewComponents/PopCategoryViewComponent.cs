using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;

namespace NEST.ViewComponents
{
    public class PopCategoryViewComponent : ViewComponent
    {
      private readonly AppDbContext _context;
        public PopCategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task <IViewComponentResult>InvokeAsync(int take)
        {

            return View(await _context.categories.
                Where(x => x.IsDeleted == false).
                OrderByDescending(p => p.Products.Count).Take(take).ToListAsync());
        }

    }
}
