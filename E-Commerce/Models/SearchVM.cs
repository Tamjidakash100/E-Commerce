using Microsoft.Build.Framework;

namespace E_Commerce.Models
{
    public class SearchVM
    {
        [Required]
        public string Query { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
