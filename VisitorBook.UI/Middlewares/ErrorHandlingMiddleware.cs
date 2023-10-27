using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception exception)
            {
                //string routeWhereExceptionOccured = context.Request.Path;

                //var path = JsonSerializer.Serialize(routeWhereExceptionOccured);

                //var result = new ErrorViewModel
                //{
                //    Path = path
                //};

                //if (exception is AggregateException ae)
                //{
                //    var messages = ae.InnerExceptions.Select(x => x.Message).ToList();

                //    result.ErrorMessages = messages;

                //    string messagesJson = JsonSerializer.Serialize(result);

                //    context.Items["ErrorMessagesJson"] = messagesJson;
                //}

                //else
                //{
                //    string message = exception.Message;

                //    result.ErrorMessages = new List<string> { message };

                //    string messageJson = JsonSerializer.Serialize(result);

                //    context.Items["ErrorMessagesJson"] = messageJson;
                //}

                //HandleError(context);
            }
        }

        private void HandleError(HttpContext context)
        {
            string messagesJson = context.Items["ErrorMessagesJson"] as string;
            string redirectUrl = $"/Home/Error?messages={Base64UrlTextEncoder.Encode(Encoding.ASCII.GetBytes(messagesJson))}";

            context.Response.Redirect(redirectUrl);
        }
    }
}
