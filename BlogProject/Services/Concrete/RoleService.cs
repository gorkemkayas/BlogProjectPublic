using Azure.Core;
using BlogProject.Areas.Admin.Controllers;
using BlogProject.Models.ViewModels;
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
                                        Name = role.Name! })
                                    .ToListAsync()
            };

            return pagedRoles;
        }
    }
}
