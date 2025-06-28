using BlogProject.Areas.Admin.Models;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Services.Abstract
{
    public interface ICategoryService
    {
        Task<ServiceResult<CategoryEntity>> AddCategoryAsync(CategoryAddViewModel model);
        Task<ServiceResult<CategoryEntity>> UpdateCategoryAsync(CategoryUpdateViewModel model);
        Task<ItemPagination<CategoryViewModel>> GetPagedCategoriesAsync(int page, int pageSize, bool includeDeleted = false);
        Task<CategoryEntity> GetCategoryByIdAsync(Guid categoryId);

        Task<ServiceResult<CategoryEntity>> DeleteCategoryByTypeAsync(string id, DeleteType deleteType, string deleterId);
        Task<ServiceResult<CategoryEntity>> ActivateCategoryById(string categoryId);

        ServiceResult<List<SelectListItem>> GetAllCategorySelectList();



    }
}
