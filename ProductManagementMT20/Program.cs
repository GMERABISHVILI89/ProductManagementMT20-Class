using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagementMT20.Interfaces;
using ProductManagementMT20.Models;
using ProductManagementMT20.Models.Entities;
using ProductManagementMT20.Policies.Handlers;
using ProductManagementMT20.Policies.Requirements;
using ProductManagementMT20.Services;

namespace ProductManagementMT20
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("FiveYearsEmployee", policy =>
                policy.Requirements.Add(new FiveYearsRequirement(5)));

                options.AddPolicy("AdminClaimPolicy", policy =>
                policy.RequireClaim("AdminClaim"));
            });

            builder.Services.AddScoped<IAuthorizationHandler, FiveYearsHandler>();

            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.AccessDeniedPath = "/Auth/AccessDenied";
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
