using BlogProject.Models.ViewModels;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Services.Abstract
{
    public interface IUserService
    {
        Task<(bool, IEnumerable<IdentityError>?)> SignUp(SignUpViewModel request);
        Task<(bool, IEnumerable<IdentityError>?)> SignInAsync(SignInViewModel request, string? returnUrl = null);

        Task<List<AppUser>> MostContributors(int countUser);
        Task<List<AppUser>> NewUsers(int countUser);
        Task<List<AppUser>> GetUsers();
    }
}
