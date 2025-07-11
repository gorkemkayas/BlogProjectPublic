using BlogProject.Domain.Entities;
using BlogProject.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents
{
    public class UserProfilePictureViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public UserProfilePictureViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, string width, string height)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return View("Default");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Default");
            }
            var userPhotoLink = string.IsNullOrEmpty(user.ProfilePicture)
                ? "/img/defaultProfilePhoto.jpg"
                : $"/img/userPhotos/{user.UserName}/{user.ProfilePicture}";

            var info = new UserProfilePictureViewModel() { UserPhotoLink = userPhotoLink, Width = width, Height = height};

            return View("Default", info);
        }
    }
}
