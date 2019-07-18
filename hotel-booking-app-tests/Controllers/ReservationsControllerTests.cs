using HotelBookingApp.Controllers;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Controllers
{
    public class ReservationsControllerTests
    {
        private readonly Mock<IReservationService> reservationServiceMock;
        private readonly Mock<IRoomService> roomServiceMock;

        public ReservationsControllerTests()
        {
            reservationServiceMock = new Mock<IReservationService>();
            roomServiceMock = new Mock<IRoomService>();
        }

        [Fact]
        public async Task Index_ShouldReturnModelWithReservations()
        {
            var input = new ReservationViewModel();
            reservationServiceMock.Setup(s => s.FindAllByHotelIdAsync(input.HotelId))
                .ReturnsAsync(GetTestReservations());
            var controller = new ReservationsController(reservationServiceMock.Object, roomServiceMock.Object);

            var result = await controller.Index(input);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ReservationViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Reservations.Count());
        }

        private IEnumerable<Reservation> GetTestReservations()
        {
            return new List<Reservation> {
                new Reservation { ReservationId = 1 },
                new Reservation { ReservationId = 2 }
            };
        }
    }
}
