using System.ComponentModel.DataAnnotations;

namespace MyShop.Web.ViewModels
{
    public class CreatedRole
    {
        [Required,MaxLength(100)]
        public string roleName { get; set; } = default!;
    }
}
