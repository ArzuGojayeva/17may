using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEST.DAL;
using NEST.Models;
using NEST.Utilities.Extension;
using NEST.ViewModels;
using NEST.ViewModels.Products;

namespace NEST.Areas.NestAdmin.Controllers
{
    [Area("NestAdmin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, int take = 5)
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
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.categories = _context.categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(ProductVm product)
        {
            ViewBag.categories = _context.categories.ToList();
            if (!ModelState.IsValid) return View();

            if (_context.products.Any(p => p.Name.Trim().ToLower().Contains(product.Name.ToLower().Trim())))
            {
                ModelState.AddModelError("Name", "Product name already exist");
                return View();
            }
            if (product.DisCountPrice != null)
            {
                if (product.DisCountPrice > product.SellPrice)
                {
                    ModelState.AddModelError("DisCountPrice", "It is wrong");
                    return View();
                }
            }
            else
                product.DisCountPrice = product.SellPrice;
            product.productImages=new List<ProductImage>();
            if(!CheckFile(product.ImageFront,200,out string messagefront))
            {
                ModelState.AddModelError("ImageFront", messagefront);
                return View();
            }
            product.productImages.Add(new ProductImage
            {
                Image = await product.ImageFront.SaveFileAsync(_env.WebRootPath, "shop"),
                IsBack =false,
                IsFront=true,
                product=new Product
                        {
                            Name = product.Name,
                            SellPrice = product.SellPrice,
                            CostPrice = product.CostPrice,
                            Discount = product.Discount,
                            StockCount = product.StockCount,
                            CategoryId = product.CategoryId,
                            ProductImages = product.ProductImages,
                            Tags = product.Tags,
                        }
            }) ;
            if (!CheckFile(product.ImageBack, 200, out string messageback))
            {
                ModelState.AddModelError("ImageBack", messageback);
                return View();
            }
            product.productImages.Add(new ProductImage
            {
                Image = await product.ImageBack.SaveFileAsync(_env.WebRootPath, "shop"),
                IsBack = true,
                IsFront = false,
                product = product
            });
            foreach(var file in product.Files)
            {
                if (!CheckFile(file, 200, out string messagefile))
                {
                    ModelState.AddModelError("Files", messagefile);
                    return View();
                }
                product.productImages.Add(new ProductImage
                {
                    Image = await file.SaveFileAsync(_env.WebRootPath, "shop"),
                    IsBack = false,
                    IsFront = false,
                    product =new Product
                        {
                            Name = product.Name,
                            SellPrice = product.SellPrice,
                            CostPrice = product.CostPrice,
                            Discount = product.Discount,
                            StockCount = product.StockCount,
                            CategoryId = product.CategoryId,
                            ProductImages = product.ProductImages,
                            Tags = product.Tags,
                        }
                });
            }
            product.Tags = new List<Tag>();

            var tags = _context.Tags.Where(x => product.TagIds.Contains(x.Id)).ToList();

            foreach (var tag in tags)
            {
                product.Tags.Add(tag);
            }

            await _context.Products.AddAsync(new Product
            {
                Name = product.Name,
                SellPrice = product.SellPrice,
                CostPrice = product.CostPrice,
                Discount = product.Discount,
                StockCount = product.StockCount,
                CategoryId = product.CategoryId,
                ProductImages = product.ProductImages,
                Tags = product.Tags,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
           

        }
        public bool CheckFile(IFormFile file,int size,out string message)
        {
            message = string.Empty;
            if (!file.CheckFileType("image/"))
            {
                message = "File must be image type";
            }
            if (file.CheckFileSize(size))
            {
                message = $"Size must be less than {size}";
            }
            return true;
        }
        public async Task< IActionResult> Edit(int id)
        {
            ViewBag.categories=_context.categories.ToListAsync();
            return View(await _context.products.Include(x=>x.Category).Include(x=>x.productImages).FirstOrDefaultAsync(x => x.Id == id));
        }
        public async Task<IActionResult> Edit(Product product)
        {
            ViewBag.categories = _context.categories.ToListAsync();
            if (!ModelState.IsValid) return View();
            Product? exist=await _context.products.
                Include(x=>x.Category).
                Include(x=>x.productImages).
                FirstOrDefaultAsync(x=>x.Id==product.Id);
            if (product.DisCountPrice != null)
            {
                if (product.DisCountPrice > product.SellPrice)
                {
                    ModelState.AddModelError("DisCountPrice", "It is wrong");
                    return View();
                }
            }
            else
                product.DisCountPrice = product.SellPrice;
            product.productImages = new List<ProductImage>();
            if(product.ImageBack != null)
            {
                if (!CheckFile(product.ImageBack, 200, out string messageback))
                {
                    ModelState.AddModelError("ImageBack", messageback);
                    return View();
                }
                product.productImages.Add(new ProductImage
                {
                    Image = await product.ImageBack.SaveFileAsync(_env.WebRootPath, "shop"),
                    IsBack = true,
                    IsFront = false,
                    product = product
                });
                product.ImageBack.DeleteFile(_env.WebRootPath, "shop", exist.productImages.FirstOrDefault(x => x.IsBack == true).Image);

            }
            if (product.ImageFront != null)
            {
                if (!CheckFile(product.ImageFront, 2000, out string messageFront))
                {
                    ModelState.AddModelError("ImageFront", messageFront);
                    return View();
                }

                _context.productsImage.Add(new ProductImage
                {
                    Image = await product.ImageFront.SaveFileAsync(_env.WebRootPath, "shop"),
                    IsBack = false,
                    IsFront = true,
                    product=product
                });
                product.ImageBack.DeleteFile(_env.WebRootPath, "shop", exist.productImages.FirstOrDefault(x => x.IsFront == true).Image);
            }
            if (product.Files != null)
            {
                foreach (IFormFile file in product.Files)
                {
                    if (!CheckFile(file, 2000, out string messageFiles))
                    {
                        ModelState.AddModelError("Files", messageFiles);
                        return View();
                    }

                    _context.productsImage.Add(new ProductImage
                    {
                        Image = await file.SaveFileAsync(_env.WebRootPath, "shop"),
                        IsBack = false,
                        IsFront = false,
                        product = product
                    });
                }
            }
            exist.Name = product.Name;
            exist.SellPrice = product.SellPrice;
            exist.CostPrice = product.CostPrice;
            exist.CategoryId = product.CategoryId;
            exist.DisCountPrice= product.DisCountPrice;
            exist.StockCount = product.StockCount;

            _context.products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id, int pId)
        {
            var productImage = await _context.productsImage.FirstOrDefaultAsync(x => x.Id == id);
            productImage.ImageFile.DeleteFile(_env.WebRootPath, "shop", productImage.Image);
            _context.productsImage.Remove(productImage);
            _context.SaveChanges();
            return RedirectToAction("Edit", new { id = pId });
        }
        public async Task<IActionResult> Delete(int id)
        {
            Product? exists = await _context.products.FirstOrDefaultAsync(x => x.Id == id);
            if (exists == null)
            {
                ModelState.AddModelError("", "Product not found");
                return View();
            }
            _context.products.Remove(exists);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }

    }

}
