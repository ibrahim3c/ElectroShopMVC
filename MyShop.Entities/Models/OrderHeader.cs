using MyShop.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Web
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


        // user Data
        [ MaxLength(100),Display(Name ="User Name")]
        public string? UserName { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; } = default!;

		[DataType(DataType.PhoneNumber) ,Display(Name ="Phone Number")]
		public string? PhoneNumber { get; set; }

		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }

    }
}
