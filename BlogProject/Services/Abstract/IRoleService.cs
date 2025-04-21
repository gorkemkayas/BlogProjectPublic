using BlogProject.Areas.Admin.Models;
using BlogProject.src.Infra.Entitites;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Services.Abstract
{
    public interface IRoleService
    {
        Task<ServiceResult<AppRole>> AddRoleAsync(AppRole role);
        Task<List<RoleViewModel>> GetAllRolesAsync();
        Task<ItemPagination<RoleViewModel>> GetPagedRolesAsync(int page = 1, int pageSize = 4);

        Task<AppRole> GetRoleByIdAsync(string id);
        Task<ServiceResult<AppRole>> UpdateRoleAsync(RoleEditViewModel request);
    }
}
