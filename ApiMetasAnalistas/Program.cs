
using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Logging;
using ApiMetasAnalistas.Middlewares;
using ApiMetasAnalistas.Repositories;
using ApiMetasAnalistas.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace ApiMetasAnalistas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<RequestLoggingFilter>();
                options.Filters.Add<ExceptionLoggingFilter>();
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(connectionString));

            
            //Repositories
            builder.Services.AddScoped<IAnalystRepository, AnalystRepository>();
            builder.Services.AddScoped<IOccurrenceRepository, OccurrenceRepository>();
            builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();
            builder.Services.AddScoped<IRegionRepository, RegionRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            
            //Services
            builder.Services.AddScoped<IAnalystService, AnalystService>();
            builder.Services.AddScoped<IOccurrenceService, OccurrenceService>();
            builder.Services.AddScoped<IHolidayService, HolidayService>();
            builder.Services.AddScoped<IRegionService, RegionService>();
            builder.Services.AddScoped<ITicketService, TicketService>();

            #region Logs
            
            builder.Services.AddScoped<RequestLoggingFilter>();

            var customLogLevelString = builder.Configuration["Logging:CustomLog:LogLevel"] ?? "Information";
            var customLogLevel = Enum.Parse<LogLevel>(customLogLevelString);

            var logPath = builder.Configuration["Logging:CustomLog:LogFilePath"] ?? @$"c:\temp\";

            builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
            {
                LogLevel = customLogLevel,
                LogPath = logPath,
                LogFile = $"Log_ApiMetasAnalistas_{DateTime.Now:yyyyMMdd}.log"
            }));
            
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.ConfigureExceptionHandler();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
