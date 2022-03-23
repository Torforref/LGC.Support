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

    public class ProductController : Controller
    {
        ApplicationContext _db;

        public ProductController(ApplicationContext _database)
        {
            _db = _database;
        }
        public IActionResult Index(ProductData model)
        {
            IEnumerable<ProductData> products = _db.Products;
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductData model)
        {
            if (String.IsNullOrEmpty(model.product_name))
            {
                ModelState.AddModelError("product_name", "Product name cannot be empty.");
            }

            var result = _db.Products.Where(w => w.product_name == model.product_name).FirstOrDefault();
            if (result != null)
            {
                ModelState.AddModelError("product_name", "Product already exit.");
                TempData["error"] = "Product already exit.";
                return View();
            }
            else
            {
                var newProduct = new ProductData();
                newProduct.product_name = model.product_name;
                newProduct.description = model.description;
                newProduct.product_category = model.product_category;
                newProduct.created_by = "Titharat";
                newProduct.updated_by = "Titharat";
                _db.Products.Add(newProduct);
                _db.SaveChanges();
                TempData["success"] = "Product created successfully.";
                return RedirectToAction("Index");
            }
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Where(w => w.id == id).FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(ProductData model)
        {
            if (String.IsNullOrEmpty(model.product_name))
            {
                ModelState.AddModelError("product_name", "Product name cannot be empty.");
            }

            model.created_by = "Titharat";
            model.updated_by = "Titharat";

            _db.Products.Update(model);
            _db.SaveChanges();
            TempData["success"] = "Product updated successfully.";
            return RedirectToAction("Index");
        }
    }

}
