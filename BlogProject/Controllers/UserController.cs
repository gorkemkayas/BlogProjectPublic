using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
