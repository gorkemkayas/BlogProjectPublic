using BlogProject.Areas.Admin.Models;
using BlogProject.Models.ViewModels;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Services.Abstract
{
    public interface IUserService
    {

        bool CheckEmailConfirmed(AppUser user);
        Task<ExtendedProfileViewModel> ConfigurePictureAsync(ExtendedProfileViewModel newUserInfo, AppUser oldUserInfo, IFormFile? formFile, PhotoType type);
        Task<ServiceResult<AppUser>> SignUp(SignUpViewModel request);
        Task<(bool, IEnumerable<IdentityError>?)> SignInAsync(SignInViewModel request);
        Task<ServiceResult<AppUser>> ConfirmEmailAsync(ConfirmEmailViewModel request);

        Task<(bool, List<IdentityError>?, bool isCritical)> UpdateProfileAsync(AppUser oldUserInfo, ExtendedProfileViewModel newUserInfo, IFormFile? fileInputProfile, IFormFile? coverInputProfile, IFormFile? IconInputWorkingAt);
        Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordLinkAsync(ForgetPasswordViewModel request);

        Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordAsync(ResetPasswordViewModel request, string? userId, string? token);
        Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(PasswordChangeViewModel request, ClaimsPrincipal user);

        Task<ItemPagination<UserViewModel>> GetPagedUsersAsync(int page, int pageSize, bool includeDeleted = false);
        Task LogInAsync(AppUser user);
        Task LogoutAsync();

        Task SuspendUser(SuspendUserViewModel request);

        Task<int> GetCommentCountByUserAsync(AppUser user);
        Task<int> GetUserTotalLikeCount(AppUser user);
        Task<int> GetPostCountByUserAsync(AppUser user);
        Task<ExtendedProfileViewModel> GetExtendedProfileInformationAsync(AppUser currentUser);
        VisitorProfileViewModel GetVisitorProfileInformation(AppUser visitedUser);

        Task<List<AppUser>> MostContributors(int countUser);
        Task<List<AppUser>> NewUsers(int countUser);
        Task<List<AppUser>> GetUsers();

        Task<ServiceResult<AppUser>> DeleteUserByTypeAsync(string id, DeleteType deleteType, string deleterUserId);

        Task<ServiceResult<AppUser>> ActivateUserById(string userId);
    }
}
