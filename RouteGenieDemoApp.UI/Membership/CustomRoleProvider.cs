using RouteGenieDemoApp.Business.Services;
using RouteGenieDemoApp.Business.Services.Interfaces;
using RouteGenieDemoApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace RouteGenieDemoApp.UI.Membership
{
    public class CustomRoleProvider : RoleProvider
    {
        ServiceManager _Manager;
        internal ServiceManager Services
        {
            get
            {
                if (_Manager == null)
                {
                    _Manager = new ServiceManager(new User { Email = "Unknown" });
                }
                return _Manager;
            }
        }

        #region -- Contructor(s) --

        public CustomRoleProvider()
        {
        }

        #endregion

        #region -- Not Implemented --

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        #endregion

        public override bool IsUserInRole(string username, string roleName)
        {
            var roles = GetRolesForUser(username);
            if (roles.Contains(roleName))
                return true;
            else return false;
        }

        public override string[] GetRolesForUser(string username)
        {
            User user = Services.Service<IUserService>().GetUserByEmail(username);
            var roles = new[] { user.Role.Name };

            return roles;
        }
    }
}