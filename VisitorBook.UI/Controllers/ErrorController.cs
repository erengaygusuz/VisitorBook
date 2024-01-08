using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.ExceptionLogDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.UI.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly IService<ExceptionLog> _exceptionLogService;

        public ErrorController(IService<ExceptionLog> exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }

        [Route("/Error")]
        [HttpGet]
        public async Task<IActionResult> Error()
        {
            var errorViewModel = new ErrorViewModel()
            {
                StatusCode = TempData["StatusCode"].ToString(),
                ExceptionMessage = TempData["ExceptionMessage"].ToString()
            };

            return View(errorViewModel);
        }

        [Route("/Error/{statusCode}")]
        [HttpGet]
        public async Task<IActionResult> Error(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var exceptionLogRequestDto = new ExceptionLogRequestDto()
            {
                StatusCode = statusCode,
                ExceptionTitle = ((HttpStatusCode)statusCode).ToString(),
                RequestMethod = HttpContext.Request.Method,
                RequestPath = feature.OriginalPath
            };

            await _exceptionLogService.AddAsync(exceptionLogRequestDto);

            var errorViewModel = new ErrorViewModel()
            {
                StatusCode = statusCode.ToString(),
                ExceptionMessage = ((HttpStatusCode)statusCode).ToString()
            };

            return View(errorViewModel);
        }
    }
}
