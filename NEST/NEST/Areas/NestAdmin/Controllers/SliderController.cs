using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;
using NEST.Models;
using NEST.Utilities.Extension;
using NEST.ViewModels;

namespace NEST.Areas.NestAdmin.Controllers
{
    [Area("NestAdmin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public SliderController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public IActionResult Index(int page = 1, int take = 5)
        {
            var sliders = _context.sliders
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            PaginateVm<Slider> paginateVM = new PaginateVm<Slider>()
            {
                Items = sliders,
                CurrentPage = page,
                PageCount = GetPageCount(take)
            };
            return View(paginateVM);
        }

        private int GetPageCount(int take)
        {
            var sliderCount = _context.sliders.Count();
            return (int)Math.Ceiling(((decimal)sliderCount / take));
        }
        public async Task<IActionResult> Details(int id)
        {
            return View(await _context.categories.FirstOrDefaultAsync(x => x.Id == id));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);
            if (!slider.ImageFile.CheckFileType("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be Image type");
                return View();
            }
            if (slider.ImageFile.CheckFileSize(200))
            {
                ModelState.AddModelError("ImageFile", "Invalid Input");
                return View();
            }

            slider.Image = await slider.ImageFile.SaveFileAsync(_environment.WebRootPath, "slider");
            await _context.sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _context.sliders.FirstOrDefaultAsync(x => x.Id == id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Slider slider)
        {
            Slider? exists = await _context.sliders.FirstOrDefaultAsync(x => x.Id == slider.Id);

            if (exists == null)
            {
                ModelState.AddModelError("", "Slider not found");
                return View(slider);
            }
            if (!exists.ImageFile.CheckFileType("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be Image type");
                return View();
            }
            if (exists.ImageFile.CheckFileSize(200))
            {
                ModelState.AddModelError("ImageFile", "Invalid Input");
                return View();
            }
            exists.Image = await slider.ImageFile.SaveFileAsync(_environment.WebRootPath, "slider");

            exists.Title1= slider.Title1;
            exists.Title2 = slider.Title2;
            exists.Image= slider.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            Slider? exists = await _context.sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (exists == null)
            {
                ModelState.AddModelError("", "Slider not found");
                return View();
            }
            _context.sliders.Remove(exists);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }

    }
}

