using AutoMapper;
using BlogProject.Application.DTOs;
using BlogProject.Application.Enums;
using BlogProject.Application.Interfaces;
using BlogProject.Application.Models;
using BlogProject.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogProject.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Manager,Takım Lideri,Bölge Sorumlusu")]
    [Area("Admin")]
    [IgnoreAntiforgeryToken]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //[Authorize(Roles = "Manager,Takım Lideri,Bölge Sorumlusu")]
        public async Task<IActionResult> UserList(int page = 1, int pageSize = 4, bool includeDeleted = false)
        {
            var users = await _userService.GetPagedUsersAsync(page, pageSize, includeDeleted);
            users.IncludeDeleted = includeDeleted;
            users.ControllerName = "Users";
            users.ActionName = "UserList";

            var itemPagination = new ItemPagination<UserViewModel>()
            {
                TotalCount = users.TotalCount,
                PageSize = pageSize,
                CurrentPage = page,
                IncludeDeleted = includeDeleted,
                ControllerName = "Users",
                ActionName = "UserList",
                Items = _mapper.Map<List<UserViewModel>>(users.Items)
            };
            return View(itemPagination);
        }

        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> SuspendUser(SuspendUserViewModel request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Failed"] = "Error in suspension model!";
                return RedirectToAction("UserList", "Users");
            }
            var mappedRequest = _mapper.Map<SuspendUserDto>(request);
            await _userService.SuspendUser(mappedRequest);

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
        //[Authorize(Roles = "Manager,Bölge Sorumlusu,Takım Lideri")]
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
