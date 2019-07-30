using HotelBookingApp.Controllers;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using HotelBookingAppTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task Add_WhenModelIsInvalid_ShouldReturnView()
        {
            var input = new ReservationViewModel();
            var controller = new ReservationsController(reservationServiceMock.Object, roomServiceMock.Object);
            controller.ModelState.AddModelError("GuestNames", "Required");

            var result = await controller.Add(input);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Add_WhenModelIsValid_ShouldRedirectToConfirm()
        {
            var input = new ReservationViewModel();
            var reservation = new Reservation { ReservationId = 1 };
            reservationServiceMock.Setup(s => s.AddAsync(input.Reservation))
                .ReturnsAsync(reservation);
            var controllerContext = ControllerContextProvider.GetDefault();
            var controller = new ReservationsController(reservationServiceMock.Object, roomServiceMock.Object)
            {
                ControllerContext = controllerContext
            };

            var result = await controller.Add(input);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            int resultReservationId = (int)redirectResult.RouteValues[nameof(reservation.ReservationId)];

            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(nameof(controller.Confirm), redirectResult.ActionName);
            Assert.Equal(reservation.ReservationId, resultReservationId);
            reservationServiceMock.Verify(s => s.AddAsync(input.Reservation), Times.Once);
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
