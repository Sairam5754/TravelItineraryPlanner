using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TravelItineraryPlanner.Controllers.Models
{
    public class ParticipationDTO
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Trip")]
        public int TripId { get; set; }
        [JsonIgnore]

        public Trip? Trip { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]

        public User? User { get; set; }
    }
}
