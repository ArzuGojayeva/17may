using Microsoft.AspNetCore.Mvc;

namespace NEST.Areas.NestAdmin.Controllers
{
    public class TagsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
