using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;
using NEST.Models;

namespace NEST.Areas.NestAdmin.Controllers
{
    [Area("NestAdmin")]
    public class TagsController : Controller
    {
        
        private readonly AppDbContext _context;
        public TagController(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tags.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid) return View();
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

