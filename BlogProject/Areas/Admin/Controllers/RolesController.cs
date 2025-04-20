using BlogProject.Areas.Admin.Models;
using BlogProject.Extensions;
using BlogProject.Services.Abstract;
using BlogProject.Services.Concrete;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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


        public async Task<IActionResult> RoleList(int page = 1, int pageSize = 4)
        {
            //var allRoles = await _roleService.GetAllRolesAsync();

            var pagedRoles = await _roleService.GetPagedRolesAsync(page,pageSize);
            return View(pagedRoles);
        }
    }
}
