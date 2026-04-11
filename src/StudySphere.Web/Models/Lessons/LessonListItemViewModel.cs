namespace StudySphere.Web.Models.Lessons;

public class LessonListItemViewModel
{
    public int Id { get; set; }

    public int Order { get; set; }

    public string Title { get; set; } = string.Empty;

    public int EstimatedMinutes { get; set; }
}
