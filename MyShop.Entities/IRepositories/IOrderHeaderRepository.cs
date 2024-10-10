using MyShop.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.IRepositories
{
	public interface IOrderHeaderRepository:IBaseRepository<OrderHeader>
	{
		public void ChangeStatus(int orderID, string orderStatus, string? PaymentStatus);
	}
}
