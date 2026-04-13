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

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext context;
    private const int PageSize = 8;

    public TaskService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<TaskIndexViewModel> GetMyTasksAsync(string userId, string? searchTerm, bool showCompleted, int page)
    {
        var query = this.context.StudyTasks
            .AsNoTracking()
            .Where(t => t.UserId == userId);

        if (!showCompleted)
        {
            query = query.Where(t => !t.IsCompleted);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm));
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderBy(t => t.DueDate)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(t => new TaskListItemViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                IsCompleted = t.IsCompleted
            })
            .ToListAsync();

        return new TaskIndexViewModel
        {
            SearchTerm = searchTerm,
            ShowCompleted = showCompleted,
            PagedTasks = new PagedResult<TaskListItemViewModel>
            {
                Items = items,
                PageNumber = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize)
            }
        };
    }

    public async Task<int> CreateAsync(string userId, TaskInputModel model)
    {
        var entity = new StudyTask
        {
            UserId = userId,
            Title = model.Title,
            Description = model.Description,
            DueDate = model.DueDate,
            Priority = model.Priority,
            IsCompleted = false
        };

        await this.context.StudyTasks.AddAsync(entity);
        await this.context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<TaskInputModel?> GetForEditAsync(int id, string userId)
        => await this.context.StudyTasks
            .AsNoTracking()
            .Where(t => t.Id == id && t.UserId == userId)
            .Select(t => new TaskInputModel
            {
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority
            })
            .FirstOrDefaultAsync();

    public async Task<bool> UpdateAsync(int id, string userId, TaskInputModel model)
    {
        var entity = await this.context.StudyTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (entity is null)
        {
            return false;
        }

        entity.Title = model.Title;
        entity.Description = model.Description;
        entity.DueDate = model.DueDate;
        entity.Priority = model.Priority;

        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var entity = await this.context.StudyTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (entity is null)
        {
            return false;
        }

        this.context.StudyTasks.Remove(entity);
        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleStatusAsync(int id, string userId)
    {
        var entity = await this.context.StudyTasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (entity is null)
        {
            return false;
        }

        entity.IsCompleted = !entity.IsCompleted;
        await this.context.SaveChangesAsync();
        return true;
    }
}
