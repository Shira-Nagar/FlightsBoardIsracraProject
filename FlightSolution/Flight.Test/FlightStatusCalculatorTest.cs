using System;
using System.Collections.Generic;
using Flights.BL;
using FlightsEntity;
using FlightsEntity.Dto;
using FlightsEntity.Enum;

namespace Flight.Test;

public class FlightStatusCalculatorTest 
{
    [Fact]
    public void FlightStatusCalculator_Scheduled()
    {
        var now = DateTime.Now;
        var departure = now.AddMinutes(40);
        var status = FlightStatusCalculator.CalculateStatus(departure, now);
        Assert.Equal(FlightStatus.Scheduled, status);
    }

    [Fact]
    public void FlightStatusCalculator_Boarding()
    {
        var now = DateTime.Now;
        var departure = now.AddMinutes(20);
        var status = FlightStatusCalculator.CalculateStatus(departure, now);
        Assert.Equal(FlightStatus.Boarding, status);
    }

    [Fact]
    public void FlightStatusCalculator_Departed()
    {
        var now = DateTime.Now;
        var departure = now.AddMinutes(-10);
        var status = FlightStatusCalculator.CalculateStatus(departure, now);
        Assert.Equal(FlightStatus.Departed, status);
    }

    [Fact]
    public void FlightStatusCalculator_Landed()
    {
        var now = DateTime.Now;
        var departure = now.AddMinutes(-70);
        var status = FlightStatusCalculator.CalculateStatus(departure, now);
        Assert.Equal(FlightStatus.Landed, status);
    }

    [Fact]
    public void FlightValidator_Throws_When_Destination_Is_Missing()
    {
        var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "", Gate = "A1", Departure = "TLV", DepartureTime = DateTime.Now.AddHours(1), ArrivalTime = DateTime.Now.AddHours(2) };
        var existing = new List<FlightEntity>();
        Assert.Throws<ArgumentException>(() => FlightValidator.Validate(flight, existing));
    }

    [Fact]
    public void FlightValidator_Throws_When_Gate_Is_Missing()
    {
        var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "", Departure = "TLV", DepartureTime = DateTime.Now.AddHours(1), ArrivalTime = DateTime.Now.AddHours(2) };
        var existing = new List<FlightEntity>();
        Assert.Throws<ArgumentException>(() => FlightValidator.Validate(flight, existing));
    }

    [Fact]
    public void FlightValidator_Throws_When_Departure_Is_Missing()
    {
        var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "A1", Departure = "", DepartureTime = DateTime.Now.AddHours(1), ArrivalTime = DateTime.Now.AddHours(2) };
        var existing = new List<FlightEntity>();
        Assert.Throws<ArgumentException>(() => FlightValidator.Validate(flight, existing));
    }

    [Fact]
    public void FlightValidator_Throws_When_DepartureTime_Is_Past()
    {
        var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "A1", Departure = "TLV", DepartureTime = DateTime.Now.AddMinutes(-1), ArrivalTime = DateTime.Now.AddHours(1) };
        var existing = new List<FlightEntity>();
        Assert.Throws<ArgumentException>(() => FlightValidator.Validate(flight, existing));
    }

    [Fact]
    public void FlightValidator_Throws_When_ArrivalTime_Before_DepartureTime()
    {
        var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "A1", Departure = "TLV", DepartureTime = DateTime.Now.AddHours(2), ArrivalTime = DateTime.Now.AddHours(1) };
        var existing = new List<FlightEntity>();
        Assert.Throws<ArgumentException>(() => FlightValidator.Validate(flight, existing));
    }

    [Fact]
    public void FlightValidator_DoesNotThrow_When_Valid()
    {
        var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "A1", Departure = "TLV", DepartureTime = DateTime.Now.AddHours(1), ArrivalTime = DateTime.Now.AddHours(2) };
        var existing = new List<FlightEntity> { new FlightEntity { FlightNumber = "999" } };
        FlightValidator.Validate(flight, existing); // Should not throw
    }
}
