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

        
        public List<User> GetActiveUsers()
        {
            return _UnitOfWork.Repository<User>().Get(filter: u => u.IsDeleted == false && u.Role.Name != AppTerms.UserRoleMaster, includeProperties: AppTerms.IncludePropertyRole).ToList();
        }

         
        public List<Role> GetAllActiveRoles()
        {
            return _UnitOfWork.Repository<Role>().Get(filter: u => u.IsDeleted == false).ToList();
        }

        public List<Role> GetAllDeletedRoles()
        {
            return _UnitOfWork.Repository<Role>().Get(filter: u => u.IsDeleted == true).ToList();
        }

        public List<Role> GetAllRoles()
        {
            return _UnitOfWork.Repository<Role>().Get().ToList();
        }

        public User GetUserByID(Guid id)
        {
            return _UnitOfWork.Repository<User>().GetSingle(filter: u => u.IsDeleted == false && u.UserID == id, includeProperties: AppTerms.IncludePropertyRole);
        }

        // Get a single user by their email address
        public User GetUserByEmail(string email)
        {
            return _UnitOfWork.Repository<User>()
                    .GetSingle(filter: u => u.IsDeleted == false 
                    &&u.IsActive==true
                    && u.Email == email, 
                    includeProperties: AppTerms.IncludePropertyRole
                );
        }



        //Need to do this feature
        public User SendResetPasswordEmail(string email)
        {
            var original = GetUserByEmail(email);
            return original;
        }
        
        public User ValidateUser(string username, string password)
        {
            User user = null;

                if (string.IsNullOrWhiteSpace(password))
                    return null;

                user = GetUserByEmail(username);


                if (user == null)
                    return null;

                string hash = new Cryptographer().HashPassword(password, user.Salt);

                var validUserQuery = _UnitOfWork.Repository<User>()
                    .Get()
                    .Where(u => u.Email == username && u.Password == hash && u.IsActive == true&&u.IsDeleted==false)
                    .FirstOrDefault();

                return validUserQuery != null ? user : null;
            
        }

        public User Add(User model)
        {            
            Cryptographer cryptographer = new Cryptographer();
            model.Salt = cryptographer.CreateSalt();
            model.Password = cryptographer.HashPassword(model.Password, model.Salt);

            _UnitOfWork.Repository<User>().Insert(model);
            _UnitOfWork.Save();
            return model;
        }


        public User Update(User model)
        {
            var original = GetUserByID(model.UserID);

            if (original != null)
               original = model;
                      
            _UnitOfWork.Repository<User>().Update(original);
            _UnitOfWork.Save();
            return model;
        }

        public void ChangeUserAccountActiveStatus(Guid id)
        {
            var dataModel = GetUserByID(id);
            dataModel.IsActive = dataModel.IsActive ? true : false;
            Update(dataModel);
        }
        public void Delete(Guid id)
        {
            var dataModel = GetUserByID(id);
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
