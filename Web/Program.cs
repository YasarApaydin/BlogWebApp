using BlogWebApp.Data.Context;
using BlogWebApp.Data.Extensions;
using BlogWebApp.Entity.Entities;
using BlogWebApp.Service.Describers;
using BlogWebApp.Service.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Net.Http.Headers;
using Web.Filters;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient("github", client =>
{
    client.BaseAddress = new Uri("https://api.github.com/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("BlogWebApp"); 
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", builder.Configuration["GitHub:Token"]);
});


builder.Services.LoadDataLayerExtension(builder.Configuration);
builder.Services.LoadServiceLayerExtension();
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews(
    opt => opt.Filters.Add<ArticleVisitorFilter>()
    )
    .AddNToastNotifyToastr(new ToastrOptions
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 3000
        
    })
    .AddRazorRuntimeCompilation();
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequiredLength = 8;
    opt.Password.RequireDigit = true;
}).AddRoleManager<RoleManager<AppRole>>()
  .AddErrorDescriber<CustomIdentityErrorDescriber>()
  .AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options => {

    
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//fazla denemede 5 dk kilitletirim
    options.Lockout.MaxFailedAccessAttempts = 5;    //5 kez yanl�� gir�e izin veriyorum
    options.Lockout.AllowedForNewUsers = true; // yeni kullan�c�larada ayn� tarife karde�im

});


builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Admin/Auth/Login");
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name="BlogWebApp",
        HttpOnly= true,
        SameSite= SameSiteMode.Strict,
        SecurePolicy= CookieSecurePolicy.SameAsRequest //yay�nlayaca��n zaman always yap 

    };

    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(1);
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");
    


});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/ErrorPage/HandleError", "?code={0}");


app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();




app.MapAreaControllerRoute(
    name:"Admin",
    areaName:"Admin",
    pattern:"Admin/{controller=Home}/{action=Index}/{id?}"
    
    );

app.MapDefaultControllerRoute();

app.Run();
