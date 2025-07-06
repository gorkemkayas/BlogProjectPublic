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

        public async Task<List<CommentEntity>> GetCommentsByPostIdAsync(string postId)
        {
            if (string.IsNullOrEmpty(postId))
            {
                throw new ArgumentException("Post ID cannot be null or empty.", nameof(postId));
            }
            Console.WriteLine("Checking if post is valid...");
            var postValid = await _blogDbContext.Posts.AnyAsync(p => p.Id.ToString() == postId);
            Console.WriteLine("Post validity check complete.");

            if (!postValid)
            {
                throw new ArgumentException("Post not found with the provided ID.", nameof(postId));
            }
            Console.WriteLine("Getting comments...");
            var comments = await _blogDbContext.Comments
                .Where(c => c.PostId.ToString() == postId)
                .Include(c => c.Author)
                .Include(c => c.Replies)
                .ToListAsync();
            if (comments == null || !comments.Any())
            {
                return new List<CommentEntity>();
            }
            Console.WriteLine("Comments loaded.");
            return comments;
        }
    }
}
