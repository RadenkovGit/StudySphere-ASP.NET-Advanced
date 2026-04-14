using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Extensions;
using StudySphere.Web.Models.Reviews;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly IReviewService reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        this.reviewService = reviewService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            this.TempData["ErrorMessage"] = "Review validation failed.";
            return this.RedirectToAction("Details", "Courses", new { id = model.CourseId });
        }

        await this.reviewService.CreateAsync(this.User.GetId()!, model);
        this.TempData["SuccessMessage"] = "Review added successfully.";
        return this.RedirectToAction("Details", "Courses", new { id = model.CourseId });
    }

    public async Task<IActionResult> Mine()
    {
        var model = await this.reviewService.GetMineAsync(this.User.GetId()!);
        return this.View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await this.reviewService.GetForEditAsync(id, this.User.GetId()!);
        if (model is null)
        {
            return this.RedirectToAction("StatusCodePage", "Error", new { statusCode = 404 });
        }

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ReviewEditInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        await this.reviewService.EditAsync(this.User.GetId()!, model);
        return this.RedirectToAction(nameof(this.Mine));
    }

    public IActionResult Delete(int id)
    {
        this.ViewBag.ReviewId = id;
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        await this.reviewService.DeleteAsync(id, this.User.GetId()!);
        return this.RedirectToAction(nameof(this.Mine));
    }
}
