using AutoMapper;
using HotelBookingApp.Controllers;
using HotelBookingApp.Data;
using HotelBookingApp.Models.API;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using HotelBookingAppTests.TestUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
        private readonly DbContextOptions<ApplicationContext> options = TestDbOptions.Get();

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
        public async Task FetchHotels_WhithInvalidCurrentPage_ShouldCallServiceAndResponseBadRequest()
        {

            // Arrange
            var hotel = new Hotel { Name = "Test hotel" };
            var paginatedList = new PaginatedList<Hotel>();
            using (var context = new ApplicationContext(options))
            {
                int currentPage = 1;
                int pageSize = 1;
                paginatedList = await PaginatedList<Hotel>
                       .CreateAsync(context.Hotels.Take(12), currentPage, pageSize);
            }
            paginatedList.Add(hotel);
            var hotelDTOlist = new List<HotelDTO>();
            int curPage = 99;
            var hotelsDto = new HotelsDTO
            {
                PageCount = 1,
                CurrentPage = curPage,
                Hotels = hotelDTOlist
            };
            var q = new QueryParams();
            q.CurrentPage = curPage;
            q.Search = "";

            hotelServiceMock.Setup(h => h.FindWithQuery(q))
                .ReturnsAsync(paginatedList);

            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object);

            // Act
            var result = await controller.Hotels(q);
            var badRequestResult = result as ObjectResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.True(badRequestResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task FetchHotels_WithValidQueryParams_ShouldCallServiceAndResponseOkWithHotel()
        {
            // Arrange
            var hotel = new Hotel { Name = "Test hotel" };
            var hotels = new PaginatedList<Hotel>();
            hotels.Add(hotel);
            var hotelDTOlist = new List<HotelDTO>();
            hotelDTOlist.Add(new HotelDTO { Name = "Test hotel"});
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
