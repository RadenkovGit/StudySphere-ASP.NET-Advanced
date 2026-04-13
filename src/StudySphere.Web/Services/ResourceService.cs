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

public class ResourceService : IResourceService
{
    private readonly ApplicationDbContext context;
    private const int PageSize = 8;

    public ResourceService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<ResourceIndexViewModel> GetAllAsync(string? searchTerm, int page)
    {
        var query = this.context.ResourceItems
            .AsNoTracking()
            .Include(r => r.Course)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(r => r.Title.Contains(searchTerm) || r.Summary.Contains(searchTerm));
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderBy(r => r.Title)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(r => new ResourceListItemViewModel
            {
                Id = r.Id,
                Title = r.Title,
                Url = r.Url,
                Summary = r.Summary,
                CourseTitle = r.Course.Title
            })
            .ToListAsync();

        return new ResourceIndexViewModel
        {
            SearchTerm = searchTerm,
            PagedResources = new PagedResult<ResourceListItemViewModel>
            {
                Items = items,
                PageNumber = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize)
            }
        };
    }

    public async Task<ResourceListItemViewModel?> GetByIdAsync(int id)
        => await this.context.ResourceItems
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new ResourceListItemViewModel
            {
                Id = r.Id,
                Title = r.Title,
                Url = r.Url,
                Summary = r.Summary,
                CourseTitle = r.Course.Title
            })
            .FirstOrDefaultAsync();
}
