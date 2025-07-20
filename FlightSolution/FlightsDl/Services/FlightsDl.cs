using FlightsDl.Interfaces;
using FlightsEntity;
using FlightsEntity.Enum;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FlightsDl.Services
{
    /// <summary>
    /// Flight data access.
    /// </summary>
    public class FlightsDl:IflightsDl
    {
        private readonly FlightDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightsDl"/> class.
        /// </summary>
        /// <param name="context">The database context to be used for data operations.</param>
        public FlightsDl(FlightDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all flights.
        /// </summary>
        public List<FlightEntity> GetAllFlights()
        {
            return _context.Flights.ToList();
        }

        /// <summary>
        /// Get flight destination by ID.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>The destination of the flight, or null if not found.</returns>
        public string GetFlightById(string id)
        {
            return _context.Flights
                           .AsNoTracking()
                           .Where(x => x.Id == int.Parse(id))
                           .FirstOrDefault() 
                           ?.Destination;
        }

        /// <summary>
        /// Add new flight.
        /// </summary>
        /// <param name="flight">The flight entity to add.</param>
        public void ScheduleNewFlight(FlightEntity flight)
        {
            _context.Flights.Add(flight);
            _context.SaveChanges();
        }
      
        /// Delete flight by ID.
     
        public void DeleteFlight(string id)
        {
            if (id != null)
            {
                FlightEntity flightfromdb = _context.Flights
                                              .Where(x => x.Id == int.Parse(id))
                                              .FirstOrDefault();

                if (flightfromdb != null)
                {
                    _context.Flights.Remove(flightfromdb);
                    _context.SaveChanges();
                }

            }
        }
        /// <summary>
        /// Get all flights async.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="FlightEntity"/> objects.</returns>
        public async Task<IEnumerable<FlightEntity>> GetAllAsync()
        {
            return await _context.Flights.AsNoTracking().ToListAsync();
        }
      


    }

        }
