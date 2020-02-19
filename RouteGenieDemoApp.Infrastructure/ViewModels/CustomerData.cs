using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure.ViewModels
{
    public class CustomerData
    {

        [Required]
        [StringLength(255)]
        [Display(Name = "Forename")]
        public string Forename { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }
    }
}
