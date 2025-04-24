using Azure.Core;
using BlogProject.Areas.Admin.Controllers;
using BlogProject.Areas.Admin.Models;
using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Services.Concrete
{
    public partial class RoleService : IRoleService
    {
        public readonly RoleManager<AppRole> _roleManager;
        public readonly UserManager<AppUser> _userManager;

        public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ServiceResult<AppRole>> AddRoleAsync(AppRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return new ServiceResult<AppRole> { IsSuccess = false, Errors = result.Errors.ToList()};
            }
            return new ServiceResult<AppRole> { IsSuccess = true };
        }

        public async Task<ServiceResult<AppRole>> DeleteRoleAsync(string id)
        {
            var role = await GetRoleByIdAsync(id);

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if(usersInRole.Count != 0)
            {
                return new ServiceResult<AppRole> { IsSuccess = false, Errors = new List<IdentityError> { new IdentityError { Code = "RoleUsedOnUsers", Description = "There are users in this role" } } };
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return new ServiceResult<AppRole> { IsSuccess = false, Errors = result.Errors.ToList() };
            }
            return new ServiceResult<AppRole> { IsSuccess = true };
        }

        public async Task<ServiceResult<AppRole>> DeleteRoleByTypeAsync(string id, DeleteType type,string deleterUserId)
        {
            var role = await GetRoleByIdAsync(id);

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

            if (usersInRole.Count != 0)
            {
                return new ServiceResult<AppRole> { IsSuccess = false, Errors = new List<IdentityError> { new IdentityError { Code = "RoleUsedOnUsers", Description = "There are users in this role" } } };
            }

            var result = new IdentityResult();

            switch (type)
            {
                case DeleteType.Hard:
                    result = await _roleManager.DeleteAsync(role);
                    break;
                case DeleteType.Soft:
                    role.IsDeleted = true;
                    role.EditedBy = deleterUserId;
                    result = await _roleManager.UpdateAsync(role);
                    break;
            }

            if (!result.Succeeded)
            {
                return new ServiceResult<AppRole> { IsSuccess = false, Errors = result.Errors.ToList() };
            }

            return new ServiceResult<AppRole> { IsSuccess = true };
        }
        public async Task<List<RoleViewModel>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name!,
                CreatedBy = role.CreatedBy!
            }).ToListAsync();
        }

        public async Task<ItemPagination<RoleViewModel>> GetPagedRolesAsync(int page, int pageSize,bool includeDeleted = false)
        {
            var itemsQuery =  _roleManager.Roles;
            if(!includeDeleted)
            {
                itemsQuery = itemsQuery.Where(p => p.IsDeleted == false);
            }

            var pagedRoles = new ItemPagination<RoleViewModel>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = (includeDeleted is true) ? _roleManager.Roles.Count() : _roleManager.Roles.Where(p => p.IsDeleted == false).Count(),
                Items = await itemsQuery
                                    .Skip((page - 1)*pageSize)
                                    .Take(pageSize)
                                    .Select(role => new RoleViewModel { 
                                        Id = role.Id, 
                                        CreatedBy = role.CreatedBy!,
                                        EditedBy = role.EditedBy,
                                        Name = role.Name!,
                                        IsDeleted = role.IsDeleted
                                    })
                                    .ToListAsync()
            };

            return pagedRoles;
        }

        public async Task<AppRole> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if(role == null) throw new Exception("Role not found");

            return role;
        }

        public async Task<ServiceResult<AppRole>> UpdateRoleAsync(RoleEditViewModel request)
        {

            var role = await GetRoleByIdAsync(request.Id);
            if (role == null) return new ServiceResult<AppRole> { IsSuccess = false, Errors = new List<IdentityError> { new IdentityError {Code = "CodeNotFound", Description = "Role not found" } } };

            role.Name = request.Name;
            role.EditedBy = request.WillEditedBy;

            var result = await _roleManager.UpdateAsync(role);

            if(!result.Succeeded)
            {
                return new ServiceResult<AppRole> { IsSuccess = false, Errors = result.Errors.ToList() };
            }

            return new ServiceResult<AppRole> { IsSuccess = true};
        }
    }
}
