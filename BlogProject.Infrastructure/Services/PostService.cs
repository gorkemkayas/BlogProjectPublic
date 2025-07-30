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
        private readonly IDbContextFactory<BlogDbContext> _contextFactory;

        public PostService(BlogDbContext blogDbContext, IDbContextFactory<BlogDbContext> contextFactory)
        {
            _blogDbContext = blogDbContext;
            _contextFactory = contextFactory;
        }

        public async Task<List<PostListItemDto>> GetPostByTagIdAsync(string tagId)
        {
            if (string.IsNullOrWhiteSpace(tagId))
                throw new ArgumentException("TagId boş olamaz.", nameof(tagId));

            var tagGuid = Guid.Parse(tagId);

            var tagExists = await _blogDbContext.Tags
                .AsNoTracking()
                .AnyAsync(t => t.Id == tagGuid);

            if (!tagExists)
                throw new KeyNotFoundException("Belirtilen Id ile ilişkili tag bulunamadı.");

            var posts = await _blogDbContext.Posts
                .AsNoTracking()
                .Where(p => p.TagPosts.Any(tp => tp.TagId == tagGuid) && !p.IsDeleted)
                .Select(p => new PostListItemDto
                {
                    Id = p.Id,
                    AuthorId = p.AuthorId,
                    CoverImageUrl = p.CoverImageUrl,
                    Title = p.Title,
                    Subtitle = p.Subtitle,
                    CreatedTime = p.CreatedTime,
                    ViewCount = p.ViewCount
                })
                .ToListAsync();

            return posts;
        }

        public async Task<List<PostListItemDto>> GetByCategoryIdAsync(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                throw new ArgumentException("CategoryId boş olamaz.", nameof(categoryId));

            var categoryGuid = Guid.Parse(categoryId);

            var categoryExists = await _blogDbContext.Categories
                .AsNoTracking()
                .AnyAsync(c => c.Id == categoryGuid);

            if (!categoryExists)
                throw new KeyNotFoundException("Belirtilen Id ile ilişkili kategori bulunamadı.");

            return await _blogDbContext.Posts
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryGuid && !p.IsDeleted)
                .Select(p => new PostListItemDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Subtitle = p.Subtitle,
                    CoverImageUrl = p.CoverImageUrl,
                    CreatedTime = p.CreatedTime,
                    ViewCount = p.ViewCount,
                    AuthorId = p.AuthorId,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name
                })
                .ToListAsync();
        }

        public async Task<bool> IsPostLikedByCurrentUserAsync(string userId, string postId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(postId))
            {
                throw new ArgumentException("UserId veya PostId boş olamaz.");
            }
            var isLiked = await _blogDbContext.Likes
                                              .AsNoTracking()
                                              .AnyAsync(p => p.PostId == Guid.Parse(postId) && p.UserId == Guid.Parse(userId));
            return isLiked;
        }

        public async Task<int> GetPostCountByUserAsync(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Kullanıcı bilgisi boş olamaz.");
            }
            return await _blogDbContext.Posts.CountAsync(p => p.AuthorId == user.Id && !p.IsDeleted);
        }
        public async Task<PostDetailsDto> GetPostByIdAsync(Guid postId, bool updateReadCount = false)
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
            var postDto = await _blogDbContext.Posts
                                            .Where(p => p.Id == postId && !p.IsDeleted)
                                            .AsNoTracking()
                                            .Select(p => new PostDetailsDto
                                            {
                                                Id = p.Id,
                                                Title = p.Title,
                                                Content = p.Content,
                                                CoverImageUrl = p.CoverImageUrl,
                                                CreatedTime = p.CreatedTime,
                                                ViewCount = p.ViewCount,
                                                LikeCount = p.Likes.Count,
                                                CategoryName = p.Category.Name,
                                                Author = new AuthorInfoDto
                                                {
                                                    FullName = p.Author.FullName,
                                                    UserName = p.Author.UserName,
                                                    ProfilePicture = p.Author.ProfilePicture,
                                                    Title = p.Author.Title,
                                                    CurrentPosition = p.Author.CurrentPosition,
                                                    UniversityName = p.Author.UniversityName,
                                                    XAddress = p.Author.XAddress,
                                                    LinkedinAddress = p.Author.LinkedinAddress,
                                                    GithubAddress = p.Author.GithubAddress,
                                                    MediumAddress = p.Author.MediumAddress,
                                                    YoutubeAddress = p.Author.YoutubeAddress,
                                                    PersonalWebAddress = p.Author.PersonalWebAddress
                                                }
                                            })
                                            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Post bulunamadı.");

            return postDto;
            //return await _blogDbContext.Posts
            //    .Include(a => a.Author)
            //    .Include(c => c.Category)
            //    .Include(l => l.Likes)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(p => p.Id == postId) ?? throw new Exception("Belirtilen Id ile ilişkili post yok.");
        }

        public async Task<ICollection<PostEntity>> GetPostsByAuthorIdAsync(Guid authorId, bool isDescending)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            var exists = await _blogDbContext.Users.AsNoTracking().AnyAsync(u => u.Id == authorId);
            if (!exists)
                throw new Exception("Bu Id ile ilişkili author bulunamadı.");

            var query = _blogDbContext.Posts.Where(p => p.AuthorId == authorId);

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

            var categoryId = await _blogDbContext.Categories.AsNoTracking().Where(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).Select(c => c.Id).FirstOrDefaultAsync();

            if (categoryId == Guid.Empty)
            {
                throw new KeyNotFoundException("Category bulunamadı.");
            }

            var query = _blogDbContext.Posts.Where(p => p.CategoryId == categoryId).AsNoTracking();

            if (additionalFilter != null)
            {
                query = query.Where(additionalFilter);
            }

            query = isDescending
                ? query.OrderByDescending(p => p.CreatedTime)
                : query.OrderBy(p => p.CreatedTime);

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<ICollection<PostWithCategoryNameDto>> GetMostViewedPostsByCategoryAsync(string categoryName, bool isDescending, Expression<Func<PostEntity, bool>>? additionalFilter = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException("Category ismi belirtilmedi.", nameof(categoryName));
            }

            var categoryId = await _blogDbContext.Categories
                .AsNoTracking()
                .Where(c => c.Name.ToLower() == categoryName.ToLower())
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (categoryId == Guid.Empty)
            {
                throw new KeyNotFoundException("Category bulunamadı.");
            }

            var query = _blogDbContext.Posts
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted);

            if (additionalFilter is not null)
            {
                query = query.Where(additionalFilter);
            }

            query = isDescending
                ? query.OrderByDescending(p => p.ViewCount)
                : query.OrderBy(p => p.ViewCount);

            return await query
                .Select(p => new PostWithCategoryNameDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Subtitle = p.Subtitle,
                    ViewCount = p.ViewCount,
                    CreatedTime = p.CreatedTime,
                    CoverImageUrl = p.CoverImageUrl,
                    CategoryName = p.Category.Name
                })
                .ToListAsync();
        }

        public async Task<ICollection<PostWithCategoryNameDto>> GetMostViewedPostsByCategoryIdAsync(string categoryId, bool isDescending, Expression<Func<PostEntity, bool>>? additionalFilter = null)
        {
            if (!Guid.TryParse(categoryId, out var parsedCategoryId))
            {
                throw new ArgumentException("Geçersiz kategori ID.");
            }

            bool categoryExists = await _blogDbContext.Categories
                                                      .AsNoTracking()
                                                      .AnyAsync(c => c.Id == parsedCategoryId);

            if (!categoryExists)
            {
                throw new KeyNotFoundException("Kategori bulunamadı.");
            }

            var query = _blogDbContext.Posts
                .Where(p => p.CategoryId == parsedCategoryId && !p.IsDeleted);

            if (additionalFilter != null)
            {
                query = query.Where(additionalFilter);
            }

            query = isDescending
                ? query.OrderByDescending(p => p.ViewCount)
                : query.OrderBy(p => p.ViewCount);

            return await query
                .Select(p => new PostWithCategoryNameDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Subtitle = p.Subtitle,
                    ViewCount = p.ViewCount,
                    CreatedTime = p.CreatedTime,
                    CoverImageUrl = p.CoverImageUrl,
                    CategoryName = p.Category.Name
                })
                .AsNoTracking()
                .ToListAsync();
        }


        //bundan sonrası kaldı.
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
                if (categoryId is not null)
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
        public async Task<ICollection<PostDto>> GetCategorizedPostsByLikeCountsAsync(bool isDescending, string categoryId)
        {
            var query = _blogDbContext.Posts
                .AsNoTracking()
                .Where(p => p.CategoryId == Guid.Parse(categoryId));

            query = isDescending
                ? query.OrderByDescending(p => p.Likes.Count())
                : query.OrderBy(p => p.Likes.Count());

            return await query
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Subtitle = p.Subtitle,
                    LikeCount = p.Likes.Count(),
                    CategoryName = p.Category.Name,
                    CreatedTime = p.CreatedTime,
                    ViewCount = p.ViewCount,
                    CoverImageUrl = p.CoverImageUrl
                })
                .Take(3)
                .ToListAsync();
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

        //public async Task<ServiceResult<object>> CreatePostAsync(CreatePostDto model)
        //{
        //    if (model == null)
        //    {
        //        return new ServiceResult<object>
        //        {
        //            IsSuccess = false,
        //            Errors = new List<IdentityError> { new IdentityError { Code = "ModelNull", Description = "Post bilgileri boş bırakılamaz." } }
        //        };
        //    }
        //    try
        //    {

        //        var newPostEntity = new PostEntity()
        //        {
        //            AuthorId = Guid.Parse(model!.AuthorId),
        //            CategoryId = Guid.Parse(model.CategoryId),
        //            Title = model.Title,
        //            Subtitle = model.Subtitle,
        //            Content = model.Content,
        //            SubContent = model.SubContent,
        //            IsDraft = model.IsDraft,
        //            IsDeleted = false,
        //            ViewCount = 0,
        //            Likes = new List<LikeEntity>(),
        //            Shares = new List<ShareEntity>(),

        //        };
        //        var author = await _blogDbContext.Users.FindAsync(Guid.Parse(model.AuthorId));
        //        var category = await _blogDbContext.Categories.FindAsync(Guid.Parse(model.CategoryId));

        //        newPostEntity.Author = author!;
        //        newPostEntity.Category = category!;
        //        newPostEntity.CreatedTime = DateTime.Now;

        //        newPostEntity.CoverImageUrl = await ImageSaver.SaveUserImageAsync(model.CoverImage, model.Title);

        //        var entity = await _blogDbContext.Posts.AddAsync(newPostEntity);
        //        await _blogDbContext.SaveChangesAsync();

        //        var addedPost = entity.Entity;
        //        //var addedPost = _blogDbContext.Posts.Where(p => p.Id == entity.Entity.Id).FirstOrDefault();
        //        if (addedPost is null)
        //            throw new Exception("Post eklenirken bir hata oluştu.");

        //        List<PostTagEntity> postTags = new List<PostTagEntity>();
        //        if (model.TagIds != null)
        //        {
        //            foreach (var tagId in model.TagIds)
        //            {
        //                postTags.Add(new PostTagEntity() { TagId = Guid.Parse(tagId), PostId = addedPost.Id, AssignedDate = DateTime.Now });
        //            }

        //            if (postTags.Count > 0)
        //            {
        //                await _blogDbContext.PostTags.AddRangeAsync(postTags);
        //                addedPost.TagPosts = postTags;
        //                addedPost.Author = author!;
        //                addedPost.Category = category!;
        //                addedPost.CreatedTime = DateTime.Now;
        //                await _blogDbContext.SaveChangesAsync();
        //            }
        //        }
        //        else
        //        {
        //            addedPost.TagPosts = new List<PostTagEntity>();
        //            addedPost.Author = author!;
        //            addedPost.Category = category!;
        //            addedPost.CreatedTime = DateTime.Now;
        //            await _blogDbContext.SaveChangesAsync();
        //        }

        //        var result = new ServiceResult<object>
        //        {
        //            IsSuccess = true
        //        };

        //        return result;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        var result = new ServiceResult<object>
        //        {
        //            IsSuccess = false,
        //            Errors = new List<IdentityError> { new IdentityError { Code = "DbUpdateError", Description = "Veritabanına post eklenirken hata oluştu." } }
        //        };
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        var result = new ServiceResult<object>
        //        {
        //            IsSuccess = false,
        //            Errors = new List<IdentityError> { new IdentityError { Code = "PostCreationError", Description = "Post ekleme sırasında hata oluştu." } }
        //        };
        //        return result;
        //    }
        //}
        public async Task<ServiceResult<object>> CreatePostAsync(CreatePostDto model)
        {
            if (model == null)
            {
                return new ServiceResult<object>
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "ModelNull",
                    Description = "Post bilgileri boş bırakılamaz."
                }
            }
                };
            }

            try
            {
                var authorId = Guid.Parse(model.AuthorId);
                var categoryId = Guid.Parse(model.CategoryId);

                var author = await _blogDbContext.Users.FindAsync(authorId);
                var category = await _blogDbContext.Categories.FindAsync(categoryId);

                if (author == null || category == null)
                {
                    return new ServiceResult<object>
                    {
                        IsSuccess = false,
                        Errors = new List<IdentityError>
                {
                    new IdentityError { Code = "NotFound", Description = "Yazar veya kategori bulunamadı." }
                }
                    };
                }

                var newPost = new PostEntity
                {
                    AuthorId = authorId,
                    CategoryId = categoryId,
                    Title = model.Title,
                    Subtitle = model.Subtitle,
                    Content = model.Content,
                    SubContent = model.SubContent,
                    IsDraft = model.IsDraft,
                    IsDeleted = false,
                    ViewCount = 0,
                    Likes = new List<LikeEntity>(),
                    Shares = new List<ShareEntity>(),
                    CreatedTime = DateTime.Now,
                    CoverImageUrl = await ImageSaver.SaveUserImageAsync(model.CoverImage, model.Title)
                };

                var result = await _blogDbContext.Posts.AddAsync(newPost);
                await _blogDbContext.SaveChangesAsync();

                // Tag'ler ekleniyorsa işle
                if (model.TagIds != null && model.TagIds.Any())
                {
                    var postTags = model.TagIds
                        .Select(tagId => new PostTagEntity
                        {
                            TagId = Guid.Parse(tagId),
                            PostId = result.Entity.Id,
                            AssignedDate = DateTime.Now
                        }).ToList();

                    await _blogDbContext.PostTags.AddRangeAsync(postTags);
                    await _blogDbContext.SaveChangesAsync();
                }

                return new ServiceResult<object>
                {
                    IsSuccess = true
                };
            }
            catch (DbUpdateException)
            {
                return new ServiceResult<object>
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>
            {
                new IdentityError { Code = "DbUpdateError", Description = "Veritabanına post eklenirken hata oluştu." }
            }
                };
            }
            catch (Exception)
            {
                return new ServiceResult<object>
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>
            {
                new IdentityError { Code = "PostCreationError", Description = "Post ekleme sırasında hata oluştu." }
            }
                };
            }
        }

        public async Task<ICollection<PostDto>> GetLatestPostsWithCount(int count = 3)
        {
            if (count <= 0)
                throw new ArgumentException("Count değeri 0'dan büyük olmalıdır.", nameof(count));

            using var context = _contextFactory.CreateDbContext();

            var postsWithCounts = await context.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedTime)
                .Take(count)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Content,
                    LikeCount = p.Likes.Count(),
                    CategoryName = p.Category.Name,
                    p.CreatedTime,
                    p.Subtitle,
                    p.CoverImageUrl
                })
                .ToListAsync();

            return postsWithCounts.Select(x => new PostDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                LikeCount = x.LikeCount,
                CategoryName = x.CategoryName,
                CreatedTime = x.CreatedTime,
                Subtitle = x.Subtitle,
                CoverImageUrl = x.CoverImageUrl
            }).ToList();
        }


        public async Task<ICollection<PostDto>> GetMostViewedPostsWithCount(int count = 3, bool currentWeek = false)
        {
            if (count <= 0)
                throw new ArgumentException("Count değeri 0'dan büyük olmalıdır.", nameof(count));

            using var context = _contextFactory.CreateDbContext();
            var query = context.Posts.AsNoTracking();

            if (currentWeek)
            {
                var startsFrom = DateTime.UtcNow.Date.AddDays(-7);
                query = query.Where(p => p.CreatedTime >= startsFrom);
            }

            var postsWithCounts = await query
                .OrderByDescending(p => p.ViewCount)
                .ThenByDescending(p => p.CreatedTime)
                .Take(count)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Subtitle,
                    p.CoverImageUrl,
                    p.Content,
                    LikeCount = p.Likes.Count(),
                    CategoryName = p.Category.Name,
                    p.CreatedTime,
                    p.ViewCount
                })
                .ToListAsync();

            return postsWithCounts.Select(x => new PostDto
            {
                Id = x.Id,
                Title = x.Title,
                Subtitle = x.Subtitle,
                CoverImageUrl = x.CoverImageUrl,
                Content = x.Content,
                LikeCount = x.LikeCount,
                CategoryName = x.CategoryName,
                CreatedTime = x.CreatedTime,
                ViewCount = x.ViewCount
            }).ToList();
        }



    }
}
