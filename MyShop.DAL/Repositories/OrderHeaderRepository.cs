using MyShop.DAL.Data;
using MyShop.Entities.IRepositories;
using MyShop.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAL.Repositories
{
	
	public class OrderHeaderRepository:BaseRepository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly AppDbContext appDbContext;

		public OrderHeaderRepository( AppDbContext appDbContext):base(appDbContext)
        {
			this.appDbContext = appDbContext;
		}

		public void ChangeStatus(int orderID, string orderStatus, string PaymentStatus)
		{
			var order=appDbContext.orderHeaders.Find(orderID);

			if (order != null)
			{
				order.OrderStatus = orderStatus;
				order.PaymentStatus = PaymentStatus;
			}
			appDbContext.SaveChanges();
		}
	}
}
