using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace E_Commerce.Areas.Admin.Models
{
    public class AssignRolesVM
    {
        [Required]
        [Display(Name ="Role")]
        public string RoleId { get; set; }
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }
    }
}
