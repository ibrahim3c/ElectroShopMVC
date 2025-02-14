using Microsoft.EntityFrameworkCore;
using MyShop.DAL.Data;
using MyShop.DAL.Repositories;
using MyShop.Entities.IRepositories;
using Microsoft.AspNetCore.Identity;
using MyShop.Entities.Models;
using MyShop.Web.Constants;
using Stripe;
using MyShop.Web.Extensions;
using MyShop.DAL.DBInializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region myConfig

// dbcontext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection" ?? throw new InvalidOperationException("No Connection String was found"));
builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(connectionString));


// Identity
//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(3);
    })
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultUI()
              .AddDefaultTokenProviders();


// repository pattern with unit of work
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// dependency injection
builder.Services.AddServiceDependencyInjection();



// Stripe
builder.Services.Configure<StripeData>(builder.Configuration.GetSection("Stripe"));

// session cards
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
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
app.UseStaticFiles();

app.UseRouting();  // First, define the routing

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();


SeedDb();

app.UseAuthentication();
app.UseAuthorization();  // Authorization goes after routing
app.UseSession();
app.MapRazorPages(); // to use Razor Pages

//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "areas",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.Run();

 void SeedDb()
{
    using var scope = app.Services.CreateScope();
    {
        var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        initializer.Initialize();
        

    }

}