using MyShop.Entities.Models;

namespace MyShop.Web.ViewModels
{
	public class OrderDetailsVM
	{
		public OrderHeader  OrderHeader { get; set; }
		public IEnumerable< OrderDetails> OrderDetails { get; set; }
	}
}
