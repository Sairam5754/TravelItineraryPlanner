using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TravelItineraryPlanner.Controllers.Models;
using System.Text.Json.Serialization;

namespace TravelItineraryPlanner.DTOs
{
    public class TripDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, ForeignKey("User")]
        public int OrganizerId { get; set; }
        [JsonIgnore]
        public User? Organizer { get; set; }
        [JsonIgnore]
        public ICollection<Destination>? Destinations { get; set; }
        [JsonIgnore]
        public ICollection<Participation>?  Participations { get; set; }
    }
}
