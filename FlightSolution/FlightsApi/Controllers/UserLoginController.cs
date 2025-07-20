using AutoMapper;
using FlightsBl.Interfaces;
using FlightsEntity;
using FlightsEntity.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace RestApi.Controllers
{
    /// <summary>
    /// API controller for managing user login, registration, and logout operations.
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserLogInBl _userbl;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">Logger for logging operations.</param>
        /// <param name="userBl">Business logic layer for user login operations.</param>
        public UserController(ILogger<UserController> logger, IUserLogInBl userBl)
        {
            _logger = logger;
            _userbl = userBl;
        }

        /// <summary>
        /// Logs in a user and returns a success or not found response.
        /// </summary>
        /// <param name="user">The user login details.</param>
        /// <returns>Success if login is successful; otherwise, not found or error.</returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] CreateUserRequestDto user)
        {
            try
            {
                _logger.LogInformation($"Login request received for user: {user?.Username}");
                string? token = _userbl.LogIn(user);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning($"Login failed for user: {user?.Username}");
                    return Unauthorized(new { message = "שם משתמש או סיסמה שגויים" });
                }
                _logger.LogInformation($"Login successful for user: {user?.Username}");
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on Login Message: {ex.Message}, InnerException: {ex.InnerException}, stacktrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// Registers a new user in the system.
      
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignUp([FromBody] CreateUserRequestDto user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User details are missing or invalid.");
                }

                string? token = _userbl.SignUp(user);
                _logger.LogInformation(message: $"{nameof(SignUp)}");

                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SignUp: {ex.Message}, Inner: {ex.InnerException?.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while registering the user.");
            }
        }

        /// <summary>
        /// Logs out the user by deleting the authentication cookie.
        /// </summary>
        /// <param name="user">The user to log out.</param>
        /// <returns>Success or error response.</returns>
        [HttpGet]
        public IActionResult Logout(UserLogIn user)
        {
            try
            {
                Response.Cookies.Delete(CookiesKeys.AccessToken);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Errror on Login Message: {ex.Message}," + $"ÏnnerException: {ex.InnerException}," + $"stacktrace: {ex.StackTrace}");
                return (StatusCode(StatusCodes.Status500InternalServerError));

            }

        }
    }
}
