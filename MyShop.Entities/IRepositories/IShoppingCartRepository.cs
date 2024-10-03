using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.IRepositories
{
    public interface IShoppingCartRepository:IBaseRepository<ShoppingCart> 
    {
        public int IncreaseCount(ShoppingCart shoppingCart, int count);
        public int DecreaseCount(ShoppingCart shoppingCart, int count);

    }
}
