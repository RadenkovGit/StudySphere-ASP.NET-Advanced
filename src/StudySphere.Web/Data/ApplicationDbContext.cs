using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudySphere.Web.Data.Models;

namespace StudySphere.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<StudyTask> StudyTasks => Set<StudyTask>();
    public DbSet<ResourceItem> ResourceItems => Set<ResourceItem>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>()
            .Property(x => x.Name)
            .HasMaxLength(60);

        builder.Entity<Course>()
            .HasOne(x => x.Category)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Lesson>()
            .HasOne(x => x.Course)
            .WithMany(x => x.Lessons)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ResourceItem>()
            .HasOne(x => x.Course)
            .WithMany(x => x.Resources)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Enrollment>()
            .HasIndex(x => new { x.UserId, x.CourseId })
            .IsUnique();

        builder.Entity<Enrollment>()
            .HasOne(x => x.Course)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Enrollment>()
            .HasOne(x => x.User)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<StudyTask>()
            .HasOne(x => x.User)
            .WithMany(x => x.StudyTasks)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Review>()
            .HasOne(x => x.Course)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Review>()
            .HasOne(x => x.User)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
