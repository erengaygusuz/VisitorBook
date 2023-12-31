using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var errorViewModel = new ErrorViewModel
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ExceptionType = context.Error.GetType().Name,
                ExceptionTitle = ((HttpStatusCode)StatusCodes.Status500InternalServerError).ToString(),
                ExceptionMessage = context.Error.Message,
                RequestMethod = HttpContext.Request.Method,
                RequestPath = context.Path
            };

            return View(errorViewModel);
        }

        [Route("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var errorViewModel = new ErrorViewModel
            {
                StatusCode = statusCode,
                ExceptionTitle = ((HttpStatusCode)statusCode).ToString(),
                RequestMethod = HttpContext.Request.Method,
                RequestPath = feature.OriginalPath
            };

            return View(errorViewModel);
        }
    }
}
