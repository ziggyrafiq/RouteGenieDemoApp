using RouteGenieDemoApp.Business.Security;
using RouteGenieDemoApp.Business.Services.Interfaces;
using RouteGenieDemoApp.Infrastructure;
using RouteGenieDemoApp.Infrastructure.Models;
using RouteGenieDemoApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Business.Services
{
    public class UserService : IUserService
    {
        UnitOfWork _UnitOfWork;
        User _User;

        public UserService(User user, UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("UnitOfWork");

            _UnitOfWork = unitOfWork;
            _User = user;
        }

        public List<User> GetAllUsers()
        {
            return _UnitOfWork.Repository<User>().Get(filter: u => u.IsDeleted == false && u.Role.Name != AppTerms.UserRoleMaster, includeProperties: AppTerms.IncludePropertyRole).ToList();
        }

        public List<User> GetAllAdminUsers()
        {
            return _UnitOfWork.Repository<User>().Get(filter: u => u.IsDeleted == false && u.Role.Name != AppTerms.UserRoleMaster && u.Role.Name != AppTerms.UserRoleStandard, includeProperties: AppTerms.IncludePropertyRole).ToList();
        }

        // Get a list of all active users
        public List<User> GetActiveUsers()
        {
            return _UnitOfWork.Repository<User>().Get(filter: u => u.IsDeleted == false && u.Role.Name != AppTerms.UserRoleMaster, includeProperties: AppTerms.IncludePropertyRole).ToList();
        }

        // Get a list of all roles
        public List<Role> GetAllRoles()
        {
            return _UnitOfWork.Repository<Role>().Get(filter: u => u.IsDeleted == false && u.Name != AppTerms.UserRoleMaster).ToList();
        }

        public User GetUserByID(Guid id)
        {
            return _UnitOfWork.Repository<User>().GetSingle(filter: u => u.IsDeleted == false && u.UserID == id, includeProperties: AppTerms.IncludePropertyRole);
        }

        // Get a single user by their email address
        public User GetUserByEmail(string email)
        {
            return _UnitOfWork.Repository<User>().GetSingle(filter: u => u.IsDeleted == false && u.Email == email, includeProperties: AppTerms.IncludePropertyRole);
        }


        public User SendResetPasswordEmail(Guid id)
        {
            var original = GetUserByID(id);
            return original;
        }
        
        public User ValidateUser(string username, string password)
        {
            User user = null;

            try
            {
                if (string.IsNullOrWhiteSpace(password))
                    return null;

                user = GetUserByEmail(username);

                if (user == null)
                    return null;

                string hash = new Cryptographer().HashPassword(password, user.Salt);
                if (user.Password == hash)
                    return user;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }


        public void Dispose()
        {
            _UnitOfWork.Dispose();
            GC.Collect();
        }
    }
}
