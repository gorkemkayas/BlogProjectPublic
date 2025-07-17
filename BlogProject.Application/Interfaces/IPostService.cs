using BlogProject.Application.Common;
using BlogProject.Application.DTOs;
using BlogProject.Domain.Entities;

namespace BlogProject.Application.Interfaces
{
    public interface IPostService
    {

        // post silme( buna ayrı önem ver, soft delete olayı var).


        // CreatePostDto ile UpdatePostDto ları baştan yapıcam şuanda boş, hata vermesin diye.
        Task<List<PostEntity>> GetByCategoryIdAsync(string categoryId);
        Task<ICollection<PostEntity>> GetLatestPostsWithCount(int count = 3);
        Task<ICollection<PostEntity>> GetMostViewedPostsWithCount(int count = 3, bool currentWeek = false);
        Task<ServiceResult<object>> CreatePostAsync(CreatePostDto model);
        Task<int> GetPostCountByUserAsync(AppUser user);
        Task<bool> IsPostLikedByCurrentUserAsync(string userId, string PostId);
        public Task SoftDeletePostAsync(Guid postId);
        public Task<PostEntity> GetPostByIdAsync(Guid postId, bool updateReadCount = false);
        public Task<ICollection<PostEntity>> GetPostsByCategoryAsync(string categoryName, bool isDescending);
        public Task<ICollection<PostEntity>> GetPostsByTitleAsync(string title, bool isDescending);
        public Task<ICollection<PostEntity>> GetPostsByAuthorIdAsync(Guid AuthorId, bool isDescending);
        public Task<ICollection<PostEntity>> GetPostsByTagIdAsync(Guid tagId, bool isDescending);

        public Task<ICollection<PostEntity>> GetPostsByLikeCountsAsync(bool isDescending);
        public Task<ICollection<PostEntity>> GetPostsByShareCountAsync(bool isDescending);
        public Task<ICollection<PostEntity>> GetPostsByCommentCountAsync(bool isDescending);
        public Task<ICollection<PostEntity>> GetPostsByDateTimeAsync(int year, int month);
        public Task<ICollection<PostEntity>> GetPostsByDateRangeAsync(DateTime startDate, DateTime endDate);
        public Task<PaginationResult<PostEntity>> GetPostsWithPagination(int page, int pageSize, bool isDescending);

    }
}
