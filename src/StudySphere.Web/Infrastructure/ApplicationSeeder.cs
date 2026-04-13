using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudySphere.Web.Data;
using StudySphere.Web.Data.Models;

namespace StudySphere.Web.Infrastructure;

public class ApplicationSeeder : ISeeder
{
    private readonly ApplicationDbContext context;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<ApplicationUser> userManager;

    public ApplicationSeeder(
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    public async Task SeedAsync()
    {
        await this.context.Database.EnsureCreatedAsync();

        foreach (var role in new[] { "Administrator", "User" })
        {
            if (!await this.roleManager.RoleExistsAsync(role))
            {
                await this.roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        const string adminEmail = "admin@studysphere.local";
        var admin = await this.userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true
            };

            await this.userManager.CreateAsync(admin, "Admin123!");
            await this.userManager.AddToRolesAsync(admin, new[] { "Administrator", "User" });
        }

        if (!this.context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Web Development", Description = "Courses for ASP.NET, APIs and front-end basics." },
                new() { Name = "Databases", Description = "SQL Server, EF Core and database design." },
                new() { Name = "Algorithms", Description = "Problem solving, data structures and complexity." }
            };

            await this.context.Categories.AddRangeAsync(categories);
            await this.context.SaveChangesAsync();

            var courses = new List<Course>
            {
                new() { Title = "ASP.NET Core MVC Mastery", Description = "Build production-ready MVC applications with Identity, EF Core and areas.", InstructorName = "Maria Petrova", CategoryId = categories[0].Id, StartDate = DateTime.UtcNow.Date.AddDays(7), DurationWeeks = 8, ImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3" },
                new() { Title = "Entity Framework Core in Practice", Description = "Work with LINQ, migrations, relationships and performance tips.", InstructorName = "Georgi Ivanov", CategoryId = categories[1].Id, StartDate = DateTime.UtcNow.Date.AddDays(14), DurationWeeks = 6, ImageUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71" },
                new() { Title = "Problem Solving for Developers", Description = "Train algorithmic thinking with real examples and patterns.", InstructorName = "Elena Stoyanova", CategoryId = categories[2].Id, StartDate = DateTime.UtcNow.Date.AddDays(10), DurationWeeks = 10, ImageUrl = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4" }
            };

            await this.context.Courses.AddRangeAsync(courses);
            await this.context.SaveChangesAsync();

            var lessons = new List<Lesson>
            {
                new() { CourseId = courses[0].Id, Order = 1, Title = "Architecture and Layers", Content = "Learn how to split an MVC app into clear layers, services and data models.", EstimatedMinutes = 45 },
                new() { CourseId = courses[0].Id, Order = 2, Title = "Identity and Authorization", Content = "Configure roles, policies and secure actions in ASP.NET Core.", EstimatedMinutes = 55 },
                new() { CourseId = courses[1].Id, Order = 1, Title = "Relationships and Configurations", Content = "Configure one-to-many and many-to-many relations with Fluent API.", EstimatedMinutes = 50 },
                new() { CourseId = courses[2].Id, Order = 1, Title = "Greedy and Sorting", Content = "Introduction to practical patterns for interview and contest tasks.", EstimatedMinutes = 40 }
            };

            var resources = new List<ResourceItem>
            {
                new() { CourseId = courses[0].Id, Title = "Official ASP.NET Docs", Url = "https://learn.microsoft.com/aspnet/core", Summary = "Primary reference for ASP.NET Core MVC and deployment." },
                new() { CourseId = courses[1].Id, Title = "EF Core Docs", Url = "https://learn.microsoft.com/ef/core", Summary = "Reference for LINQ queries, migrations and SQL Server provider." },
                new() { CourseId = courses[2].Id, Title = "Big-O Cheatsheet", Url = "https://www.bigocheatsheet.com/", Summary = "Quick lookup table for algorithmic complexity." }
            };

            await this.context.Lessons.AddRangeAsync(lessons);
            await this.context.ResourceItems.AddRangeAsync(resources);
            await this.context.SaveChangesAsync();
        }
    }
}
