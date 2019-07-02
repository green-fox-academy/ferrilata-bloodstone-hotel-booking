﻿using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Models.User;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingApp
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<Bed> Beds { get; set; }
        public virtual DbSet<HotelModel> Hotels { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomBed> RoomBeds { get; set; }
        public virtual DbSet<UserModel> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
