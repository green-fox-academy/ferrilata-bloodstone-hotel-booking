using Microsoft.EntityFrameworkCore;
namespace hotel_booking_app
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}
