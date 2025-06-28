using BlogProject.Models.ViewModels;
using BlogProject.Services.DTOs;
using BlogProject.src.Infra.Entitites;
using BlogProject.src.Infra.Entitites.PartialEntities;
using BlogProject.Utilities;

namespace BlogProject.Services.Abstract
{
    public interface IPostService
    {

        // post silme( buna ayrı önem ver, soft delete olayı var).


        // CreatePostDto ile UpdatePostDto ları baştan yapıcam şuanda boş, hata vermesin diye.
        public Task AddPostAsync(CreatePostDto createPostDto);
        public Task UpdatePostAsync(Guid postId, UpdatePostDto updatePostDto);

        Task<ServiceResult<object>> CreatePostAsync(CreatePostViewModel model);

        public Task SoftDeletePostAsync(Guid postId);
        public Task<UpdatePostDto> GetPostInfoForUpdate(Guid postId);
        public Task<PostEntity> GetPostByIdAsync(Guid postId);
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
