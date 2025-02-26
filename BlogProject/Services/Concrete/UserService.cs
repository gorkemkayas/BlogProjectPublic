using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using BlogProject.Services.CustomMethods.Abstract;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.Reflection.Metadata;

namespace BlogProject.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUsernameGenerator _usernameGenerator;

        public UserService(UserManager<AppUser> userManager, IUsernameGenerator usernameGenerator)
        {
            _userManager = userManager;
            _usernameGenerator = usernameGenerator;
        }
        public async Task<(bool, IEnumerable<IdentityError>?)> SignUp(SignUpViewModel request)
        {
            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = _usernameGenerator.GenerateUsernameByEmail(request.Email),
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = request.BirthDate
            },request.Password);

            if (identityResult.Succeeded)
            {
                return (true, null);
            }

            return (false, identityResult.Errors);
        }
    }
}
