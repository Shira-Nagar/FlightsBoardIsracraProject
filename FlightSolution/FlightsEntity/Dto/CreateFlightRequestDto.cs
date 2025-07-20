using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FlightsEntity.Dto
{
    public class CreateFlightRequestDto
    {
        
        public string FlightNumber { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public string Departure { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Gate { get; set; } = null!;

    }
}

