using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;

namespace NEST.ViewComponents
{
    public class RandomCategoryViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public RandomCategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.categories.Where(x => x.IsDeleted == false).OrderBy(x => Guid.NewGuid()).ToListAsync());
            
        }
    }
}
