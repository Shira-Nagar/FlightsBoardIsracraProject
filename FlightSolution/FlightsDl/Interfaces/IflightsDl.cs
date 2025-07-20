using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsEntity;
using Microsoft.EntityFrameworkCore;

namespace FlightsDl.Interfaces
{
    /// <summary>
    /// Flight data access interface.
    /// </summary>
    public interface IflightsDl
    {
       
        List<FlightEntity> GetAllFlights();
       
        string GetFlightById(string id);
       
        void DeleteFlight(string id );
        
        void ScheduleNewFlight(FlightEntity flight);
     
       
        Task<IEnumerable<FlightEntity>> GetAllAsync();
    }
}
