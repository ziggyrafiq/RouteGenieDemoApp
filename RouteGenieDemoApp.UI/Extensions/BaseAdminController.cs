using RouteGenieDemoApp.Business.Services;
using RouteGenieDemoApp.Business.Services.Interfaces;
using RouteGenieDemoApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RouteGenieDemoApp.UI.Extensions
{
    public class BaseAdminController : Controller
    {
        ServiceManager _Manager;
        internal ServiceManager Services
        {
            get
            {
                if (_Manager == null)
                {
                    _Manager = new ServiceManager(new User { Email = "Unknown" });

                    if (User != null && User.Identity != null && !string.IsNullOrWhiteSpace(User.Identity.Name))
                    {
                        var user = Services.Service<IUserService>().GetUserByEmail(User.Identity.Name);
                        
                        _Manager = new ServiceManager(user);
                    }
                }
                return _Manager;
            }
        }
    }
}