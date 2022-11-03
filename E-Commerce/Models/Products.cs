using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace E_Commerce.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? Image { get; set; }
        [Display(Name = "Product Color")]
        public string ProductColor { get; set; }
        [Required]
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        [Display(Name = "Tag")]
        [Required]
        public int TagId { get; set; }
        [ForeignKey("TagId")]
        public virtual Tags? Tags { get; set; }
    }
}
