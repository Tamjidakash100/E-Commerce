using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
}
