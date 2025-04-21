using Azure.Core;
using BlogProject.Areas.Admin.Controllers;
using BlogProject.Areas.Admin.Models;
using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Services.Concrete
{
    public class RoleService : IRoleService
    {
        public readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
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

        public async Task<List<RoleViewModel>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name!,
                CreatedBy = role.CreatedBy!
            }).ToListAsync();
        }

        public async Task<ItemPagination<RoleViewModel>> GetPagedRolesAsync(int page, int pageSize)
        {
            var pagedRoles = new ItemPagination<RoleViewModel>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = _roleManager.Roles.Count(),
                Items = await _roleManager.Roles
                                    .Skip((page - 1)*pageSize)
                                    .Take(pageSize)
                                    .Select(role => new RoleViewModel { 
                                        Id = role.Id, 
                                        CreatedBy = role.CreatedBy!,
                                        EditedBy = role.EditedBy,
                                        Name = role.Name! })
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
