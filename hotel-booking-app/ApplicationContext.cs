using Microsoft.EntityFrameworkCore;
namespace HotelBookingApp
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}
