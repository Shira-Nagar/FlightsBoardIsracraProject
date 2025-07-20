using AutoMapper;
using Flights.BL;
using FlightsBl.Interfaces;
using FlightsDl.Interfaces;
using FlightsEntity;
using FlightsEntity.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightsBl.Services
{
    /// <summary>
    /// User login business logic.
    /// </summary>
    public class UserLogInBl : IUserLogInBl
    {
        private readonly ILogger<UserLogInBl> _logger;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly AppSetting _appSetting;
        private readonly IUserLoginDl _userLoginDl;
        private readonly IMapper _mapper;

        public UserLogInBl(ILogger<UserLogInBl> logger, IHttpContextAccessor httpContextAccessor, IOptions<AppSetting> options, IUserLoginDl userLoginDl,IMapper mapper)
        {
            _logger = logger;
            _httpContextAccesor = httpContextAccessor;
            _appSetting = options.Value;
            _userLoginDl = userLoginDl;
            _mapper = mapper;
        }

        /// <summary>
        /// Login user and create token.
        /// </summary>
        public string? LogIn(CreateUserRequestDto user)
        {
            _logger.LogInformation($"Login attempt for user: {user.Username}");
            var userEntity = _mapper.Map<CreateUserRequestDto, UserLogIn>(user);
            var userFromDb = _userLoginDl.GetByUsername(userEntity.Username);

            if (userFromDb != null)
            {
                _logger.LogInformation($"User found in database: {userFromDb.Username}");
                if (userFromDb.Password == user.Password)
                {
                    _logger.LogInformation($"Password match for user: {user.Username}");
                    string token = GenerateAccessToken(user.Username);
                    CreateUserToken(user.Username);
                    return token;
                }
                else
                {
                    _logger.LogWarning($"Password mismatch for user: {user.Username}");
                }
            }
            else
            {
                _logger.LogWarning($"User not found: {user.Username}");
            }
            return null;
        }
        public string? SignUp(CreateUserRequestDto user)
        {
            var userEntity = _mapper.Map<CreateUserRequestDto, UserLogIn> (user);
            _userLoginDl.SignUp(userEntity);
            
            // Generate and return token for new user
            string token = GenerateAccessToken(user.Username);
            CreateUserToken(user.Username);
            return token;
        }

        /// <summary>
        /// Create JWT token and set cookie.
        /// </summary>
        private void CreateUserToken(string userName)
        {
            string newAccessToken = GenerateAccessToken(userName);

            CookieOptions cookieTokenOption = new CookieOptions()
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(10)
            };

            _httpContextAccesor.HttpContext.Response.Cookies.Append(CookiesKeys.AccessToken, newAccessToken, cookieTokenOption);
        }
  


        /// <summary>
        /// Generate JWT token.
        /// </summary>
        private string GenerateAccessToken(string userName)
        {
            var jwt = _appSetting.Jwt;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
            };

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwt.ExpireMinute),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


}
}

