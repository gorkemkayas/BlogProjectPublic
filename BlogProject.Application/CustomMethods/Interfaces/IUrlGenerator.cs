using BlogProject.Domain.Entities;

namespace BlogProject.Application.CustomMethods.Interfaces
{
    public interface IUrlGenerator
    {
        string GenerateResetPasswordUrl(AppUser user, string token);
        string GenerateEmailConfirmationUrl(AppUser user, string token);
        string GenerateCustomUrl(string? controller, string? action, string? area = "");
    }
}
