using BlogProject.Application.CustomMethods.Interfaces;
using BlogProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace BlogProject.Infrastructure.CustomMethods
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
