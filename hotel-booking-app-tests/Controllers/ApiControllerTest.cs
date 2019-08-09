using AutoMapper;
using HotelBookingApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using HotelBookingAppTests.TestUtils;
using System.Threading.Tasks;
using HotelBookingApp.Controllers;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using System.Net;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Models.API;
using Microsoft.AspNetCore.Http;

namespace HotelBookingAppTests.Controllers
{
    public class ApiControllerTest
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

        [Fact]
        public async Task FetchHotels_WhenValid_ShouldCallServiceAndResponseOkWithHotel()
        {
            // Arrange
            var hotel = new Hotel { Name = "Test hotel"};
            var hotels = new PaginatedList<Hotel>();
            hotels.Add(hotel);
            var hotelDTOlist = new List<HotelDTO>();
            hotelDTOlist.Add(new HotelDTO { Name = "Test hotel" });
            var hotelsDto = new HotelsDTO
            {
                PageCount = 1,
                CurrentPage = 1,
                Hotels = hotelDTOlist
            };
            var q = new QueryParams();
            q.CurrentPage = 1;
            q.Search = "";

            hotelServiceMock.Setup(h => h.FindWithQuery(q))
                .ReturnsAsync(hotels);
            hotelServiceMock.Setup(h => h.GetHotelDTOs(hotels))
                .Returns(hotelsDto);

            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object);

            // Act
            var result = await controller.Hotels(q);
            var okResult = result as ObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.True(okResult is OkObjectResult);
            Assert.IsType<HotelsDTO>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }
    }
}
