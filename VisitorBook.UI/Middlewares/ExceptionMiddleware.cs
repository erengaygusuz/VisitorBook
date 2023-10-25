using System.Net;
using VisitorBook.Core.Models;

namespace VisitorBook.UI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(new ErrorDetail()
            {
                StatusCode = context.Response.StatusCode,
                ExceptionType = exception.GetType().Name,
                ExceptionTitle = "An error occured..",
                ExceptionMessage = exception.Message,
                RequestMethod = context.Request.Method,
                RequestPath = context.Request.Path
            }.ToString());            
        }
    }
}
