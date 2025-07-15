using BlogProject.Application.Common;
using BlogProject.Application.CustomMethods.Interfaces;
using BlogProject.Application.DTOs;
using BlogProject.Application.Enums;
using BlogProject.Application.Interfaces;
using BlogProject.Application.Models;
using BlogProject.Domain.Entities;
using BlogProject.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using static BlogProject.Domain.Entities.AppUser;

namespace BlogProject.Infrastructure.Services
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
        public UserService(UserManager<AppUser> userManager, IUsernameGenerator usernameGenerator, SignInManager<AppUser> signInManager, IUserTokenGenerator userTokenService, IUrlGenerator urlGenerator, IEmailService emailService, BlogDbContext blogdbContext, ICommentService commentService)
        {
            _userManager = userManager;
            _usernameGenerator = usernameGenerator;
            _signInManager = signInManager;
            _userTokenGenerator = userTokenService;
            _urlGenerator = urlGenerator;
            _emailService = emailService;
            _blogdbContext = blogdbContext;
            _commentService = commentService;
        }
        public async Task<ServiceResult<AppUser>> DeleteUserByTypeAsync(string id, DeleteType deleteType, string deleterId)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                var serviceResult = new ServiceResult<AppUser>()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "UserNotFound", Description = "User not found." } }
                };

                return serviceResult;
            }
            var deleterUserId = deleterId;

            var result = new IdentityResult();
            switch (deleteType)
            {
                case DeleteType.Soft:
                    user.IsDeleted = true;
                    //user.DeletedById = deleterUserId;
                    //user.DeletedDate = DateTime.Now;
                    result = await _userManager.UpdateAsync(user);
                    break;
                case DeleteType.Hard:
                    user.IsDeleted = true;
                    user.DeletedBy = deleterUserId;
                    result = await _userManager.DeleteAsync(user);
                    break;
            }
            if (!result.Succeeded)
            {
                return new ServiceResult<AppUser>() { IsSuccess = false, Errors = new List<IdentityError>() { new IdentityError() { Code = "UserCannotDeleted", Description = "User cannot deleted." } } };
            }
            return new ServiceResult<AppUser>()
            {
                IsSuccess = true
            };
        }

        public bool CheckEmailConfirmed(AppUser user)
        {
            return user.EmailConfirmed;
        }
        public async Task SaveChangesAsync()
        {
            await _blogdbContext.SaveChangesAsync();
        }
        public async Task<ServiceResult<AppUser>> SubscribeToNotificationsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "UserNotFound", Description = "User not found." } }
                };
            }
            user.GeneralNotificationActive = true;
            var result = await _userManager.UpdateAsync(user);
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
                IsSuccess = true,
                Data = user
            };
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

        public async Task<ItemPagination<UserDto>> GetPagedUsersAsync(int page, int pageSize, bool includeDeleted = false)
        {
            var itemsQuery = _userManager.Users;
            if (!includeDeleted)
            {
                itemsQuery = itemsQuery.Where(p => p.IsDeleted == false);
            }

            var pagedUsers = new ItemPagination<UserDto>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = (includeDeleted is true) ? _userManager.Users.Count() : _userManager.Users.Where(p => p.IsDeleted == false).Count(),
                Items = await itemsQuery
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(user => new UserDto
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

        public VisitorProfileDto GetVisitorProfileInformation(AppUser visitedUser)
        {
            var visitorProfileInfo = new VisitorProfileDto()
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
        public async Task<ExtendedProfileDto> GetExtendedProfileInformationAsync(AppUser currentUser)
        {
            var extendedProfile = new ExtendedProfileDto()
            {
                Id = currentUser.Id.ToString(),
                PhoneNumber = currentUser.PhoneNumber,
                EmailAddress = currentUser.Email!,
                EmailConfirmed = currentUser.EmailConfirmed,
                TwoFactorEnabled = currentUser.TwoFactorEnabled,
                LockoutEnabled = currentUser.LockoutEnabled,
                SecurityStamp = currentUser.SecurityStamp,
                ConcurrencyStamp = currentUser.ConcurrencyStamp!.ToString(),

                UserName = currentUser.UserName,
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                Title = currentUser.Title,
                Bio = currentUser.Bio,
                WorkingAt = currentUser.WorkingAt,
                Country = currentUser.Country,
                FollowersCount = currentUser.FollowersCount,
                FollowingCount = currentUser.FollowingCount,
                PostCount = (currentUser.Posts is null) ? 0 : currentUser.Posts.Count(),
                CommentCount = (currentUser.Comments is null) ? 0 : currentUser.Comments.Count(),
                LikeCount = (currentUser.Likes is null) ? 0 : currentUser.Likes.Count(),
                CurrentPosition = currentUser.CurrentPosition,
                City = currentUser.City,
                Address = currentUser.Address,
                XAddress = currentUser.XAddress,
                LinkedinAddress = currentUser.LinkedinAddress,
                GithubAddress = currentUser.GithubAddress,
                MediumAddress = currentUser.MediumAddress,
                YoutubeAddress = currentUser.YoutubeAddress,
                PersonalWebAddress = currentUser.PersonalWebAddress,
                HighSchoolName = currentUser.HighSchoolName,
                HighSchoolStartYear = currentUser.HighSchoolStartYear,
                HighSchoolGraduationYear = currentUser.HighSchoolGraduationYear,
                BirthDate = currentUser.BirthDate,
                RegisteredDate = currentUser.RegisteredDate,
                ProfilePicture = currentUser.ProfilePicture,
                CoverImagePicture = currentUser.CoverImagePicture,
                WorkingAtLogo = currentUser.WorkingAtLogo
            };
            extendedProfile.CommentCount = await GetCommentCountByUserAsync(currentUser);
            extendedProfile.LikeCount = await GetUserTotalLikeCount(currentUser);
            extendedProfile.PostCount = await GetPostCountByUserAsync(currentUser);



            return extendedProfile;

        }


        public async Task<(bool, List<IdentityError>?, bool isCritical)> UpdateProfileAsync(AppUser oldUserInfo, ExtendedProfileDto newUserInfo, IFormFile? fileInputProfile, IFormFile? coverInputProfile, IFormFile? IconInputWorkingAt)
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

            var updatedUser = new AppUser()
            {
                //From ExtendedProfileDto
                Id = Guid.Parse(step3.Id),
                PhoneNumber = step3.PhoneNumber,
                Email =step3.EmailAddress,
                EmailConfirmed = step3.EmailConfirmed,
                TwoFactorEnabled = step3.TwoFactorEnabled,
                LockoutEnabled = step3.LockoutEnabled,
                SecurityStamp = step3.SecurityStamp,
                ConcurrencyStamp = step3.ConcurrencyStamp,

                //From VisitorProfileDto - which base of ExtendedProfileDto
                UserName = step3.UserName,
                Name = step3.Name,
                Surname = step3.Surname,
                Title = step3.Title,
                Bio = step3.Bio,
                WorkingAt = step3.WorkingAt,
                Country = step3.Country,
                FollowersCount = step3.FollowersCount,
                FollowingCount = step3.FollowingCount,
                Posts = oldUserInfo.Posts,
                Comments = oldUserInfo.Comments,
                Likes = oldUserInfo.Likes,
                CurrentPosition = step3.CurrentPosition,
                City = step3.City,
                Address = step3.Address,
                XAddress = step3.XAddress,
                LinkedinAddress = step3.LinkedinAddress,
                GithubAddress = step3.GithubAddress,
                MediumAddress = step3.MediumAddress,
                YoutubeAddress = step3.YoutubeAddress,
                PersonalWebAddress = step3.PersonalWebAddress,
                HighSchoolName = step3.HighSchoolName,
                HighSchoolStartYear = step3.HighSchoolStartYear,
                HighSchoolGraduationYear = step3.HighSchoolGraduationYear,
                UniversityName = step3.UniversityName,
                UniversityStartYear = step3.UniversityStartYear,
                UniversityGraduationYear = step3.UniversityGraduationYear,
                BirthDate = step3.BirthDate,
                ProfilePicture = step3.ProfilePicture,
                CoverImagePicture = step3.CoverImagePicture,
                WorkingAtLogo = step3.WorkingAtLogo,

            };
            await _userManager.UpdateAsync(updatedUser);

            if (criticalUpdate)
            {
                await _userManager.UpdateSecurityStampAsync(oldUserInfo);
                return (true, null, true);
            }

            return (true, null, false);
        }
        public async Task<ExtendedProfileDto> ConfigurePictureAsync(ExtendedProfileDto newUserInfo, AppUser oldUserInfo, IFormFile? formFile, PhotoType type)
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

        public async Task<ServiceResult<AppUser>> SignUp(SignUpDto request)
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
        public async Task<ServiceResult<AppUser>> ConfirmEmailAsync(ConfirmEmailDto request)
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

        public async Task<(bool, IEnumerable<IdentityError>?)> SignInAsync(SignInDto request)
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
            var userClaims = await _userManager.GetClaimsAsync(hasUser);
            var claims = new List<Claim>(userClaims)
    {
        new Claim("ProfilePictureUrl", $"{hasUser.ProfilePicture}")
    };
            _userManager.AddClaimsAsync(hasUser, claims).Wait(); // Add ProfilePictureUrl claim
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

        public async Task<AppUser?> FindByUsername(string? userName)
        {
            if(userName == null)
            {
                return null;
            }
            return await _blogdbContext.Users
                    .FirstOrDefaultAsync(u => u.UserName == userName);
        }
        public async Task<List<AppUser>> GetUsersByCount(int countUser)
        {
            var users = await _userManager.Users.Take(countUser).ToListAsync();
            return users;
        }
        public async Task SuspendUser(SuspendUserDto request)
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

        public async Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(PasswordChangeDto request, ClaimsPrincipal user)
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

        public async Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordLinkAsync(ForgetPasswordDto request)
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

        public async Task<(bool, IEnumerable<IdentityError>?)> ResetPasswordAsync(ResetPasswordDto request, string? userId, string? token)
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

        public async Task<ServiceResult<AppUser>> ActivateUserById(string userId)
        {
            var user = await _blogdbContext.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "UserNotFound", Description = "User not found." } }
                };
            }
            try
            {
                user.IsDeleted = false;
                _blogdbContext.Users.Update(user);
                await _blogdbContext.SaveChangesAsync();
                return new ServiceResult<AppUser>() { IsSuccess = true, Data = user };
            }
            catch (Exception)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "ActivateUserError", Description = "An error occurred while activating the user." } }
                };
            }
        }
    }

}
