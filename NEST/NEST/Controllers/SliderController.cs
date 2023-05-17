using Microsoft.AspNetCore.Mvc;

namespace NEST.Controllers
{
    public class SliderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
