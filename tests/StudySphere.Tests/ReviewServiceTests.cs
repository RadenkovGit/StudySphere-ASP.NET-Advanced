using Microsoft.EntityFrameworkCore;
using StudySphere.Web.Data;
using StudySphere.Web.Data.Models;
using StudySphere.Web.Models.Reviews;
using StudySphere.Web.Services;
using Xunit;

namespace StudySphere.Tests;

public class ReviewServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldAddReview_WhenCourseExists()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        context.Users.Add(new ApplicationUser { Id = "user-1", UserName = "user", Email = "user@test.com", FullName = "Test User" });
        context.Categories.Add(new Category { Id = 1, Name = "Web", Description = "Web" });
        context.Courses.Add(new Course
        {
            Id = 1,
            Title = "Course",
            Description = "Description for course",
            InstructorName = "Teacher",
            CategoryId = 1,
            StartDate = DateTime.UtcNow.Date,
            DurationWeeks = 4,
            ImageUrl = "https://example.com"
        });
        context.SaveChanges();

        var service = new ReviewService(context);
        var result = await service.CreateAsync("user-1", new CreateReviewInputModel
        {
            CourseId = 1,
            Rating = 5,
            Comment = "Great and very practical course."
        });

        Assert.True(result);
        Assert.Single(context.Reviews);
    }
}
