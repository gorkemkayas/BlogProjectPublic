using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Context;
using BlogProject.src.Infra.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            return Json(new
            {
                Success = true,
                Comment = model.Comment,
                AuthorName = User.Identity.Name,
                CreatedDate = DateTime.Now
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
