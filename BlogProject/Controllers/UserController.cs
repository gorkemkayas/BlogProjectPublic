using BlogProject.Areas.Admin.Models;
using BlogProject.Extensions;
using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection.Metadata;

namespace BlogProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> userManager;

        public UserController(IUserService userService, IEmailService emailService, UserManager<AppUser> userManager, ICommentService commentService)
        {
            _userService = userService;
            this.userManager = userManager;
            _commentService = commentService;
            _emailService = emailService;
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

            if (!result.Item1)
            {
                if(result.Item2!.Any(err => err.Code == "SuspendedAccount"))
                {
                    var description = result.Item2!.Where(error => error.Code == "SuspendedAccount").FirstOrDefault()!.Description;
                    TempData["SuspensionMessage"] = description;
                    ModelState.AddModelError(string.Empty, description );
                    return View();
                }
                ModelState.AddModelErrorList(result.Item2!.ToList());
                return View();
            }

            returnUrl = returnUrl ?? Url.Action("Index", "Home");
            TempData["Succeed"] = "You have logged in successfully.";

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
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.SignUp(request);

            if (result.Item1)
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
            TempData["Succeed"] = "You have logged out successfully.";
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
            (bool isOk, IEnumerable<IdentityError>? errors) = await _userService.ResetPasswordLinkAsync(request);

            if (!isOk)
            {
                ModelState.AddModelErrorList(errors!.ToList());

                return View();
            }

            TempData["Succeed"] = "Password reset link has been sent to your e-mail address.";
            return RedirectToAction("forgetpassword", "user");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string userId)
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

                var user = await userManager.FindByIdAsync(userId!.ToString()!);
                var userEmail = user!.Email;

                await _emailService.SendPasswordChangedNotificationAsync("You changed your password", userEmail!);

                return RedirectToAction("resetpassword", "user");
            }

           
            ModelState.AddModelErrorList(isOk.Item2!.ToList());

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewModel request)
        {
            if(!ModelState.IsValid)
            {
                return View(request);
            }
            
            var result = await _userService.ChangePasswordAsync(request, User);
            if(!result.Item1)
            {
                ModelState.AddModelErrorList(result.Item2!.ToList());
                return View();
            }

            var user = await userManager.GetUserAsync(User);
            var userEmail = user!.Email;

            TempData["Succeed"] = "Password changed successfully.";
            await _emailService.SendPasswordChangedNotificationAsync("You changed your password",userEmail!);

            return RedirectToAction(nameof(Profile), "User", new { userName = User.Identity!.Name});
        }

        public async Task<IActionResult> Profile(string? userName)
        {
            if(userName == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var visitedUser = await userManager.FindByNameAsync(userName);
            
            if(visitedUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var currentUser = await userManager.GetUserAsync(User);

            var postCount = await _commentService.GetCommentCountByUserAsync(visitedUser!);
            var commentCount = await _userService.GetCommentCountByUserAsync(visitedUser!);


            if (currentUser == visitedUser)
            {
                ViewBag.IsOwner = true;

                var extendedProfileInfo = await _userService.GetExtendedProfileInformationAsync(currentUser);

                return View(extendedProfileInfo);
            }

            var visitorProfileInfo = _userService.GetVisitorProfileInformation(visitedUser);


            return View(visitorProfileInfo);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ExtendedProfileViewModel request, IFormFile? fileInputProfile, IFormFile? coverInputProfile, IFormFile? IconInputWorkingAt)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userService.UpdateProfileAsync((await userManager.GetUserAsync(User!))!, request, fileInputProfile, coverInputProfile, IconInputWorkingAt);

            if (!result.Item1)
            {
                ModelState.AddModelErrorList(result.Item2!.ToList());

                TempData["Failed"] = "An error occurred while updating the profile.";
                return RedirectToAction(nameof(Profile), new { userName = User.Identity!.Name });
            }

            if (result.Item3)
            {   
                await _userService.LogoutAsync();
                await _userService.LogInAsync((await userManager.FindByIdAsync(request.Id))!);

                TempData["Succeed"] = "Profile updated successfully.";

                return RedirectToAction(nameof(Profile), new { userName = User.Identity!.Name });
            }

            TempData["Succeed"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Profile), new { userName = User.Identity!.Name });
        }

    }
}
