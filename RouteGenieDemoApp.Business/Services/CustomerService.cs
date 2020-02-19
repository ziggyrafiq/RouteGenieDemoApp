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
   public  class CustomerService:ICustomerService
    {
        UnitOfWork _UnitOfWork;
        User _User;

        public CustomerService(User user, UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("UnitOfWork");

            _UnitOfWork = unitOfWork;
            _User = user;
        }


        public List<Customer> GetAllCustomers()
        {
            return _UnitOfWork.Repository<Customer>().Get(filter: u => u.IsDeleted == false).ToList();
        }

        public List<Customer> GetAllCustomerByDeletedStatus(bool isDeleted)
        {
            return _UnitOfWork.Repository<Customer>().Get(filter: u => u.IsDeleted == isDeleted).ToList();
        }

        //All known customers and any vehicles they own.
        public List<Customer> GetAllCustomerAndVehiclesByCustomerID(int customerID)
        {
            return _UnitOfWork.Repository<Customer>().Get(filter:u=> u.CustomerID==customerID).ToList();
        }

        //All customers between the age of 20 and 30.
        public List<Customer> GetAllCustomerBetweenAgeGroup(int firstAge, int SecondAge)
        {
            return _UnitOfWork.Repository<Customer>().Get().ToList();
        }

        public Customer GetCustomerByID(int id)
        {
            return _UnitOfWork.Repository<Customer>().GetSingle(filter: u => u.IsDeleted == false && u.CustomerID == id);
        }

        public Customer Add(Customer model)
        {
            _UnitOfWork.Repository<Customer>().Insert(model);
            _UnitOfWork.Save();
            return model;
        }

        public void AddRange(List<Customer> customers)
        {
            _UnitOfWork.Repository<Customer>().InsertRange(customers);
            _UnitOfWork.Save();
           
        }

        public Customer Update(Customer model)
        {
            var original = GetCustomerByID(model.CustomerID);
            if (original != null)
                original = model;

            _UnitOfWork.Repository<Customer>().Update(original);
            _UnitOfWork.Save();
            return model;
        }

        public void Delete(int id)
        {
            var dataModel = GetCustomerByID(id);
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
