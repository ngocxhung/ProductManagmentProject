using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Models;

namespace ProductManagmentProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Đăng ký DbContext
            builder.Services.AddDbContext<FoodManagmentContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DB")));

            // Đăng ký Session
            builder.Services.AddSession();

            // Đăng ký IHttpContextAccessor để sử dụng trong view
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            // Kích hoạt Session
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
