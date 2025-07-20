namespace FlightsEntity
{
    using FlightsEntity.Enum;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Flight entity for flight details and status.
    /// </summary>
    public class FlightEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FlightNumber { get; set; } = null!;

        [Required]
        public string Departure { get; set; } = null!;

        [Required]
        public string Destination { get; set; } = null!;

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        public string? Gate { get; set; }

        [Required]
        public FlightStatus Status { get; set; } 

        public ICollection<UserFlight> UserFlights { get; set; } = new List<UserFlight>();
    }
}
