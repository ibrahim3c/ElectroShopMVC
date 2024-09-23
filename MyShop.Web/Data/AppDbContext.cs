using Microsoft.EntityFrameworkCore;
using MyShop.Web.Models;
using System.Data;

namespace MyShop.Web.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext>options):base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
    }
}
