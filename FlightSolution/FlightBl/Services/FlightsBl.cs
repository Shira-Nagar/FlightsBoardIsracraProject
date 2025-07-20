using Flights.BL;
using FlightsBl.Interfaces;
using FlightsDl.Interfaces;
using FlightsEntity;
using FlightsEntity.Dto;
using FlightsEntity.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FlightsBl.Services
{
    /// <summary>
    /// Flight business logic.
    /// </summary>
    public class FlightsBl : IFlightsBl
    {
        private readonly IflightsDl _flightsDl;
        private readonly ILogger<FlightsBl> _logger;
        private readonly IMapper _mapper;

        public FlightsBl(IflightsDl flightsDl, ILogger<FlightsBl> logger, IMapper mapper)
        {
            _flightsDl = flightsDl;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete flight by ID.
        /// </summary>
        public void DeleteFlight(string id)
        {
            _flightsDl.DeleteFlight(id);
        }

        /// <summary>
        /// Get all flights.
        /// </summary>
        public List<FlightEntity> GetAllFlights()
        {
            return _flightsDl.GetAllFlights();  
        }

        /// <summary>
        /// Get flight destination by ID.
        /// </summary>
        public string GetFlightById(string id)
        {
           return _flightsDl.GetFlightById(id);
        }

        /// <summary>
        /// Get all flights with their calculated status.
        /// </summary>
        public List<FlightEntity> GetAllFlightsWithStatus()
        {
            var allFlights = _flightsDl.GetAllFlights();
            var now = DateTime.Now;
            return allFlights.Select(f => new FlightEntity
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber,
                Destination = f.Destination,
                DepartureTime = f.DepartureTime,
                Gate = f.Gate,
                Status = FlightStatusCalculator.CalculateStatus(f.DepartureTime, now)
            }).ToList();
        }

        /// <summary>
        /// Add new flight.
        /// </summary>
      
        public void ScheduleNewFlight(CreateFlightRequestDto flight)
        {
            var existingFlights = _flightsDl.GetAllFlights();
            FlightValidator.Validate(flight, existingFlights);

            var flightEntity = _mapper.Map<FlightEntity>(flight);

            var now = DateTime.Now;
            flightEntity.Status = FlightStatusCalculator.CalculateStatus(flight.DepartureTime, now);

            _flightsDl.ScheduleNewFlight(flightEntity);
        }
        
        

        /// <summary>
        /// Search flights by status and destination.
        /// </summary>
        public async Task<IEnumerable<FlightEntity>> SearchFlightsAsync(string? status, string? destination)
        {
            var allFlights = await _flightsDl.GetAllAsync(); 
            var now = DateTime.Now;

            var flightsWithStatus = allFlights.Select(f => new FlightEntity
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber,
                Destination = f.Destination,
                DepartureTime = f.DepartureTime,
                Gate = f.Gate,
                Status = FlightStatusCalculator.CalculateStatus(f.DepartureTime, now)
            });

            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<FlightStatus>(status, true, out var parsedStatus))
            {
                flightsWithStatus = flightsWithStatus.Where(f => f.Status == parsedStatus);
            }

            if (!string.IsNullOrWhiteSpace(destination))
            {
                flightsWithStatus = flightsWithStatus.Where(f =>
                    f.Destination.Contains(destination, StringComparison.OrdinalIgnoreCase));
            }

            return flightsWithStatus.ToList();
        }
    }
}
