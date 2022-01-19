using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;
using TravelAgency.Models.Views;

namespace TravelAgency.Areas.Customer.Views
{
    [Area("Customer")]
    public class TravelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<IActionResult> Index(TravelView travelView)
        {           
            IQueryable<Travel> wycieczki = _context.Travels.Include(x => x.TravelPlace)
                .Where(x => x.DateFrom >= DateTime.Now
                && x.UserId == null);

            if(travelView.DateFrom != null)
            {
                wycieczki = wycieczki.Where(x => x.DateFrom >= travelView.DateFrom);
            }
            if(travelView.DateTo != null)
            {
                wycieczki = wycieczki.Where(x => x.DateTo <= travelView.DateTo);
            }
            if(travelView.PersonNumber != null)
            {
                wycieczki = wycieczki.Where(x => x.PersonNumber == travelView.PersonNumber);
            }
            if(travelView.Place != null)
            {
                wycieczki = wycieczki.Where(x => x.TravelPlace.PlaceName == travelView.Place);
            }

            var list = await wycieczki.ToListAsync();
            
            return View(list);
        }

        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travel = _context.Travels.Include(x => x.TravelPlace)
                .FirstOrDefault(m => m.Id == id);
            if (travel == null)
            {
                return NotFound();
            }

            return View(travel);
        }

        [Authorize(Roles = "Customer, Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var travel = await _context.Travels.FindAsync(id);
            if (travel == null)
            {
                return Json(new { success = false, message = "Błąd podczas rezerwacji." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            travel.UserId = userId;
            _context.Travels.Update(travel);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Zarezerwowano pomyślnie." });
        }
    }
}
