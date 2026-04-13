using StudySphere.Web.Models.Reviews;

namespace StudySphere.Web.Services.Contracts;

public interface IReviewService
{
    Task<bool> CreateAsync(string userId, CreateReviewInputModel model);
    Task<IEnumerable<ReviewListItemViewModel>> GetMineAsync(string userId);
    Task<ReviewEditInputModel?> GetForEditAsync(int id, string userId);
    Task<bool> EditAsync(string userId, ReviewEditInputModel model);
    Task<bool> DeleteAsync(int id, string userId);
}
