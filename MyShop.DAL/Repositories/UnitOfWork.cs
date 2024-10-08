using MyShop.DAL.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;

namespace MyShop.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IBaseRepository<Category> Categories { get; private set; }
        public IBaseRepository<Product> Products { get; private set; }
        public IShoppingCartRepository ShoppingCarts { get; private set; }

		public IOrderDetailsRepository OrderDetails { get; private set; }

		public IOrderHeaderRepository OrderHeaders   { get; private set; }
        public IUserRepository userRepository { get; private set; }

	public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Categories = new BaseRepository<Category>(_context);
            Products = new BaseRepository<Product>(_context);
            ShoppingCarts= new ShoppingCartRepository(_context);
            OrderDetails = new OrderDetailsRepository(_context);
            OrderHeaders = new OrderHeaderRepository(_context);
            userRepository = new UserRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
