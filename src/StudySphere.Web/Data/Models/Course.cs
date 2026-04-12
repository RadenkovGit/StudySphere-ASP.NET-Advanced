using System.ComponentModel.DataAnnotations;

namespace StudySphere.Web.Data.Models;

public class Course
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(100)]
    public string InstructorName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public int DurationWeeks { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<Lesson> Lessons { get; set; } = new HashSet<Lesson>();

    public ICollection<ResourceItem> Resources { get; set; } = new HashSet<ResourceItem>();

    public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();

    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
}
