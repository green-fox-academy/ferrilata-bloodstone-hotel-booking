using HotelBookingApp.Controllers;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Controllers
{
    public class RoomControllerTests
    {
        private readonly Mock<IRoomService> roomServiceMock;
        private readonly Mock<IBedService> bedServiceMock;

        public RoomControllerTests()
        {
            roomServiceMock = new Mock<IRoomService>();
            bedServiceMock = new Mock<IBedService>();
        }

        [Fact]
        public async Task Add_WhenValid_ShouldCallServiceAndRedirect()
        {
            var controller = new RoomsController(bedServiceMock.Object, roomServiceMock.Object);
            var hotelId = 1;
            var room = new Room();

            var result = await controller.Add(hotelId, room);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var resultHotelId = (int)redirectResult.RouteValues["id"];

            Assert.Equal("Hotels", redirectResult.ControllerName);
            Assert.Equal(hotelId, resultHotelId);
            Assert.Equal(nameof(HotelsController.Hotel), redirectResult.ActionName);
            roomServiceMock.Verify(s => s.AddRoom(hotelId, room), Times.Once);
        }
    }
}
