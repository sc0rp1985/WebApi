using BLL;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using WebApp.Utils;

namespace WebApp.Filters
{
    public class ExceptionLoggingFilter : IExceptionFilter
    {
        readonly ILogger _logger;
        readonly ILogService _logService;        
        
        public ExceptionLoggingFilter(ILoggerFactory loggerFactory, ILogService logService)
        {
            _logger = loggerFactory.CreateLogger<LoggingFilter>();
            _logService = logService;                        
        }


        public void OnException(ExceptionContext context)
        {            
            context.HttpContext.Request.Headers.TryGetValue("RequestGuid", out var logCorrelation);
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            var logDto = new LogDto
            {
                Controller = actionName ?? string.Empty,
                Method = context.HttpContext.Request.Path,
                Correlation = new Guid(logCorrelation.ToString()),//logCorrelation,
                Logged = DateTime.Now,
                Parameters = string.Empty,
                Message = exceptionMessage,
                Status = (int)HttpStatusCode.InternalServerError
            };
            _logService.AddLog(logDto).Wait();

            context.HttpContext.HandleExceptionMessageAsync(context.Exception).ConfigureAwait(false);
            context.ExceptionHandled = true;
        }
    }
}
