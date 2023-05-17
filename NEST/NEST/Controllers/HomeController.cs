using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;
using NEST.ViewModels;

namespace NEST.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task< IActionResult> Index()
        {
            HomeVm vm = new HomeVm()
            {
                sliders = await _context.sliders.ToListAsync(),
         
            };
            return View(vm);
        }
    }
}
