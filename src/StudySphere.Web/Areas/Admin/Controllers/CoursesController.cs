using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Models.Admin;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Areas.Admin.Controllers;

public class CoursesController : BaseAdminController
{
    private readonly ICourseService courseService;

    public CoursesController(ICourseService courseService)
    {
        this.courseService = courseService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await this.courseService.GetAdminCoursesAsync();
        return this.View(model);
    }

    public async Task<IActionResult> Create()
    {
        return this.View(new AdminCourseInputModel
        {
            Categories = await this.courseService.GetCategoryOptionsAsync()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdminCourseInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.Categories = await this.courseService.GetCategoryOptionsAsync();
            return this.View(model);
        }

        await this.courseService.CreateAsync(model);
        return this.RedirectToAction(nameof(this.Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await this.courseService.GetCourseForEditAsync(id);
        if (model is null)
        {
            return this.RedirectToAction("StatusCodePage", "Error", new { area = "", statusCode = 404 });
        }

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AdminCourseInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            model.Categories = await this.courseService.GetCategoryOptionsAsync();
            return this.View(model);
        }

        await this.courseService.EditAsync(model);
        return this.RedirectToAction(nameof(this.Index));
    }

    public IActionResult Delete(int id)
    {
        this.ViewBag.CourseId = id;
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        await this.courseService.DeleteAsync(id);
        return this.RedirectToAction(nameof(this.Index));
    }
}
