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
            var query = this.calendarProxyService.CalendarItems.AsQueryable();

            if (start.HasValue)
            {
                query = query.Where(x => x.End >= start.Value);
            }

            if (end.HasValue)
            {
                query = query.Where(x => x.Start <= end.Value);
            }

            return query;
        }
    }
}