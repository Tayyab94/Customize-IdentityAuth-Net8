using Microsoft.AspNetCore.Mvc;

namespace Identity_Net8.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            throw new Exception();
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
