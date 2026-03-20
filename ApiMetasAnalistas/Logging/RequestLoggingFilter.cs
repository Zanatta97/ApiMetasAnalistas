using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiMetasAnalistas.Logging
{
    public class RequestLoggingFilter : IActionFilter
    {
        private readonly ILogger<RequestLoggingFilter> _logger;

        public RequestLoggingFilter(ILogger<RequestLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{Method} - {Path} - Iniciada a Requisição",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{Method} - {Path} - Finalizada a Requisição com StatusCode {StatusCode}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                context.HttpContext.Response.StatusCode);
        }
    }
}
