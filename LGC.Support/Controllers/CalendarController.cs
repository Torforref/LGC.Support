using LGC.Support.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LGC.Support.Controllers
{
    public class CalendarController : Controller
    {
        private readonly CalendarService _calendar;
        private readonly EmailService _email;

        public CalendarController(CalendarService calendar, EmailService email)
        {
            _calendar = calendar;
            _email = email;
        }

        public IActionResult Index()
        {
            ViewBag.events = _calendar.GetAll().Result;

/*            if (ViewBag.events != null)
            {
                foreach (var item in ViewBag.events)
                {
                    var today = DateTime.Now;
                    var alert_day = today.AddDays(item.remind_before_day);
                    alert_day = alert_day.ToString("yyyy-MM-dd");

                    if (alert_day == item.event_date)
                    {
                        try
                        {
                           // _email.Send("testingforautomail@gmail.com", "aksanddd@gmail.com", $"{item.title}", $"{item.description}");
                        }
                        catch (Exception ex)
                        {
                            return Json(new { result = ex.Message });
                        }
                    }
                }
            }*/

            return View();
        }

        [HttpPost]
        public IActionResult Index(CalendarData model)
        {

            // _email.Send("purchase@logicode.co.th", "k.titharat@gmail.com", "Test!", "Long time no see.");

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
