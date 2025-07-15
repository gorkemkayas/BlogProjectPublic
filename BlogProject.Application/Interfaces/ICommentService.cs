using BlogProject.Application.Common;
using BlogProject.Domain.Entities;

namespace BlogProject.Application.Interfaces
{
    public interface ICommentService
    {
        Task<int> GetCommentCountByUserAsync(AppUser user);
        Task<List<CommentEntity>> GetCommentsByPostIdAsync(string postId);
        Task<ServiceResult<CommentEntity>> AddCommentAsync(CommentEntity entity);
    }
}
