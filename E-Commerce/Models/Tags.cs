using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace E_Commerce.Models
{
    public class Tags
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tag Name")]
        public string Tag { get; set; }
    }
}
