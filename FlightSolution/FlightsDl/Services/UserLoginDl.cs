using FlightsDl;
using FlightsDl.Interfaces;
using FlightsEntity;
using FlightsEntity.Dto;
using System.Linq;

namespace FlightsDl.Services
{
    /// <summary>
    /// User login data access.
    /// </summary>
    public class UserLoginDl : IUserLoginDl
    {
        private readonly FlightDbContext _context;
        public UserLoginDl(FlightDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Login user.

        public UserLogIn? GetByUsername(string username)
        {
            return _context.UserLogIns.FirstOrDefault(u => u.Username == username);
        }


        /// Register user
        public void SignUp(UserLogIn user)
        {
            _context.UserLogIns.Add(user);
            _context.SaveChanges();
        }

      
    }
}


