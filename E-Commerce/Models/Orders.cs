using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace E_Commerce.Models
{
    public class Orders
    {
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
        [Required]
        [Display(Name ="Billing To")]
        public string BillingTo { get; set; }
        [Required]
        [Display(Name = "Ship To")]
        public string ShipTo { get; set; }
        [Display(Name = "Order Date")]
        public string Token { get; set;}
        public double Total { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

    }
}
