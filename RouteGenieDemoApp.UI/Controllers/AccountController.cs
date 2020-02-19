using RouteGenieDemoApp.Business.Services.Interfaces;
using RouteGenieDemoApp.Infrastructure.Models;
using RouteGenieDemoApp.Infrastructure.ViewModels;
using RouteGenieDemoApp.UI.Extensions;
using RouteGenieDemoApp.UI.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 
using Security = System.Web.Security;
namespace RouteGenieDemoApp.UI.Controllers
{
    public class AccountController : BaseSiteController
    {
        private CustomMembershipProvider _Provider = (CustomMembershipProvider)System.Web.Security.Membership.Provider;
        // GET: Account
        [Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnurl, string message = null)
        {
            TempData["message"] = message;
            ViewBag.ReturnUrl = returnurl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginData model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateLogOn(model.Username, model.Password))
                {
                    User user = _Provider.User;
                    Security.FormsAuthentication.SetAuthCookie(user.Email, true);
                    Role roles = new Role();
                    Response.Cookies.Add(new HttpCookie("UserGUID", user.UserID.ToString()));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginSystemError", "The user name or password provided is incorrect. ZIGGY");
                }
            }

          return View(model);
        }


        [Authorize]
        public ActionResult Logout()
        {
           Security.FormsAuthentication.SignOut();
            System.Web.Security.FormsAuthentication.SignOut();

            if (Request.Cookies["UserGUID"] != null)
            {
                var c = new HttpCookie("UserGUID");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Login");
        }


        
        [AllowAnonymous]
        public ActionResult Register()
        {
            
            ViewData["Roles"] = new SelectList(Services.Service<IUserService>().GetAllActiveRoles(), "RoleID", "Name"); 
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUserData model)
        {

            

            if (ModelState.IsValid)
            {

                User userModel = new User();
                userModel.FirstName = model.FirstName;
                userModel.LastName = model.LastName;
                userModel.Email = model.Email;
                userModel.Password = model.Password;
                Guid guid = new Guid(model.Roles.ToString());
                userModel.RoleID = guid;
                userModel.IsActive = false;
                userModel.IsDeleted = false;

                Services.Service<IUserService>().Add(userModel);
                return RedirectToAction("RegisterThanks");
            }


                return View(model);
        }

        [AllowAnonymous]
        public ActionResult RegisterThanks()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Forgot()
        {
            return View();
        }

        
        private bool ValidateLogOn(string userName, string password)
       {
           if (String.IsNullOrEmpty(userName))
           {
               ModelState.AddModelError("username", "You must specify a username.");
           }
           if (String.IsNullOrEmpty(password))
           {
               ModelState.AddModelError("password", "You must specify a password.");
           }
           if (!_Provider.ValidateUser(userName, password))
           {
               ModelState.AddModelError("LoginSystemError", "The username or password provided is incorrect.");
           }

           return ModelState.IsValid;
       }
 

    }
}