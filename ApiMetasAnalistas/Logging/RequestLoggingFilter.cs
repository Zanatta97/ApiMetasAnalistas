using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

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

            string? responseContent = null;

            //Ativar o log de Debug no appsettings.Development.json para ver o conteúdo do retorno da requisição
            //TODO: alterar para Debug antes de finalizar
            if (_logger.IsEnabled(LogLevel.Information))
            {
                if (context.Result is ObjectResult objectResult)
                {
                    responseContent = JsonSerializer.Serialize(objectResult.Value);
                }
                else if (context.Result is JsonResult jsonResult)
                {
                    responseContent = JsonSerializer.Serialize(jsonResult.Value);
                }
                else if (context.Result is ContentResult contentResult)
                {
                    responseContent = contentResult.Content;
                }

                _logger.LogInformation("Retorno da requisição: {Response}", responseContent ?? "Sem retorno");
            }

            // Mantém apenas os dados básicos no nível Information (que é mais leve e comum em produção)
            _logger.LogInformation("{Method} - {Path} - Finalizada a Requisição com StatusCode {StatusCode}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                context.HttpContext.Response.StatusCode);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{Method} - {Path} - Iniciada a Requisição",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path);
        }
    }
}
