using Microsoft.AspNetCore.Mvc;

namespace PostSearchPlatform.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
