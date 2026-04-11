using StudySphere.Web.Models.Paging;

namespace StudySphere.Web.Models.Tasks;

public class TaskIndexViewModel
{
    public string? SearchTerm { get; set; }

    public bool ShowCompleted { get; set; }

    public PagedResult<TaskListItemViewModel> PagedTasks { get; set; } = new();
}
