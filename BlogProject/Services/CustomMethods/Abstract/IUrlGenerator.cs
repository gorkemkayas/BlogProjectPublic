using BlogProject.src.Infra.Entitites;

namespace BlogProject.Services.CustomMethods.Abstract
{
    public interface IUrlGenerator
    {
        string GenerateResetPasswordUrl(AppUser user, string token);
    }
}
