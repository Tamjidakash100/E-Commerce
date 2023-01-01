using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace E_Commerce.Models
{
    public class Orders
    {
        public Orders()
        {
            OrderDetails= new List<OrderDetails>();
        }
        public int Id { get; set; }
        [Display(Name="Order No")]
        public string OrderNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Phone No")]
        [Required]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Display(Name = "Order Date")]
        public string Token { get; set;}
        public double Total { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public virtual List<OrderDetails> OrderDetails { get; set; }

    }
}
