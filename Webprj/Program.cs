using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webprj.Models;

namespace Webprj
{
    public class Program
    {
        public static void Main ( string[] args )
        {
            var builder = WebApplication.CreateBuilder (args);

            builder.Services.AddDbContext<Test2WebContext> (options =>
            options.UseSqlServer (builder.Configuration.GetConnectionString ("DefaultConnection")));
            builder.Services.AddIdentity<Customer , IdentityRole<int>>(options =>
            {
                options.User.RequireUniqueEmail = true;      
            })
            .AddEntityFrameworkStores<Test2WebContext>()
            .AddDefaultTokenProviders();
            // Add services to the container.
            builder.Services.AddControllersWithViews ();

            var app = builder.Build ();

            if (!app.Environment.IsDevelopment ())
            {
                app.UseExceptionHandler ("/Home/Error");
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseAuthentication ();

            app.UseAuthorization ();

            app.MapControllerRoute (
                name: "default" ,
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run ();
        }
    }
}