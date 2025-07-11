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
    public partial class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly BlogDbContext _context;

        public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, BlogDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
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
        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                CreatedBy = role.CreatedBy!
            }).ToListAsync();
        }

        public async Task<ItemPagination<RoleDto>> GetPagedRolesAsync(int page, int pageSize,bool includeDeleted = false)
        {
            var itemsQuery =  _roleManager.Roles;
            if(!includeDeleted)
            {
                itemsQuery = itemsQuery.Where(p => p.IsDeleted == false);
            }

            var pagedRoles = new ItemPagination<RoleDto>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = (includeDeleted is true) ? _roleManager.Roles.Count() : _roleManager.Roles.Where(p => p.IsDeleted == false).Count(),
                Items = await itemsQuery
                                    .Skip((page - 1)*pageSize)
                                    .Take(pageSize)
                                    .Select(role => new RoleDto { 
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

        public async Task<ServiceResult<AppRole>> UpdateRoleAsync(RoleEditDto request)
        {

            var role = await GetRoleByIdAsync(request.Id);
            if (role == null) return new ServiceResult<AppRole> { IsSuccess = false, Errors = new List<IdentityError> { new IdentityError {Code = "CodeNotFound", Description = "Role not found" } } };

            role.Name = request.Name;
            role.EditedBy = request.WillEditedBy;
            role.ModifiedDate = DateTime.Now;

            var result = await _roleManager.UpdateAsync(role);

            if(!result.Succeeded)
            {
                return new ServiceResult<AppRole> { IsSuccess = false, Errors = result.Errors.ToList() };
            }

            return new ServiceResult<AppRole> { IsSuccess = true};
        }

        public async Task<RoleUsersDto> GetRolesWithUsers(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null) throw new Exception("Role not found");

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

            var creator = await _userManager.FindByIdAsync(role.CreatedBy!);
            var roleUsers = new RoleUsersDto
            {
                Id = role.Id.ToString(),
                Name = role.Name!,
                CreatedBy = role.CreatedBy!,
                CreatorName = creator!.Name + " " + creator.Surname,
                CreatorUserName = creator.UserName!,
                ModifiedBy = role.EditedBy,
                CreatedDate = role.CreatedDate,
                ModifiedDate = role.ModifiedDate,
                Users = usersInRole.Select(user => new UserDto
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName!,
                    ProfileImage = user.ProfilePicture,
                    LastLoginDate = user.LastLoginDate,
                    IsSuspended = user.IsSuspended,
                    Birthdate = user.BirthDate,
                    Email = user.Email!
                }).ToList()
            };

            return roleUsers; 
        }

        public async Task<ServiceResult<AppRole>> ActivateRoleById(string roleId)
        {
            var role = await _context.Roles.FindAsync(Guid.Parse(roleId));
            if (role == null)
            {
                return new ServiceResult<AppRole>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "RoleNotFound", Description = "Role not found." } }
                };
            }
            try
            {
                role.IsDeleted = false;
                role.ModifiedDate = DateTime.Now;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                return new ServiceResult<AppRole>() { IsSuccess = true, Data = role };
            }
            catch (Exception)
            {
                return new ServiceResult<AppRole>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError { Code = "ActivateRoleError", Description = "An error occurred while activating the role." } }
                };
            }
        }
    }
}
