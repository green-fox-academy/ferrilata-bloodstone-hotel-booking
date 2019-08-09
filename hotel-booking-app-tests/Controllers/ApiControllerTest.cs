using AutoMapper;
using HotelBookingApp.Controllers;
using HotelBookingApp.Data;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.API;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Models.Image;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using HotelBookingAppTests.TestUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        public async Task FetchHotels_WithValidQueryParams_ShouldCallServiceAndResponseOkWithHotels()
        {
            // Arrange
            var hotel = new Hotel { Name = "Test hotel" };
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

        [Fact]
        public async Task FindHotel_WhichExists_ShouldCallServiceAndResponseOkWithHotel()
        {
            // Arrange
            var hotelId = 1;
            var hotel = new Hotel { HotelId = hotelId, Name = "Test hotel" };
            hotelServiceMock.Setup(h => h.FindByIdAsync(hotelId))
                .ReturnsAsync(hotel);
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object);

            // Act
            var result = await controller.FindHotel(hotelId);
            var okResult = result as ObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.True(okResult is OkObjectResult);
            Assert.IsType<Hotel>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task FindHotel_WhichNotExists_ShouldCallServiceAndResponseBadRequest()
        {
            // Arrange
            var hotelId = 1;
            hotelServiceMock.Setup(h => h.FindByIdAsync(hotelId))
                .Throws(new ItemNotFoundException());
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object);

            // Act
            var result = await controller.FindHotel(hotelId);
            var badRequestResult = result as ObjectResult;

            // Assert
            Assert.NotNull(badRequestResult);
            Assert.True(badRequestResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task FindAllHotelImages_WhichExists_ShouldCallServiceAndResponseOk()
        {
            // Arrange
            var hotelId = 1;
            var imageDetails = new List<ImageDetails>();
            imageDetails.Add(new ImageDetails { Name = "Test image" });
            imageServiceMock.Setup(h => h.GetImageListAsync(hotelId))
                .ReturnsAsync(imageDetails);
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object);

            // Act
            var result = await controller.FindAllHotelImages(hotelId);
            var okResult = result as ObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.True(okResult is OkObjectResult);
            Assert.IsType<List<ImageDetails>>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task FindAllHotelImages_WhenBlobNotAvailable_ShouldThrowServerError()
        {
            // Arrange
            var hotelId = 1;
            imageServiceMock.Setup(h => h.GetImageListAsync(hotelId))
                .Throws(new SocketException());
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object);

            // Act
            var response = await controller.FindAllHotelImages(hotelId);
            var result = response as StatusCodeResult;

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task Rooms_WhenHotelExists_ShouldResponseOkWithRoomDTOS()
        {
            // Arrange
            var hotelId = 1;
            var roomDTOs = new List<RoomDTO>();
            roomDTOs.Add(new RoomDTO { Name = "Test room" });
            roomServiceMock.Setup(h => h.GetRoomDTOs(hotelId))
                .ReturnsAsync(roomDTOs);
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object);

            // Act
            var response = await controller.Rooms(hotelId);
            var result = response as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result is OkObjectResult);
            Assert.IsType<List<RoomDTO>>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Rooms_WhenHotelNotExists_ShouldResponseOkWithRoomDTOS()
        {
            // Arrange
            var hotelId = 1;
            roomServiceMock.Setup(h => h.GetRoomDTOs(hotelId))
                .Throws(new ItemNotFoundException());
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object);

            // Act
            var response = await controller.Rooms(hotelId);
            var result = response as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result is BadRequestObjectResult);
            Assert.IsType<string>(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Reserve_WhenHotelAndRoomExists_ShouldResponseOkWithReservation()
        {
            // Arrange
            var roomId = 1;
            var model = new ReservationViewModel();
            var reservation = new Reservation
            {
                ReservationId = 1,
                GuestNames = "Jani Kiss",
                GuestNumber = 2
            };
            model.Reservation = reservation;
            reservationServiceMock.Setup(h => h.AddAsync(reservation))
                .ReturnsAsync(reservation);
            var controllerContext = ControllerContextProvider.GetDefault();
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var response = await controller.Reserve(roomId, model);
            var result = response as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result is OkObjectResult);
            Assert.IsType<Reservation>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Reserve_WhenModelisInvalid_ShouldResponseBadRequest()
        {
            // Arrange
            var roomId = 1;
            var model = new ReservationViewModel();
            var reservation = new Reservation
            {
                ReservationId = 1,
                GuestNumber = 1
            };
            model.Reservation = reservation;
            reservationServiceMock.Setup(h => h.AddAsync(reservation))
                .ReturnsAsync(reservation);
            var controllerContext = ControllerContextProvider.GetDefault();
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object)
            {
                ControllerContext = controllerContext
            };
            controller.ModelState.AddModelError("GuestNames", "Required");

            // Act
            var response = await controller.Reserve(roomId, model);
            var result = response as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result is BadRequestObjectResult);
            Assert.IsType<ReservationViewModel>(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Confirm_WhenIsOccupied_ShouldResponseOk()
        {
            // Arrange
            var reservationId = 1;
            var model = new ReservationViewModel();
            var userName = "John Doe";
            var reservation = new Reservation
            {
                ReservationId = reservationId,
                GuestNames = "Jani Kiss",
                GuestNumber = 1
            };
            model.Reservation = reservation;
            reservationServiceMock.Setup(r => r.ConfirmAsync(reservation.ReservationId, userName))
                .ReturnsAsync(reservation);
            reservationServiceMock.Setup(r => r.IsIntervalOccupied(reservation))
                .ReturnsAsync(true);
            var controllerContext = ControllerContextProvider.GetDefault();
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var response = await controller.Confirm(reservationId, reservation);
            var result = response as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result is BadRequestObjectResult);
            Assert.IsType<string>(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Confirm_WhenNotOccupied_ShouldResponseOk()
        {
            // Arrange
            var reservationId = 1;
            var model = new ReservationViewModel();
            var userName = "John Doe";
            var reservation = new Reservation
            {
                ReservationId = reservationId,
                GuestNames = "Jani Kiss",
                GuestNumber = 1
            };
            model.Reservation = reservation;
            reservationServiceMock.Setup(r => r.ConfirmAsync(reservation.ReservationId, userName))
                .ReturnsAsync(reservation);
            reservationServiceMock.Setup(r => r.IsIntervalOccupied(reservation))
                .ReturnsAsync(false);
            var controllerContext = ControllerContextProvider.GetDefault();
            var controller = new ApiController(hotelServiceMock.Object, roomServiceMock.Object, accountServiceMock.Object, reservationServiceMock.Object, mapperMock.Object, imageServiceMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var response = await controller.Confirm(reservationId, reservation);
            var result = response as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result is OkObjectResult);
            Assert.IsType<Reservation>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
