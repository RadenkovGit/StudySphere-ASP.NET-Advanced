using System.ComponentModel.DataAnnotations;

namespace StudySphere.Web.Models.Tasks;

public class TaskInputModel
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; } = DateTime.UtcNow.Date.AddDays(1);

    [Required]
    public string Priority { get; set; } = "Medium";
}
