using Microsoft.EntityFrameworkCore;
using StudySphere.Web.Data;
using StudySphere.Web.Data.Models;
using StudySphere.Web.Models.Tasks;
using StudySphere.Web.Services;
using Xunit;

namespace StudySphere.Tests;

public class TaskServiceTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        context.Users.Add(new ApplicationUser { Id = "user-1", UserName = "user", Email = "user@test.com" });
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task CreateAsync_ShouldAddTask()
    {
        using var context = this.CreateContext();
        var service = new TaskService(context);

        var id = await service.CreateAsync("user-1", new TaskInputModel
        {
            Title = "Prepare project",
            Description = "Implement services and controllers for the final project.",
            DueDate = DateTime.UtcNow.Date.AddDays(2),
            Priority = "High"
        });

        Assert.True(id > 0);
        Assert.Single(context.StudyTasks);
    }

    [Fact]
    public async Task ToggleStatusAsync_ShouldChangeCompletionState()
    {
        using var context = this.CreateContext();
        context.StudyTasks.Add(new StudyTask
        {
            Id = 1,
            UserId = "user-1",
            Title = "Task",
            Description = "Description",
            DueDate = DateTime.UtcNow.Date,
            Priority = "Low"
        });
        context.SaveChanges();

        var service = new TaskService(context);
        await service.ToggleStatusAsync(1, "user-1");

        Assert.True(context.StudyTasks.First().IsCompleted);
    }
}
