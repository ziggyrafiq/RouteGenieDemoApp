using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure.ViewModels
{
   public  class UserData
    {
        [Required]
        [Display(Name = "Role")]
        public string Roles { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Forename")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        [Required]
        [StringLength(255)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Activate User?")]
        public bool IsActive { get; set; }
    }
}
