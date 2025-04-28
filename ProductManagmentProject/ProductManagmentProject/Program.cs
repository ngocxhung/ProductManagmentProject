using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Hubs;
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
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký Session
            builder.Services.AddSession();
            builder.Services.AddSignalR();

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
            //Sử dụng SignalR 
           

            // Kích hoạt Session
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var path = context.Request.Path;
                if (!path.StartsWithSegments("/Auth") && string.IsNullOrEmpty(context.Session.GetString("UserEmail")))
                {
                    context.Response.Redirect("/Auth/Login");
                    return;
                }
                await next();
            });
            app.MapHub<NotificationHub>("/notificationHub");
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
