using BlogProject.Models.ViewModels;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Services.Abstract
{
    public interface IUserService
    {
        public Task<(bool, IEnumerable<IdentityError>?)> SignUp(SignUpViewModel request);

        Task<List<AppUser>> MostContributors(int countUser);
        Task<List<AppUser>> NewUsers(int countUser);
        Task<List<AppUser>> GetUsers();
    }
}
