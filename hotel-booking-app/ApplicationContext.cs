using HotelBookingApp.Models;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<HotelModel> Hotels { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

    }
}
