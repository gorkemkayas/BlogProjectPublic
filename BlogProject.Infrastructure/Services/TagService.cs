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

        public TagService(BlogDbContext context)
        {
            _context = context;
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
            try
            {
                return await _context.Tags.Where(t => t.IsDeleted == false).Take(count).OrderByDescending(v => v.UsageCount).ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving all tags.");
            }
        }
    }
}
