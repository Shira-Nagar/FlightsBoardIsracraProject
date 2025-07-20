using FlightsEntity;
using FlightsEntity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsBl.Interfaces
{
    /// <summary>
    /// User login business logic interface.
    /// </summary>
    public interface IUserLogInBl
    {
        string?  LogIn(CreateUserRequestDto user);
        string? SignUp(CreateUserRequestDto user);
        
    }
}
