using BlogProject.Models.ViewModels;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogProject.Services.Abstract
{
    public interface IUserService
    {
        Task<(bool, IEnumerable<IdentityError>?)> SignUp(SignUpViewModel request);
        Task<(bool, IEnumerable<IdentityError>?)> SignInAsync(SignInViewModel request);

        Task<(bool, List<IdentityError>?, bool isCritical)> UpdateProfileAsync(AppUser oldUserInfo, ExtendedProfileViewModel newUserInfo, IFormFile? fileInputProfile, IFormFile? coverInputProfile);
        Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordLinkAsync(ForgetPasswordViewModel request);

        Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordAsync(ResetPasswordViewModel request, string? userId, string? token);
        Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(PasswordChangeViewModel request, ClaimsPrincipal user);

        Task LogInAsync(AppUser user);
        Task LogoutAsync();

        Task<int> GetCommentCountByUserAsync(AppUser user);
        Task<int> GetUserTotalLikeCount(AppUser user);
        Task<int> GetPostCountByUserAsync(AppUser user);
        Task<ExtendedProfileViewModel> GetExtendedProfileInformationAsync(AppUser currentUser);
        VisitorProfileViewModel GetVisitorProfileInformation(AppUser visitedUser);

        Task<List<AppUser>> MostContributors(int countUser);
        Task<List<AppUser>> NewUsers(int countUser);
        Task<List<AppUser>> GetUsers();
    }
}
