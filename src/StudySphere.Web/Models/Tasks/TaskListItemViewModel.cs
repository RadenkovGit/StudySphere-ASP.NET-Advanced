namespace StudySphere.Web.Models.Tasks;

public class TaskListItemViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public string Description { get; set; } = string.Empty;
}
