using AutoMapper;
using BlogProject.Areas.Admin.Models;
using BlogProject.Extensions;
using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using BlogProject.Services.CustomMethods.Abstract;
using BlogProject.Services.CustomMethods.Concrete;
using BlogProject.Services.DTOs.MappingProfile;
using BlogProject.src.Infra.Context;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using NuGet.Common;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlogProject.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly BlogDbContext _blogdbContext;
        private readonly IEmailService _emailService;
        private readonly ICommentService _commentService;
        private readonly IUsernameGenerator _usernameGenerator;
        private readonly IUserTokenGenerator _userTokenGenerator;
        private readonly IUrlGenerator _urlGenerator;
        private readonly IMapper _mapper;
        public UserService(UserManager<AppUser> userManager, IUsernameGenerator usernameGenerator, SignInManager<AppUser> signInManager, IUserTokenGenerator userTokenService, IUrlGenerator urlGenerator, IEmailService emailService, BlogDbContext blogdbContext, ICommentService commentService, IMapper mapper)
        {
            _userManager = userManager;
            _usernameGenerator = usernameGenerator;
            _signInManager = signInManager;
            _userTokenGenerator = userTokenService;
            _urlGenerator = urlGenerator;
            _emailService = emailService;
            _blogdbContext = blogdbContext;
            _commentService = commentService;
            _mapper = mapper;
        }

        public bool CheckEmailConfirmed(AppUser user)
        {
            return user.EmailConfirmed;
        }

        public async Task<int> GetCommentCountByUserAsync(AppUser user)
        {
            var commentCount = await _blogdbContext.Comments.CountAsync(x => x.AuthorId == user.Id);
            return commentCount;
        }
        public async Task<List<AppUser>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<ItemPagination<UserViewModel>> GetPagedUsersAsync(int page, int pageSize, bool includeDeleted = false)
        {
            var itemsQuery = _userManager.Users;
            if (!includeDeleted)
            {
                itemsQuery = itemsQuery.Where(p => p.IsDeleted == false);
            }

            var pagedUsers = new ItemPagination<UserViewModel>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = (includeDeleted is true) ? _userManager.Users.Count() : _userManager.Users.Where(p => p.IsDeleted == false).Count(),
                Items = await itemsQuery
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(user => new UserViewModel
                                    {
                                        Id = user.Id.ToString(),
                                        Name = user.Name,
                                        Surname = user.Surname,
                                        Username = user.UserName!,
                                        Email = user.Email!,
                                        IsDeleted = user.IsDeleted,
                                        PhoneNumber = user.PhoneNumber,
                                        RegisteredDate = user.RegisteredDate,
                                        EmailConfirmed = user.EmailConfirmed,
                                        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                                        TwoFactorEnabled = user.TwoFactorEnabled,
                                        LastLoginDate = user.LastLoginDate,
                                        SuspendedTo = user.SuspendedTo,
                                        IsSuspended = user.IsSuspended

                                    }).ToListAsync()
            };
            return pagedUsers;
        }

        public async Task<int> GetUserTotalLikeCount(AppUser user)
        {
            var likeCount = await _blogdbContext.Likes.CountAsync(x => x.UserId == user.Id);
            return likeCount;
        }

        public async Task<int> GetPostCountByUserAsync(AppUser user)
        {
            var postCount = await _blogdbContext.Posts.CountAsync(x => x.AuthorId == user.Id);
            return postCount;
        }

        public VisitorProfileViewModel GetVisitorProfileInformation(AppUser visitedUser)
        {
            var visitorProfileInfo = new VisitorProfileViewModel()
            {
                UserName = visitedUser.UserName!,
                Name = visitedUser.Name,
                Surname = visitedUser.Surname,
                Bio = visitedUser.Bio,
                BirthDate = visitedUser.BirthDate,
                Country = visitedUser.Country,
                Title = visitedUser.Title,
                RegisteredDate = visitedUser.RegisteredDate,
                ProfilePicture = visitedUser.ProfilePicture,
                CoverImagePicture = visitedUser.CoverImagePicture,
                WorkingAtLogo = visitedUser.WorkingAtLogo,
                FollowersCount = visitedUser.FollowersCount,
                FollowingCount = visitedUser.FollowingCount,
                WorkingAt = visitedUser.WorkingAt,
            };

            return visitorProfileInfo;

        }
        public async Task<ExtendedProfileViewModel> GetExtendedProfileInformationAsync(AppUser currentUser)
        {

            var extendedProfile = _mapper.Map<ExtendedProfileViewModel>(currentUser);
            extendedProfile.CommentCount = await GetCommentCountByUserAsync(currentUser);
            extendedProfile.LikeCount = await GetUserTotalLikeCount(currentUser);
            extendedProfile.PostCount = await GetPostCountByUserAsync(currentUser);



            return extendedProfile;

        }


        public async Task<(bool, List<IdentityError>?, bool isCritical)> UpdateProfileAsync(AppUser oldUserInfo, ExtendedProfileViewModel newUserInfo, IFormFile? fileInputProfile, IFormFile? coverInputProfile, IFormFile? IconInputWorkingAt)
        {
            var errors = new List<IdentityError>();

            bool criticalUpdate = false;

            if (oldUserInfo == null)
            {
                errors.Add(new() { Code = "UserNotFound", Description = "The user not found in the system." });
                return (false, errors, false);
            }

            if (oldUserInfo.Id.ToString() != newUserInfo.Id)
            {
                errors.Add(new() { Code = "UsersNotMatched", Description = "The users not matched." });
                return (false, errors, false);
            }

            if (oldUserInfo.Email != newUserInfo.EmailAddress) criticalUpdate = true;

            var step1 = await ConfigurePictureAsync(newUserInfo, oldUserInfo, fileInputProfile, PhotoType.ProfilePicture);
            var step2 = await ConfigurePictureAsync(step1, oldUserInfo, coverInputProfile, PhotoType.CoverImagePicture);
            var step3 = await ConfigurePictureAsync(step2, oldUserInfo, IconInputWorkingAt, PhotoType.WorkingAtLogo);


            //var step1 = await ConfigureProfilePictureOfNewUserInfoAsync(newUserInfo, oldUserInfo, fileInputProfile);
            //var step2 = await ConfigureCoverPictureOfNewUserInfoAsync(step1, oldUserInfo, coverInputProfile);
            //var step3 = await ConfigureWorkingIconOfNewUserInfoAsync(step2, oldUserInfo, IconInputWorkingAt);

            var updatedUser = _mapper.Map(step3, oldUserInfo);
            await _userManager.UpdateAsync(updatedUser);

            if (criticalUpdate)
            {
                await _userManager.UpdateSecurityStampAsync(oldUserInfo);
                return (true, null, true);
            }

            return (true, null, false);
        }
        public async Task<ExtendedProfileViewModel> ConfigurePictureAsync(ExtendedProfileViewModel newUserInfo, AppUser oldUserInfo, IFormFile? formFile, PhotoType type)
        {

            if (formFile is null)
            {
                newUserInfo.SetProperty(type, oldUserInfo.GetPropertyValue(type));
                return newUserInfo;
            }

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "userPhotos", $"{oldUserInfo.UserName}");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = formFile.FileName.Replace(" ", "_");
            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            newUserInfo.SetProperty(type, fileName);

            if (oldUserInfo.GetPropertyValue(type) != null)
            {
                var oldPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, oldUserInfo.GetPropertyValue(type));

                if (File.Exists(oldPhotoPath))
                {
                    File.Delete(oldPhotoPath);
                }

            }
            return newUserInfo;
        }

        public async Task<List<AppUser>> MostContributors(int countUser)
        {
            var users = await _userManager.Users.OrderByDescending(x => x.Posts.Count).Take(countUser).ToListAsync();
            return users;
        }
        public async Task<List<AppUser>> NewUsers(int countUser)
        {
            var users = await _userManager.Users.OrderByDescending(x => x.RegisteredDate).Take(countUser).ToListAsync();
            return users;
        }

        public async Task<ServiceResult<AppUser>> SignUp(SignUpViewModel request)
        {
            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = _usernameGenerator.GenerateUsernameByEmail(request.Email),
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = (DateTime)request.BirthDate!
            }, request.Password);

            if (identityResult.Succeeded)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = true,
                    Data = await _userManager.FindByEmailAsync(request.Email)
                };
            }

            return new ServiceResult<AppUser>()
            {
                IsSuccess = false,
                Errors = identityResult.Errors.ToList()
            };
        }
        public async Task<ServiceResult<AppUser>> ConfirmEmailAsync(ConfirmEmailViewModel request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Token))
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError() { Code = "ModelEmpty", Description = "Request cannot be null" } }
                };
            }
            var user = await _userManager.FindByIdAsync(request.UserId);

            var result = await _userManager.ConfirmEmailAsync(user!, request.Token);
            if (!result.Succeeded)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = result.Errors.ToList()
                };
            }
            return new ServiceResult<AppUser>()
            {
                IsSuccess = true
            };
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> SignInAsync(SignInViewModel request)
        {
            var errors = new List<IdentityError>();

            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null)
            {
                errors.Add(new IdentityError() { Code = "SignInError", Description = "The email or password is incorrect." });
                return (false, errors);
            }
            if (hasUser.SuspendedTo != null && hasUser.SuspendedTo > DateTime.Now)
            {
                errors.Add(new IdentityError() { Code = "SuspendedAccount", Description = $"Your account is suspended, will be accesible at {hasUser.SuspendedTo}" });
                errors.Add(new IdentityError() { Code = "SuspendedAccountCategory", Description = $"{hasUser.SuspensionReasonCategory}" });
                errors.Add(new IdentityError() { Code = "SuspendedAccountReason", Description = $"{hasUser.SuspensionReasonDetail}" });
                return (false, errors);
            }
            if (!hasUser.EmailConfirmed)
            {
                errors.Add(new IdentityError() { Code = "EmailNotConfirmed", Description = "Your email is not confirmed." });
                return (false, errors);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, request.Password, request.RememberMe, true);

            if (signInResult.Succeeded)
            {
                hasUser.SuspendedTo = null!;
                hasUser.SuspensionReasonCategory = null!;
                hasUser.SuspensionReasonDetail = null!;

                hasUser.LastLoginDate = DateTime.Now;
                await _userManager.UpdateAsync(hasUser);
                return (true, null);
            }

            if (signInResult.IsLockedOut)
            {
                errors.Add(new IdentityError() { Code = "SignInError", Description = $"Your account is locked, will be accesible at {hasUser.LockoutEnd}" });
                return (false, errors);
            }


            errors.Add(new IdentityError() { Code = "SignInError", Description = $"You have tried {hasUser.AccessFailedCount} times. Remain attempts: {5 - hasUser.AccessFailedCount}" });
            errors.Add(new IdentityError() { Code = "SignInError", Description = "The email or password is incorrect." });
            return (false, errors);

        }

        public async Task SuspendUser(SuspendUserViewModel request)
        {

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) throw new Exception("User not found");

            //user.IsSuspended = true;
            if (request.SuspensionMinutes == 0)
            {
                user.SuspendedTo = null;
            }
            else
            {
                user.SuspendedTo = DateTime.Now.AddMinutes(request.SuspensionMinutes);
            }

            user.SuspensionReasonCategory = request.ReasonCategory;
            user.SuspensionReasonDetail = request.ReasonDetail;


            var signInUrl = _urlGenerator.GenerateCustomUrl("User", "SignIn");
            await _emailService.SendSuspensionNotificationEmailAsync(user.Email, signInUrl, request.ReasonCategory == "Remove");
            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.UpdateAsync(user);
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();

        }
        public async Task LogInAsync(AppUser user)
        {
            await _signInManager.SignInAsync(user, false);

            user.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(user);
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(PasswordChangeViewModel request, ClaimsPrincipal user)
        {
            var errors = new List<IdentityError>();

            var currentUser = await _userManager.GetUserAsync(user);

            var isSame = await _userManager.CheckPasswordAsync(currentUser, request.OldPassword);

            if (!isSame)
            {
                errors.Add(new IdentityError() { Code = "ChangePasswordError", Description = "The old password is incorrect." });
                return (false, errors);
            }

            var result = await _userManager.ChangePasswordAsync(currentUser, request.OldPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors);
                return (false, errors);
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();

            await _signInManager.SignInAsync(currentUser, true);

            return (true, null);




        }

        public async Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordLinkAsync(ForgetPasswordViewModel request)
        {
            var errors = new List<IdentityError>();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                errors.Add(new IdentityError() { Code = "ResetPasswordError", Description = "The email is not registered." });
                return (false, errors);
            }

            var passwordResetToken = await _userTokenGenerator.GeneratePasswordResetTokenAsync(user);
            var passwordResetLink = _urlGenerator.GenerateResetPasswordUrl(user, passwordResetToken);

            await _emailService.SendResetPasswordEmailAsync(passwordResetLink, user.Email!);

            return (true, null);
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordAsync(ResetPasswordViewModel request, string? userId, string? token)
        {
            var errors = new List<IdentityError>();

            if (userId == null || token == null)
            {
                errors.Add(new IdentityError() { Code = "ResetPasswordError", Description = "Token or userId not found." });
                return (false, errors);
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

            if (hasUser == null)
            {
                errors.Add(new IdentityError() { Code = "ResetPasswordError", Description = "User not found." });
                return (false, errors);
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.NewPassword);

            if (result.Succeeded)
            {
                return (true, null);
            }

            errors.AddRange(result.Errors);

            return (false, errors);
        }
    }

}
