using BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;
using System.Text;
using WebApp.CustomMiddlewares;

namespace WebApp.Filters
{
    public class LoggingFilter : IActionFilter
    {
        readonly ILogger _logger;
        readonly ILogService _logService;
        readonly Stopwatch sw = new Stopwatch();
        readonly Guid logContext;
        public LoggingFilter(ILoggerFactory loggerFactory, ILogService logService)
        {
            _logger = loggerFactory.CreateLogger<LoggingFilter>();
            _logService = logService;
            logContext = Guid.NewGuid();
            _logger.LogInformation("{diaplayName} created", typeof(LoggingFilter));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Controller is ControllerBase controller)
            {
                var methodName = controller.ControllerContext.RouteData.Values["action"]?.ToString(); ;
                var controllerName = controller.ControllerContext.RouteData.Values["controller"]?.ToString();
                var result = context.Result as IStatusCodeActionResult;
                var statusCode = result?.StatusCode;
                sw.Stop();                
                var logDto = new LogDto
                {
                    Controller = controllerName ?? string.Empty,
                    Method = methodName ?? string.Empty,
                    Duration = sw.ElapsedMilliseconds,
                    Correlation = logContext,
                    Logged = DateTime.Now,
                    Parameters = string.Empty,
                    Message = string.Empty,
                    Status = statusCode ?? 0, //controller.HttpContext.Response.StatusCode,
                };
                _logService.AddLog(logDto).Wait();
                _logger.LogInformation("{displayName} executed; exec time(ms) - {execTime}. correlation ({correlation})  ", context.ActionDescriptor.DisplayName, sw.ElapsedMilliseconds, logContext);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is ControllerBase controller)
            {   
                var methodName = controller.ControllerContext.RouteData.Values["action"]?.ToString(); ;
                var controllerName = controller.ControllerContext.RouteData.Values["controller"]?.ToString();                                               
                //var body = context.HttpContext?.Request?.BodyToString();
                var paramStr = string.Join(",", context.ActionArguments.Select(x => $"{{{x.Key} = {ParamToString(x.Value)}}}"));
                var logDto = new LogDto
                {
                    Controller = controllerName ?? string.Empty,
                    Method = methodName ?? string.Empty,
                    Duration = 0,
                    Correlation = logContext,
                    Logged = DateTime.Now,
                    Parameters = paramStr,
                    Message = string.Empty,
                };
                _logService.AddLog(logDto).Wait();
                sw.Start();
                _logger.LogInformation("{displayName}: {controller} {method} {params} - is executing. correlation ({correlation})", context.ActionDescriptor.DisplayName, controllerName, methodName, paramStr,logContext);
            }
        }

        string ParamToString(object param)
        {
            if (param == null) return string.Empty;
            if (param is IEnumerable<object> collection)
                return string.Join(";", collection.Select(x => x.ToString()));
            return param?.ToString() ?? string.Empty;
        }
    }

    public static class FilterExt
    {
        public static string BodyToString(this HttpRequest request)
        {
            var returnValue = string.Empty;
            request.EnableBuffering();            
            request.Body.Position = 0;            
            using (var stream = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
            {
                returnValue = stream.ReadToEnd();
            }            
            request.Body.Position = 0;
            return returnValue;
        }
    }
}
