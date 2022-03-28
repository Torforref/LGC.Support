using LGC.Suport.Models;
using LGC.Support.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LGC.Support.Controllers
{
    [Authorize]

    public class ProductController : Controller
    {
        private readonly ProductService _product;

        public ProductController(ProductService product)
        {
            _product = product;
        }
        public IActionResult Index()
        {
            var data = _product.GetAll().Result;
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductData model)
        {
            try
            {
                var result = _product.Create(model).Result;
                if (result != null)
                {
                    TempData["ErrorMessage"] = "Product is duplicate.";
                    return View(model);
                }
                else
                {
                    TempData["Message"] = "Product Created!";
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
            var result = _product.Get(id).Result;

            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(ProductData model)
        {

            var result = _product.Update(model).Result;
            try
            {
                if (result != null)
                {
                    TempData["Message"] = "Product Updated!";
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
        public IActionResult Delete(ProductData model)
        {
            try
            {
                var result = _product.Delete(model).Result;
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Json(new { result = ex.Message });
            }
        }

    }

}
