using BLL;
using Newtonsoft.Json;
using System.Net;
using WebApp.Utils;

namespace WebApp.CustomMiddlewares
{
    public class ExceptionHandlerMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILoggerFactory _loggerFactory;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Request.Headers.Add("RequestGuid", Guid.NewGuid().ToString());                
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
                context.Request.Headers.TryGetValue("RequestGuid", out var requestGuid);
                logger.LogError($"{ex.Message} - correlation ({requestGuid})");                ;
                await context.HandleExceptionMessageAsync(ex).ConfigureAwait(false);
                
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}


