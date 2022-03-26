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
            if (String.IsNullOrEmpty(model.name) || model.category != "NULL" || model.brand != "NULL")
            {
                ModelState.AddModelError("name", "Name cannot be empty.");
                ModelState.AddModelError("category", "Category cannot be empty.");
                ModelState.AddModelError("brand", "Brand cannot be empty.");
                return View(model);
            }

            var result = _product.Create(model).Result;
            if (result != null)
            {
                ModelState.AddModelError("name", "Product already exit.");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Product");
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
            if (model.category != "NULL" || model.brand != "NULL")
            {
                var result = _product.Update(model).Result;
                if (result != null)
                {
                    return RedirectToAction("Index", "Product");
                }
            }
            else
            {
                ModelState.AddModelError("category", "Category cannot be empty.");
                ModelState.AddModelError("brand", "Brand cannot be empty.");
                return View(model);
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            var result = _product.Get(id).Result;

            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Delete(ProductData model)
        {
            var result = _product.Delete(model).Result;
            return RedirectToAction("Index", "Product");
        }
    }

}
