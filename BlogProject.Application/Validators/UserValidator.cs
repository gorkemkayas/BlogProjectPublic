using BlogProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Application.Validators
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var isDigit = int.TryParse(user.UserName![0].ToString(), out _);
            var isDigitE = int.TryParse(user.Email![0].ToString(), out _);

            if (isDigit)
            {
                errors.Add(new()
                {
                    Code = "UsernameStartsWithDigit",
                    Description = "Username cannot starts with digit."

                });
            }
            if(isDigitE)
            {
                errors.Add(new()
                {
                    Code = "EmailStartsWithDigit",
                    Description = "Email cannot starts with digit."
                });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
