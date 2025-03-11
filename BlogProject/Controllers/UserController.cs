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
        private readonly UserManager<AppUser> userManager;

        public UserController(IUserService userService, IEmailService emailService, UserManager<AppUser> userManager, ICommentService commentService)
        {
            _userService = userService;
            this.userManager = userManager;
            _commentService = commentService;
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
                ModelState.AddModelErrorList(result.Item2!.ToList());
                return View();
            }

            returnUrl = returnUrl ?? Url.Action("Index", "Home");

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

                return RedirectToAction("resetpassword", "user");
            }

            ModelState.AddModelErrorList(isOk.Item2!.ToList());

            return View();
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

                var extendedProfileInfo = new ExtendedProfileViewModel()
                {
                    Id = currentUser!.Id.ToString(),
                    Name = currentUser.Name,
                    Surname = currentUser.Surname,
                    Bio = currentUser.Bio,
                    BirthDate = currentUser.BirthDate,
                    Country = currentUser.Country,
                    Email = currentUser.Email,
                    PhoneNumber = currentUser.PhoneNumber,
                    Title = currentUser.Title,
                    RegisteredDate = currentUser.RegisteredDate,
                    ProfilePicture = currentUser.ProfilePicture,
                    CoverImagePicture = currentUser.CoverImagePicture,
                    FollowersCount = currentUser.FollowersCount,
                    FollowingCount = currentUser.FollowingCount,
                    TwoFactorEnabled = currentUser.TwoFactorEnabled,
                    WorkingAt = currentUser.WorkingAt,
                    CommentCount = commentCount,
                    PostCount = postCount
                };

                return View(extendedProfileInfo);
            }

            var visitorProfileInfo = new VisitorProfileViewModel()
            {

                Name = visitedUser.Name,
                Surname = visitedUser.Surname,
                Bio = visitedUser.Bio,
                BirthDate = visitedUser.BirthDate,
                Country = visitedUser.Country,
                Title = visitedUser.Title,
                RegisteredDate = visitedUser.RegisteredDate,
                ProfilePicture = visitedUser.ProfilePicture,
                CoverImagePicture = visitedUser.CoverImagePicture,
                FollowersCount = visitedUser.FollowersCount,
                FollowingCount = visitedUser.FollowingCount,
                WorkingAt = visitedUser.WorkingAt,
            };


            return View(visitorProfileInfo);
        }

        // Başka kullanıcının profiline ziyaret etmek için VisitProfile() metodu ekleyeceğim.
    }
}
