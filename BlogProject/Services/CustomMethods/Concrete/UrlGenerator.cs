using BlogProject.Services.CustomMethods.Abstract;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BlogProject.Services.CustomMethods.Concrete
{
    public class UrlGenerator : IUrlGenerator
    {
        private readonly IUrlHelper _urlHelper;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public UrlGenerator(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _httpContextAccessor = httpContextAccessor;
        }
        public string GenerateEmailConfirmationUrl(AppUser user, string token)
        {
            var emailConfirmationLink = _urlHelper.Action("ConfirmEmail", "User", new { userId = user.Id, token = token }, _httpContextAccessor.HttpContext.Request.Scheme);

            return emailConfirmationLink!;
        }
        public string GenerateResetPasswordUrl(AppUser user, string _passwordResetToken)
        {
            var passwordResetLink = _urlHelper.Action("ResetPassword","User",new { userId = user.Id, token = _passwordResetToken}, _httpContextAccessor.HttpContext.Request.Scheme);

            return passwordResetLink!;
        }

        public string GenerateCustomUrl(string? controller, string? action, string? area)
        {
            var customLink = _urlHelper.Action(
                                action: action,
                                controller: controller,
                                values: new { area = area },
                                protocol: _httpContextAccessor.HttpContext.Request.Scheme
                            );

            return customLink!;
        }


    }
}

//new UrlActionContext
//{
//    Action = "ResetPassword",
//    Controller = "User",
//    Values = new { userId = user.Id, token = _passwordResetToken },
//    Protocol = "https"
//}
