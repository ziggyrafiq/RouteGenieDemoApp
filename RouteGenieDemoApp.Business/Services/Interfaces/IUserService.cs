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
        User GetUserByID(Guid id);
        User GetUserByEmail(string email);
        User SendResetPasswordEmail(Guid id);
        User ValidateUser(string username, string password);
    }
}
