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
    public static class ServiceFactory
    {
        // List of service constructors accessible from their interface types to cover the DI + IOC
        static Dictionary<Type, Func<User, UnitOfWork, IService>> _Services =
            new Dictionary<Type, Func<User, UnitOfWork, IService>> {
               { typeof(ICustomerService), (user, uow) => { return new CustomerService(user, uow); } },
                { typeof(IVehicleService), (user, uow) => { return new VehicleService(user, uow); } },
                { typeof(IUserService), (user, uow) => { return new UserService(user, uow); } },

            };

        public static T GetService<T>(User user, UnitOfWork unitOfWork) where T : IService
        {

            if (!_Services.ContainsKey(typeof(T)))
            {
                throw new ArgumentOutOfRangeException("Object of type '" + typeof(T).ToString() + "' is not declared for construction with this factory.");
                
            }


            return (T)_Services[typeof(T)](user, unitOfWork);
        }

        public static T GetUnauthorisedService<T>() where T : IService
        {
            return GetService<T>(new User { Email = "Unknow User" }, new UnitOfWork("Unknow User"));
        }


    }
}
