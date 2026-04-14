using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StudySphere.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    [Route("{statusCode}")]
    public IActionResult StatusCodePage(int statusCode)
    {
        return statusCode switch
        {
            404 => this.View("NotFound"),
            500 => this.View("ServerError"),
            _ => this.View("ServerError")
        };
    }

    [Route("ServerError")]
    public IActionResult ServerError()
    {
        var exceptionFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        this.ViewData["Path"] = exceptionFeature?.Path;
        return this.View();
    }
}
