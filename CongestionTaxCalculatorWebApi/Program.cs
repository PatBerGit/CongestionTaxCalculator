using CongestionModels.Models.Configs;
using Microsoft.OpenApi.Models;
using CongestionTaxCalculatorWebApi.Filters;
using CongestionTaxCalculatorWebApi.Helpers;
using CongestionTaxCalculatorWebApi.Services.Interfaces;
using CongestionTaxCalculatorWebApi.Services;
using congestion.calculator;

namespace CongestionTaxCalculatorWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var taxPeriods = ResourcesHelper.ReadConfig<TaxPeriods>(builder.Configuration["HourTaxes"]);
            var tollFreeVehicles = ResourcesHelper.ReadConfig<List<string>>(builder.Configuration["TollfreeVehicles"]);
            var holidays = ResourcesHelper.ReadConfig<List<DateTime>>(builder.Configuration["Holidays"]).Select(x => DateOnly.FromDateTime(x));

            var congestionTaxCalc = new CongestionCalculator(holidays, taxPeriods, tollFreeVehicles,
                Convert.ToDouble(builder.Configuration["GracePeriod"]),
                Int32.Parse(builder.Configuration["DayMaxFee"]));
            builder.Services.AddSingleton((ICongestionCalculator)congestionTaxCalc);
            builder.Services.AddTransient<ICongestionTaxCalculatorService, TaxCalculatorService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CongestionTaxApi",
                    Description = "A ASP.NET Core web api that generates the Congestion tax fee for a vehicle in periods of time.",
                    Contact = new OpenApiContact
                    {
                        Name = "Congestion Tax - Api",
                        Email = "patrik.bergsangel@capgemini.com",
                    }
                });

                c.OperationFilter<AddHeaderOperationFilter>("x-api-key",
                    "Required api key to access execute requests",
                    "CongestionTaxController");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}