using Microsoft.AspNetCore.Mvc;

namespace NEST.Areas.NestAdmin.Controllers
{
    [Area("NestAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
