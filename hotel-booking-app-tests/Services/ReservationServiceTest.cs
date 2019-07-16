using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Services;
using HotelBookingAppTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Services
{
    [Collection("Database collection")]
    public class ReservationServiceTest
    {
        private readonly DbContextOptions<ApplicationContext> options;
        private readonly Mock<IStringLocalizer<ReservationService>> localizerMock;

        public ReservationServiceTest()
        {
            options = TestDbOptions.Get();
            localizerMock = new Mock<IStringLocalizer<ReservationService>>();
        }

        [Fact]
        public async Task AddAsync_WhenCalledWithNew_ShouldAdd()
        {
            using (var context = new ApplicationContext(options))
            {
                var reservationService = new ReservationService(context, localizerMock.Object);
                var guestNames = "Guest Name";

                var reservation = await reservationService.AddAsync(
                    new Reservation { GuestNumber = 1, GuestNames = guestNames }
                );

                Assert.True(context.Reservations.Contains(reservation));
                Assert.Equal(1, context.Reservations.Find(reservation.ReservationId).GuestNumber);
                Assert.Equal(guestNames, context.Reservations.Find(reservation.ReservationId).GuestNames);
            }
        }

        [Fact]
        public async Task AddAsync_WhenCalledWithExisting_ShouldUpdate()
        {
            int existingId = 0;
            using (var context = new ApplicationContext(options))
            {
                var reservation = context.Add(
                    new Reservation { GuestNumber = 1, GuestNames = "Guest1" }
                );
                await context.SaveChangesAsync();
                existingId = reservation.Entity.ReservationId;
            }

            using (var context = new ApplicationContext(options))
            {
                var reservationService = new ReservationService(context, localizerMock.Object);

                var reservation = await reservationService.AddAsync(
                    new Reservation { ReservationId = existingId, GuestNumber = 2, GuestNames = "Guest1, Guest2" }
                );

                Assert.True(context.Reservations.Contains(reservation));
                Assert.Equal(2, context.Reservations.Find(reservation.ReservationId).GuestNumber);
                Assert.Equal("Guest1, Guest2", context.Reservations.Find(reservation.ReservationId).GuestNames);
            }
        }
    }
}