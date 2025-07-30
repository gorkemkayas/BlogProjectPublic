using BlogProject.Application.Common;
using BlogProject.Application.DTOs;
using BlogProject.Domain.Entities;

namespace BlogProject.Application.Interfaces
{
    public interface ICommentService
    {
        Task<int> GetCommentCountByUserAsync(AppUser user);
        Task<List<CommentViewModel>> GetCommentsByPostIdAsync(string postId);
        Task<ServiceResult<CommentEntity>> AddCommentAsync(CommentEntity entity);
    }
}
