using AutoMapper;
using FlightsEntity.Dto;

namespace FlightsEntity
{
    /// <summary>
    /// AutoMapper profile for entity mappings.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserRequestDto, UserLogIn>();
            CreateMap<CreateFlightRequestDto, FlightEntity>();
            CreateMap<UserFlightDto, UserFlight>();
        }
    }
}
