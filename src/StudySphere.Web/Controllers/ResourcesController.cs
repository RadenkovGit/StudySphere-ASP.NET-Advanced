using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Controllers;

public class ResourcesController : Controller
{
    private readonly IResourceService resourceService;

    public ResourcesController(IResourceService resourceService)
    {
        this.resourceService = resourceService;
    }

    public async Task<IActionResult> Index(string? searchTerm, int page = 1)
    {
        var model = await this.resourceService.GetAllAsync(searchTerm, page);
        return this.View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await this.resourceService.GetByIdAsync(id);
        if (model is null)
        {
            return this.RedirectToAction("StatusCodePage", "Error", new { statusCode = 404 });
        }

        return this.View(model);
    }
}
