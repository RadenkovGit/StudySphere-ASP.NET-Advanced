using StudySphere.Web.Models.Resources;

namespace StudySphere.Web.Services.Contracts;

public interface IResourceService
{
    Task<ResourceIndexViewModel> GetAllAsync(string? searchTerm, int page);
    Task<ResourceListItemViewModel?> GetByIdAsync(int id);
}
