using RouteGenieDemoApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Business.Services.Interfaces
{
   public interface ICustomerService : IService
    {
        List<Customer> GetAllCustomers();

        List<Customer> GetAllCustomerByDeletedStatus(bool isDeleted);

        //All known customers and any vehicles they own.
        List<Customer> GetAllCustomerAndVehiclesByCustomerID(int customerID);

        //All customers between the age of 20 and 30.
        List<Customer> GetAllCustomerBetweenAgeGroup(int firstAge, int SecondAge);

        Customer GetCustomerByID(int id);

        /*Ziggy Rafiq has currently left the comment features out of current development
        Customer AddWithVehicles(Customer model);
        */
        Customer Add(Customer model);
        void AddRange(List<Customer> customers);
        Customer Update(Customer model);

        void Delete(int id);

    }
}
