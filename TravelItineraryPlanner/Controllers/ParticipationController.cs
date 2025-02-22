using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Controllers.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelItineraryPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ParticipationController(MyDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> CreateParticipation([FromBody] Participation participation)
        {
            if (participation == null)
            {
                return BadRequest("Invalid participation data.");
            }

            
            var tripExists = await _context.Trips.AnyAsync(t => t.Id == participation.TripId);
            if (!tripExists)
            {
                return NotFound("Trip not found.");
            }

       
            var userExists = await _context.Users.AnyAsync(u => u.Id == participation.UserId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Participation created successfully",
                participation
            });
        }

        [HttpGet("{tripId:int}")]
        public async Task<IActionResult> GetParticipations(int tripId)
        {
            if (tripId <= 0)
            {
                return BadRequest(new { message = "Invalid TripId. It must be a positive integer." });
            }

            Console.WriteLine($"Fetching participants for TripId: {tripId}");

            var participations = await _context.Participations
                .Where(p => p.TripId == tripId)
                .Select(p => new
                {
                    p.Id,
                    p.TripId,
                    p.UserId
                })
                .ToListAsync();

            return Ok(new
            {
                message = participations.Any() ? "Participants retrieved successfully" : "No participants found for this trip",
                participants = participations
            });
        }

        [HttpDelete("{tripId:int}/{userId:int}")]
        public async Task<IActionResult> DeleteParticipation(int tripId, int userId)
        {
            var participation = await _context.Participations
                                              .FirstOrDefaultAsync(p => p.TripId == tripId && p.UserId == userId);

            if (participation == null)
            {
                return NotFound(new { message = $"Participation for Trip ID {tripId} and User ID {userId} not found" });
            }

            _context.Participations.Remove(participation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Participation deleted successfully" });
        }

        




    }
}
