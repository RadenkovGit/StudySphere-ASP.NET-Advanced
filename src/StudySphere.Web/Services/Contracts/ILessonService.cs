using StudySphere.Web.Models.Lessons;

namespace StudySphere.Web.Services.Contracts;

public interface ILessonService
{
    Task<LessonDetailsViewModel?> GetByIdAsync(int id);
}
