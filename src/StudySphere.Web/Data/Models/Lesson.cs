using System.ComponentModel.DataAnnotations;

namespace StudySphere.Web.Data.Models;

public class Lesson
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Content { get; set; } = string.Empty;

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    public int Order { get; set; }

    public int EstimatedMinutes { get; set; }
}
