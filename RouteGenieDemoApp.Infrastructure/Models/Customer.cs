using RouteGenieDemoApp.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure.Models
{
    public class Customer : EntityBase
    {

        public Customer()
        {
            Vehicle = Vehicle ?? new List<Vehicle>();

        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }
        public string Forename { get; set; }

        public string Surname { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }


        public virtual ICollection<Vehicle> Vehicle { get; set; }

    }
}
