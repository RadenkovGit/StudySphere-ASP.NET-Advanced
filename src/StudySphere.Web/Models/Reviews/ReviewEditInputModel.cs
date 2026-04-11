using System.ComponentModel.DataAnnotations;

namespace StudySphere.Web.Models.Reviews;

public class ReviewEditInputModel
{
    public int Id { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Comment { get; set; } = string.Empty;
}
