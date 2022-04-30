using LGC.Suport.Models;
using LGC.Support.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace LGC.Support.Controllers
{
    [Authorize]

    public class JobController : Controller
    {
        private readonly JobService _job;
        private readonly ProductService _product;
        private readonly CustomerService _customer;
        private readonly CalendarService _calendar;

        public JobController(JobService job, ProductService product, CustomerService customer, CalendarService calendar)
        {
            _job = job;
            _product = product;
            _customer = customer;
            _calendar = calendar;
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
            ViewBag.SN_form_DB = _job.GetAllJobDetails().Result;

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

            /*    var job_number = _job.GetJobNumber().Result;

                if (job_number != null)
                {
                    model.job_number = $"LGCSV{(job_number.id + 1).ToString("D4")}";
                }
                else
                {
                    model.job_number = "LGCSV0001";
                }*/

            try
            {
                var job_result = _job.Create(model).Result;
                var calendar_result = _calendar.CreateForJob(model);
                TempData["Message"] = "Job Created!";
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
            ViewBag.SN_form_DB = _job.GetAllJobDetails().Result;

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var re = new JobData();
                re = _job.GetForEdit(id).Result;

                List<JobProductDetailData> data = new List<JobProductDetailData>();
                data.Add(new JobProductDetailData { product_id = re.JodDetails[0].product_id, product_name = re.JodDetails[0].product_name });

                for (var i = 1; i<re.JodDetails.Count; i++)
                {
                    var duplicate = false;
                    var new_id = re.JodDetails[i].product_id;

                    foreach (var old_id in data)
                    {
                        if (old_id.product_id == new_id)
                        {
                            duplicate = true;
                            break;
                        }
                    }

                    if (!duplicate)
                    {
                        data.Add(new JobProductDetailData { product_id = re.JodDetails[i].product_id, product_name = re.JodDetails[i].product_name });
                    }
                }

                ViewBag.Count_Product_Unique = data;

                return View(re);
            }
        }

        [HttpPost]
        public IActionResult Edit(JobData model)
        {
            try
            {
                var result = _job.Update(model).Result;
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

        public IActionResult SearchBySN()
        {
            var model = new JobData()
            {
                job_number = null
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SearchBySN(JobData model)
        {
            try
            {
                var result = _job.GetJobBySN(model).Result;

                if (result != null)
                {
                    // var data = _job.Get(result.job_id).Result;
                    // data.CustomerDetails = _customer.Get(data.customer_id).Result;
                    result.CustomerDetails =_customer.Get(result.customer_id).Result;

                    return View(result);
                }
                else
                {
                    TempData["ErrorMessage"] = "Serial Number not found, please try again.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(JobData model)
        {
            try
            {
                var result = _job.Delete(model).Result;
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Json(new { result = ex.Message });
            }
        }
    }
}



