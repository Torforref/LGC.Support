using LGC.Suport.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LGC.Support.Controllers
{
    [Authorize]

    public class CustomerController : Controller
    {
        private readonly CustomerService _customer;

        public CustomerController(CustomerService customer)
        {
            _customer = customer;
        }
        public IActionResult Index()
        {
            var data = _customer.GetAll().Result;
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CustomerData model)
        {

            var result = _customer.Create(model).Result;
            if (result != null)
            {
                ModelState.AddModelError("name", "Customer already exit.");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Customer");
            }
        }

        public IActionResult Edit(int? id)
        {
            var result = _customer.Get(id).Result;

            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(CustomerData model)
        {
            var result = _customer.Update(model).Result;
            if (result != null)
            {
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Delete(CustomerData model)
        {
            try
            {
                var result = _customer.Delete(model).Result;
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Json(new { result = ex.Message });
            }
        }
    }
}
