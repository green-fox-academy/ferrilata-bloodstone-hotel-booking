using HotelBookingApp.Models.Account;
using HotelBookingApp.Models.Hotel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Bed> Beds { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomBed> RoomBeds { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoomBed>()
                .HasKey(pt => new { pt.RoomId, pt.BedId });

            modelBuilder.Entity<RoomBed>()
                .HasOne(pt => pt.Room)
                .WithMany(p => p.RoomBeds)
                .HasForeignKey(pt => pt.RoomId);

            modelBuilder.Entity<RoomBed>()
                .HasOne(pt => pt.Bed)
                .WithMany(t => t.RoomBeds)
                .HasForeignKey(pt => pt.BedId);
        }
    }
}
