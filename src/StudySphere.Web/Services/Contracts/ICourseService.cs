using StudySphere.Web.Models.Admin;
using StudySphere.Web.Models.Courses;

namespace StudySphere.Web.Services.Contracts;

public interface ICourseService
{
    Task<CourseIndexViewModel> GetAllAsync(string? searchTerm, int? categoryId, int page);
    Task<CourseDetailsViewModel?> GetDetailsAsync(int id, string? userId);
    Task EnrollAsync(int courseId, string userId);
    Task<IEnumerable<CourseCategoryOptionViewModel>> GetCategoryOptionsAsync();
    Task<IEnumerable<CourseCardViewModel>> GetAdminCoursesAsync();
    Task<AdminCourseInputModel?> GetCourseForEditAsync(int id);
    Task<int> CreateAsync(AdminCourseInputModel model);
    Task<bool> EditAsync(AdminCourseInputModel model);
    Task<bool> DeleteAsync(int id);
}
