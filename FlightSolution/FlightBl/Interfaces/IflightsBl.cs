using FlightsEntity;
using FlightsEntity.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightsBl.Interfaces
{
    /// <summary>
    /// Flights business logic interface.
    /// </summary>
    public interface IFlightsBl
    {
        List<FlightEntity> GetAllFlights();
        List<FlightEntity> GetAllFlightsWithStatus();
        string GetFlightById(string id);
        void DeleteFlight(string id);
        void ScheduleNewFlight(CreateFlightRequestDto flight);
        Task<IEnumerable<FlightEntity>> SearchFlightsAsync(string? status, string? destination);
    }
}
