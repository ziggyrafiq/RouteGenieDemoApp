using RouteGenieDemoApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure.ViewModels
{
    public class CustomerWithVehicleData:CustomerData
    {
        public List<Vehicle> Vehicles { get; set; }
    }
}
