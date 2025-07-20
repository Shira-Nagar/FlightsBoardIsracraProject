using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEntity.Dto
{
    public class UserFlightDto
    {
        public int UserLogInId { get; set; }
        public int FlightEntityId { get; set; }
        public string? SeatNumber { get; set; }
    }
}
