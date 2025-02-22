using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Controllers.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelItineraryPlanner.Controllers
{
    [Route("api/destinations")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly MyDbContext _context;

        public DestinationController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDestination([FromBody] DestinationDTO request)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request data", errors = ModelState });
            }

           
            var tripExists = await _context.Trips.AnyAsync(t => t.Id == request.TripId);
            if (!tripExists)
            {
                return NotFound(new { message = "Trip not found" });
            }

            
            var destination = new Destination
            {
                TripId = request.TripId,
                Location = request.Location,
                ArrivalDate = request.ArrivalDate
            };

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Destination added successfully", destinationId = destination.Id });
        }



       
        [HttpGet("{tripId:int}")]  
        public async Task<IActionResult> GetDestinationsByTrip(int tripId)
        {
            if (tripId <= 0)
            {
                return BadRequest(new { message = "Invalid Trip ID" }); 
            }

            var destinations = await _context.Destinations
                                             .Where(d => d.TripId == tripId)
                                             .ToListAsync();

            if (destinations == null || destinations.Count == 0)
            {
                return Ok(new { message = "No destinations found for this trip", destinations = new List<Destination>() });
            }

            return Ok(destinations);
        }

    }
}
