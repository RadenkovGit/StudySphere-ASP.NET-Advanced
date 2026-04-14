using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Extensions;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Controllers;

public class CoursesController : Controller
{
    private readonly ICourseService courseService;

    public CoursesController(ICourseService courseService)
    {
        this.courseService = courseService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int page = 1)
    {
        var model = await this.courseService.GetAllAsync(searchTerm, categoryId, page);
        return this.View(model);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var model = await this.courseService.GetDetailsAsync(id, this.User.GetId());
        if (model is null)
        {
            return this.RedirectToAction("StatusCodePage", "Error", new { statusCode = 404 });
        }

        return this.View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Enroll(int id)
    {
        var userId = this.User.GetId()!;
        await this.courseService.EnrollAsync(id, userId);
        this.TempData["SuccessMessage"] = "You successfully enrolled in the course.";
        return this.RedirectToAction(nameof(this.Details), new { id });
    }
}
