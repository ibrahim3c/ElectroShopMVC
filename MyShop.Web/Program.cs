using Microsoft.EntityFrameworkCore;
using MyShop.DAL.Data;
using MyShop.DAL.Repositories;
using MyShop.Entities.IRepositories;
using MyShop.Services.Implementations;
using MyShop.Services.Interfaces;
using MyShop.Web.Services.Implementations;
using MyShop.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



#region myConfig

// dbcontext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection" ?? throw new InvalidOperationException("No Connection String was found"));
builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(connectionString));

// repository pattern with unit of work
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// dependency injection
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IFileService, FileService>();


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
app.UseAuthorization();  // Authorization goes after routing

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
