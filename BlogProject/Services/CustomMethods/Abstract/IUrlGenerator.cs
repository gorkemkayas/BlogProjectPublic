using BlogProject.src.Infra.Entitites;

namespace BlogProject.Services.CustomMethods.Abstract
{
    public interface IUrlGenerator
    {
        string GenerateResetPasswordUrl(AppUser user, string token);
        string GenerateEmailConfirmationUrl(AppUser user, string token);
    }
}
