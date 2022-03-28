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

            try
            {
                var result = _customer.Create(model).Result;
                if (result != null)
                {
                    TempData["ErrorMessage"] = "Customer is duplicate.";
                    return View(model);
                }
                else
                {
                    TempData["Message"] = "Customer Created!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
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
            try
            {
                if (result != null)
                {
                    TempData["Message"] = "Customer Updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
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
