using MyShop.DAL.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAL.Repositories
{
    public class UserRepository:BaseRepository<AppUser>,IUserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext appDbContext):base(appDbContext)
        {
            this.appDbContext = appDbContext;
        }
    }
}
