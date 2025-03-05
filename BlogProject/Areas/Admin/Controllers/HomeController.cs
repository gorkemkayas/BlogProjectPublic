using AutoMapper;
using BlogProject.Areas.Admin.Models;
using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize]
    public class HomeController : Controller
    {
        UserManager<AppUser> _userManager;
        IUserService userService;
        IMapper mapper;

        public HomeController(UserManager<AppUser> userManager, IUserService userService, IMapper mapper)
        {
            _userManager = userManager;
            this.userService = userService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await userService.GetUsers();
            var mostContributors = await userService.MostContributors(5);
            var newUsers = await userService.NewUsers(5);

            var mappedUsers = mapper.Map<List<UserViewModel>>(users);
            var mappedMostContributors = mapper.Map<List<UserViewModel>>(mostContributors);
            var mappedNewUsers = mapper.Map<List<UserViewModel>>(newUsers);

            var combinedViewModel = new CombinedViewModel
            {
                Users = mappedUsers,
                MostContributors = mappedMostContributors,
                NewMembers = mappedNewUsers
            };
            return View(combinedViewModel);
        }
    }
}
