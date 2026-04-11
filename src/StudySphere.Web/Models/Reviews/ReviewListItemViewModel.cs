namespace StudySphere.Web.Models.Reviews;

public class ReviewListItemViewModel
{
    public int Id { get; set; }

    public string Author { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }
}
