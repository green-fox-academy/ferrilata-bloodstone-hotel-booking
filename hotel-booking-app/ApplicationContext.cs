using HotelBookingApp.Models;
using HotelBookingApp.Models.User;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HotelModel>().HasData(
                new HotelModel() { Id = 1, Name = "hotel1", Description = "description1", Price = 1 },
                new HotelModel() { Id = 2, Name = "hotel2", Description = "description2", Price = 2 },
                new HotelModel() { Id = 3, Name = "hotel3", Description = "description3", Price = 3 }
                ) ;
        }
    }
}
