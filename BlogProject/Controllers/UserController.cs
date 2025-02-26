using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            var result = await _userService.SignUp(request);
            if(result.Item1)
            {
                TempData["Succeed"] = "User created successfully. You are being redirected to the login page...";

            }
            else if (!result.Item1)
            {
                TempData["Failed"] = "User could not be created.";

                foreach (var error in result.Item2!)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
