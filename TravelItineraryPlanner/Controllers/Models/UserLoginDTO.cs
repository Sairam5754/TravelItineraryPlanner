using System.ComponentModel.DataAnnotations;

namespace TravelItineraryPlanner.Controllers.Models
{
    public class UserLoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
