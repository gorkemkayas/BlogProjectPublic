using BlogProject.Extensions;
using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using BlogProject.Services.CustomMethods.Abstract;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Reflection.Metadata;
using System.Security.Policy;

namespace BlogProject.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUsernameGenerator _usernameGenerator;

        public UserService(UserManager<AppUser> userManager, IUsernameGenerator usernameGenerator, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _usernameGenerator = usernameGenerator;
            _signInManager = signInManager;
        }

        public async Task<List<AppUser>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<List<AppUser>> MostContributors(int countUser)
        {
            var users = await _userManager.Users.OrderByDescending(x => x.Posts.Count).Take(countUser).ToListAsync();
            return users;
        }
        public async Task<List<AppUser>> NewUsers(int countUser)
        {
            var users = await _userManager.Users.OrderByDescending(x => x.RegisteredDate).Take(countUser).ToListAsync();
            return users;
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> SignUp(SignUpViewModel request)
        {
            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = _usernameGenerator.GenerateUsernameByEmail(request.Email),
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = (DateTime)request.BirthDate!
            }, request.Password);

            if (identityResult.Succeeded)
            {
                return (true, null);
            }

            return (false, identityResult.Errors);
        }
        
        public async Task<(bool, IEnumerable<IdentityError>?)> SignInAsync(SignInViewModel request)
        {
            var errors = new List<IdentityError>();

            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null) 
            {
                errors.Add(new IdentityError() { Code = "SignInError", Description = "The email or password is incorrect." });
                return (false, errors);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser,request.Password, request.RememberMe, true);

            if(signInResult.Succeeded)
            {
                return (true, null);
            }

            if(signInResult.IsLockedOut)
            {
                errors.Add(new IdentityError() { Code = "SignInError", Description = $"Your account is locked, will be accesible at {hasUser.LockoutEnd}" });
                return (false, errors);
            }


            errors.Add(new IdentityError() { Code = "SignInError", Description = $"You have tried {hasUser.AccessFailedCount} times. Remain attempts: {5 - hasUser.AccessFailedCount}" });
            errors.Add(new IdentityError() { Code = "SignInError", Description = "The email or password is incorrect." });
            return (false, errors);

        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();

        }
    }
}
