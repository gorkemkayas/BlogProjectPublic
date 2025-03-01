using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlogProject.CustomValidators
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();

            if(password!.ToLower().Contains((user.Email!.ToLower()).Substring(0,user.Email.IndexOf("@"))))
            {
                errors.Add(new()
                {
                    Code = "PasswordContainsEmail",
                    Description = "Password cannot contain email address."
                });
            }
            if(password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new()
                {
                    Code = "PasswordContainsUsername",
                    Description = "Password cannot contain username"
                });
            }

            if(errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
