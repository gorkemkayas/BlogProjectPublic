using BlogProject.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Services.Abstract
{
    public interface IUserService
    {
        public Task<(bool, IEnumerable<IdentityError>?)> SignUp(SignUpViewModel request);
    }
}
