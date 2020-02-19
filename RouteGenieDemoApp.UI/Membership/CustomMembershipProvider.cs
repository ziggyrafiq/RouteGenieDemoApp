using RouteGenieDemoApp.Business.Services;
using RouteGenieDemoApp.Business.Services.Interfaces;
using RouteGenieDemoApp.Infrastructure.Models;
using RouteGenieDemoApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace RouteGenieDemoApp.UI.Membership
{
    public class CustomMembershipProvider : MembershipProvider
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


        public CustomMembershipProvider()
        {
        }

        #endregion

        #region -- Unimplemented MembershipProvider Methods --

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion

        public User User
        {
            get;
            private set;
        }

        public User GetUser(string username)
        {
            return Services.Service<IUserService>().GetUserByEmail(username);
        }

        public override bool ValidateUser(string username, string password)
        {
            // -- Username is used as the email address

            User user = null;
            Services.RefreshConnection();
            user = Services.Service<IUserService>().ValidateUser(username, password);

            User = user;
            return user != null ? true : false;
        }

        public User ForgottenPassword(string email)
        {
            var user = Services.Service<IUserService>().GetUserByEmail(email);
            if (user == null)
                throw new ArgumentException(AppMessages.UserLoginForgotDetailsError, "email");

            return Services.Service<IUserService>().SendResetPasswordEmail(user.Email);
        }
    }
}