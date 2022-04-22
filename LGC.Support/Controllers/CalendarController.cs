using LGC.Support.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LGC.Support.Controllers
{
    public class CalendarController : Controller
    {
        private readonly CalendarService _calendar;

        public CalendarController(CalendarService calendar)
        {
            _calendar = calendar;
        }

        public IActionResult Index()
        {
            ViewBag.events = _calendar.GetAll().Result;
            return View();
        }

        [HttpPost]
        public IActionResult Index(CalendarData model)
        {

            try
            {
                if (model.id == 0)
                {
                    var created = _calendar.Create(model);
                    TempData["Message"] = "Event Created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    var result = _calendar.Update(model).Result;
                    if (result != null)
                    {
                        TempData["Message"] = "Update Successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Event not found.";
                        return RedirectToAction("Index");
                    }

                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }

        }


        [HttpPost]
        public IActionResult Delete(CalendarData model)
        {
            try
            {
                var result = _calendar.Delete(model).Result;
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Json(new { result = ex.Message });
            }
        }

    }
}
