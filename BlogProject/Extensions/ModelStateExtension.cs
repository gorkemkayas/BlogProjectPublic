using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlogProject.Extensions
{
    public static class ModelStateExtension
    {
        public static void AddModelErrorList(this ModelStateDictionary modelState, List<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
