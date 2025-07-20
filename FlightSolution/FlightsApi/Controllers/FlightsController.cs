using FlightsBl.Interfaces;
using FlightsBl.Services;
using FlightsEntity;
using FlightsEntity.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FlightsApi.Controllers
{
    /// <summary>
    /// API controller for managing flights, including retrieval, creation, deletion, and search operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Allow access without authentication
    public class FlightsController : ControllerBase
    {
        private readonly IFlightsBl _flightBl;
        private readonly ILogger<FlightsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightsController"/> class.
        /// </summary>
        /// <param name="flightsBl">Business logic layer for flights.</param>
        /// <param name="logger">Logger for logging operations.</param>
        public FlightsController(IFlightsBl flightsBl, ILogger<FlightsController> logger)
        {
            _flightBl = flightsBl;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all flights.
        /// </summary>
        /// <returns>List of all flights.</returns>
        [HttpGet]
        public IActionResult GetAllFlights()
        {
            try
            {
                _logger.LogInformation($"{nameof(GetAllFlights)}");
                return Ok(_flightBl.GetAllFlightsWithStatus());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
        [HttpGet("{id?}")]
        public IActionResult GetFlightById([FromRoute] string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetFlightById)}");
                return Ok(_flightBl.GetFlightById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ScheduleNewFlightAsync([FromBody] CreateFlightRequestDto flight)
        {
            try
            {
                if (flight == null)
                {
                    return BadRequest("Flight details are missing or invalid.");
                }
              
                 _flightBl.ScheduleNewFlight(flight);
                _logger.LogInformation(message: $"{nameof(ScheduleNewFlightAsync)}");
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// Deletes a flight by its ID.
      
        [HttpDelete("{id?}")]
        public IActionResult DeleteFlight(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(DeleteFlight)}: {id}");

                _flightBl.DeleteFlight(id);

                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

   
        /// Searches for flights by status and destination.
   
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? status, [FromQuery] string? destination)
        {
            try
            {
                _logger.LogInformation($"{nameof(Search)}");
                var result = await _flightBl.SearchFlightsAsync(status, destination);
                return Ok(result);
            }
            catch (Exception ex) {

                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
