﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZimsCityProject.Loggers;
using ZimsCityProject.Models.Entities;
using ZimsCityProject.Services.Implementations;
using ZimsCityProject.Services.Interfaces;
using ZimsCityProject.Utils;

namespace ZimsCityProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Services.AddScoped<CustomLogger>();

            builder.Services.AddControllersWithViews(o =>
            {
                // Adds antiforgery tokens for all unsafe HTTP methods (PUT, POST, PATCH etc).
                o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            var serviceImplementation = builder.Configuration["ServiceImplementation"];

            switch (serviceImplementation)
            {
                case "HouseServiceDB":
                    builder.Services.AddScoped<ServiceUtilsDB>();
                    builder.Services.AddTransient<IWebApiService, WebApiServiceDB>();
                    builder.Services.AddTransient<IHouseService, HouseServiceDB>();
                    builder.Services.AddTransient<IIdentityService, IdentityServiceDB>();

                    var typeOfConnection = "DefaultConnection";
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    if (string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase))
                    {
                        typeOfConnection = "ProductionConnection";
                    }
                    else
                    {
                        builder.Logging.AddConsole();
                    }

                    var connectionString = builder.Configuration.GetConnectionString(typeOfConnection);
                    builder.Services.AddDbContext<CityContext>(o => o.UseSqlServer(connectionString));
                    ConfigureIdentity(builder, connectionString);
                    builder.Services.AddHttpContextAccessor();
                    break;
                default:
                    throw new InvalidOperationException("Invalid service implementation specified in the configuration.");
            }

            var app = builder.Build();

            app.UseRouting();
            switch (serviceImplementation)
            {
                case "HouseServiceDB":
                    app.UseAuthentication();
                    app.UseAuthorization();
                    break;
                default:
                    // Handle the case when no valid service implementation is specified
                    throw new InvalidOperationException("Invalid service implementation specified in the configuration.");
            }

            app.UseEndpoints(o => o.MapControllers());
            app.UseStaticFiles();
            app.Run();
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder, string connectionString)
        {
            builder.Services.AddDbContext<IdentityDbContext>(o => o.UseSqlServer(connectionString));
            builder.Services.AddSession();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Change default password requirements
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });
        }

    }
}