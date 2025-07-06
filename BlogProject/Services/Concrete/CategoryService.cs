using AutoMapper;
using BlogProject.Areas.Admin.Models;
using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Context;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(BlogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResult<CategoryEntity>> AddCategoryAsync(CategoryAddViewModel model)
        {
            try
            {
                var exists = await _context.Categories.AnyAsync(c => c.Name == model.Name);
                if (exists)
                {
                    // If the category already exists, return an error result
                    return new ServiceResult<CategoryEntity>
                    {
                        IsSuccess = false,
                        Errors = new List<IdentityError>() { new IdentityError { Code="CategoryExists", Description = "Category with this name already exists." } }
                    };
                }

                var result = await _context.Categories.AddAsync(new src.Infra.Entitites.CategoryEntity() { Name = model.Name, CreatedDate = model.CreatedDate, CreatedBy = model.CreatedBy });

                await _context.SaveChangesAsync();

                return new ServiceResult<CategoryEntity>() { IsSuccess = true};

            }
            catch (Exception)
            {
                return new ServiceResult<CategoryEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "AddCategoryError", Description = "An error occurred while adding the new category." } }
                };
            }
        }

        public async Task<ServiceResult<CategoryEntity>> UpdateCategoryAsync(CategoryUpdateViewModel model)
        {
            var category = await _context.Categories.FindAsync(model.Id);
            if(category == null)
            {
                return new ServiceResult<CategoryEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "CategoryNotFound", Description = "Category not found." } }
                };
            }
            try
            {
                category.Name = model.Name;
                category.EditedBy = model.UpdatedBy;
                category.EditedDate = DateTime.Now;
                _context.Categories.Update(category);

                await _context.SaveChangesAsync();

                return new ServiceResult<CategoryEntity>() { IsSuccess = true, Data = category };
            }
            catch (Exception)
            {
                return new ServiceResult<CategoryEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "UpdateCategoryError", Description = "An error occurred while updating the category." } }
                };
            }

        }
        public async Task<ItemPagination<CategoryViewModel>> GetPagedCategoriesAsync(int page, int pageSize, bool includeDeleted = false)
        {
            var itemsQuery = _context.Categories.AsQueryable();
            if (!includeDeleted)
            {
                itemsQuery = itemsQuery.Where(p => p.IsDeleted == false);
            }

            var pagedUsers = new ItemPagination<CategoryViewModel>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = (includeDeleted is true) ? _context.Categories.Count() : _context.Categories.Where(p => p.IsDeleted == false).Count(),
                Items = await itemsQuery
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(user => _mapper.Map<CategoryViewModel>(user)).ToListAsync()
            };
            return pagedUsers;
        }
        public async Task<CategoryEntity> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            return category ?? throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
        }


        public async Task<ServiceResult<CategoryEntity>> DeleteCategoryByTypeAsync(string id, DeleteType deleteType, string deleterId)
        {
            var category = await _context.Categories.FindAsync(Guid.Parse(id));
            if (category == null)
            {
                var serviceResult = new ServiceResult<CategoryEntity>()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "CategoryNotFound", Description = "Category not found." } }
                };

                return serviceResult;
            }
            var deleterUserId = deleterId;
            switch (deleteType)
            {
                case DeleteType.Soft:
                    category.IsDeleted = true;
                    category.EditedBy = deleterUserId;
                    category.EditedDate = DateTime.Now;

                    try
                    {
                        _context.Categories.Update(category);
                        await _context.SaveChangesAsync();

                        return new ServiceResult<CategoryEntity>()
                        {
                            IsSuccess = true,
                            Data = category
                        };
                    }
                    catch (Exception)
                    {
                        return new ServiceResult<CategoryEntity>()
                        {
                            IsSuccess = false,
                            Errors = new List<IdentityError>() { new IdentityError() { Code = "CategoryCannotDeleted", Description = "Category cannot deleted." } }
                        };
                    }
                case DeleteType.Hard:
                    category.IsDeleted = true;
                    category.EditedBy = deleterUserId;
                    try
                    {
                        _context.Categories.Remove(category);
                        await _context.SaveChangesAsync();

                        return new ServiceResult<CategoryEntity>()
                        {
                            IsSuccess = true,
                            Data = category
                        };
                    }
                    catch (Exception)
                    {
                        return new ServiceResult<CategoryEntity>()
                        {
                            IsSuccess = false,
                            Errors = new List<IdentityError>() { new IdentityError() { Code = "CategoryCannotDeleted", Description = "Category cannot deleted." } }
                        };
                    }

                default:
                    return new ServiceResult<CategoryEntity>()
                    {
                        IsSuccess = false,
                        Errors = new List<IdentityError>() { new IdentityError() { Code = "InvalidDeleteType", Description = "Invalid delete type specified." } }
                    };
            }
        }

        public async Task<ServiceResult<CategoryEntity>> ActivateCategoryById(string categoryId)
        {
            var category = await _context.Categories.FindAsync(Guid.Parse(categoryId));
            if(category == null)
            {
                return new ServiceResult<CategoryEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "CategoryNotFound", Description = "Category not found." } }
                };
            }
            try
            {
                category.IsDeleted = false;
                category.EditedDate = DateTime.Now;
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return new ServiceResult<CategoryEntity>() { IsSuccess = true, Data = category };
            }
            catch (Exception)
            {
                return new ServiceResult<CategoryEntity>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "ActivateCategoryError", Description = "An error occurred while activating the category." } }
                };
            }
        }

        public ServiceResult<List<SelectListItem>> GetAllCategorySelectList()
        {
            try
            {
                var selectListCategories = _context.Categories.Where(a => a.IsDeleted == false).Select(t => new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });
                var result = new ServiceResult<List<SelectListItem>>()
                {
                    IsSuccess = true,
                    Data = selectListCategories.ToList()
                };

                return result;
            }
            catch (Exception)
            {
                var result = new ServiceResult<List<SelectListItem>>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "GetAllCategoriesSelectListError", Description = "An error occurred while retrieving the category select list." } }
                };
                return result;
            }
        }
    }
}
