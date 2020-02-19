using RouteGenieDemoApp.Business.Services.Interfaces;
using RouteGenieDemoApp.Infrastructure;
using RouteGenieDemoApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Business.Services
{
    public class VehicleService: IVehicleService
    {
        UnitOfWork _UnitOfWork;
        User _User;

        public VehicleService(User user, UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("UnitOfWork");

            _UnitOfWork = unitOfWork;
            _User = user;
        }


        public List<Vehicle> GetAllVehicles()
        {
            return _UnitOfWork.Repository<Vehicle>().Get(filter: u => u.IsDeleted == false).ToList();
        }

        //All vehicles with an engine size over 1100.
        public List<Vehicle> GetAllVehiclesWithEngineSizeOver1100()
        {
            return _UnitOfWork.Repository<Vehicle>()
                .Get(filter: u => u.IsDeleted == false)
                .Where(x => x.EngineSize > 1100)
                .ToList();
        }

        //All vehicles registered before 1st January 2010.
        public List<Vehicle> GetAllVehiclesRegisteredBefore1stJanuary2010()
        {
            return _UnitOfWork.Repository<Vehicle>()
                .Get(filter: u => u.IsDeleted == false)
                .Where(x => x.RegistrationDate < DateTime.Parse("1st January 2010"))
                .ToList();
        }

        public List<Vehicle> GetAllVehiclesByDeletedStatus(bool isDeleted)
        {
            return _UnitOfWork.Repository<Vehicle>().Get(filter: u => u.IsDeleted == isDeleted).ToList();

        }

        public List<Vehicle> GetAllVehiclesByCustomerID(int customerID)
        {
            return _UnitOfWork.Repository<Vehicle>().Get(filter: u => u.IsDeleted == false&&u.CustomerID==customerID).ToList();
        }

        public Vehicle GetVehicleByID(int id)
        {
            return _UnitOfWork.Repository<Vehicle>().GetSingle(filter: u => u.IsDeleted == false &&u.VehicleID==id);
        }

        public Vehicle Add(Vehicle model)
        {
            _UnitOfWork.Repository<Vehicle>().Insert(model);
            _UnitOfWork.Save();
            return model;
        }

        public void AddRange(List<Vehicle> vehicles)
        {
            _UnitOfWork.Repository<Vehicle>().InsertRange(vehicles);
            _UnitOfWork.Save();

        }
        public Vehicle Update(Vehicle model)
        {
            var original = GetVehicleByID(model.VehicleID);

            if (original != null)
                original = model;

            _UnitOfWork.Repository<Vehicle>().Update(original);
            _UnitOfWork.Save();
            return model;
        }

        public void Delete(int id)
        {
            var dataModel = GetVehicleByID(id);
            dataModel.IsDeleted = true;
            Update(dataModel);
        }

        public void Dispose()
        {
            _UnitOfWork.Dispose();
            GC.Collect();
        }
    }
}
