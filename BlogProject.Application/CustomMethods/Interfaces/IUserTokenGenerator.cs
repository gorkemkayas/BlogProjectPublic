using BlogProject.Domain.Entities;

namespace BlogProject.Application.CustomMethods.Interfaces
{
    public interface IUserTokenGenerator
    {
        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
    }
}
