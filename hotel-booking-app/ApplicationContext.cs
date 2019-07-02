using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingApp
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<HotelModel> Hotels { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
