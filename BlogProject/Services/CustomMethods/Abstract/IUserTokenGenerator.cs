using BlogProject.src.Infra.Entitites;

namespace BlogProject.Services.CustomMethods.Abstract
{
    public interface IUserTokenGenerator
    {
        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
    }
}
