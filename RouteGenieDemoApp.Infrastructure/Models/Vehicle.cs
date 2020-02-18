using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleID { get; set; }

        // [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public string RegistrationNumber { get; set; }

        //DD/MM/YYYY
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        public string Manufacturer { get; set; }
        public string Model { get; set; }

        public long EngineSize { get; set; }

        public string InteriorColour { get; set; }





    }
}
