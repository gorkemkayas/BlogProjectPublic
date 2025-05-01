using BlogProject.Areas.Admin.Models;
using BlogProject.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> UserList(int page = 1, int pageSize = 4, bool includeDeleted = false)
        {
            var users = await _userService.GetPagedUsersAsync(page, pageSize, includeDeleted);
            users.IncludeDeleted = includeDeleted;
            users.ControllerName = "Users";
            users.ActionName = "UserList";
            return View(users);
        }

        [HttpPost]
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
    }
}
