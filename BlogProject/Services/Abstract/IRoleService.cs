using BlogProject.Areas.Admin.Models;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Services.Abstract
{
    public interface IRoleService
    {
        Task<ServiceResult<AppRole>> AddRoleAsync(AppRole role);
        Task<List<RoleViewModel>> GetAllRolesAsync();
        Task<ItemPagination<RoleViewModel>> GetPagedRolesAsync(int page = 1, int pageSize = 4, bool includeDeleted = false);
        Task<AppRole> GetRoleByIdAsync(string id);
        Task<ServiceResult<AppRole>> UpdateRoleAsync(RoleEditViewModel request);
        Task<ServiceResult<AppRole>> DeleteRoleByTypeAsync(string id, DeleteType deleteType, string deleterUserId);
        Task<RoleUsersViewModel> GetRolesWithUsers(string roleName);
        Task<ServiceResult<AppRole>> ActivateRoleById(string roleId);
    }
}
