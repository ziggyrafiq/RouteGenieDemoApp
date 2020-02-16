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
    public class ServiceManager : IDisposable
    {

        UnitOfWork _UnitOfWork;
        User _User;


        Dictionary<Type, IService> _Services = new Dictionary<Type, IService>();


        public ServiceManager()
        {
            _UnitOfWork = new UnitOfWork("Unknown");
        }


        public ServiceManager(User user)
        {
            _User = user;

            if (_User != null)
                _UnitOfWork = new UnitOfWork(_User.Email);
            else _UnitOfWork = new UnitOfWork("Unknown");
        }




        public void RefreshConnection()
        {
            _UnitOfWork.RefreshConnection();

            if (_User != null)
                _UnitOfWork = new UnitOfWork(_User.Email);
            else _UnitOfWork = new UnitOfWork("Unknown");

            _Services.Clear();
        }

        public int Services { get { return _Services.Count; } }
        public User User { get { return _User; } }


        public T Service<T>() where T : IService
        {
            if (_Services.ContainsKey(typeof(T)))
            {
                return (T)_Services[typeof(T)];
            }
            var service = ServiceFactory.GetService<T>(User, _UnitOfWork);
            _Services.Add(typeof(T), service);
            return (T)service;
        }


        public void Remove<T>() where T : IService
        {
            if (_Services.ContainsKey(typeof(T)))
            {
                _Services[typeof(T)].Dispose();
                _Services.Remove(typeof(T));
            }
        }

        public void Dispose()
        {

            foreach (var service in _Services)
            {
                service.Value.Dispose();
            }


            _Services.Clear();


            _UnitOfWork.Dispose();
        }
    }
}
