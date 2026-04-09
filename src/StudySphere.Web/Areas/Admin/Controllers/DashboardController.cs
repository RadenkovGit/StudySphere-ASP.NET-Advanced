using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Areas.Admin.Controllers;

public class DashboardController : BaseAdminController
{
    private readonly IAdminDashboardService adminDashboardService;

    public DashboardController(IAdminDashboardService adminDashboardService)
    {
        this.adminDashboardService = adminDashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await this.adminDashboardService.GetStatisticsAsync();
        return this.View(model);
    }
}
