using BlogProject.Application.Common;
using BlogProject.Application.DTOs;
using BlogProject.Application.Enums;
using BlogProject.Application.Models;
using BlogProject.Domain.Entities;
using System.Web.Mvc;

namespace BlogProject.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<object> GetDailyPostCountsAsync(string categoryId);
        Task<List<CategoryIdAndNameDto>> GetRelatedCategoriesAsync(string categoryId);
        Task<ServiceResult<CategoryEntity>> AddCategoryAsync(CategoryAddDto model);
        Task<ServiceResult<CategoryEntity>> UpdateCategoryAsync(CategoryUpdateDto model);
        Task<ItemPagination<CategoryDto>> GetPagedCategoriesAsync(int page, int pageSize, bool includeDeleted = false);
        Task<CategoryEntity> GetCategoryByIdAsync(Guid categoryId);
        Task<CategoryIdNameDescriptionDto> GetByIdAsync(string id);
        Task<ServiceResult<CategoryEntity>> DeleteCategoryByTypeAsync(string id, DeleteType deleteType, string deleterId);
        Task<ServiceResult<CategoryEntity>> ActivateCategoryById(string categoryId);

        ServiceResult<List<SelectItemDto>> GetAllCategorySelectList();



    }
}
