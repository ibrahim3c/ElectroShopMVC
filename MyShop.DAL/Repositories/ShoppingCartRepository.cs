using MyShop.DAL.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;

namespace MyShop.DAL.Repositories
{
    public class ShoppingCartRepository: BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(AppDbContext dbContext):base(dbContext)
        {
            
        }

        public int DecreaseCount(ShoppingCart shoppingCart, int count)
        {
            return shoppingCart.Count-count;    
        }

        public int IncreaseCount(ShoppingCart shoppingCart, int count)
        {
            return shoppingCart.Count + count;
        }
    }
}
