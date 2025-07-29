using BlogProject.Application.Common;
using BlogProject.Application.DTOs;
using BlogProject.Application.Enums;
using BlogProject.Application.Interfaces;
using BlogProject.Application.Models;
using BlogProject.Domain.Entities;
using BlogProject.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly BlogDbContext _context;
        private readonly IDbContextFactory<BlogDbContext> _contextFactory;
        public TagService(BlogDbContext context, IDbContextFactory<BlogDbContext> contextFactory)
        {
            _context = context;
            _contextFactory = contextFactory;
        }

        public async Task<object> GetDailyPostCountsAsync(string tagId)
        {
            var today = DateTime.Today;
            var startDate = today.AddDays(-29); // Son 30 gün

            // 30 günlük sabit tarih listesi (gün bazlı)
            var dateRange = Enumerable.Range(0, 30)
                .Select(offset => startDate.AddDays(offset))
                .ToList();

            // İlgili tagdeki son 30 gün içindeki gönderileri gün bazında gruplama
            var postCounts = await _context.Posts.AsNoTracking()
                                           .Where(p => p.TagPosts.Any(tp => tp.TagId == Guid.Parse(tagId)) &&
                                           p.CreatedTime >= startDate &&
                                           p.CreatedTime <= today.AddDays(1) &&
                                           !p.IsDeleted)
                                           .GroupBy(p => p.CreatedTime.Date)
                                           .Select(g => new
                                           {
                                               Date = g.Key,
                                               Count = g.Count()
                                           })
                                           .ToListAsync();


            // Her gün için veri oluştur, gönderi olmayan günlere 0 ata
            var chartData = dateRange.Select(date => new
            {
                Date = date.ToString("yyyy-MM-dd"),
                Count = postCounts.FirstOrDefault(x => x.Date == date)?.Count ?? 0
            }).ToList();

            // Chart.js ile uyumlu JSON yapısı
            var result = new
            {
                labels = chartData.Select(x => x.Date),
                datasets = new[]
                {
            new {
                label = "Daily post counts",
                data = chartData.Select(x => x.Count),
                backgroundColor = "rgba(75, 192, 192, 0.2)",
                borderColor = "rgba(75, 192, 192, 1)",
                borderWidth = 1
            }
        }
            };

            return result;
        }
        public async Task<List<TagEntity>> GetRelatedTagsAsync(string tagId)
        {
            if (string.IsNullOrEmpty(tagId))
            {
                throw new ArgumentException("Tag ID cannot be null or empty.", nameof(tagId));
            }
            var tagGuid = Guid.Parse(tagId);
            var tag = await _context.Tags.FindAsync(tagGuid);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
            }
            // Assuming related tags are those that share the same name
            return await _context.Tags
                .Where(t => t.Name == tag.Name && t.Id != tagGuid && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<TagEntity> GetByIdAsync(string tagId)
        {
            if (string.IsNullOrEmpty(tagId))
            {
                throw new ArgumentException("Tag ID cannot be null or empty.", nameof(tagId));
            }
            var tag = await _context.Tags.FindAsync(Guid.Parse(tagId));
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
            }
            return tag;
        }
        public async Task<ServiceResult<TagEntity>> AddTagAsync(TagAddDto model)
        {
            try
            {
                var exists = await _context.Tags.AnyAsync(c => c.Name == model.Name);
                if (exists)
                {
                    // If the tag already exists, return an error result
                    return new ServiceResult<TagEntity>
                    {
                        IsSuccess = false,
                        Errors = new List<IdentityError>() { new IdentityError { Code = "TagExists", Description = "Tag with this name already exists." } }
                    };
                }

                var result = await _context.Tags.AddAsync(new TagEntity() { Name = model.Name, CreatedDate = model.CreatedDate, CreatedBy = model.CreatedBy });

                await _context.SaveChangesAsync();

                return new ServiceResult<TagEntity>() { IsSuccess = true };

            }
            catch (Exception)
            {
                return new ServiceResult<TagEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "AddTagError", Description = "An error occurred while adding the new tag." } }
                };
            }
        }

        public async Task<ItemPagination<TagDto>> GetPagedTagsAsync(int page, int pageSize, bool includeDeleted = false)
        {
            var itemsQuery = _context.Tags.AsQueryable();
            if (!includeDeleted)
            {
                itemsQuery = itemsQuery.Where(p => p.IsDeleted == false);
            }

            var pagedTags = new ItemPagination<TagDto>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = (includeDeleted is true) ? _context.Tags.Count() : _context.Tags.Where(p => p.IsDeleted == false).Count(),
                Items = await itemsQuery
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(tag => new TagDto()
                                    {
                                        Id = tag.Id.ToString(),
                                        Name = tag.Name,
                                        UsageCount = tag.UsageCount,
                                        IsDraft = tag.IsDraft,
                                        CreatedDate = tag.CreatedDate,
                                        ModifiedTime = tag.EditedDate ?? DateTime.Now,
                                        IsDeleted = tag.IsDeleted,
                                        CreatedBy = tag.CreatedBy,
                                        EditedBy = tag.EditedBy,
                                        EditedDate = tag.EditedDate
                                    })
                                    .ToListAsync()
            };
            return pagedTags;
        }

        public async Task<TagEntity> GetTagByIdAsync(Guid tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            return tag ?? throw new KeyNotFoundException($"Category with ID {tagId} not found.");
        }

        public async Task<ServiceResult<TagEntity>> UpdateTagAsync(TagUpdateDto model)
        {

            var tag = await _context.Tags.FindAsync(model.Id);
            if (tag == null)
            {
                return new ServiceResult<TagEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "TagNotFound", Description = "Tag not found." } }
                };
            }
            try
            {
                tag.Name = model.Name;
                tag.EditedBy = model.UpdatedBy;
                tag.EditedDate = DateTime.Now;
                _context.Tags.Update(tag);

                await _context.SaveChangesAsync();

                return new ServiceResult<TagEntity>() { IsSuccess = true, Data = tag };
            }
            catch (Exception)
            {
                return new ServiceResult<TagEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "UpdateTagError", Description = "An error occurred while updating the tag." } }
                };
            }
        }

        public async Task<ServiceResult<TagEntity>> DeleteTagByTypeAsync(string id, DeleteType deleteType, string deleterId)
        {
            var tag = await _context.Tags.FindAsync(Guid.Parse(id));
            if (tag == null)
            {
                var serviceResult = new ServiceResult<TagEntity>()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "TagNotFound", Description = "Tag not found." } }
                };

                return serviceResult;
            }
            var deleterUserId = deleterId;
            switch (deleteType)
            {
                case DeleteType.Soft:
                    tag.IsDeleted = true;
                    tag.EditedBy = deleterUserId;
                    tag.EditedDate = DateTime.Now;

                    try
                    {
                        _context.Tags.Update(tag);
                        await _context.SaveChangesAsync();

                        return new ServiceResult<TagEntity>()
                        {
                            IsSuccess = true,
                            Data = tag
                        };
                    }
                    catch (Exception)
                    {
                        return new ServiceResult<TagEntity>()
                        {
                            IsSuccess = false,
                            Errors = new List<IdentityError>() { new IdentityError() { Code = "TagCannotDeleted", Description = "Tag cannot deleted." } }
                        };
                    }
                case DeleteType.Hard:
                    tag.IsDeleted = true;
                    tag.EditedBy = deleterUserId;
                    try
                    {
                        _context.Tags.Remove(tag);
                        await _context.SaveChangesAsync();

                        return new ServiceResult<TagEntity>()
                        {
                            IsSuccess = true,
                            Data = tag
                        };
                    }
                    catch (Exception)
                    {
                        return new ServiceResult<TagEntity>()
                        {
                            IsSuccess = false,
                            Errors = new List<IdentityError>() { new IdentityError() { Code = "TagCannotDeleted", Description = "Tag cannot deleted." } }
                        };
                    }

                default:
                    return new ServiceResult<TagEntity>()
                    {
                        IsSuccess = false,
                        Errors = new List<IdentityError>() { new IdentityError() { Code = "InvalidDeleteType", Description = "Invalid delete type specified." } }
                    };
            }
        }

        public async Task<ServiceResult<TagEntity>> ActivateTagById(string tagId)
        {
            var tag = await _context.Tags.FindAsync(Guid.Parse(tagId));
            if (tag == null)
            {
                return new ServiceResult<TagEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "TagNotFound", Description = "Tag not found." } }
                };
            }
            try
            {
                tag.IsDeleted = false;
                tag.EditedDate = DateTime.Now;
                _context.Tags.Update(tag);
                await _context.SaveChangesAsync();
                return new ServiceResult<TagEntity>() { IsSuccess = true, Data = tag };
            }
            catch (Exception)
            {
                return new ServiceResult<TagEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "ActivateTagError", Description = "An error occurred while activating the tag." } }
                };
            }
        }

        public ServiceResult<List<SelectItemDto>> GetAllTagSelectList()
        {
            try
            {
                var selectListTags = _context.Tags.Where(a => a.IsDeleted == false).Select(t => new SelectItemDto() { Text = t.Name, Value = t.Id.ToString() });
                var result = new ServiceResult<List<SelectItemDto>>()
                {
                    IsSuccess = true,
                    Data = selectListTags.ToList()
                };

                return result;
            }
            catch (Exception)
            {
                var result = new ServiceResult<List<SelectItemDto>>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "GetAllTagSelectListError", Description = "An error occurred while retrieving the tag select list." } }
                };
                return result;
            }
        }
        public async Task<List<TagEntity>> GetPopularTags(int count = 10)
        {
            var context = _contextFactory.CreateDbContext();
            try
            {
                return await context.Tags.AsNoTracking().Where(t => t.IsDeleted == false).Take(count).OrderByDescending(v => v.UsageCount).ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving all tags.");
            }
        }
    }
}
