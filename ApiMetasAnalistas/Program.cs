
using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
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

            builder.Services.AddControllers();
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
