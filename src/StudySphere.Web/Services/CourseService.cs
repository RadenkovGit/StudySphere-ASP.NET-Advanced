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

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext context;
    private const int PageSize = 6;

    public CourseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<CourseIndexViewModel> GetAllAsync(string? searchTerm, int? categoryId, int page)
    {
        var query = this.context.Courses
            .AsNoTracking()
            .Include(c => c.Category)
            .Include(c => c.Reviews)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.Title.Contains(searchTerm) || c.Description.Contains(searchTerm));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(c => c.CategoryId == categoryId.Value);
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderBy(c => c.StartDate)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(c => new CourseCardViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Category = c.Category.Name,
                Description = c.Description,
                InstructorName = c.InstructorName,
                ImageUrl = c.ImageUrl,
                StartDate = c.StartDate,
                AverageRating = c.Reviews.Any() ? c.Reviews.Average(r => r.Rating) : 0
            })
            .ToListAsync();

        return new CourseIndexViewModel
        {
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            Categories = await this.GetCategoryOptionsAsync(),
            PagedCourses = new PagedResult<CourseCardViewModel>
            {
                Items = items,
                PageNumber = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize)
            }
        };
    }

    public async Task<CourseDetailsViewModel?> GetDetailsAsync(int id, string? userId)
    {
        var course = await this.context.Courses
            .AsNoTracking()
            .Include(c => c.Category)
            .Include(c => c.Lessons)
            .Include(c => c.Resources)
            .Include(c => c.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course is null)
        {
            return null;
        }

        var isEnrolled = userId is not null && await this.context.Enrollments.AnyAsync(x => x.CourseId == id && x.UserId == userId && x.IsActive);

        return new CourseDetailsViewModel
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Category = course.Category.Name,
            InstructorName = course.InstructorName,
            StartDate = course.StartDate,
            DurationWeeks = course.DurationWeeks,
            ImageUrl = course.ImageUrl,
            IsEnrolled = isEnrolled,
            AverageRating = course.Reviews.Any() ? course.Reviews.Average(r => r.Rating) : 0,
            Lessons = course.Lessons
                .OrderBy(l => l.Order)
                .Select(l => new LessonListItemViewModel
                {
                    Id = l.Id,
                    Order = l.Order,
                    Title = l.Title,
                    EstimatedMinutes = l.EstimatedMinutes
                })
                .ToList(),
            Resources = course.Resources
                .Select(r => new ResourceListItemViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    Url = r.Url,
                    Summary = r.Summary,
                    CourseTitle = course.Title
                })
                .ToList(),
            Reviews = course.Reviews
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new ReviewListItemViewModel
                {
                    Id = r.Id,
                    Author = r.User.FullName == string.Empty ? r.User.Email! : r.User.FullName,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    CreatedOn = r.CreatedOn
                })
                .ToList(),
            NewReview = new CreateReviewInputModel { CourseId = course.Id }
        };
    }

    public async Task EnrollAsync(int courseId, string userId)
    {
        var existing = await this.context.Enrollments
            .FirstOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId);

        if (existing is null)
        {
            await this.context.Enrollments.AddAsync(new Enrollment
            {
                CourseId = courseId,
                UserId = userId,
                EnrolledOn = DateTime.UtcNow,
                IsActive = true
            });
        }
        else
        {
            existing.IsActive = true;
        }

        await this.context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseCategoryOptionViewModel>> GetCategoryOptionsAsync()
        => await this.context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CourseCategoryOptionViewModel { Id = c.Id, Name = c.Name })
            .ToListAsync();

    public async Task<IEnumerable<CourseCardViewModel>> GetAdminCoursesAsync()
        => await this.context.Courses
            .AsNoTracking()
            .Include(c => c.Category)
            .OrderByDescending(c => c.StartDate)
            .Select(c => new CourseCardViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Category = c.Category.Name,
                Description = c.Description,
                InstructorName = c.InstructorName,
                ImageUrl = c.ImageUrl,
                StartDate = c.StartDate
            })
            .ToListAsync();

    public async Task<AdminCourseInputModel?> GetCourseForEditAsync(int id)
    {
        var course = await this.context.Courses.FindAsync(id);
        if (course is null)
        {
            return null;
        }

        return new AdminCourseInputModel
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            InstructorName = course.InstructorName,
            CategoryId = course.CategoryId,
            StartDate = course.StartDate,
            DurationWeeks = course.DurationWeeks,
            ImageUrl = course.ImageUrl,
            Categories = await this.GetCategoryOptionsAsync()
        };
    }

    public async Task<int> CreateAsync(AdminCourseInputModel model)
    {
        var entity = new Course
        {
            Title = model.Title,
            Description = model.Description,
            InstructorName = model.InstructorName,
            CategoryId = model.CategoryId,
            StartDate = model.StartDate,
            DurationWeeks = model.DurationWeeks,
            ImageUrl = model.ImageUrl
        };

        await this.context.Courses.AddAsync(entity);
        await this.context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<bool> EditAsync(AdminCourseInputModel model)
    {
        var entity = await this.context.Courses.FindAsync(model.Id);
        if (entity is null)
        {
            return false;
        }

        entity.Title = model.Title;
        entity.Description = model.Description;
        entity.InstructorName = model.InstructorName;
        entity.CategoryId = model.CategoryId;
        entity.StartDate = model.StartDate;
        entity.DurationWeeks = model.DurationWeeks;
        entity.ImageUrl = model.ImageUrl;

        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await this.context.Courses.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        this.context.Courses.Remove(entity);
        await this.context.SaveChangesAsync();
        return true;
    }
}
