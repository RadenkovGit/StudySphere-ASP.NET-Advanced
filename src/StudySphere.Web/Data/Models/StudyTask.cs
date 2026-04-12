using System.ComponentModel.DataAnnotations;

namespace StudySphere.Web.Data.Models;

public class StudyTask
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    [MaxLength(20)]
    public string Priority { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
