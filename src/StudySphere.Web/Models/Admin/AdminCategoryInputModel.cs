using System.ComponentModel.DataAnnotations;

namespace StudySphere.Web.Models.Admin;

public class AdminCategoryInputModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;
}
