using AutoMapper;
using FlightsEntity;
using FlightsEntity.Dto;

namespace FlightsBl
{
    /// <summary>
    /// AutoMapper profile for flight mapping.
    /// </summary>
    public class FlightMappingProfile : Profile
    {
        public FlightMappingProfile()
        {
            CreateMap<CreateFlightRequestDto, FlightEntity>()
                .ForMember(dest => dest.FlightNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.UserFlights, opt => opt.Ignore());
        }
    }
} 