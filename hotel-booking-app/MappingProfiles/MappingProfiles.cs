using AutoMapper;
using HotelBookingApp.Models.HotelModels;

namespace HotelBookingApp.MappingProfiles
{
    public class MappingProfiles
    {
        public static MapperConfiguration GetAutoMapperProfiles()
        {
            return new MapperConfiguration(mc =>
            {
                mc.CreateMap<Hotel, ApiHotelDTO>()
                    .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Location.City))
                    .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Location.Country))
                    .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Location.Address));
            });
        }
    }
}
