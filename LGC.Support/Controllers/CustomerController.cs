using LGC.Suport.Models;
using LGC.Support.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LGC.Support.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        ApplicationContext _db;

        public CustomerController(ApplicationContext _database)
        {
            _db = _database;
        }
        public IActionResult Index(CustomerData model)
        {
            IEnumerable<CustomerData> customers = _db.Customers;
            return View(customers);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CustomerData model)
        {
            if (String.IsNullOrEmpty(model.customer_name))
            {
                ModelState.AddModelError("customer_name", "Customer name cannot be empty.");
            }

            var result = _db.Customers.Where(w => w.customer_name == model.customer_name).FirstOrDefault();
            if (result != null)
            {
                ModelState.AddModelError("customer_name", "Customer already exit.");
                TempData["error"] = "Customer already exit.";
                return View();
            }
            else
            {
                var newCustomer = new CustomerData();
                newCustomer.customer_name = model.customer_name;
                newCustomer.address = model.address;
                newCustomer.contact = model.contact;
                newCustomer.email = model.email;
                newCustomer.phone_number = model.phone_number;
                newCustomer.created_by = "Titharat";
                newCustomer.updated_by = "Titharat";
                _db.Customers.Add(newCustomer);
                _db.SaveChanges();
                TempData["success"] = "Customer created successfully.";
                return RedirectToAction("Index");
            }
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _db.Customers.Where(w => w.id == id).FirstOrDefault();

            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(CustomerData model)
        {
            if (String.IsNullOrEmpty(model.customer_name))
            {
                ModelState.AddModelError("customer_name", "Customer name cannot be empty.");
            }

            model.created_by = "Titharat";
            model.updated_by = "Titharat";

            _db.Customers.Update(model);
            _db.SaveChanges();
            TempData["success"] = "Customer updated successfully.";
            return RedirectToAction("Index");
        }
    }
}
