using BlogProject.Areas.Admin.Models;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Services.Abstract
{
    public interface ITagService
    {
        Task<ServiceResult<TagEntity>> AddTagAsync(TagAddViewModel model);
        Task<ItemPagination<TagViewModel>> GetPagedTagsAsync(int page, int pageSize, bool includeDeleted = false);
        Task<TagEntity> GetTagByIdAsync(Guid tagId);
        Task<ServiceResult<TagEntity>> UpdateTagAsync(TagUpdateViewModel model);
        Task<ServiceResult<TagEntity>> ActivateTagById(string tagId);
        Task<ServiceResult<TagEntity>> DeleteTagByTypeAsync(string id, DeleteType deleteType, string deleterId);
        ServiceResult<List<SelectListItem>> GetAllTagSelectList();
        Task<List<TagEntity>> GetPopularTags(int count = 10);
    }
}
