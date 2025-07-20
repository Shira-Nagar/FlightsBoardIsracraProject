using FlightsEntity.Enum;

namespace Flights.BL;

/// <summary>
/// Flight status calculation logic.
/// </summary>
public class FlightStatusCalculator
{
    /// <summary>
    /// Calculate flight status.
    /// </summary>
    public static FlightStatus CalculateStatus(DateTime departureTime, DateTime currentTime)
    {
        var diff = departureTime - currentTime;

        if (diff.TotalMinutes > 30)
            return FlightStatus.Scheduled;
        if (diff.TotalMinutes <= 30 && diff.TotalMinutes > 0)
            return FlightStatus.Boarding;
        if (diff.TotalMinutes <= 0 && diff.TotalMinutes >= -60)
            return FlightStatus.Departed;

        return FlightStatus.Landed;
    }
}
