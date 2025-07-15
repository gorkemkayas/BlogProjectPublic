using BlogProject.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents
{
    public class SuggestedUsersViewComponent : ViewComponent
    {
        public readonly IUserService _userService;

        public SuggestedUsersViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = await _userService.GetUsersByCount(3);
            return View(users);
        }
    }
}
