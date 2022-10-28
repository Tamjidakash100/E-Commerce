using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace E_Commerce.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        [Display(Name="Product Color")]
        public string ProductColor { get; set; }
        [Required]
        [Display(Name="Available")]
        public bool IsAvailable { get; set; }
        [Required]
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        [Display(Name="Special Tag")]
        public int TagId { get; set; }
        [ForeignKey("TagId")]
        public Tags Tags { get; set; }
    }
}
