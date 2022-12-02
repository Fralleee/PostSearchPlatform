using Microsoft.AspNetCore.Mvc;

namespace CareersFralle.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
