using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Controllers;

[Authorize]
public class LessonsController : Controller
{
    private readonly ILessonService lessonService;

    public LessonsController(ILessonService lessonService)
    {
        this.lessonService = lessonService;
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await this.lessonService.GetByIdAsync(id);
        if (model is null)
        {
            return this.RedirectToAction("StatusCodePage", "Error", new { statusCode = 404 });
        }

        return this.View(model);
    }
}
