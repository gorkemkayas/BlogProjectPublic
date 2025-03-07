using BlogProject.Services.CustomMethods.Abstract;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Services.CustomMethods.Concrete
{
    public class UserTokenGenerator : IUserTokenGenerator
    {
        private readonly UserManager<AppUser> _userManager;
        public UserTokenGenerator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
    }
}
