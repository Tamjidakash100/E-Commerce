using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Display(Name = "Order")]
        public int OrderId { get; set; }
        [Display(Name ="Product")]
        public int ProductId { get; set; }
        [ForeignKey("OrderId")]
        public Orders Order { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ProductId")]
        public Products Product { get; set; }
    }
}
