using FlightsEntity;
using FlightsEntity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsDl.Interfaces
{
    /// <summary>
    /// User login data access interface.
    /// </summary>
    public interface IUserLoginDl
    {
        void SignUp(UserLogIn user);
        UserLogIn? GetByUsername(string username);
    }
}
