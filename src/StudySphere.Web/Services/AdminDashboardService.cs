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

public class AdminDashboardService : IAdminDashboardService
{
    private readonly ApplicationDbContext context;

    public AdminDashboardService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<AdminDashboardViewModel> GetStatisticsAsync()
        => new()
        {
            CoursesCount = await this.context.Courses.CountAsync(),
            UsersCount = await this.context.Users.CountAsync(),
            ReviewsCount = await this.context.Reviews.CountAsync(),
            TasksCount = await this.context.StudyTasks.CountAsync()
        };

    public async Task<IEnumerable<AdminCategoryInputModel>> GetCategoriesAsync()
        => await this.context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new AdminCategoryInputModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync();

    public async Task<AdminCategoryInputModel?> GetCategoryAsync(int id)
        => await this.context.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new AdminCategoryInputModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .FirstOrDefaultAsync();

    public async Task<int> CreateCategoryAsync(AdminCategoryInputModel model)
    {
        var category = new Category
        {
            Name = model.Name,
            Description = model.Description
        };

        await this.context.Categories.AddAsync(category);
        await this.context.SaveChangesAsync();
        return category.Id;
    }

    public async Task<bool> UpdateCategoryAsync(AdminCategoryInputModel model)
    {
        var category = await this.context.Categories.FindAsync(model.Id);
        if (category is null)
        {
            return false;
        }

        category.Name = model.Name;
        category.Description = model.Description;

        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await this.context.Categories.FindAsync(id);
        if (category is null || await this.context.Courses.AnyAsync(c => c.CategoryId == id))
        {
            return false;
        }

        this.context.Categories.Remove(category);
        await this.context.SaveChangesAsync();
        return true;
    }
}
