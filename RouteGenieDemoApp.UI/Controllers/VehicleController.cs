using RouteGenieDemoApp.Infrastructure.Models;
using RouteGenieDemoApp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RouteGenieDemoApp.UI.Controllers
{

    //[Authorize(Roles = "Master Admin")]
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "User")]
    public class VehicleController : BaseAdminController
    {
        //View All Vehicles in a Table
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Vehicle model)
        {
            return View();
        }

        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Vehicle model)
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index");
        }
    }
}