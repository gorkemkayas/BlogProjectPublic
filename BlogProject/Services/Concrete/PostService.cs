using AutoMapper;
using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using BlogProject.Services.DTOs;
using BlogProject.src.Infra.Context;
using BlogProject.src.Infra.Entitites;
using BlogProject.src.Infra.Entitites.PartialEntities;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlogProject.Services.Concrete
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _blogDbContext;
        private readonly IMapper _mapper;

        public PostService(BlogDbContext blogDbContext, IMapper mapper)
        {
            _blogDbContext = blogDbContext;
            _mapper = mapper;
        }

        public async Task<PostEntity> GetPostByIdAsync(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(postId));
            }
            return await _blogDbContext.Posts.FindAsync(postId) ?? throw new Exception("Belirtilen Id ile ilişkili post yok.");
        }

        public async Task<ICollection<PostEntity>> GetPostsByAuthorIdAsync(Guid authorId, bool isDescending)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            var author = await _blogDbContext.Users.FindAsync(authorId) ?? throw new Exception("Bu Id ile ilişkili 'author' bulunamadı.");

            var query = _blogDbContext.Posts.Where(p => p.AuthorId == author.Id);

            query = isDescending
                ? query.OrderByDescending(p => p.CreatedTime)
                : query.OrderBy(p => p.CreatedTime);

            return await query.ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByCategoryAsync(string categoryName, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException("Category ismi belirtilmedi.", nameof(categoryName));
            }

            var category = await _blogDbContext.Categories
                                               .FirstOrDefaultAsync(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category == null)
            {
                throw new KeyNotFoundException("Category bulunamadı.");
            }

            var query = _blogDbContext.Posts.Where(p => p.CategoryId == category.Id);

            query = isDescending
                ? query.OrderByDescending(p => p.CreatedTime)
                : query.OrderBy(p => p.CreatedTime);

            return await query.ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByCommentCountAsync(bool isDescending)
        {
            var query = isDescending
                ? _blogDbContext.Posts.OrderByDescending(p => p.CreatedTime)
                : _blogDbContext.Posts.OrderBy(p => p.CreatedTime);

            return await query.ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByLikeCountsAsync(bool isDescending)
        {
            var query = isDescending
                ? _blogDbContext.Posts.OrderByDescending(p => p.Likes.Count())
                : _blogDbContext.Posts.OrderBy(p => p.Likes.Count());

            return await query.ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByShareCountAsync(bool isDescending)
        {
            var query = isDescending
                ? _blogDbContext.Posts.OrderByDescending(p => p.Shares.Count())
                : _blogDbContext.Posts.OrderBy(p => p.Shares.Count());

            return await query.ToListAsync();
        }
        public async Task<ICollection<PostEntity>> GetPostsByTagIdAsync(Guid tagId, bool isDescending)
        {
            if (tagId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(tagId));
            }

            var query = _blogDbContext.Posts
                                .Where(p => p.TagPosts.Any(tp => tp.TagId == tagId))
                                .Include(p => p.TagPosts)
                                .AsQueryable();

            query = isDescending
                ? query.OrderByDescending(p => p.CreatedTime)
                : query.OrderBy(p => p.CreatedTime);

            return await query.ToListAsync();

        }

        public async Task<ICollection<PostEntity>> GetPostsByTitleAsync(string title, bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Girilen title bilgisi boş", nameof(title));
            }

            var query = _blogDbContext.Posts.Where(p => p.Title.Contains(title));

            query = isDescending
                ? query.OrderByDescending(p => p.CreatedTime)
                : query.OrderBy(p => p.CreatedTime);

            return await query.ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Başlangıç tarihi, bitiş tarihinden büyük olamaz.");
            }

            return await _blogDbContext.Posts
                .Where(p => p.CreatedTime >= startDate && p.CreatedTime <= endDate)
                .OrderByDescending(p => p.CreatedTime)
                .ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByDateTimeAsync(int year, int month)
        {
            if (year <= 0 || month < 1 || month > 12)
            {
                throw new ArgumentException("Geçersiz yıl veya ay bilgisi.");
            }

            return await _blogDbContext.Posts
                .Where(p => p.CreatedTime.Year == year && p.CreatedTime.Month == month)
                .OrderByDescending(p => p.CreatedTime)
                .ToListAsync();
        }

        public async Task<PaginationResult<PostEntity>> GetPostsWithPagination(int page, int pageSize, bool isDescending)
        {
            var totalPosts = await _blogDbContext.Posts.CountAsync();

            var query = _blogDbContext.Posts;

            var pagePosts = isDescending
                ? 
                query
                .OrderByDescending(p => p.CreatedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                : 
                query.OrderBy(p => p.CreatedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return new PaginationResult<PostEntity>
            {
                Posts = await pagePosts,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalPosts
            };
        }

        public async Task AddPostAsync(CreatePostDto createPostDto)
        {
            if (createPostDto == null)
            {
                throw new ArgumentNullException(nameof(createPostDto), "Post bilgileri boş bırakılamaz.");
            }

            try
            {
                var newPostEntity = _mapper.Map<PostEntity>(createPostDto);

                await _blogDbContext.Posts.AddAsync(newPostEntity);
                await _blogDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Veritabanına post eklenirken hata oluştu.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Post ekleme sırasında hata oluştu.", ex);
            }

        }

        public async Task UpdatePostAsync(Guid postId, UpdatePostDto updatePostDto)
        {
            if (updatePostDto == null)
            {
                throw new ArgumentNullException(nameof(updatePostDto), "Güncelleme bilgileri boş olamaz.");
            }

            var existingPost = await _blogDbContext.Posts.FindAsync(postId) ?? throw new Exception("Güncellenmek istenen post bulunamadı.");

            try
            {
                _mapper.Map(updatePostDto, existingPost);
                await _blogDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                throw new Exception("Post Güncellenirken veritabanında hata oluştu", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Post güncellenirken hata oluştu.",ex);
            }

        }
        public async Task<UpdatePostDto> GetPostInfoForUpdate(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(postId), "Update sayfasına gönderilecek postId bilgisi boş.");
            }

            var post = await _blogDbContext.Posts.FindAsync(postId) ?? throw new KeyNotFoundException("Veritabanında bu postId ile ilişkili post yok");

            return _mapper.Map<UpdatePostDto>(post);
        }

        public async Task SoftDeletePostAsync(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(postId), "Update sayfasına gönderilecek postId bilgisi boş.");
            }
            var post = await _blogDbContext.Posts.FindAsync(postId) ?? throw new KeyNotFoundException("Veritabanında bu postId ile ilişkili post yok");

            post.IsDeleted = true;

            _blogDbContext.Update(post);

            await _blogDbContext.SaveChangesAsync();
        }

        public async Task<ServiceResult<object>> CreatePostAsync(CreatePostViewModel model)
        {
            if (model == null)
            {
                new ServiceResult<object>
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError { Code = "ModelNull", Description = "Post bilgileri boş bırakılamaz." } }
                };
            }
            try
            {
                var newPostEntity = _mapper.Map<PostEntity>(model);
                await _blogDbContext.Posts.AddAsync(newPostEntity);
                await _blogDbContext.SaveChangesAsync();

                var result = new ServiceResult<object>
                {
                    IsSuccess = true
                };

                return result;
            }
            catch (DbUpdateException ex)
            {
                var result = new ServiceResult<object>
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError { Code = "DbUpdateError", Description = "Veritabanına post eklenirken hata oluştu." } }
                };
                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResult<object>
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError { Code = "PostCreationError", Description = "Post ekleme sırasında hata oluştu." } }
                };
                return result;
            }
        }
    }
}
