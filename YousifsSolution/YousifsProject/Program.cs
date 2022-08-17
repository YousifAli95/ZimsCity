
using Microsoft.EntityFrameworkCore;
using YousifsProject.Models;
using YousifsProject.Models.Entities;

namespace YousifsProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<HouseService>(); //( Singleton = Statisk , Transient = Dynamisk )
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<CityContext>(o => o.UseSqlServer(connString));
            var app = builder.Build();
            app.UseRouting();
            app.UseEndpoints(o => o.MapControllers());
            app.UseStaticFiles();
            app.Run();
        }
    }
}