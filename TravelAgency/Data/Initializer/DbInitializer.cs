using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelAgency.Models;
using TravelAgency.Utility;

namespace TravelAgency.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;      

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            if (_db.Roles.Any(r => r.Name == SD.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();


            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@wp.pl",
                Email = "admin@wp.pl",
                EmailConfirmed = true,
                FirstName = "Administrator"
            }, "Admin1234!").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(u => u.Email == "admin@wp.pl").FirstOrDefault();
            _userManager.AddToRoleAsync(user, SD.Admin).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "user@wp.pl",
                Email = "user@wp.pl",
                EmailConfirmed = true,
                FirstName = "KindDaniel"
            }, "User1234!").GetAwaiter().GetResult();

            ApplicationUser customer = _db.ApplicationUser.Where(u => u.Email == "user@wp.pl").FirstOrDefault();
            _userManager.AddToRoleAsync(customer, SD.Customer).GetAwaiter().GetResult();

            List<TravelPlace> travelPlaces = new List<TravelPlace>()
            { 
                new TravelPlace(){ PlaceName = "Barcelona" },
                new TravelPlace(){ PlaceName = "Kraków" },
                new TravelPlace(){ PlaceName = "Londyn" },
                new TravelPlace(){ PlaceName = "Lubaczów" },
                new TravelPlace(){ PlaceName = "Tarnów" },
            };           

            travelPlaces.ForEach(x => _db.TravelPlaces.Add(x));

            List<Travel> travels = new List<Travel>()
            {
                new Travel() { TravelPlace = travelPlaces.FirstOrDefault(x => x.PlaceName == "Barcelona"), Price = 4500, PersonNumber = 6, Description = "Testowy opis", ImageUrl = "https://fly.pl/wp-content/uploads/2014/06/Barcelona.jpg", DateFrom = DateTime.Today.AddDays(2), DateTo = DateTime.Today.AddDays(7) },
                new Travel() { TravelPlace = travelPlaces.FirstOrDefault(x => x.PlaceName == "Kraków"), Price = 2900, PersonNumber = 3, Description = "Testowy opis", ImageUrl = "https://www.polska.travel/images/pl-PL/glowne-miasta/krakow/krakow_wawel_2_1170.jpg", DateFrom = DateTime.Today.AddDays(10), DateTo = DateTime.Today.AddDays(20) },
                new Travel() { TravelPlace = travelPlaces.FirstOrDefault(x => x.PlaceName == "Londyn"), Price = 4000, PersonNumber = 4, Description = "Testowy opis", ImageUrl = "https://cdn.flixbus.de/2017-06/Londyn-informacje-ogolne_0.jpg", DateFrom = DateTime.Today.AddDays(6), DateTo = DateTime.Today.AddDays(15) },
                new Travel() { TravelPlace = travelPlaces.FirstOrDefault(x => x.PlaceName == "Lubaczów"),User = customer,Price = 50, PersonNumber = 11, Description = "Testowy opis", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/09/Rynek_Miejski_w_Lubaczowie.jpg", DateFrom = DateTime.Today.AddDays(40), DateTo = DateTime.Today.AddDays(50) },
                new Travel() { TravelPlace = travelPlaces.FirstOrDefault(x => x.PlaceName == "Tarnów"), Price = 1, PersonNumber = 5, Description = "Piękne miasto duchów", ImageUrl = "https://d-art.ppstatic.pl/kadry/k/r/1/48/f2/5d976563834dd_o_full.jpg", DateFrom = DateTime.Today.AddDays(6), DateTo = DateTime.Today.AddDays(15) },
            };

            travels.ForEach(x => _db.Travels.Add(x));

            _db.SaveChanges();
           
           
        }
    }
}
