using LGC.Suport.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            return View();
        }

        public IActionResult Create()
        {

            /*var result = _job.GetJobNumber().Result;

            model.job_number = $"LGCSV{(result.id + 1).ToString("D4")}";*/

            dynamic mymodel = new ExpandoObject();
            mymodel.product = _product.GetAll().Result;
            mymodel.customer = _customer.GetAll().Result;
            mymodel.job = _job.GetJobNumber().Result;

            return View(mymodel);

        }

        [HttpPost]
        public IActionResult Create(JobData model)
        {
            var x = model.service_type;
           
            return View("Index");

        }

    }
}
