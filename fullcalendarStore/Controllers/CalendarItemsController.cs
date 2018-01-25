using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fullcalendarStore.Data;
using fullcalendarStore.Models;

namespace fullcalendarStore.Controllers
{
    [Produces("application/json")]
    [Route("api/CalendarItems")]
    public class CalendarItemsController : Controller
    {
        private readonly FullcalendarStoreContext _context;

        public CalendarItemsController(FullcalendarStoreContext context)
        {
            _context = context;
        }

        // GET: api/CalendarItems
        [HttpGet]
        public IEnumerable<CalendarItem> GetCalendarItems()
        {
            return _context.CalendarItems;
        }

        // GET: api/CalendarItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCalendarItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var calendarItem = await _context.CalendarItems.SingleOrDefaultAsync(m => m.Id == id);

            if (calendarItem == null)
            {
                return NotFound();
            }

            return Ok(calendarItem);
        }

        // PUT: api/CalendarItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarItem([FromRoute] Guid id, [FromBody] CalendarItem calendarItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calendarItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(calendarItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CalendarItems
        [HttpPost]
        public async Task<IActionResult> PostCalendarItem([FromBody] CalendarItem calendarItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CalendarItems.Add(calendarItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCalendarItem", new { id = calendarItem.Id }, calendarItem);
        }

        // DELETE: api/CalendarItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var calendarItem = await _context.CalendarItems.SingleOrDefaultAsync(m => m.Id == id);
            if (calendarItem == null)
            {
                return NotFound();
            }

            _context.CalendarItems.Remove(calendarItem);
            await _context.SaveChangesAsync();

            return Ok(calendarItem);
        }

        private bool CalendarItemExists(Guid id)
        {
            return _context.CalendarItems.Any(e => e.Id == id);
        }
    }
}