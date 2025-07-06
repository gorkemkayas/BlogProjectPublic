using BlogProject.src.Infra.Entitites;

namespace BlogProject.Services.Abstract
{
    public interface ICommentService
    {
        Task<int> GetCommentCountByUserAsync(AppUser user);
        Task<List<CommentEntity>> GetCommentsByPostIdAsync(string postId);
    }
}
