using StudySphere.Web.Models.Tasks;

namespace StudySphere.Web.Services.Contracts;

public interface ITaskService
{
    Task<TaskIndexViewModel> GetMyTasksAsync(string userId, string? searchTerm, bool showCompleted, int page);
    Task<int> CreateAsync(string userId, TaskInputModel model);
    Task<TaskInputModel?> GetForEditAsync(int id, string userId);
    Task<bool> UpdateAsync(int id, string userId, TaskInputModel model);
    Task<bool> DeleteAsync(int id, string userId);
    Task<bool> ToggleStatusAsync(int id, string userId);
}
