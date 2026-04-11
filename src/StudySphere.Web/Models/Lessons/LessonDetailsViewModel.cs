namespace StudySphere.Web.Models.Lessons;

public class LessonDetailsViewModel
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string CourseTitle { get; set; } = string.Empty;

    public int Order { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int EstimatedMinutes { get; set; }
}
