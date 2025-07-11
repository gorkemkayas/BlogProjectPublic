using AutoMapper;
using BlogProject.Application.DTOs;
using BlogProject.Application.Enums;
using BlogProject.Application.Interfaces;
using BlogProject.Application.Models;
using BlogProject.Areas.Admin.Models;
using BlogProject.Domain.Entities;
using BlogProject.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BlogProject.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    //[Authorize(Roles = "Manager,Takım Lideri,Bölge Sorumlusu")]
    [IgnoreAntiforgeryToken]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        private readonly IRoleService _roleService;
        public RolesController(UserManager<AppUser> userManager, IRoleService roleService, RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleService = roleService;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Roles = "Manager,Bölge Sorumlusu")]
        public IActionResult RoleAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleAdd(RoleAddViewModel request)
        {
            var result = await _roleService.AddRoleAsync(new AppRole() { Name = request.Name, CreatedBy = request.CreatedBy });
            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors!);
                TempData["Failed"] = "The role could not be created.";
                return View();
            }
            TempData["Succeed"] = "The role was created successfully.";
            return RedirectToAction(nameof(RolesController.RoleList));
        }

        [HttpGet]
        //[Authorize(Roles = "Manager,Bölge Sorumlusu")]
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
        //[Authorize(Roles = "Manager,Bölge Sorumlusu")]

        public async Task<IActionResult> RoleEdit(RoleEditViewModel request)
        {
            var mappedRequest = _mapper.Map<RoleEditDto>(request);
            var result = await _roleService.UpdateRoleAsync(mappedRequest);

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
        //[Authorize(Roles = "Manager,Bölge Sorumlusu,Takım Lideri")]
        public async Task<IActionResult> RoleList(int page = 1, int pageSize = 4, bool includeDeleted = false)
        {
            var pagedRoles = await _roleService.GetPagedRolesAsync(page, pageSize, includeDeleted);
            pagedRoles.IncludeDeleted = includeDeleted;
            pagedRoles.ControllerName = "Roles";
            pagedRoles.ActionName = "RoleList";

            var mappedRoles = _mapper.Map<ItemPagination<RoleViewModel>>(pagedRoles);
            return View(mappedRoles);
        }


        [HttpPost("Roles/RoleDelete/{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var deleterUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _roleService.DeleteRoleByTypeAsync(id, DeleteType.Soft, deleterUserId!);

            if (!result.IsSuccess)
            {
                if (result.Errors!.Any(result => result.Code == "RoleUsedOnUsers"))
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

        [HttpPost("Roles/RoleActivate/{id}")]
        //[Authorize(Roles = "Manager,Bölge Sorumlusu,Takım Lideri")]
        public async Task<IActionResult> RoleActivate(string id)
        {
            var activatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(activatorUserId))
            {
                TempData["Failed"] = "Invalid user ID!";
                return RedirectToAction(nameof(RoleList), "Role");
            }
            var result = await _roleService.ActivateRoleById(id);

            if (!result.IsSuccess)
            {
                TempData["Failed"] = "An error occured while attemping activate role.";
                return Json(new { status = false, redirectUrl = Url.Action(nameof(RoleList)) });
            }
            TempData["Succeed"] = "Role activated successfully!";
            return Json(new { status = true, redirectUrl = Url.Action(nameof(RoleList)) });
        }

        //[Authorize(Roles = "Manager,Takım Lideri,Bölge Sorumlusu")]
        public async Task<IActionResult> RoleAssign(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            var userModel = new UserViewModel()
            {
                Id = currentUser!.Id.ToString(),
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                Username = currentUser.UserName!,
                ProfileImage = currentUser.ProfilePicture,
                LastLoginDate = currentUser.LastLoginDate,
                IsSuspended = currentUser.IsSuspended,
                Birthdate = currentUser.BirthDate,
                Email = currentUser.Email!
            };

            var roles = await _roleManager.Roles.ToListAsync();

            var userRoles = new List<UserRoleViewModel>();

            foreach (var role in roles)
            {
                var roleExists = await _userManager.IsInRoleAsync((await _userManager.FindByIdAsync(userModel.Id))!, role.Name!);
                userRoles.Add(new UserRoleViewModel()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name!,
                    Exists = roleExists
                });
            }
            userModel.Roles = userRoles;

            return View(userModel);
        }

        //[Authorize(Roles = "Manager,Bölge Sorumlusu")]
        public async Task<IActionResult> RoleAssignToUser(string userName,string roleId)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var role = await _roleManager.FindByIdAsync(roleId);
            if (user == null || role == null)
            {
                TempData["Failed"] = "User or role not found.";
                return NotFound();
            }

            var result = await _userManager.AddToRoleAsync(user,role.Name!);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.ToList());
                TempData["Failed"] = "The role could not be assigned to the user.";

                return View("RoleAssign", new UserViewModel()
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName!,
                    ProfileImage = user.ProfilePicture,
                    LastLoginDate = user.LastLoginDate,
                    IsSuspended = user.IsSuspended,
                    Birthdate = user.BirthDate,
                    Email = user.Email!
                });
            }
            TempData["Succeed"] = "The role was assigned to the user successfully.";

            return RedirectToAction("RoleAssign", new { userName = user.UserName });
        }

        [HttpPost]  
        //[Authorize(Roles = "Manager,Takım Lideri,Bölge Sorumlusu")]
        public async Task<IActionResult> RoleRemoveFromUser(string userName, string roleId, bool fromRoleUsers = false)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var role = await _roleManager.FindByIdAsync(roleId);
            if (user == null || role == null)
            {
                TempData["Failed"] = "User or role not found.";
                return NotFound();
            }

            var result = await _userManager.RemoveFromRoleAsync(user,role.Name!);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.ToList());
                TempData["Failed"] = "The role could not be assigned to the user.";

                return View("RoleAssign", new UserViewModel()
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName!,
                    ProfileImage = user.ProfilePicture,
                    LastLoginDate = user.LastLoginDate,
                    IsSuspended = user.IsSuspended,
                    Birthdate = user.BirthDate,
                    Email = user.Email!
                });
            }
            TempData["Succeed"] = "The role was removed from the user successfully.";
            if(fromRoleUsers)
            {
                return RedirectToAction("RoleUsers", new { roleName = role.Name });
            }
            return RedirectToAction("RoleAssign", new { userName = user.UserName });
        }

        public async Task<IActionResult> RoleUsers(string roleName)
        {
            var roleWithUsers = await _roleService.GetRolesWithUsers(roleName);

            var mappedRoles = _mapper.Map<RoleUsersViewModel>(roleWithUsers);
            return View(mappedRoles);
        }

    }
}
