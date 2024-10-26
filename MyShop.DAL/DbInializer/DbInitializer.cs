using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.DAL.Data;
using MyShop.Entities.Models;


namespace MyShop.DAL.DBInializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<AppUser> userManager;
        private readonly AppDbContext appDbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public DbInitializer(UserManager<AppUser> userManager ,AppDbContext appDbContext,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.appDbContext = appDbContext;
            this.roleManager = roleManager;
        }
        public  void Initialize()
        {
           
            if (appDbContext.Database.GetMigrations().Any())
            {
            // Apply migrations
              appDbContext.Database.Migrate(); 
               
            }


          
                // user
                var createUserResult =  userManager.CreateAsync(new AppUser
                {
                    Email = "Admin123@gmail.com",
                    FirstName = "Ibrahim",
                    LastName = "Hany",
                    Address = "Egypt",
                    PhoneNumber = "01012345678",
                    UserName= "Admin123@gmail.com"
                }, "Ibrahim1020+").GetAwaiter().GetResult();

              

                var existingUser = appDbContext.Users.FirstOrDefault(u=>u.Email=="Admin123@gmail.com");
            

            // Add user to role if not already assigned
            if (! userManager.IsInRoleAsync(existingUser, "Admin").Result)
            {
                 userManager.AddToRoleAsync(existingUser, "Admin").GetAwaiter().GetResult();
            }

        }

    }
}
