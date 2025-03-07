using BlogProject.Services.CustomMethods.Abstract;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BlogProject.Services.CustomMethods.Concrete
{
    public class UrlGenerator : IUrlGenerator
    {
        private readonly IUrlHelper _urlHelper;
        public UrlGenerator(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        public string GenerateResetPasswordUrl(AppUser user, string _passwordResetToken)
        {
            var passwordResetLink = _urlHelper.Action(new UrlActionContext
            {
                Action = "ResetPassword",
                Controller = "User",
                Values = new { userId = user.Id, token = _passwordResetToken },
                Protocol = "https"
            });

            return passwordResetLink!;
        }
    }
}
