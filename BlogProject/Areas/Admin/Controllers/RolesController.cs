using BlogProject.Areas.Admin.Models;
using BlogProject.Extensions;
using BlogProject.Services.Abstract;
using BlogProject.Services.Concrete;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IRoleService _roleService;
        public RolesController(UserManager<AppUser> userManager, IRoleService roleService)
        {
            _userManager = userManager;
            _roleService = roleService;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult RoleAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleAdd(RoleAddViewModel request)
        {
            var result = await _roleService.AddRoleAsync(new AppRole() { Name = request.Name, CreatedBy = request.CreatedBy});
            if(!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors!);
                TempData["Failed"] = "The role could not be created.";
                return View();
            }
            TempData["Succeed"] = "The role was created successfully.";
            return RedirectToAction(nameof(RolesController.RoleList));
        }

        [HttpGet]
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);

            return role == null ? NotFound() : View(new RoleEditViewModel()
            {
                Id = role.Id.ToString(),
                Name = role.Name!
            });
        }

        [HttpPost]

        public async Task<IActionResult> RoleEdit(RoleEditViewModel request)
        {
            var result = await _roleService.UpdateRoleAsync(request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors!);
                TempData["Failed"] = "The role could not be updated.";
                return View();
            }

            TempData["Succeed"] = "The role was updated successfully.";
            return RedirectToAction(nameof(RolesController.RoleList));
        }


        [HttpGet]
        public async Task<IActionResult> RoleList(int page = 1, int pageSize = 4, bool includeDeleted = false)
        {
            var pagedRoles = await _roleService.GetPagedRolesAsync(page,pageSize, includeDeleted);
            pagedRoles.IncludeDeleted = includeDeleted;
            return View(pagedRoles);
        }


        [HttpPost("Roles/RoleDelete/{id}")]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var deleterUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _roleService.DeleteRoleByTypeAsync(id, DeleteType.Soft, deleterUserId!);

            if(!result.IsSuccess)
            {
                if(result.Errors!.Any(result => result.Code == "RoleUsedOnUsers"))
                {
                    TempData["Failed"] = "There are users in this role. You cannot delete it.";
                    return Json(new { status = false, redirectUrl = Url.Action(nameof(RoleList)) });
                }

                ModelState.AddModelErrorList(result.Errors!);
                TempData["Failed"] = "The role could not be deleted.";
                return Json(new { status = false, redirectUrl = Url.Action(nameof(RoleList)) });
            }

            TempData["Succeed"] = "The role was deleted successfully.";
            return Json(new { status = true, redirectUrl = Url.Action(nameof(RoleList)) });
        }
    }
}
