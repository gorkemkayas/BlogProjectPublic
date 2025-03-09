using BlogProject.Extensions;
using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace BlogProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> userManager;

        public UserController(IUserService userService, IEmailService emailService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request, string? returnUrl = null)
        {
            var result = await _userService.SignInAsync(request);
            
            if(!result.Item1)
            {
                ModelState.AddModelErrorList(result.Item2!.ToList());
                return View();
            }

            returnUrl = returnUrl ?? Url.Action("Index","Home");

            return Redirect(returnUrl!);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if(!ModelState.IsValid)
            {
                return View(request);
            }

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

        public async Task Logout()
        {
            await _userService.LogoutAsync();
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            (bool isOk,IEnumerable<IdentityError>? errors) = await _userService.ResetPasswordLinkAsync(request);

            if(!isOk)
            {
                ModelState.AddModelErrorList(errors!.ToList());

                return View();
            }

            TempData["Succeed"] = "Password reset link has been sent to your e-mail address.";
            return RedirectToAction("forgetpassword", "user");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string userId)
        {
            TempData["Token"] = token;
            TempData["userId"] = userId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["Token"];

            var isOk = await _userService.ResetPasswordAsync(request, userId?.ToString(), token?.ToString());

            if (isOk.Item1)
            {
                TempData["Succeed"] = "Password reset successfully.";

                return RedirectToAction("resetpassword", "user");
            }
            
            ModelState.AddModelErrorList(isOk.Item2!.ToList());

            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
