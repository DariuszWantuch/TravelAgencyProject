using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Areas.Customer.Controllers.MyTravels
{
    [Authorize(Roles = "Customer, Admin")]
    [Area("Customer")]
    public class MyTravelsController : Controller
    {
        private readonly ApplicationDbContext _context;      

        public MyTravelsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wyswietl = _context.Travels.Include(t => t.TravelPlace)
               .Where(t =>t.UserId  == userId).ToList();
            return View(wyswietl);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travel = await _context.Travels
                .Include(t => t.User)
                .Include(t => t.TravelPlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (travel == null)
            {
                return NotFound();
            }

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

            travel.UserId = null;

            _context.Travels.Update(travel);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Usunięto pomyślnie." });

        }
    }
}
