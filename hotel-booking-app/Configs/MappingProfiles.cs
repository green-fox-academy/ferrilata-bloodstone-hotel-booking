﻿using AutoMapper;
using HotelBookingApp.Models.API;
using HotelBookingApp.Models.HotelModels;
using System.Linq;

namespace HotelBookingApp.Configs
{
    public class MappingProfiles
    {
        public static MapperConfiguration GetAutoMapperProfiles()
        {
            return new MapperConfiguration(mc =>
            {
                mc.CreateMap<Hotel, HotelDTO>()
                    .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Location.City))
                    .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Location.Country))
                    .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Location.Address));
                mc.CreateMap<Room, RoomDTO>()
                    .ForMember(dest => dest.PricePerNight, opts => opts.MapFrom(src => src.Price))
                    .ForMember(dest => dest.NumberOfBeds, opts => opts.MapFrom(src => src.RoomBeds.ToList().Count));
                mc.CreateMap<Reservation, ReservationDTO>()
                    .ForMember(dest => dest.Room, opts => opts.MapFrom(src => src.Room));
            });
        }
    }
}
