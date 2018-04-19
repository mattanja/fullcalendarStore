using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private CalendarProxyService calendarProxyService;

        public CalendarProxyController(CalendarProxyService calendarProxyService)
        {
            this.calendarProxyService = calendarProxyService;
        }

        // GET: api/CalendarItems
        [HttpGet]
        public IEnumerable<CalendarItem> GetCalendarItems(DateTime? start, DateTime? end)
        {
            return this.calendarProxyService.GetCalendarItems(start, end);
        }

        [HttpGet]
        [Route("Update")]
        public void Update()
        {
            this.calendarProxyService.TryUpdateCalendarItemsCache();
        }

        [HttpGet]
        [Route("TodayView")]
        public IActionResult TodayView()
        {
            return View(
                this.calendarProxyService
                    .GetCalendarItems(DateTime.Today, DateTime.Today.AddDays(1))
                    .Select(x => x.Title)
                    .Distinct()
            );
        }
    }
}