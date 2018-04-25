using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fullcalendarStore.Configuration;
using fullcalendarStore.Data;
using fullcalendarStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fullcalendarStore.Controllers
{
    [Produces("application/json")]
    [Route("api/CalendarProxy")]
    public class CalendarProxyController : Controller
    {
        private readonly CalendarProxyService calendarProxyService;
        private readonly IAppSettings appSettings;

        public CalendarProxyController(CalendarProxyService calendarProxyService, IAppSettings appSettings)
        {
            this.calendarProxyService = calendarProxyService;
            this.appSettings = appSettings;
        }

        // GET: api/CalendarItems
        [HttpGet]
        public IEnumerable<CalendarItem> GetCalendarItems(DateTime? start, DateTime? end)
        {
            return this.calendarProxyService.GetCalendarItems(start, end);
        }

        [HttpGet]
        [Route("CalendarView")]
        public IActionResult CalendarView()
        {
            var model = new FullcalendarSettings
            {
                AspectRatio = 2,
                EnableEditing = false,
                FirstDay = appSettings.CalendarFirstDay,
                FirstHour = 0,
                ItemsUrl = "/api/CalendarProxy",
                StartDate = DateTime.Today.ToString("yyyy-MM-dd"),
            };

            return View(
                "Calendar",
                model
            );
        }

        [HttpGet]
        [Route("Update")]
        public void Update()
        {
            this.calendarProxyService.TryUpdateCalendarItemsCache();
        }

        [HttpGet]
        [Route("Current")]
        public IEnumerable<string> Current()
        {
            var model = this.calendarProxyService
                .GetCalendarItems(DateTime.Now, DateTime.Now)
                .Select(x => x.Title.Trim())
                .Distinct()
                .OrderBy(x => x);

            if (!String.IsNullOrWhiteSpace(appSettings.EmptyListDefaultEntry))
            {
                model.DefaultIfEmpty(appSettings.EmptyListDefaultEntry);
            }

            return model;
        }

        [HttpGet]
        [Route("CurrentView")]
        public IActionResult CurrentView()
        {
            var model = this.calendarProxyService
                .GetCalendarItems(DateTime.Now, DateTime.Now)
                .Select(x => x.Title.Trim())
                .Distinct()
                .OrderBy(x => x);

            if (!String.IsNullOrWhiteSpace(appSettings.EmptyListDefaultEntry))
            {
                model.DefaultIfEmpty(appSettings.EmptyListDefaultEntry);
            }

            return View(
                "ListView",
                model
            );
        }

        [HttpGet]
        [Route("Today")]
        public IEnumerable<string> Today()
        {
            return this.calendarProxyService
                .GetCalendarItems(DateTime.Today, DateTime.Today.AddDays(1))
                .Select(x => x.Title.Trim())
                .Distinct()
                .OrderBy(x => x);
        }

        [HttpGet]
        [Route("TodayView")]
        public IActionResult TodayView()
        {
            return View(
                "ListView",
                this.calendarProxyService
                    .GetCalendarItems(DateTime.Today, DateTime.Today.AddDays(1))
                    .Select(x => x.Title.Trim())
                    .Distinct()
                    .OrderBy(x => x)
            );
        }

        [HttpGet]
        [Route("ListAll")]
        public IEnumerable<string> ListAll()
        {
            return this.calendarProxyService
                .CalendarItems
                .Select(x => x.Title.Trim())
                .Distinct()
                .OrderBy(x => x);
        }

        [HttpGet]
        [Route("ListAllView")]
        public IActionResult ListAllView()
        {
            return View(
                "ListView",
                this.calendarProxyService
                    .CalendarItems
                    .Select(x => x.Title.Trim())
                    .Distinct()
                    .OrderBy(x => x)
            );
        }
    }
}