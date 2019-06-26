﻿using HotelBookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelModel>> FindAll();
        Task<IEnumerable<HotelModel>> FindAllOrderByName();
        Task Add(HotelModel hotel);
        Task Delete(long hotelId);
    }
}
