using System.ComponentModel.DataAnnotations;

namespace MyShop.Web.ViewModels
{
    public class UserForIndexVM
    {
        public string Id { get; set; }
        public  string? UserName { get; set; }
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public  string? Email { get; set; }
        public string? Address { get; set; } = default!;


        [DataType(DataType.PhoneNumber)]
        public  string? PhoneNumber { get; set; }

        public bool IsLocked {  get; set; }
    }
}
