using Microsoft.EntityFrameworkCore;
using StudySphere.Web.Data;
using StudySphere.Web.Data.Models;
using StudySphere.Web.Models.Admin;
using StudySphere.Web.Services;
using Xunit;

namespace StudySphere.Tests;

public class CourseServiceTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var category = new Category { Id = 1, Name = "Web", Description = "Web category" };
        context.Categories.Add(category);
        context.Courses.Add(new Course
        {
            Id = 1,
            Title = "ASP.NET Advanced",
            Description = "Deep dive into architecture and MVC.",
            InstructorName = "Ivan Teacher",
            CategoryId = 1,
            StartDate = DateTime.UtcNow.Date,
            DurationWeeks = 8,
            ImageUrl = "https://example.com/img.jpg"
        });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCourses()
    {
        using var context = this.CreateContext();
        var service = new CourseService(context);

        var result = await service.GetAllAsync("ASP.NET", null, 1);

        Assert.Single(result.PagedCourses.Items);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistCourse()
    {
        using var context = this.CreateContext();
        var service = new CourseService(context);

        var id = await service.CreateAsync(new AdminCourseInputModel
        {
            Title = "EF Core",
            Description = "Entity Framework Core hands-on training for real projects.",
            InstructorName = "Maria",
            CategoryId = 1,
            StartDate = DateTime.UtcNow.Date.AddDays(3),
            DurationWeeks = 5,
            ImageUrl = "https://example.com/ef.jpg"
        });

        Assert.True(id > 0);
        Assert.Equal(2, context.Courses.Count());
    }

    [Fact]
    public async Task EnrollAsync_ShouldCreateEnrollment()
    {
        using var context = this.CreateContext();
        context.Users.Add(new ApplicationUser { Id = "user-1", UserName = "u", Email = "u@a.com" });
        context.SaveChanges();

        var service = new CourseService(context);
        await service.EnrollAsync(1, "user-1");

        Assert.Single(context.Enrollments);
    }
}
