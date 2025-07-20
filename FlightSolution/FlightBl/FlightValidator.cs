
using FlightsEntity;
using FlightsEntity.Dto;

namespace Flights.BL;

/// <summary>
/// Flight validation logic.
/// </summary>
public class FlightValidator
{
    /// <summary>
    /// Validate flight creation request.
    /// </summary>
    public static void Validate(CreateFlightRequestDto flight, IEnumerable<FlightEntity> existingFlights)
    {
        if (string.IsNullOrWhiteSpace(flight.Destination))
            throw new ArgumentException("Destination is required.");

        if (string.IsNullOrWhiteSpace(flight.Gate))
            throw new ArgumentException("Gate is required.");

        if (string.IsNullOrWhiteSpace(flight.Departure))
            throw new ArgumentException("Departure is required.");

        if (flight.DepartureTime <= DateTime.Now)
            throw new ArgumentException("Departure Time must be in the future.");

        if (flight.ArrivalTime <= flight.DepartureTime)
            throw new ArgumentException("Arrival Time must be after Departure Time.");
    }
}
