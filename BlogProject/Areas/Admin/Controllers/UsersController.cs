using BlogProject.Areas.Admin.Models;
using BlogProject.Services.Abstract;
using BlogProject.Services.Concrete;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Manager,Takım Lideri,Bölge Sorumlusu")]
    [Area("Admin")]
    [IgnoreAntiforgeryToken]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Manager,Takım Lideri,Bölge Sorumlusu")]
        public async Task<IActionResult> UserList(int page = 1, int pageSize = 4, bool includeDeleted = false)
        {
            var users = await _userService.GetPagedUsersAsync(page, pageSize, includeDeleted);
            users.IncludeDeleted = includeDeleted;
            users.ControllerName = "Users";
            users.ActionName = "UserList";
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> SuspendUser(SuspendUserViewModel request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Failed"] = "Error in suspension model!";
                return RedirectToAction("UserList", "Users");
            }

            await _userService.SuspendUser(request);

            if(request.SuspensionMinutes == 0)
            {
                TempData["Succeed"] = "User suspension removed successfully!";
            }
            else
            {
                TempData["Succeed"] = "User suspended successfully!";
            }

            return RedirectToAction("UserList", "Users");
        }

        [HttpPost("Users/UserDelete/{id}")]
        public async Task<IActionResult> UserDelete(string id)
        {
            var deleterUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(deleterUserId))
            {
                TempData["Failed"] = "Invalid user ID!";
                return RedirectToAction("UserList", "Users");
            }
            var result = await _userService.DeleteUserByTypeAsync(id,DeleteType.Soft, deleterUserId);

            if (!result.IsSuccess)
            {
                TempData["Failed"] = "Invalid user ID!";
                return Json(new { status = false, redirectUrl = Url.Action(nameof(UserList)) });
            }
            TempData["Succeed"] = "User deleted successfully!";
            return Json(new { status = true, redirectUrl = Url.Action(nameof(UserList)) });
        }


        [HttpPost("Users/UserActivate/{id}")]
        [Authorize(Roles = "Manager,Bölge Sorumlusu,Takım Lideri")]
        public async Task<IActionResult> UserActivate(string id)
        {
            var activatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(activatorUserId))
            {
                TempData["Failed"] = "Invalid user ID!";
                return RedirectToAction(nameof(UserList), "Users");
            }
            var result = await _userService.ActivateUserById(id);

            if (!result.IsSuccess)
            {
                TempData["Failed"] = "An error occured while attemping activate user.";
                return Json(new { status = false, redirectUrl = Url.Action(nameof(UserList)) });
            }
            TempData["Succeed"] = "User activated successfully!";
            return Json(new { status = true, redirectUrl = Url.Action(nameof(UserList)) });
        }
    }
}
