using BlogProject.Application.Common;
using BlogProject.Application.Interfaces;
using BlogProject.Domain.Entities;
using BlogProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Infrastructure.Services
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

        public async Task<ServiceResult<CommentEntity>> AddCommentAsync(CommentEntity entity)
        {
            if (entity == null)
                throw new Exception("Comment entity cannot be null.");

            if (entity.AuthorId == Guid.Empty)
                throw new ArgumentException("Author ID cannot be empty.", nameof(entity.AuthorId));

            try
            {
            var addedComment = await _blogDbContext.Comments.AddAsync(entity);
            await _blogDbContext.SaveChangesAsync();
                return new ServiceResult<CommentEntity>() { IsSuccess = true };
            }
            catch (Exception)
            {

                return new ServiceResult<CommentEntity>() { IsSuccess = false };
            }

        }
    }
}
