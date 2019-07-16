using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;

namespace HotelBookingAppTests.TestUtils
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            var options = TestDbOptions.Get();
            using (var context = new ApplicationContext(options))
            {
                SeedHotels(context);
                context.SaveChanges();
            }
        }

        private void SeedHotels(ApplicationContext context)
        {
            context.Hotels.Add(new Hotel { HotelId = 1, Name = "Hotel1" });
            context.Hotels.Add(new Hotel { HotelId = 2, Name = "Hotel2" });
            context.Hotels.Add(new Hotel { HotelId = 3, Name = "Hotel3" });
            context.Hotels.Add(new Hotel { HotelId = 4, Name = "Hotel4" });
            context.Hotels.Add(new Hotel { HotelId = 5, Name = "Hotel5" });
            context.Hotels.Add(new Hotel { HotelId = 6, Name = "Hotel6" });
            context.Hotels.Add(new Hotel { HotelId = 7, Name = "Hotel7" });
            context.Hotels.Add(new Hotel { HotelId = 8, Name = "Hotel8" });
            context.Hotels.Add(new Hotel { HotelId = 9, Name = "Hotel9" });
            context.Hotels.Add(new Hotel { HotelId = 10, Name = "Hotel10" });
            context.Hotels.Add(new Hotel { HotelId = 11, Name = "Hotel11" });
            context.Hotels.Add(new Hotel { HotelId = 12, Name = "Hotel12" });
            context.Hotels.Add(new Hotel { HotelId = 13, Name = "Hotel13" });
            context.Hotels.Add(new Hotel { HotelId = 14, Name = "Hotel14" });
            context.Hotels.Add(new Hotel { HotelId = 15, Name = "Hotel15" });
        }
    }
}
