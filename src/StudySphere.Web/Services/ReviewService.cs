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

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext context;

    public ReviewService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> CreateAsync(string userId, CreateReviewInputModel model)
    {
        var courseExists = await this.context.Courses.AnyAsync(c => c.Id == model.CourseId);
        if (!courseExists)
        {
            return false;
        }

        await this.context.Reviews.AddAsync(new Review
        {
            CourseId = model.CourseId,
            UserId = userId,
            Rating = model.Rating,
            Comment = model.Comment,
            CreatedOn = DateTime.UtcNow
        });

        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ReviewListItemViewModel>> GetMineAsync(string userId)
        => await this.context.Reviews
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedOn)
            .Select(r => new ReviewListItemViewModel
            {
                Id = r.Id,
                Author = r.Course.Title,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedOn = r.CreatedOn
            })
            .ToListAsync();

    public async Task<ReviewEditInputModel?> GetForEditAsync(int id, string userId)
        => await this.context.Reviews
            .AsNoTracking()
            .Where(r => r.Id == id && r.UserId == userId)
            .Select(r => new ReviewEditInputModel
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment
            })
            .FirstOrDefaultAsync();

    public async Task<bool> EditAsync(string userId, ReviewEditInputModel model)
    {
        var review = await this.context.Reviews.FirstOrDefaultAsync(r => r.Id == model.Id && r.UserId == userId);
        if (review is null)
        {
            return false;
        }

        review.Rating = model.Rating;
        review.Comment = model.Comment;
        await this.context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var review = await this.context.Reviews.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
        if (review is null)
        {
            return false;
        }

        this.context.Reviews.Remove(review);
        await this.context.SaveChangesAsync();
        return true;
    }
}
