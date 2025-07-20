namespace FlightsEntity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// User-flight relation entity.
    /// </summary>


    public class UserFlight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Identity

        public int UserLogInId { get; set; }
        public int FlightEntityId { get; set; }

        [ForeignKey(nameof(UserLogInId))]
        public virtual UserLogIn UserLogIn { get; set; } = null!;

        [ForeignKey(nameof(FlightEntityId))]
        public virtual FlightEntity FlightEntity { get; set; } = null!;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public string? SeatNumber { get; set; }
    }

}