using Company.Data.Contexts;
using Company.Data.Models;
using Company.Repository.Interfaces;
using Company.Repository.Repositories;
using Company.Service.Interfaces;
using Company.Service.Interfaces.Employee;
using Company.Service.Mapping.DepartmentMap;
using Company.Service.Mapping.EmployeeMap;
using Company.Service.Services.Department;
using Company.Service.Services.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Web;

public class Program
{
    public static void Main(string[] args)
    {

        #region Service Injection
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<CompanyDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // to get conntection string from appsetting
        });
        
        // builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IDepartmentService,DepartmentService>();

        builder.Services.AddAutoMapper(x => x.AddProfile(new EmployeeProfile())); // first one
        builder.Services.AddAutoMapper(x => x.AddProfile(new DepartmentProfile()));
        
       // builder.Services.AddScoped<ILogger<EmployeeService>, Logger<EmployeeService>>();

       builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
           {
                config.SignIn.RequireConfirmedAccount = false;
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireUppercase = true;
                config.Password.RequireNonAlphanumeric = false;
                config.Lockout.MaxFailedAccessAttempts = 3;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
           }
       ).AddEntityFrameworkStores<CompanyDbContext>().AddDefaultTokenProviders();

       builder.Services.ConfigureApplicationCookie(options =>
       {
           options.Cookie.HttpOnly = true;
           options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
           options.SlidingExpiration = true;
           options.LoginPath = "/Account/Login";
           options.LogoutPath = "/Account/Logout";
           options.AccessDeniedPath = "/Account/AccessDenied";
           options.Cookie.Name = "CompanyCookie";
           options.Cookie.SameSite = SameSiteMode.Strict;
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
       });
        #endregion



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization(); // who are you
        app.UseAuthentication(); // are you have permission?

        app.MapStaticAssets();
        // app.MapControllerRoute(
        //         name: "default",
        //         pattern: "{controller=Home}/{action=Index}/{id?}")
        //     .WithStaticAssets();

        
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=SignUp}")
            .WithStaticAssets();
        
        app.Run();
    }
}
