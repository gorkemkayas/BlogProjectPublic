using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents
{
    public class SuggestedUsersViewComponent : ViewComponent
    {
        public readonly BlogDbContext _blogDbContext;

        public SuggestedUsersViewComponent(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = _blogDbContext.Users.Take(3).ToList();
            return View(users);
        }
    }
}
