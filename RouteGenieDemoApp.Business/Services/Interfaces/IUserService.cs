using RouteGenieDemoApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Business.Services.Interfaces
{
    public interface IUserService : IService
    {
        List<User> GetAllUsers();
        List<User> GetAllAdminUsers();
        List<User> GetActiveUsers();
        List<Role> GetAllRoles();
        List<Role> GetAllActiveRoles();
        List<Role> GetAllDeletedRoles();

        /*Ziggy Rafiq has currently left the comment features out of current development
        Role AddRole(Role model);
        Role UpdateRole(Role model);
        Role GetRoleByID(Guid roleID);
        void DeleteRole(Guid roleID);
        User ChangeUserRole{User model);
        */

        User GetUserByID(Guid id);
        User GetUserByEmail(string email);
        User SendResetPasswordEmail(string email);
        User ValidateUser(string username, string password);
        User Add(User model);

        User Update(User model);

        void ChangeUserAccountActiveStatus(Guid id);
        void Delete(Guid id);
    }
}
