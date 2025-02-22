using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TravelItineraryPlanner.Controllers.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } 
        [JsonIgnore]
        public ICollection<Trip> Trips { get; set; }
        [JsonIgnore]
        public ICollection<Participation> Participations { get; set; }
    }

}
