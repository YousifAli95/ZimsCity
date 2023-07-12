
using Microsoft.EntityFrameworkCore;
using YousifsProject.Models.Entities;
using YousifsProject.Services;

namespace YousifsProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            var serviceImplementation = builder.Configuration["ServiceImplementation"];

            switch (serviceImplementation)
            {
                case "HouseServiceDB":
                    builder.Services.AddTransient<IHouseService, HouseServiceDB>();
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var typeOfConnection = string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase) ? "DefaultConnection" : "ProductionConnection";
                    var connectionString = builder.Configuration.GetConnectionString(typeOfConnection);
                    builder.Services.AddDbContext<CityContext>(o => o.UseSqlServer(connectionString));
                    break;
                default:
                    // Handle the case when no valid service implementation is specified
                    throw new InvalidOperationException("Invalid service implementation specified in the configuration.");
            }

            var app = builder.Build();
            app.UseRouting();
            app.UseEndpoints(o => o.MapControllers());
            app.UseStaticFiles();
            app.Run();
        }
    }
}