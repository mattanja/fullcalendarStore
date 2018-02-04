using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fullcalendarStore.Data;
using fullcalendarStore.Models;

namespace fullcalendarStore.Controllers
{
    public class CalendarAdminController : Controller
    {
        private readonly FullcalendarStoreContext _context;

        public CalendarAdminController(FullcalendarStoreContext context)
        {
            _context = context;
        }

        // GET: CalendarAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.CalendarItems.ToListAsync());
        }

        // GET: CalendarAdmin/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarItem = await _context.CalendarItems
                .SingleOrDefaultAsync(m => m.Id == id);
            if (calendarItem == null)
            {
                return NotFound();
            }

            return View(calendarItem);
        }

        // GET: CalendarAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CalendarAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Username,Start,End")] CalendarItem calendarItem)
        {
            if (ModelState.IsValid)
            {
                calendarItem.Id = Guid.NewGuid();
                _context.Add(calendarItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calendarItem);
        }

        // GET: CalendarAdmin/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarItem = await _context.CalendarItems.SingleOrDefaultAsync(m => m.Id == id);
            if (calendarItem == null)
            {
                return NotFound();
            }
            return View(calendarItem);
        }

        // POST: CalendarAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Username,Start,End")] CalendarItem calendarItem)
        {
            if (id != calendarItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calendarItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarItemExists(calendarItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(calendarItem);
        }

        // GET: CalendarAdmin/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarItem = await _context.CalendarItems
                .SingleOrDefaultAsync(m => m.Id == id);
            if (calendarItem == null)
            {
                return NotFound();
            }

            return View(calendarItem);
        }

        // POST: CalendarAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var calendarItem = await _context.CalendarItems.SingleOrDefaultAsync(m => m.Id == id);
            _context.CalendarItems.Remove(calendarItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarItemExists(Guid id)
        {
            return _context.CalendarItems.Any(e => e.Id == id);
        }
    }
}
