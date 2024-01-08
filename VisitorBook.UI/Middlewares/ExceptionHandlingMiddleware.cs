using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.ExceptionLogDtos;
using VisitorBook.Core.Entities;

namespace VisitorBook.UI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _provider;

        public ExceptionHandlingMiddleware(RequestDelegate next, IServiceProvider provider)
        {
            _next = next;
            _provider = provider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception exception)
            {
                await Handle(context, exception);
            }
        }

        private async Task Handle(HttpContext context, Exception exception)
        {
            var exceptionLogRequestDto = new ExceptionLogRequestDto()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ExceptionType = exception.GetType().Name,
                ExceptionTitle = ((HttpStatusCode)StatusCodes.Status500InternalServerError).ToString(),
                ExceptionMessage = exception.InnerException != null ? exception.InnerException.Message : exception.Message,
                RequestMethod = context.Request.Method,
                RequestPath = context.Request.Path
            };

            using (var scope = _provider.CreateScope())
            {
                var exceptionLogservice = scope.ServiceProvider.GetRequiredService<IService<ExceptionLog>>();

                await exceptionLogservice.AddAsync(exceptionLogRequestDto);
            }

            var tempData = context.RequestServices.GetRequiredService<ITempDataProvider>().LoadTempData(context);

            tempData["StatusCode"] = exceptionLogRequestDto.StatusCode;
            tempData["ExceptionMessage"] = exceptionLogRequestDto.ExceptionMessage;

            context.RequestServices.GetRequiredService<ITempDataProvider>().SaveTempData(context, tempData);

            context.Response.Redirect("/Error");
        }
    }
}
