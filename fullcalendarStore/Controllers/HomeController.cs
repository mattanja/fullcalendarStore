using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using fullcalendarStore.Models;
using fullcalendarStore.Configuration;

namespace fullcalendarStore.Controllers
{
    public class HomeController : Controller
    {
        public IAppSettings AppSettings { get; }

        public HomeController(IAppSettings appSettings)
        {
            AppSettings = appSettings;
        }

        public IActionResult Index()
        {
            ViewBag.CalendarInitialDisplayDate = AppSettings.CalendarInitialDisplayDate;
            ViewBag.CalendarFirstDay = AppSettings.CalendarFirstDay;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
