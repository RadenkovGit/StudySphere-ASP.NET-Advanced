using System.ComponentModel.DataAnnotations;

namespace StudySphere.Web.Data.Models;

public class ResourceItem
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Url { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Summary { get; set; } = string.Empty;

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;
}
