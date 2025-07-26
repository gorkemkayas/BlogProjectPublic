using BlogProject.Application.Common;
using BlogProject.Application.DTOs;
using BlogProject.Application.Interfaces;
using BlogProject.Domain.Entities;
using BlogProject.Infrastructure.CustomMethods;
using BlogProject.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq.Expressions;

namespace BlogProject.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _blogDbContext;

        public PostService(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<List<PostEntity>> GetPostByTagIdAsync(string tagId)
        {
            if (string.IsNullOrWhiteSpace(tagId))
            {
                throw new ArgumentException("TagId boş olamaz.", nameof(tagId));
            }
            var tag = await _blogDbContext.Tags
                                          .FirstOrDefaultAsync(t => t.Id == Guid.Parse(tagId));
            if (tag == null)
            {
                throw new KeyNotFoundException("Belirtilen Id ile ilişkili tag bulunamadı.");
            }
            var posts = await _blogDbContext.Posts
                                       .Where(p => p.TagPosts.Any(tp => tp.TagId == tag.Id) && !p.IsDeleted)
                                       .Include(p => p.Author)
                                       .AsNoTracking()
                                       .ToListAsync();

            if (posts == null)
            {
                return new List<PostEntity>();
            }

            return posts;
        }
        public async Task<List<PostEntity>> GetByCategoryIdAsync(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                throw new ArgumentException("CategoryId boş olamaz.", nameof(categoryId));
            }
            var category = await _blogDbContext.Categories.AsNoTracking()
                                               .FirstOrDefaultAsync(c => c.Id == Guid.Parse(categoryId));
            if (category == null)
            {
                throw new KeyNotFoundException("Belirtilen Id ile ilişkili kategori bulunamadı.");
            }
            return await _blogDbContext.Posts
                                       .Where(p => p.CategoryId == category.Id && !p.IsDeleted)
                                       .Include(p => p.Author)
                                       .AsNoTracking()
                                       .ToListAsync();
        }
        public async Task<bool> IsPostLikedByCurrentUserAsync(string userId, string postId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(postId))
            {
                throw new ArgumentException("UserId veya PostId boş olamaz.");
            }
            var likeCheck = await _blogDbContext.Likes.Where(p => p.PostId == Guid.Parse(postId) && p.UserId == Guid.Parse(userId)).AsNoTracking().FirstOrDefaultAsync();
            if (likeCheck == null)
            {
                return false;
            }
            return true;
        }

        public async Task<int> GetPostCountByUserAsync(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Kullanıcı bilgisi boş olamaz.");
            }
            return await _blogDbContext.Posts.CountAsync(p => p.AuthorId == user.Id && !p.IsDeleted);
        }
        public async Task<PostEntity> GetPostByIdAsync(Guid postId, bool updateReadCount = false)
        {
            if (postId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(postId));
            }
            if (updateReadCount)
            {
                var post = await _blogDbContext.Posts.FindAsync(postId) ?? throw new KeyNotFoundException("Belirtilen Id ile ilişkili post bulunamadı.");
                post.ViewCount++;
                _blogDbContext.Update(post);
                await _blogDbContext.SaveChangesAsync();
            }
            return await _blogDbContext.Posts.Include(a => a.Author).Include(c => c.Category).AsNoTracking().FirstOrDefaultAsync(p => p.Id == postId) ?? throw new Exception("Belirtilen Id ile ilişkili post yok.");
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

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByCategoryAsync(string categoryName, bool isDescending, Expression<Func<PostEntity, bool>>? additionalFilter = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException("Category ismi belirtilmedi.", nameof(categoryName));
            }

            var category = await _blogDbContext.Categories.AsNoTracking()
                                               .FirstOrDefaultAsync(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category == null)
            {
                throw new KeyNotFoundException("Category bulunamadı.");
            }

            var query = _blogDbContext.Posts.Where(p => p.CategoryId == category.Id).AsNoTracking();

            if (additionalFilter != null)
            {
                query = query.Where(additionalFilter);
            }

            query = isDescending
                ? query.OrderByDescending(p => p.CreatedTime)
                : query.OrderBy(p => p.CreatedTime);

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<ICollection<PostEntity>> GetMostViewedPostsByCategoryAsync(string categoryName, bool isDescending, Expression<Func<PostEntity, bool>>? additionalFilter = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException("Category ismi belirtilmedi.", nameof(categoryName));
            }

            var category = await _blogDbContext.Categories.AsNoTracking()
                                               .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

            if (category == null)
            {
                throw new KeyNotFoundException("Category bulunamadı.");
            }

            var query = _blogDbContext.Posts.Where(p => p.CategoryId == category.Id).AsNoTracking();

            if (additionalFilter != null)
            {
                query = query.Where(additionalFilter);
            }

            query = isDescending
                ? query.OrderByDescending(p => p.ViewCount)
                : query.OrderBy(p => p.ViewCount);

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<ICollection<PostEntity>> GetMostViewedPostsByCategoryIdAsync(string categoryId, bool isDescending, Expression<Func<PostEntity, bool>>? additionalFilter = null)
        {

            var category = await _blogDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Guid.Parse(categoryId));

            if (category == null)
            {
                throw new KeyNotFoundException("Category bulunamadı.");
            }

            var query = _blogDbContext.Posts.Where(p => p.CategoryId == category.Id).AsNoTracking();

            if (additionalFilter != null)
            {
                query = query.Where(additionalFilter);
            }

            query = isDescending
                ? query.OrderByDescending(p => p.ViewCount)
                : query.OrderBy(p => p.ViewCount);

            return await query.Include(c => c.Category).AsNoTracking().ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByCommentCountAsync(bool isDescending)
        {
            var query = isDescending
                ? _blogDbContext.Posts.AsNoTracking().OrderByDescending(p => p.CreatedTime)
                : _blogDbContext.Posts.AsNoTracking().OrderBy(p => p.CreatedTime);

            return await query.ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByLikeCountsAsync(bool isDescending)
        {
            var query = isDescending
                ? _blogDbContext.Posts.AsNoTracking().OrderByDescending(p => p.Likes.Count())
                : _blogDbContext.Posts.AsNoTracking().OrderBy(p => p.Likes.Count());

            return await query.ToListAsync();
        }

        public async Task<List<PostEntity>> LoadMoreMostViewedPostScrollPosts(int page, int pageSize, string? categoryId)
        {
            try
            {
                var query = _blogDbContext.Posts.AsNoTracking().AsQueryable();
                if(categoryId is not null)
                {
                    query = query.Where(p => p.CategoryId == Guid.Parse(categoryId) && !p.IsDeleted);
                }
                else
                {
                    query = query.Where(p => !p.IsDeleted);
                }
                var posts = await query
                .OrderByDescending(p => p.ViewCount)
                .Include(p => p.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (posts == null || !posts.Any())
            {
                return new List<PostEntity>();
            }

            return posts;
            }
            catch (Exception ex)
            {

                throw new Exception("Hata mesajı:" + ex.Message);
            }
        }
        public async Task<List<PostEntity>> LoadMoreMostLikedPostScrollPosts(int page, int pageSize, string? categoryId)
        {
            try
            {
                var query = _blogDbContext.Posts.AsNoTracking().AsQueryable();
                if (categoryId is not null)
                {
                    query = query.Where(p => p.CategoryId == Guid.Parse(categoryId) && !p.IsDeleted);
                }
                else
                {
                    query = query.Where(p => !p.IsDeleted);
                }
                var posts = await query
                .OrderByDescending(p => p.Likes.Count)
                .Include(p => p.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                if (posts == null || !posts.Any())
                {
                    return new List<PostEntity>();
                }

                return posts;
            }
            catch (Exception ex)
            {

                throw new Exception("Hata mesajı:" + ex.Message);
            }
        }
        public async Task<ICollection<PostEntity>> GetCategorizedPostsByLikeCountsAsync(bool isDescending, string categoryId)
        {
            var query = isDescending
                ? _blogDbContext.Posts.AsNoTracking().Where(p => p.CategoryId == Guid.Parse(categoryId)).OrderByDescending(p => p.Likes.Count())
                : _blogDbContext.Posts.AsNoTracking().Where(p => p.CategoryId == Guid.Parse(categoryId)).OrderBy(p => p.Likes.Count());

            return await query.Take(3).ToListAsync();
        }

        public async Task<ICollection<PostEntity>> GetPostsByShareCountAsync(bool isDescending)
        {
            var query = isDescending
                ? _blogDbContext.Posts.AsNoTracking().OrderByDescending(p => p.Shares.Count())
                : _blogDbContext.Posts.AsNoTracking().OrderBy(p => p.Shares.Count());

            return await query.ToListAsync();
        }
        public async Task<ICollection<PostEntity>> GetPostsByTagIdAsync(Guid tagId, bool isDescending)
        {
            if (tagId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(tagId));
            }

            var query = _blogDbContext.Posts.AsNoTracking()
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

            var query = _blogDbContext.Posts.AsNoTracking().Where(p => p.Title.Contains(title));

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

            return await _blogDbContext.Posts.AsNoTracking()
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

            return await _blogDbContext.Posts.AsNoTracking()
                .Where(p => p.CreatedTime.Year == year && p.CreatedTime.Month == month)
                .OrderByDescending(p => p.CreatedTime)
                .ToListAsync();
        }

        public async Task<PaginationResult<PostEntity>> GetPostsWithPagination(int page, int pageSize, bool isDescending)
        {
            var totalPosts = await _blogDbContext.Posts.AsNoTracking().CountAsync();

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

        public async Task<ServiceResult<object>> CreatePostAsync(CreatePostDto model)
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

                var newPostEntity = new PostEntity()
                {
                    AuthorId = Guid.Parse(model!.AuthorId),
                    CategoryId = Guid.Parse(model.CategoryId),
                    Title = model.Title,
                    Subtitle = model.Subtitle,
                    Content = model.Content,
                    SubContent = model.SubContent,
                    IsDraft = model.IsDraft,
                    IsDeleted = false,
                    ViewCount = 0,
                    Likes = new List<LikeEntity>(),
                    Shares = new List<ShareEntity>(),

                };
                var author = await _blogDbContext.Users.FindAsync(Guid.Parse(model.AuthorId));
                var category = await _blogDbContext.Categories.FindAsync(Guid.Parse(model.CategoryId));

                newPostEntity.Author = author!;
                newPostEntity.Category = category!;
                newPostEntity.CreatedTime = DateTime.Now;

                newPostEntity.CoverImageUrl = await ImageSaver.SaveUserImageAsync(model.CoverImage, model.Title);

                var entity = await _blogDbContext.Posts.AddAsync(newPostEntity);
                await _blogDbContext.SaveChangesAsync();

                var addedPost = _blogDbContext.Posts.Where(p => p.Id == entity.Entity.Id).FirstOrDefault();
                if (addedPost is null)
                    throw new Exception("Post eklenirken bir hata oluştu.");

                List<PostTagEntity> postTags = new List<PostTagEntity>();
                if (model.TagIds != null)
                {
                    foreach (var tagId in model.TagIds)
                    {
                        postTags.Add(new PostTagEntity() { TagId = Guid.Parse(tagId), PostId = addedPost.Id, AssignedDate = DateTime.Now });
                    }

                    if (postTags.Count > 0)
                    {
                        await _blogDbContext.PostTags.AddRangeAsync(postTags);
                        addedPost.TagPosts = postTags;
                        addedPost.Author = author!;
                        addedPost.Category = category!;
                        addedPost.CreatedTime = DateTime.Now;
                        await _blogDbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    addedPost.TagPosts = new List<PostTagEntity>();
                    addedPost.Author = author!;
                    addedPost.Category = category!;
                    addedPost.CreatedTime = DateTime.Now;
                    await _blogDbContext.SaveChangesAsync();
                }

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

        public async Task<ICollection<PostEntity>> GetLatestPostsWithCount(int count = 3)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count değeri 0'dan büyük olmalıdır.", nameof(count));
            }
            return await _blogDbContext.Posts.AsNoTracking()
                .OrderByDescending(p => p.CreatedTime)
                .Take(count)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ICollection<PostEntity>> GetMostViewedPostsWithCount(int count = 3, bool currentWeek = false)
        {
            if (count <= 0)
                throw new ArgumentException("Count değeri 0'dan büyük olmalıdır.", nameof(count));

            var query = _blogDbContext.Posts.AsNoTracking();

            if (currentWeek)
            {
                var startsFrom = DateTime.UtcNow.Date.AddDays(-7);
                query = query.Where(p => p.CreatedTime >= startsFrom);
            }

            return await query
                .OrderByDescending(p => p.ViewCount)
                .ThenByDescending(p => p.CreatedTime)
                .Include(p => p.Category)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
