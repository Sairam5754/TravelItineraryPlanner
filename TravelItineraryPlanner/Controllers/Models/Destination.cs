using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TravelItineraryPlanner.Controllers.Models
{

    public class Destination
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Trip")]
        public int TripId { get; set; }
        [JsonIgnore]

        public Trip? Trip { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime ArrivalDate { get; set; }
    }

}
