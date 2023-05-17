using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;
using NEST.Models;
using NEST.ViewModels;
using Newtonsoft.Json;

namespace NEST.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task< IActionResult >Index()
        {
            return View(await _context.products.Include(x=>x.Category).Include(x=>x.productImages).ToListAsync());
        }
        public async Task<IActionResult> LoadMore(int take,int skip)
        {
            return ViewComponent("Product" ,new{skip=skip,take=take });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null) return NotFound();
            Product? product = await _context.products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return BadRequest();
            List<BasketVm> basketvm=GetBasket();
            UpdateBasket(product.Id, basketvm);
            return RedirectToAction("Index", "Product");
        }
        private List<BasketVm> GetBasket()
        {
            List<BasketVm> basketvm;
            if (Request.Cookies["product"] != null)
            {
                basketvm = JsonConvert.DeserializeObject<List<BasketVm>>(Request.Cookies["product"]);
            }
            else
            {
                basketvm = new List<BasketVm>();
            }
            return basketvm;

        }
        private void UpdateBasket(int? id,List<BasketVm>basketvm)
        {
            BasketVm basket = basketvm.FirstOrDefault(x => x.Id == id);
            if (basketvm.Any(x=>x.Id==id))
            {
                basket.Count++;
            }
            else
            {
                basketvm.Add(new BasketVm
                {
                    Id = basket.Id,
                    Count = 1,
                });
            }
            Response.Cookies.Append("Product", JsonConvert.SerializeObject(basketvm));
        }
        public async Task<IActionResult> DeleteBasket(int? id)
        {
            List<BasketVm> basketvm = GetBasket();
            BasketVm basket = basketvm.FirstOrDefault(x => x.Id == id);
            basketvm.Remove(basket);
            Response.Cookies.Append("Product", JsonConvert.SerializeObject(basketvm));
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Cart()
        {
            return View();
        }
       
            public IActionResult Shop(int page = 1, int take = 5)
            {
                var products = _context.products
                    .Where(x => x.IsDeleted == false)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .Include(x => x.Category)
                    .Include(x => x.productImages)
                    .ToList();
                PaginateVm<Product> paginateVM = new PaginateVm<Product>()
                {
                    Items = products,
                    CurrentPage = page,
                    PageCount = GetPageCount(take)
                };
                return View(paginateVM);
            }

            private int GetPageCount(int take)
            {
                var productCount = _context.products.Where(x => x.IsDeleted == false).Count();
                return (int)Math.Ceiling(((decimal)productCount / take));
            }
        }
    }
    

