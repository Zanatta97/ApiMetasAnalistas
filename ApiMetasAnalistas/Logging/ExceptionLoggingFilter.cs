using ApiMetasAnalistas.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Logging
{
    public class ExceptionLoggingFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionLoggingFilter> _logger;

        public ExceptionLoggingFilter(ILogger<ExceptionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var message = context.Exception.Message;

            switch (context.Exception)
            {
                case ArgumentException or ArgumentNullException:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case InvalidOperationException: // Conflitos ou regras de negócio furadas
                    statusCode = StatusCodes.Status409Conflict;
                    break;
                case DbUpdateException dbEx:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Erro de integridade ou gravação no banco de dados.";
                    _logger.LogError(dbEx, "Erro de banco: {Detalhe}", dbEx.InnerException?.Message);
                    break;
                default:
                    // Deixa passar porque o Middleware Global(ApiExceptionMiddleware) vai tratar erros inesperados(500)
                    return;
            }

            var errorResponse = new ErrorResponseDTO
            {
                StatusCode = statusCode,
                ErrorMessage = context.Exception.Message, // Você pode retornar a msg original se forem erros de domínio mapeados
                StatusMessage = ObterStatusMessage(statusCode),
                Timestamp = DateTime.UtcNow
            };

            _logger.LogError("{StatusCode} - {ErrorMessage}", statusCode, message);
            _logger.LogError(errorResponse.ToString());

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }

        private string ObterStatusMessage(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            404 => "Not Found",
            409 => "Conflict",
            _ => "Internal Server Error"
        };
    }
}
