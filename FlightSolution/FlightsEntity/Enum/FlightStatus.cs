using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FlightsEntity.Enum
{
    /// <summary>
    /// Flight status values.
    /// </summary>
    public enum FlightStatus
    {
        /// <summary>Flight is scheduled.</summary>
        Scheduled,
        /// <summary>Flight is boarding.</summary>
        Boarding,
        /// <summary>Flight has departed.</summary>
        Departed,
        /// <summary>Flight has landed.</summary>
        Landed
    }
}