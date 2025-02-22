using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace TravelItineraryPlanner.Controllers.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Participation> Participations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Trip Relationship (Organizer)
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Organizer)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Trip - Destination Relationship
            modelBuilder.Entity<Destination>()
                .HasOne(d => d.Trip)
                .WithMany(t => t.Destinations)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete if needed

            // Trip - Participation Relationship
            modelBuilder.Entity<Participation>()
                .HasOne(p => p.Trip)
                .WithMany(t => t.Participations)
                .HasForeignKey(p => p.TripId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // User - Participation Relationship
            modelBuilder.Entity<Participation>()
                .HasOne(p => p.User)
                .WithMany(u => u.Participations)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            base.OnModelCreating(modelBuilder);
        }


    }

}
