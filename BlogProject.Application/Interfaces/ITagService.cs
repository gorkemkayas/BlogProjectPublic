using BlogProject.Application.Common;
using BlogProject.Application.DTOs;
using BlogProject.Application.Enums;
using BlogProject.Application.Models;
using BlogProject.Domain.Entities;

namespace BlogProject.Application.Interfaces
{
    public interface ITagService
    {
        Task<object> GetDailyPostCountsAsync(string tagId);
        Task<List<TagEntity>> GetRelatedTagsAsync(string tagId);
        Task<TagEntity> GetByIdAsync(string tagId);
        Task<ServiceResult<TagEntity>> AddTagAsync(TagAddDto model);
        Task<ItemPagination<TagDto>> GetPagedTagsAsync(int page, int pageSize, bool includeDeleted = false);
        Task<TagEntity> GetTagByIdAsync(Guid tagId);
        Task<ServiceResult<TagEntity>> UpdateTagAsync(TagUpdateDto model);
        Task<ServiceResult<TagEntity>> ActivateTagById(string tagId);
        Task<ServiceResult<TagEntity>> DeleteTagByTypeAsync(string id, DeleteType deleteType, string deleterId);
        ServiceResult<List<SelectItemDto>> GetAllTagSelectList();
        Task<List<TagEntity>> GetPopularTags(int count = 10);
    }
}
