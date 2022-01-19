using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TravelAgency.Models;

namespace TravelAgency.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Travel>()
              .HasOne(c => c.User)
              .WithMany(x => x.Travels)
              .HasForeignKey(f => f.UserId)
              .HasConstraintName("UserId")
              .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<TravelPlace> TravelPlaces { get; set; }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        
    }
}
