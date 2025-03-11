using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Context;
using BlogProject.src.Infra.Entitites;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Services.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly BlogDbContext _blogDbContext;
        public CommentService(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }
        public async Task<int> GetCommentCountByUserAsync(AppUser user)
        {
            var commentCount = await _blogDbContext.Comments.CountAsync(c => c.AuthorId == user.Id);
            return commentCount;
        }
    }
}
