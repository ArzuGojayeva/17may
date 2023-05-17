using Microsoft.AspNetCore.Mvc;

namespace NEST.Controllers
{
    public class ProductImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
