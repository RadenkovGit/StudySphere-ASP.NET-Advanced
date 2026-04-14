using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Extensions;
using StudySphere.Web.Models.Tasks;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Controllers;

[Authorize]
public class TasksController : Controller
{
    private readonly ITaskService taskService;

    public TasksController(ITaskService taskService)
    {
        this.taskService = taskService;
    }

    public async Task<IActionResult> Index(string? searchTerm, bool showCompleted = false, int page = 1)
    {
        var model = await this.taskService.GetMyTasksAsync(this.User.GetId()!, searchTerm, showCompleted, page);
        return this.View(model);
    }

    public IActionResult Create()
    {
        return this.View(new TaskInputModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        await this.taskService.CreateAsync(this.User.GetId()!, model);
        this.TempData["SuccessMessage"] = "Task created successfully.";
        return this.RedirectToAction(nameof(this.Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await this.taskService.GetForEditAsync(id, this.User.GetId()!);
        if (model is null)
        {
            return this.RedirectToAction("StatusCodePage", "Error", new { statusCode = 404 });
        }

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, TaskInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var isUpdated = await this.taskService.UpdateAsync(id, this.User.GetId()!, model);
        if (!isUpdated)
        {
            return this.RedirectToAction("StatusCodePage", "Error", new { statusCode = 404 });
        }

        this.TempData["SuccessMessage"] = "Task updated successfully.";
        return this.RedirectToAction(nameof(this.Index));
    }

    public IActionResult Delete(int id)
    {
        this.ViewBag.TaskId = id;
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        await this.taskService.DeleteAsync(id, this.User.GetId()!);
        this.TempData["SuccessMessage"] = "Task deleted successfully.";
        return this.RedirectToAction(nameof(this.Index));
    }

    [HttpPost]
    public async Task<IActionResult> Toggle(int id)
    {
        await this.taskService.ToggleStatusAsync(id, this.User.GetId()!);
        return this.RedirectToAction(nameof(this.Index));
    }
}
