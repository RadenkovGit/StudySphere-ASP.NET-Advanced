using Microsoft.EntityFrameworkCore;
using StudySphere.Web.Data;
using StudySphere.Web.Data.Models;
using StudySphere.Web.Models.Admin;
using StudySphere.Web.Models.Courses;
using StudySphere.Web.Models.Lessons;
using StudySphere.Web.Models.Paging;
using StudySphere.Web.Models.Resources;
using StudySphere.Web.Models.Reviews;
using StudySphere.Web.Models.Tasks;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Services;

public class LessonService : ILessonService
{
    private readonly ApplicationDbContext context;

    public LessonService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<LessonDetailsViewModel?> GetByIdAsync(int id)
        => await this.context.Lessons
            .AsNoTracking()
            .Select(l => new LessonDetailsViewModel
            {
                Id = l.Id,
                CourseId = l.CourseId,
                CourseTitle = l.Course.Title,
                Order = l.Order,
                Title = l.Title,
                Content = l.Content,
                EstimatedMinutes = l.EstimatedMinutes
            })
            .FirstOrDefaultAsync(l => l.Id == id);
}
