namespace MyShop.Entities.Models
{
	public class OrderHeader
	{
		public int Id {  get; set; }
		public string UserId {  get; set; }
		public AppUser User { get; set; }

		public DateTime OrderDate { get; set; }
		public DateTime ShippingDate { get; set; }
		public decimal TotalPrice { get; set; }

		public string? OrderStatus {  get; set; }
		public string? PaymentStatus {  get; set; }
		public string? TrackingNumber {  get; set; }
		public string? Carrier {  get; set; }

		public DateTime PaymentDate { get; set; }

		//stripe properties
		public string? SessionId {  get; set; }
		public string? PaymentIntentId {  get; set; }


		// user Date

	}
}
