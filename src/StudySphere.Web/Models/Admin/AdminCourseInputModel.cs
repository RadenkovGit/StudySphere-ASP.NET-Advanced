using System.ComponentModel.DataAnnotations;
using StudySphere.Web.Models.Courses;

namespace StudySphere.Web.Models.Admin;

public class AdminCourseInputModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 20)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string InstructorName { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date.AddDays(7);

    [Range(1, 52)]
    public int DurationWeeks { get; set; }

    [Required]
    [Url]
    public string ImageUrl { get; set; } = string.Empty;

    public IEnumerable<CourseCategoryOptionViewModel> Categories { get; set; } = new List<CourseCategoryOptionViewModel>();
}
