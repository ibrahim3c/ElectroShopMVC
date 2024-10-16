namespace MyShop.Web.ViewModels
{
    public class DashboardVM
    {
        public int ProductsCount { get; set; }
        public int OrdersCount { get; set; }
        public int UsersCount { get; set; }
        public int ApprovedOrders  { get; set; }
        public int CancelledOrders { get; set; }
        public int PendingOrders { get; set; }
    }
}
