using ApiMetasAnalistas.DTO;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace ApiMetasAnalistas.Middlewares
{
    public static class ApiExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ApiExceptionMiddleware");

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error, "Erro inesperado: {StatusCode} - {ErrorMessage}",
                            context.Response.StatusCode,
                            contextFeature.Error.Message);

                        var dto = new ErrorResponseDTO
                        {
                            StatusCode = context.Response.StatusCode,
                            ErrorMessage = $"Ocorreu um erro interno no servidor: {contextFeature.Error.Message}",
                            StatusMessage = contextFeature.Error.Message,
                            Timestamp = DateTime.UtcNow
                        };

                        logger.LogError(contextFeature.Error, "Erro Inesperado: {Response}", dto.ToString());

                        await context.Response.WriteAsync(dto.ToString());
                    }
                });
            });
        }
    }
}
