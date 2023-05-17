using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;
using NEST.Models;
using NEST.Utilities.Extension;
using NEST.ViewModels;
using NEST.ViewModels.Categories;

namespace NEST.Controllers
{
    [Area("NestAdmin")]
    [Authorize(Roles ="Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public CategoriesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        [HttpGet]
        public IActionResult Index(int page = 1, int take = 5)
        {
            var categories = _context.categories
                .Where(x => x.IsDeleted == false)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            PaginateVm<Category> paginateVM = new PaginateVm<Category>()
            {
                Items = categories,
                CurrentPage = page,
                PageCount = GetPageCount(take)
            };
            return View(paginateVM);
        }

        private int GetPageCount(int take)
        {
            var categoryCount = _context.categories.Where(x => x.IsDeleted == false).Count();
            return (int)Math.Ceiling(((decimal)categoryCount / take));
        }
        public async Task< IActionResult> Details(int id)
        {
            return View(await _context.categories.FirstOrDefaultAsync(x => x.Id == id));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
       public async Task <IActionResult> Create(Category category) {
            if (!ModelState.IsValid) return View();
            if (!category.ImageFile.CheckFileType("image"))
            {
                ModelState.AddModelError("PhotoFile", "File must be image format");
                return View();
            }
            if (category.ImageFile.CheckFileSize(200))
            {
                ModelState.AddModelError("PhotoFile", "File must be less than 200kb");
                return View();
            }
            category.Image = await category.ImageFile.SaveFileAsync(_environment.WebRootPath, "shop");
            await _context.categories.AddAsync(category);
            await _context.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }
       public async Task < IActionResult> Edit(int id)
        {
            return View( await _context.categories.FirstOrDefaultAsync(x=>x.Id==id));
        }

        [HttpPost]
        public async Task <IActionResult> Edit(CategoryEditVm category)
        {
           Category? exists= await _context.categories.FirstOrDefaultAsync(x=>x.Id==category.Id);
        
            if (exists == null)
            {
                ModelState.AddModelError("", "Category not found");
                    return View(category);
            }
            if (category.ImageFile != null)
            {
                if (!category.ImageFile.CheckFileType("image"))
                {
                    ModelState.AddModelError("PhotoFile", "File must be image format");
                    return View();
                }
                if (category.ImageFile.CheckFileSize(200))
                {
                    ModelState.AddModelError("PhotoFile", "File must be less than 200kb");
                    return View();
                }
                string path = Path.Combine(_environment.WebRootPath, "assets", "imgs", "shop",exists.ImageFile.FileName);
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                exists.Image = await category.ImageFile.SaveFileAsync(_environment.WebRootPath, "shop");
            }
            exists.Name = category.Name;

            exists.Logo = category.Logo;
            exists.Image=category.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            Category? exists = await _context.categories.FirstOrDefaultAsync(x => x.Id == id);
            if (exists == null) return View("Error"); 
            string path = Path.Combine(_environment.WebRootPath, "assets", "imgs", "shop", exists.ImageFile.FileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            exists.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        
    }
}
