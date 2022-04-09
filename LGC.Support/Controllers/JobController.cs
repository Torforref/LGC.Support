using LGC.Suport.Models;
using LGC.Support.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace LGC.Support.Controllers
{
    [Authorize]

    public class JobController : Controller
    {
        private readonly JobService _job;
        private readonly ProductService _product;
        private readonly CustomerService _customer;

        public JobController(JobService job, ProductService product, CustomerService customer)
        {
            _job = job;
            _product = product;
            _customer = customer;
        }

        public IActionResult Index()
        {
            ViewBag.product = _product.GetAll().Result;
            ViewBag.customer = _customer.GetAll().Result;
            var data = _job.GetAll().Result;

            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.product = _product.GetAll().Result;
            ViewBag.customer = _customer.GetAll().Result;

            var job_number = _job.GetJobNumber().Result;
            if (job_number != null)
            {
                ViewBag.job_number = $"LGCSV{(job_number.id + 1).ToString("D4")}";
            }
            else
            {
                ViewBag.job_number = "LGCSV0001";
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(JobData model)
        {
            ViewBag.product = _product.GetAll().Result;
            ViewBag.customer = _customer.GetAll().Result;

            var job_number = _job.GetJobNumber().Result;

            if (job_number != null)
            {
                model.job_number = $"LGCSV{(job_number.id + 1).ToString("D4")}";
            }
            else
            {
                model.job_number = "LGCSV0001";
            }


            try
            {
                var result = _job.Create(model).Result;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public IActionResult Edit(int? id)
        {
            var result = _job.Get(id).Result;

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var re = new JobData();
                re = _job.GetForEdit(id).Result;
                return View(re);
            }
        }

        [HttpPost]
        public IActionResult Edit(JobData model)
        {
            var result = _job.Update(model).Result;
            try
            {
                if (result != null)
                {
                    TempData["Message"] = "Job Updated!";
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
    }
}



