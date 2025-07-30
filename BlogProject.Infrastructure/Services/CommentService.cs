using BlogProject.Application.Common;
using BlogProject.Application.DTOs;
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

        public async Task<List<CommentViewModel>> GetCommentsByPostIdAsync(string postId)
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
                                                .AsNoTracking()
                                                .Where(c => c.PostId == Guid.Parse(postId) && !c.IsDeleted)
                                                .Select(c => new CommentViewModel
                                                {
                                                    Id = c.Id,
                                                    Content = c.Content,
                                                    CreatedTime = c.CreatedTime,
                                                    ParentCommentId = c.ParentCommentId,
                                                    LikeCount = c.Likes.Count,
                                                    Author = new AuthorDto
                                                    {
                                                        UserName = c.Author.UserName,
                                                        FullName = c.Author.FullName,
                                                        ProfilePicture = c.Author.ProfilePicture
                                                    },
                                                    Replies = c.Replies
                                                        .Where(r => !r.IsDeleted)
                                                        .Select(r => new CommentViewModel
                                                        {
                                                            Id = r.Id,
                                                            Content = r.Content,
                                                            CreatedTime = r.CreatedTime,
                                                            ParentCommentId = r.ParentCommentId,
                                                            LikeCount = r.Likes.Count,
                                                            Author = new AuthorDto
                                                            {
                                                                UserName = r.Author.UserName,
                                                                FullName = r.Author.FullName,
                                                                ProfilePicture = r.Author.ProfilePicture
                                                            },
                                                            Replies = new List<CommentViewModel>() // derin reply yok
                                                        }).ToList()
                                                })
                                                .ToListAsync();
            if (comments == null || !comments.Any())
            {
                return new List<CommentViewModel>();
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
