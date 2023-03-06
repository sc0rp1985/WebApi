namespace WebApp.CustomMiddlewares
{
    public class ReadRequestBodyMiddleware
    {
        private readonly RequestDelegate _next;

        public ReadRequestBodyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var bodyAsText = await new System.IO.StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;
            await _next.Invoke(context);
        }
    }

    public static class ReadRequestBodyMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ReadRequestBodyMiddleware>();
        }
    }
    
}
