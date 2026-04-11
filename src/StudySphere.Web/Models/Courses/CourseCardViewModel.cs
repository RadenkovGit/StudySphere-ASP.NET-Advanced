namespace StudySphere.Web.Models.Courses;

public class CourseCardViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string InstructorName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public double AverageRating { get; set; }
}
