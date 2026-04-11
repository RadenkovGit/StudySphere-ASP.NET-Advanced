namespace StudySphere.Web.Models.Resources;

public class ResourceListItemViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;
}
