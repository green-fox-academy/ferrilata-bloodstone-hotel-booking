using AutoMapper;
using HotelBookingApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingAppTests.Controllers
{
    class ApiControllerTest
    {
        private readonly Mock<IHotelService> hotelServiceMock;
        private readonly Mock<IRoomService> roomServiceMock;
        private readonly Mock<IAccountService> accountServiceMock;
        private readonly Mock<IReservationService> reservationServiceMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IImageService> imageServiceMock;

        public ApiControllerTest()
        {
            hotelServiceMock = new Mock<IHotelService>();
            roomServiceMock = new Mock<IRoomService>();
            accountServiceMock = new Mock<IAccountService>();
            reservationServiceMock = new Mock<IReservationService>();
            mapperMock = new Mock<IMapper>();
            imageServiceMock = new Mock<IImageService>();
        }
    }
}
