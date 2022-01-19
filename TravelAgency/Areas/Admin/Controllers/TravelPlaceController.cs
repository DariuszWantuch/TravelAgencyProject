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
    public class TravelPlaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelPlaceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TravelPlaces.ToListAsync());
        }       

        public IActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlaceName")] TravelPlace travelPlace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(travelPlace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(travelPlace);
        }
    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travelPlace = await _context.TravelPlaces.FindAsync(id);
            if (travelPlace == null)
            {
                return NotFound();
            }
            return View(travelPlace);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlaceName")] TravelPlace travelPlace)
        {
            if (id != travelPlace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(travelPlace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TravelPlaceExists(travelPlace.Id))
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
            return View(travelPlace);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var travel = await _context.TravelPlaces.FindAsync(id);
            if (travel == null)
            {
                return Json(new { success = false, message = "Błąd podczas usuwania." });
            }

            _context.TravelPlaces.Remove(travel);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Usunięto pomyślnie." });

        }

        private bool TravelPlaceExists(int id)
        {
            return _context.TravelPlaces.Any(e => e.Id == id);
        }
    }
}
