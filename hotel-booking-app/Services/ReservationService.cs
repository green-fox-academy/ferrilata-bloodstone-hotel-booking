using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;

namespace HotelBookingApp.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationContext context;

        public ReservationService(ApplicationContext context)
        {
            this.context = context;
        }

        public Task<Reservation> AddAsync(Reservation reservation)
        {
            throw new NotImplementedException();
        }

        public Task<Reservation> FindById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
