using RouteGenieDemoApp.Business.Services.Interfaces;
using RouteGenieDemoApp.Infrastructure.Models;
using RouteGenieDemoApp.Infrastructure.ViewModels;
using RouteGenieDemoApp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RouteGenieDemoApp.UI.Controllers
{
    //[Authorize(Roles = "Master Admin")]
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "User")]
    public class CustomerController : BaseAdminController
    {
        //View All Customers in a Table
        public ActionResult Index()
        {
            List<Customer> customers = new List<Customer>();

            return View(customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".csv")
                    {
                        ViewBag.Message = "Please select the csv file with .csv extension";
                        return View();
                    }


                    //var employees = new List<Employee>();
                    var customers = new List<Customer>();
                    var vehicles = new List<Vehicle>();
                    



                    using (var sreader = new StreamReader(postedFile.InputStream))
                    {
                        //First line is header. If header is not passed in csv then we can neglect the below line.
                        string[] headers = sreader.ReadLine().Split(',');
                        //Loop through the records
                        while (!sreader.EndOfStream)
                        {
                            string[] rows = sreader.ReadLine().Split(',');

                            customers.Add(new Customer
                            {
                                CustomerID = int.Parse(rows[0].ToString()),
                                Forename = rows[1].ToString(),
                                Surname = rows[2].ToString(),
                                DateOfBirth = DateTime.Parse(rows[3].ToString()),
                            });

                            vehicles.Add(new Vehicle
                            {
                                CustomerID = int.Parse(rows[0].ToString()),
                                VehicleID = int.Parse(rows[4].ToString()),
                                RegistrationNumber = rows[5].ToString(),
                                Manufacturer = rows[6].ToString(),
                                Model = rows[7].ToString(),
                                EngineSize = int.Parse(rows[8].ToString()),
                                RegistrationDate = DateTime.Parse(rows[9].ToString()),
                                InteriorColour = rows[10].ToString(),
                            });
                        }
                    }



                    // List<Customer> removeDuplicateCustomers = customers.Distinct().ToList();
                    customers = customers.Distinct().ToList();

                    Services.Service<ICustomerService>().AddRange(customers);
                    Services.Service<IVehicleService>().AddRange(vehicles);
                    
                    return View(customers);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Please select the file first to upload.";
            }
            return View();
        }

            public ActionResult Detail()
        {
            return View();
        }

        public ActionResult CustomerWithVehicle()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            CustomerWithVehicleData customerWithVehicleData = new CustomerWithVehicleData();
            customerWithVehicleData.Vehicles = vehicles;
            return View(customerWithVehicleData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerWithVehicle(CustomerWithVehicleData model)
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Customer model)
        {
            return View();
        }


        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Customer model)
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