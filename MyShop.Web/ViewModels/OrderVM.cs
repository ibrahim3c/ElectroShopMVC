namespace MyShop.Web.ViewModels
{
    public class OrderVM
    {
        public int OrderID { get; set; }
        public string Name {  get; set; }
        public string? PhoneNumber {  get; set; }
        public string Email {  get; set; }
        public string? OrderStatus { get; set; }
        public decimal? OrderTotal { get; set; }
    }
}
