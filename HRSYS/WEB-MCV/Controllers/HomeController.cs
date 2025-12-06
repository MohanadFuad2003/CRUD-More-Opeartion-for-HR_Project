using Microsoft.AspNetCore.Mvc;

namespace WEB_MCV.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

