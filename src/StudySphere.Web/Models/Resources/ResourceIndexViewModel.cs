using StudySphere.Web.Models.Paging;

namespace StudySphere.Web.Models.Resources;

public class ResourceIndexViewModel
{
    public string? SearchTerm { get; set; }

    public PagedResult<ResourceListItemViewModel> PagedResources { get; set; } = new();
}
