using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Controllers;

public class HomeController : Controller
{
    private readonly ICourseService courseService;

    public HomeController(ICourseService courseService)
    {
        this.courseService = courseService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await this.courseService.GetAllAsync(null, null, 1);
        return this.View(model);
    }

    public IActionResult Privacy()
    {
        return this.View();
    }
}
