using Microsoft.AspNetCore.Mvc;

namespace PM_and_MA_record_application.Controllers
{
    public class JobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
