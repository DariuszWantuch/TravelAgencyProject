using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TravelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelController(ApplicationDbContext context)
        {
            _context = context;
        }
     
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Travels.Include(t => t.TravelPlace);
            
            return View(await applicationDbContext.ToListAsync());
        }     
     
        public IActionResult Create()
        {
            ViewData["TravelPlaceId"] = new SelectList(_context.TravelPlaces, "Id", "PlaceName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateFrom,DateTo,Place,Price,PersonNumber,ImageUrl,Description,TravelPlaceId")] Travel travel)
        {
            if (ModelState.IsValid)
            {                            
                _context.Add(travel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TravelPlaceId"] = new SelectList(_context.TravelPlaces, "Id", "PlaceName", travel.TravelPlaceId);
            return View(travel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travel = await _context.Travels.FindAsync(id);
            if (travel == null)
            {
                return NotFound();
            }
            ViewData["TravelPlaceId"] = new SelectList(_context.TravelPlaces, "Id", "PlaceName", travel.TravelPlaceId);
            return View(travel);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateFrom,DateTo,Place,Price,PersonNumber,ImageUrl,Description, TravelPlaceId")] Travel travel)
        {
            if (id != travel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(travel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TravelExists(travel.Id))
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
            ViewData["TravelPlaceId"] = new SelectList(_context.TravelPlaces, "Id", "PlaceName", travel.TravelPlaceId);
            return View(travel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var travel = await _context.Travels.FindAsync(id);
            if (travel == null)
            {
                return Json(new { success = false, message = "Błąd podczas usuwania." });
            }

            _context.Travels.Remove(travel);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Usunięto pomyślnie." });

        }

        private bool TravelExists(int id)
        {
            return _context.Travels.Any(e => e.Id == id);
        }
    }
}
