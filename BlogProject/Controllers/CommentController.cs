using BlogProject.Domain.Entities;
using BlogProject.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BlogProject.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly BlogDbContext _blogDbContext;

        public CommentController(UserManager<AppUser> userManager, BlogDbContext blogDbContext)
        {
            _userManager = userManager;
            _blogDbContext = blogDbContext;
        }
        [HttpPost("Comment/AddComment")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AddComment([FromBody] AddCommentViewModel model)
        {

            var commentEntity = new CommentEntity
            {
                AuthorId = Guid.Parse(model.AuthorId),
                PostId = Guid.Parse(model.PostId!),
                Content = model.Comment,
                ParentCommentId = string.IsNullOrEmpty(model.ParentCommentId) ? null : Guid.Parse(model.ParentCommentId)
            };

            try
            {
            var addedComment = await _blogDbContext.Comments.AddAsync(commentEntity);
            await _blogDbContext.SaveChangesAsync();
                var author = await _userManager.FindByIdAsync(model.AuthorId);
                var profilePhotoUrl = author.ProfilePicture;
                var url = $"/img/userPhotos/{author.UserName}/{profilePhotoUrl}";
                return Json(new
            {
                Success = true,
                Comment = model.Comment,
                AuthorName = author.FullName,
                AuthorProfileUrl = url,
                CreatedDate = DateTime.Now.ToString("dddd d, yyyy", CultureInfo.InvariantCulture)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding comment: {ex.Message}");
                throw;
            }
        }
    }
}
