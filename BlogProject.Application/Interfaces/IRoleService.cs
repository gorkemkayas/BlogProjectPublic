using BlogProject.Application.Common;
using BlogProject.Application.DTOs;
using BlogProject.Application.Enums;
using BlogProject.Application.Models;
using BlogProject.Domain.Entities;

namespace BlogProject.Application.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResult<AppRole>> AddRoleAsync(AppRole role);
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<ItemPagination<RoleDto>> GetPagedRolesAsync(int page = 1, int pageSize = 4, bool includeDeleted = false);
        Task<AppRole> GetRoleByIdAsync(string id);
        Task<ServiceResult<AppRole>> UpdateRoleAsync(RoleEditDto request);
        Task<ServiceResult<AppRole>> DeleteRoleByTypeAsync(string id, DeleteType deleteType, string deleterUserId);
        Task<RoleUsersDto> GetRolesWithUsers(string roleName);
        Task<ServiceResult<AppRole>> ActivateRoleById(string roleId);
    }
}
