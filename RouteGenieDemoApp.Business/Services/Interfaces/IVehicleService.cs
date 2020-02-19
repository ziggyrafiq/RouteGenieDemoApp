using RouteGenieDemoApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Business.Services.Interfaces
{
    public interface IVehicleService : IService
    {
        List<Vehicle> GetAllVehicles();

        //All vehicles with an engine size over 1100.
        List<Vehicle> GetAllVehiclesWithEngineSizeOver1100();

        //All vehicles registered before 1st January 2010.
        List<Vehicle> GetAllVehiclesRegisteredBefore1stJanuary2010();
        List<Vehicle> GetAllVehiclesByDeletedStatus(bool isDeleted);

        List<Vehicle> GetAllVehiclesByCustomerID(int customerID);
        Vehicle GetVehicleByID(int id);

        Vehicle Add(Vehicle model);
        void AddRange(List<Vehicle> vehicles);
        Vehicle Update(Vehicle model);

        void Delete(int id);
    }
}
