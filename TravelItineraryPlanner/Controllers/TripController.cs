using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Controllers.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelItineraryPlanner.DTOs;

namespace TravelItineraryPlanner.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly MyDbContext _context;

        public TripController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _context.Trips.Include(t => t.Organizer).ToListAsync();
            return Ok(trips);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            var trip = await _context.Trips.Include(t => t.Organizer)
                                           .FirstOrDefaultAsync(t => t.Id == id);
            if (trip == null)
                return NotFound("Trip not found.");

            return Ok(trip);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip(TripDTO request)
        {
            var trip = new Trip
            {
                Title = request.Title,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                OrganizerId = request.OrganizerId,
                Organizer = null,  
                Destinations = new List<Destination>(), 
                Participations = new List<Participation>() 
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Trip created successfully", tripId = trip.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, TripDTO  request)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
                return NotFound("Trip not found.");

            trip.Title = request.Title;
            trip.StartDate = request.StartDate;
            trip.EndDate = request.EndDate;
            trip.OrganizerId = request.OrganizerId;

            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();

            return Ok("Trip updated successfully.");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
                return NotFound("Trip not found.");

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return Ok("Trip deleted successfully.");
        }
    }
}

