using StudySphere.Web.Models.Admin;

namespace StudySphere.Web.Services.Contracts;

public interface IAdminDashboardService
{
    Task<AdminDashboardViewModel> GetStatisticsAsync();
    Task<IEnumerable<AdminCategoryInputModel>> GetCategoriesAsync();
    Task<AdminCategoryInputModel?> GetCategoryAsync(int id);
    Task<int> CreateCategoryAsync(AdminCategoryInputModel model);
    Task<bool> UpdateCategoryAsync(AdminCategoryInputModel model);
    Task<bool> DeleteCategoryAsync(int id);
}
