using MyShop.Web.ViewModels;

namespace MyShop.Web.Services.Interfaces
{
    public interface IOrderService
    {
        public IEnumerable<OrderVM> GetAllOrderVMs();
        public OrderDetailsVM GetOrderDetails(int id);

	}
}
