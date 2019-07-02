using HotelBookingApp.Models.Account;
using HotelBookingApp.Models.Hotel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingApp
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

            modelBuilder.Entity<PropertyType>().HasData(
                new PropertyType { PropertyTypeId = 1, Type = "Apartment" },
                new PropertyType { PropertyTypeId = 2, Type = "Hostel" },
                new PropertyType { PropertyTypeId = 3, Type = "Hotel" },
                new PropertyType { PropertyTypeId = 4, Type = "Guesthouse" }
                );

            modelBuilder.Entity<Location>().HasData(
                new Location { LocationId = 1, Country = "Moon", City = "Moon", Region = "Moon District?", Address = "a street 1" },
                new Location { LocationId = 2, Country = "Mars", City = "Mars", Region = "Mars District?", Address = "a street mars" },
                new Location { LocationId = 3, Country = "Pluto", City = "p", Region = "Pluto District?", Address = "a street p" }
                );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel() { HotelId = 1, Name = "hotel1", Description = "description1", Price = 1, StarRating = 1, LocationId = 1, PropertyTypeId = 1 },
                new Hotel() { HotelId = 2, Name = "hotel2", Description = "description2", Price = 2, StarRating = 1, LocationId = 2, PropertyTypeId = 2 },
                new Hotel() { HotelId = 3, Name = "hotel3", Description = "description3", Price = 3, StarRating = 1, LocationId = 3, PropertyTypeId = 1 }
            );
        }
    }
}
